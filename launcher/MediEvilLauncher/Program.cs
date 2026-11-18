// SPDX-License-Identifier: GPL-3.0-or-later
//
// MediEvilLauncher - one launcher for the Italian, Spanish, French (this PAL project) and USA
// (original BlackLabelHQ project) releases of MediEvil. It contains no game data: the game
// code is generated on the player's computer from their own disc.
//
// Without arguments it opens the window. Command line (output on the console):
//   MediEvilLauncher --detect <disc>   which release the disc is and its build state
//   MediEvilLauncher --build <disc>    build that release (exit code 0 = built)
//   MediEvilLauncher --update <disc>   download the updates of that release's project

using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace MediEvilLauncher;

static class Program
{
    [DllImport("kernel32.dll")]
    static extern bool AttachConsole(int pid);

    [STAThread]
    static int Main(string[] args)
    {
        if (args.Length == 2 && args[0] is "--detect" or "--build" or "--update")
            return Cli(args[0], args[1]);
        if (args.Length == 1 && args[0] == "--tools")
        {
            AttachConsole(-1);
            Console.WriteLine("before: " + Tools.Describe());
            try { Tools.Ensure(Console.WriteLine).GetAwaiter().GetResult(); }
            catch (Exception e) { Console.WriteLine(e.Message); return 1; }
            Console.WriteLine("after:  " + Tools.Describe());
            return 0;
        }
        ApplicationConfiguration.Initialize();
        Application.Run(new MainForm());
        return 0;
    }

    static int Cli(string cmd, string disc)
    {
        AttachConsole(-1);
        var builder = new Builder(Builder.FindPalRoot(), Console.WriteLine);
        if (!Builder.IsPalRoot(builder.PalRoot))
        {
            Console.WriteLine($"PAL project not found: {builder.PalRoot}");
            return 2;
        }
        var cnf = Builder.ReadSystemCnf(disc);
        if (cnf == null)
        {
            Console.WriteLine(T.S("unreadable"));
            return 2;
        }
        var v = GameVersions.FromSystemCnf(cnf);
        if (v == null)
        {
            Console.WriteLine(T.F("unknown", cnf.Split('\n')[0].Trim()));
            return 2;
        }
        Console.WriteLine($"{T.S("ver." + v.Code)}  [{v.Code}]  project: {builder.Root(v)}");
        Console.WriteLine($"state: {builder.State(v, disc)}");
        try
        {
            if (cmd == "--build")
                builder.Build(v, disc, (s, n, total) => Console.WriteLine($"== {n}/{total} {s}")).GetAwaiter().GetResult();
            else if (cmd == "--update")
                builder.Update(v, (s, n, total) => Console.WriteLine($"== {n}/{total} {s}")).GetAwaiter().GetResult();
            else
                return 0;
        }
        catch (Builder.StepFailed e)
        {
            Console.WriteLine(T.F("failed", e.Message));
            return 1;
        }
        Console.WriteLine($"state: {builder.State(v, disc)}");
        return 0;
    }
}
