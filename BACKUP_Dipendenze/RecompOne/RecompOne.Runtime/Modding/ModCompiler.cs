using System.Reflection;
using System.Reflection.Metadata;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using Microsoft.CodeAnalysis.Text;

namespace RecompOne.Runtime.Modding;

public static class ModCompiler
{
    private static List<MetadataReference>? _references;

    private static readonly string[] Preloaded =
        ["System.Net.Http", "System.Net.Primitives", "System.IO.Compression", "System.Text.Json"];

    public static byte[]? Compile(string modId, IReadOnlyList<(string Path, string Text)> sources)
    {
        var parseOptions = CSharpParseOptions.Default.WithLanguageVersion(LanguageVersion.Latest);
        var trees = sources.Select(s =>
            CSharpSyntaxTree.ParseText(SourceText.From(s.Text, Encoding.UTF8), parseOptions, s.Path)).ToList();
        var options = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary).WithAllowUnsafe(true)
            .WithOptimizationLevel(OptimizationLevel.Release);

        var compilation = CSharpCompilation.Create($"mod-{modId}", trees, References(), options);
        using var ms = new MemoryStream();
        var result = compilation.Emit(ms,
            options: new EmitOptions(debugInformationFormat: DebugInformationFormat.Embedded));
        if (!result.Success)
        {
            foreach (var diag in result.Diagnostics.Where(d => d.Severity == DiagnosticSeverity.Error))
                Console.Error.WriteLine($"[Mods] {modId}: {diag}");
            return null;
        }

        return ms.ToArray();
    }

    private static unsafe List<MetadataReference> References()
    {
        if (_references != null) return _references;

        foreach (var preload in Preloaded)
            try
            {
                Assembly.Load(preload);
            }
            catch (Exception)
            {
            }

        var refs = new List<MetadataReference>();
        var names = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (var a in AppDomain.CurrentDomain.GetAssemblies())
        {
            if (a.IsDynamic) continue;

            if (!string.IsNullOrEmpty(a.Location))
            {
                refs.Add(MetadataReference.CreateFromFile(a.Location));
                names.Add(Path.GetFileNameWithoutExtension(a.Location));
                continue;
            }

            //use the budnled dll instead of trying to find ones that odnt exist, should fix mod on single file publish
            if (a.TryGetRawMetadata(out var blob, out var length))
                refs.Add(
                    AssemblyMetadata.Create(ModuleMetadata.CreateFromMetadata((IntPtr)blob, length)).GetReference());
        }

        if (AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES") is string platform)
            foreach (var path in platform.Split(Path.PathSeparator, StringSplitOptions.RemoveEmptyEntries))
            {
                var name = Path.GetFileNameWithoutExtension(path);
                if (!name.StartsWith("System.", StringComparison.Ordinal) && name is not ("System" or "netstandard" or "mscorlib"))
                    continue;
                if (!names.Add(name)) continue;
                try
                {
                    refs.Add(MetadataReference.CreateFromFile(path));
                }
                catch (Exception)
                {
                }
            }

        _references = refs;
        return _references;
    }
}