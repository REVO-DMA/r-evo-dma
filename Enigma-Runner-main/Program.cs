using static EnigmaRunner.Processor;

namespace EnigmaRunner
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != ExpectedParamCount)
            {
                Console.Error.WriteLine("[!] Invalid arg count!\n");

                Console.Error.WriteLine("Usage:");
                Console.Error.WriteLine($"\tArg {(int)ParamIndexes.enigma64} -> enigma64.exe");
                Console.Error.WriteLine($"\tArg {(int)ParamIndexes.enigmaProject} -> Enigma Project");
                Console.Error.WriteLine($"\tArg {(int)ParamIndexes.input} -> Input EXE");
                Console.Error.WriteLine($"\tArg {(int)ParamIndexes.output} -> Output EXE");
                Console.Error.WriteLine($"\tArg {(int)ParamIndexes.hwid} -> HWID");
                Console.Error.WriteLine($"\tArg {(int)ParamIndexes.cmdLine} -> Command Line");

                Environment.Exit(1);
            }

            Run(args);
        }
    }
}
