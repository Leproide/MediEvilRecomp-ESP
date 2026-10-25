namespace RecompOne.Runtime.Pgxp;

public static class PgxpGpu
{
    private const int CacheDim = 0x400 * 2;
    private const int CacheOrigin = 0x400;
    private const int CacheSize = CacheDim * CacheDim;
    
    private const uint TagAmbiguous = 1u;
    
    private const uint ModeInit = 0;
    private const uint ModeWrite = 1;
    private const uint ModeRead = 2;
    private const uint ModeFail = 3;
    
    private const uint SeqWindow = 1024u;
    private const int MaxAlternates = 3;
    
    private struct Cell
    {
        public float X;
        public float Y;
        public float Z;
        public uint Seq;
        public uint Tag;
        public int Transform;
    }
    
    private struct Alt
    {
        public uint Generation;
        public int Count;
        public float X0, Y0, Z0;
        public float X1, Y1, Z1;
        public float X2, Y2, Z2;
        public uint S0, S1, S2;
        public int T0, T1, T2;
        
        public void Get(int index, out float x, out float y, out float z, out uint seq, out int transform)
        {
            switch (index)
            {
                case 0: x = X0; y = Y0; z = Z0; seq = S0; transform = T0; return;
                case 1: x = X1; y = Y1; z = Z1; seq = S1; transform = T1; return;
                default: x = X2; y = Y2; z = Z2; seq = S2; transform = T2; return;
            }
        }
        
        public void Set(int index, float x, float y, float z, uint seq, int transform)
        {
            switch (index)
            {
                case 0: X0 = x; Y0 = y; Z0 = z; S0 = seq; T0 = transform; return;
                case 1: X1 = x; Y1 = y; Z1 = z; S1 = seq; T1 = transform; return;
                default: X2 = x; Y2 = y; Z2 = z; S2 = seq; T2 = transform; return;
            }
        }
    }
    
    private static readonly Dictionary<int, Alt> _alternates = new();
    
    private static Cell[]? _cache;
    private static uint _generation;
    private static uint _mode = ModeInit;
    
    public static void Init()
    {
        _cache = null;
        _alternates.Clear();
        _generation = 0;
        _mode = ModeInit;
    }
    public static void Free()
    {
        _cache = null;
        _alternates.Clear();
        _mode = ModeInit;
    }
    
    private static bool EnsureAllocated()
    {
        if (_cache != null) return true;
        
        try
        {
            _cache = new Cell[CacheSize];
        }
        catch (OutOfMemoryException)
        {
            return false;
        }
        
        return true;
    }

    public static void CacheVertex(int sx, int sy, in PgxpValue value)
    {
        if (_mode != ModeWrite)
        {
            if (!EnsureAllocated())
            {
                _mode = ModeFail;
                return;
            }
            
            if (_generation < 0x7FFFFFFFu) _generation++;
            _mode = ModeWrite;
        }

        if (!TrySlot(sx, sy, out var index)) return;
        
        ref var cell = ref _cache![index];
        
        if (Generation(cell.Tag) == _generation)
        {
            if (cell.X == value.X && cell.Y == value.Y && cell.Z == value.Z) return;
            
            if (!_alternates.TryGetValue(index, out var alt) || alt.Generation != _generation)
            {
                alt = new Alt { Generation = _generation, Count = 1 };
                alt.Set(0, value.X, value.Y, value.Z, value.Count, value.Transform);
                _alternates[index] = alt;
                return;
            }
            
            for (var i = 0; i < alt.Count; i++)
            {
                alt.Get(i, out var ax, out var ay, out var az, out _, out _);
                if (ax == value.X && ay == value.Y && az == value.Z) return;
            }
            
            if (alt.Count >= MaxAlternates)
            {
                cell.Tag |= TagAmbiguous;
                
                return;
            }
            
            alt.Set(alt.Count, value.X, value.Y, value.Z, value.Count, value.Transform);
            alt.Count++;
            _alternates[index] = alt;
            return;
        }
        
        cell.X = value.X;
        cell.Y = value.Y;
        cell.Z = value.Z;
        cell.Seq = value.Count; //ad seq
        cell.Transform = value.Transform;
        cell.Tag = _generation << 1;
    }
    
