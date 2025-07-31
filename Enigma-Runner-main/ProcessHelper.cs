using System.Diagnostics;

namespace EnigmaRunner
{
    public class ProcessHelper
    {
        private const bool Debug = false;

        private readonly string _fileName;
        private readonly string _parameters;
        private readonly string _workingDirectory;

        private bool _runSuccess = false;

        public ProcessHelper(string fileName, string parameters, string workingDirectory = null)
        {
            _fileName = fileName;
            _parameters = parameters;
            _workingDirectory = workingDirectory;
        }

        private Process Start()
        {
            Process process = new();

            process.StartInfo.FileName = _fileName;
            process.StartInfo.Arguments = _parameters;
            if (_workingDirectory != null) process.StartInfo.WorkingDirectory = _workingDirectory;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;

            process.Start();

            process.BeginOutputReadLine();
            process.OutputDataReceived += (sender, args) =>
            {
                if (args.Data != null && args.Data.Length > 0)
                {
                    if (Debug) Console.WriteLine(args.Data.Trim());

                    if (args.Data.Contains("File successfully protected", StringComparison.OrdinalIgnoreCase))
                        _runSuccess = true;
                }
            };

            process.BeginErrorReadLine();
            process.ErrorDataReceived += (sender, args) =>
            {
                if (args.Data != null && args.Data.Length > 0)
                {
                    if (Debug) Console.WriteLine(args.Data.Trim());

                    if (args.Data.Contains("File successfully protected", StringComparison.OrdinalIgnoreCase))
                        _runSuccess = true;
                }
            };

            process.WaitForExit();

            return process;
        }

        public bool Run()
        {
            Start();

            return _runSuccess;
        }
    }
}
