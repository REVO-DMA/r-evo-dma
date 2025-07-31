namespace EnigmaRunner
{
    public static class Processor
    {
        public const int ExpectedParamCount = 6;

        public enum ParamIndexes
        {
            enigma64,
            enigmaProject,
            input,
            output,
            hwid,
            cmdLine
        }

        public static void Run(string[] args)
        {
            string enigma64 = args[(int)ParamIndexes.enigma64];
            string enigmaProject = args[(int)ParamIndexes.enigmaProject];
            string inputEXE = args[(int)ParamIndexes.input];
            string outputEXE = args[(int)ParamIndexes.output];
            string hwid = args[(int)ParamIndexes.hwid];
            string commandLine = args[(int)ParamIndexes.cmdLine];

            if (!File.Exists(enigma64))
            {
                Console.Error.WriteLine("[!] Enigma EXE does not exist!");
                goto fail;
            }

            if (!File.Exists(enigmaProject))
            {
                Console.Error.WriteLine("[!] Enigma project file does not exist!");
                goto fail;
            }

            if (!File.Exists(inputEXE))
            {
                Console.Error.WriteLine("[!] Input EXE file does not exist!");
                goto fail;
            }

            EnigmaProjectHelper eph = new(enigmaProject);

            // Copy input exe to a temp locaton and work off that copy
            byte[] inputExeData = File.ReadAllBytes(inputEXE);
            inputEXE = eph.GetTempFile(Path.GetExtension(inputEXE));
            File.WriteAllBytes(inputEXE, inputExeData);

            if (!eph.PatchTarget("/HWID_CHANGEME", hwid) ||
                !eph.PatchTarget("/CMD_CHANGEME", commandLine) ||
                !eph.PatchTarget("/INPUT_CHANGEME", inputEXE) ||
                !eph.PatchTarget("/OUTPUT_CHANGEME", outputEXE))
            {
                Console.Error.WriteLine("[!] Unable to fully patch enigma project file!");
                goto fail;
            }

            // Save patched file to temp dir
            enigmaProject = eph.Save();

            Console.WriteLine("[i] Protecting program...");

            string phCommand = $"protect \"{enigmaProject}\"";
            ProcessHelper ph = new(enigma64, phCommand, Path.GetDirectoryName(enigma64));

            if (!ph.Run())
                goto fail;
            else
            {
                Console.WriteLine("[i] Program protected successfully!");
                return;
            }

        fail:
            Console.Error.WriteLine("[!] Failed to protect program!\n");

            Console.Error.WriteLine("[i] Runtime args dump:");
            Console.Error.WriteLine($"\tenigma64.exe        -> \"{enigma64}\"");
            Console.Error.WriteLine($"\tEnigma Project      -> \"{enigmaProject}\"");
            Console.Error.WriteLine($"\tInput EXE           -> \"{inputEXE}\"");
            Console.Error.WriteLine($"\tOutput EXE          -> \"{outputEXE}\"");
            Console.Error.WriteLine($"\tHWID                -> \"{hwid}\"");
            Console.Error.WriteLine($"\tCommand Line        -> \"{commandLine}\"");

            Environment.Exit(1);
        }
    }
}
