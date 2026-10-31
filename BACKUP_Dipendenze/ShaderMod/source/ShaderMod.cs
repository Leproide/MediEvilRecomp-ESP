using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using ImGuiNET;
using RecompOne.Runtime.Config;
using RecompOne.Runtime.Hle;
using RecompOne.Runtime.Host.Window;
using RecompOne.Runtime.Modding;

//this is not sotn specific this works with any recomp powered by recompone in theory

public class ShaderMod : IMod
{
    internal const string ModId = "shaders";
    internal const string Prefix = "mods.shaders.";
    internal const string DefaultRepository = "https://github.com/libretro/slang-shaders";
    private const string MenuKey = "menu.shaders";

    internal ShaderLibrary Library = null!;
    internal ShaderRepository Repository = null!;
    private ShadersPopup? popup;
    private string repositoryInput = "";
    private string branchInput = "";

    internal string Selected { get; private set; } = "";

    public void OnLoad()
    {
        var source = ModLoader.LoadedMods.FirstOrDefault(m => m.Id == ModId)?.SourcePath ?? Path.GetFullPath(Path.Combine("mods", ModId));
        LoadStrings(source);

        Library = new ShaderLibrary(Path.GetFullPath("shaders"));
        Repository = new ShaderRepository(Library);

        repositoryInput = Settings.Get(Prefix + "repository", DefaultRepository);
        branchInput = Settings.Get(Prefix + "branch", "");
        Repository.Configure(repositoryInput, branchInput);

        PostFx.NativeInput = Settings.Get(Prefix + "native", true);
        Library.Refresh();
        Apply(Settings.Get(Prefix + "selected", ""));

        popup = new ShadersPopup(this);
        PopupManager.Register(popup);
        MenuRegistry.BarItem(MenuKey, popup.Open).After("menu.mods");
    }

    public void OnUnload()
    {
        MenuRegistry.Remove(MenuKey);
        if (popup != null) PopupManager.Unregister(popup);
        popup = null;
        Repository.Cancel();
        PostFx.Clear();
    }

    public void DrawSettings()
    {
        ImGui.SetNextItemWidth(320f);
        ImGui.InputText(Localization.T("shaders.repository"), ref repositoryInput, 512);
        if (ImGui.IsItemDeactivatedAfterEdit()) SaveRepository();
        SettingsWidgets.Hint("shaders.repository_hint");

        ImGui.SetNextItemWidth(320f);
        ImGui.InputTextWithHint(Localization.T("shaders.branch"), "HEAD", ref branchInput, 128);
        if (ImGui.IsItemDeactivatedAfterEdit()) SaveRepository();
        SettingsWidgets.Hint("shaders.branch_hint");

        if (!ShaderRepository.TryParse(repositoryInput, out _, out _))
            ImGui.TextColored(new System.Numerics.Vector4(1f, 0.4f, 0.4f, 1f), Localization.T("shaders.bad_repository"));

        if (ImGui.Button(Localization.T("shaders.default_repository")))
        {
            repositoryInput = DefaultRepository;
            branchInput = "";
            SaveRepository();
        }

        ImGui.Spacing();
        if (ImGui.Button(FontSet.With(FontSet.FolderOpen, Localization.T("shaders.open_folder")))) OpenFolder();
    }

    internal void SetNative(bool native)
    {
        PostFx.NativeInput = native;
        Settings.Set(Prefix + "native", native);
    }

    private void SaveRepository()
    {
        repositoryInput = repositoryInput.Trim();
        branchInput = branchInput.Trim();
        Settings.Set(Prefix + "repository", repositoryInput);
        Settings.Set(Prefix + "branch", branchInput);
        Repository.Configure(repositoryInput, branchInput);
    }

    internal void Apply(string relative)
    {
        Selected = relative;
        Settings.Set(Prefix + "selected", relative);

        if (string.IsNullOrEmpty(relative) || !File.Exists(Library.FullPath(relative)))
        {
            PostFx.Clear();
            return;
        }

        if (PostFx.Load(Library.FullPath(relative))) PostFx.SetParams(SavedParams(relative));
        else Console.Error.WriteLine($"[Shaders] {relative}: {PostFx.Error}");
    }

    internal void Reload()
    {
        Library.Refresh();
        Apply(Selected);
    }

    internal void SetParam(SlangParameter parameter, float value)
    {
        PostFx.SetParam(parameter.Name, value);
        var saved = SavedParams(Selected);
        if (value == parameter.Default) saved.Remove(parameter.Name);
        else saved[parameter.Name] = value;
        SaveParams(saved);
    }

    internal void ResetParams()
    {
        PostFx.ResetParams();
        Settings.Remove(ParamsKey(Selected));
    }

    internal void Remove(string relative)
    {
        Settings.Remove(ParamsKey(relative));
        Library.Remove(relative);
        if (relative == Selected) Apply("");
    }

    private Dictionary<string, float> SavedParams(string relative)
    {
        return Settings.Get(ParamsKey(relative), new Dictionary<string, float>(StringComparer.Ordinal));
    }

    private void SaveParams(Dictionary<string, float> values)
    {
        if (values.Count == 0) Settings.Remove(ParamsKey(Selected));
        else Settings.Set(ParamsKey(Selected), values);
    }

    private static string ParamsKey(string relative)
    {
        return Prefix + "params." + relative;
    }

    internal void OpenFolder()
    {
        try
        {
            Directory.CreateDirectory(Library.Root);
            Process.Start(new ProcessStartInfo(Library.Root) { UseShellExecute = true });
        }
        catch (Exception e)
        {
            Console.Error.WriteLine($"[Shaders] fail to open the shaders folder: {e.Message}");
        }
    }

    private static void LoadStrings(string source)
    {
        try
        {
            if (Directory.Exists(source))
            {
                var path = Path.Combine(source, "languages.json");
                if (File.Exists(path)) Localization.Merge(File.ReadAllText(path));
                return;
            }

            using var zip = ZipFile.OpenRead(source);
            var entry = zip.GetEntry("languages.json");
            if (entry == null) return;
            using var reader = new StreamReader(entry.Open());
            Localization.Merge(reader.ReadToEnd());
        }
        catch (Exception e)
        {
            Console.Error.WriteLine($"[Shaders] failed to load strings: {e.Message}");
        }
    }
}
