using System.Diagnostics;

namespace XPDB.Misc
{
    public static class Runner
    {
        public static bool RunCommand(string command)
        {
            ProcessStartInfo processStartInfo = new()
            {
                FileName = "cmd.exe",
                Arguments = $"/C {command}",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using Process process = new();
            process.StartInfo = processStartInfo;
            process.Start();

            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();

            process.WaitForExit();

            if (process.ExitCode != 0)
            {
                Logger.WriteLine($"[RUNNER] -> RunCommand(): Error ~ {error}");
                return false;
            }

            return true;
        }
    }
}