    public static bool TryGetVertex(uint packed, uint hintSeq, bool hasHint, out float x, out float y, out float w,
        out bool validW, out uint seq, out int transform)
    {
        validW = false;
        x = y = 0f;
        w = 1f;
        seq = 0u;
        transform = 0;
        var found = false;
        var ambiguous = false;
        var bestDistance = uint.MaxValue;
        for (var i = 0; i < 3; i++)
        {
            ref readonly var candidate = ref (i == 0 ? ref PgxpGte.Sxy2 : ref (i == 1 ? ref PgxpGte.Sxy1 : ref PgxpGte.Sxy0));
            if (!TryMatch(in candidate, packed, out var cx, out var cy, out var cw, out var cv, out var cs, out var ct)) continue;
            var distance = hasHint ? Distance(cs, hintSeq) : 0u;
            if (hasHint && distance > SeqWindow) continue;
            if (!found || distance < bestDistance)
            {
                found = true;
                ambiguous = false;
                bestDistance = distance;
                x = cx; y = cy; w = cw; validW = cv; seq = cs; transform = ct;
            }
            else if (distance == bestDistance && (x != cx || y != cy || w != cw || transform != ct))
            {
                ambiguous = true;
            }
        }
        if (found && !ambiguous) return true;

        if (!Pgxp.VertexCache || !TryCache(packed, hintSeq, hasHint, out x, out y, out w, out seq, out transform)) return false;
        
        validW = Pgxp.CacheW && float.IsFinite(w) && w > 0f;
        return true;
    }
    private static bool TryMatch(in PgxpValue value, uint packed, out float x, out float y, out float w, out bool validW, out uint seq,
        out int transform)
    {
        x = 0f;
        y = 0f;
        w = 1f;
        seq = 0u;
        transform = 0;
        
        validW = false;
        if (!PgxpFlags.Matches(in value, packed) || !float.IsFinite(value.X) || !float.IsFinite(value.Y)) return false;
        
        x = value.X;
        y = value.Y;
        w = value.Z;
        validW = (value.Flags & PgxpFlags.Valid2) != 0 && float.IsFinite(w) && w > 0f;
        seq = value.Count;
        transform = value.Transform;
        return true;
    }
    
    private static bool TryCache(uint packed, uint hintSeq, bool hasHint, out float x, out float y, out float w,
        out uint seq, out int transform)
    {
        x = 0f;
        y = 0f;
        w = 1f;
        seq = 0u;
        transform = 0;
        
        if (_mode == ModeFail || _cache == null) return false;
        if (_mode != ModeRead) _mode = ModeRead;
        if (!TrySlot((short)(packed & 0xFFFF), (short)(packed >> 16), out var index)) return false;
        
        ref var cell = ref _cache[index];
        if (cell.Tag == 0) return false;
        if (Generation(cell.Tag) != _generation) return false;
        if ((cell.Tag & TagAmbiguous) != 0) return false;
        
        x = cell.X;
        y = cell.Y;
        w = cell.Z;
        seq = cell.Seq;
        transform = cell.Transform;
        
        if (!_alternates.TryGetValue(index, out var alt) || alt.Generation != _generation) return true;
        
        if (!hasHint) return false;
        
        var best = Distance(seq, hintSeq);
        var tied = false;
        
        for (var i = 0; i < alt.Count; i++)
        {
            alt.Get(i, out var ax, out var ay, out var az, out var aseq, out var atransform);
            
            var distance = Distance(aseq, hintSeq);
            if (distance == best)
            {
                if (x != ax || y != ay || w != az || transform != atransform) tied = true;
                continue;
            }
            if (distance > best) continue;

            tied = false;
            best = distance;
            x = ax;
            y = ay;
            w = az;
            seq = aseq;
            transform = atransform;
        }
        
        return !tied && best <= SeqWindow;
    }
    
    private static bool TrySlot(int sx, int sy, out int index)
    {
        index = 0;
        if (sx < -CacheOrigin || sx >= CacheOrigin || sy < -CacheOrigin || sy >= CacheOrigin) return false;
        
        index = (sy + CacheOrigin) * CacheDim + (sx + CacheOrigin);
        return true;
    }
    
    private static uint Distance(uint a, uint b)
    {
        return Math.Min(unchecked(a - b), unchecked(b - a));
    }
    
    private static uint Generation(uint tag)
    {
        return tag >> 1;
    }
}
