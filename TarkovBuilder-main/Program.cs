using System.Diagnostics;

namespace TarkovBuilder
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 3)
            {
                Console.WriteLine("Invalid args! Usage:");
                Console.WriteLine("\t [1] .sln target file");
                Console.WriteLine("\t [2] configuration name");
                Console.WriteLine("\t [3] protect [y/n]");
                Console.WriteLine("\nExample: TarkovBuilder.exe \"C:\\Users\\microPower\\Documents\\GitHub\\Tarkov-DMA-Backend\\Tarkov DMA Backend.sln\" Commercial y");
                return;
            }
            else if (!File.Exists(args[0]))
            {
                Console.WriteLine("Invalid .sln target file!");
                return;
            }
            else if (string.IsNullOrEmpty(args[1]))
            {
                Console.WriteLine("Invalid configuration name!");
                return;
            }
            else if (args[2].ToLower() != "y" && args[2].ToLower() != "n")
            {
                Console.WriteLine("Protection preference!");
                return;
            }

            bool protect = false;
            if (args[2].ToLower() == "y")
                protect = true;

            string outputDirectory = Path.GetDirectoryName(args[0]);
            if (outputDirectory == null)
            {
                Console.WriteLine("[ERROR] Failed to get the output directory!");
                return;
            }

            string publishDirectory = Path.Combine(outputDirectory, $"Publish_{args[1]}");
            // Clean up
            if (Directory.Exists(publishDirectory)) Directory.Delete(publishDirectory, true);

            // Protobuf stuff
            {
                Directory.CreateDirectory(Path.Combine(outputDirectory, "ProtoCompiled"));

                string protoEXE = Path.Combine(outputDirectory, "build/protoc.exe");
                string protoFilesDir = Path.Combine(outputDirectory, "Proto");

                RunProgram(outputDirectory, protoEXE, $"-I={protoFilesDir} --csharp_out={outputDirectory}\\ProtoCompiled {protoFilesDir}\\*.proto");
            }

            RunProgram(@"C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin", "MSBuild.exe", $"\"{args[0]}\" -r -t:Publish -p:Configuration=\"{args[1]}\" -p:Platform=x64 -p:PublishDir=\"Publish_{args[1]}\" -p:GenerateMapFile=true");

            File.Copy(Path.Combine(outputDirectory, "bin/Shellcode/Shellcode.dll"), Path.Combine(outputDirectory, $"Publish_{args[1]}/Shellcode.dll"), true);
            File.Copy(Path.Combine(outputDirectory, "bin/Shellcode/Shellcode.pdb"), Path.Combine(outputDirectory, $"Publish_{args[1]}/Shellcode.pdb"), true);

            if (protect)
            {
                File.Copy(Path.Combine(outputDirectory, $"Publish_{args[1]}Tarkov DMA Backend.map"), Path.Combine(outputDirectory, $"Publish_{args[1]}/Tarkov_DMA_Backend.cv.map"), true);
                // Clean up
                File.Delete(Path.Combine(outputDirectory, $"Publish_{args[1]}Tarkov DMA Backend.map"));

                File.Copy(Path.Combine(outputDirectory, "build/Tarkov DMA Backend.cv"), Path.Combine(outputDirectory, $"Publish_{args[1]}/Tarkov DMA Backend.cv"), true);
                File.Copy(Path.Combine(outputDirectory, "build/Tarkov DMA Backend.vmp"), Path.Combine(outputDirectory, $"Publish_{args[1]}/Tarkov DMA Backend.vmp"), true);

                RunProgram(publishDirectory, @"C:\Program Files\Code Virtualizer\Virtualizer.exe", $"/protect \"{Path.Combine(outputDirectory, $"Publish_{args[1]}/Tarkov DMA Backend.cv")}\"");
                RunProgram(publishDirectory, @"C:\Program Files\VMProtect Ultimate\VMProtect_Con.exe", $"\"{Path.Combine(outputDirectory, $"Publish_{args[1]}/Tarkov_DMA_Backend.cv.exe")}\" -pf \"{Path.Combine(outputDirectory, $"Publish_{args[1]}/Tarkov DMA Backend.vmp")}\"");
            }
        }

        private static void RunProgram(string cwd, string exeName, string args)
        {
            using Process process = new();

            process.StartInfo.FileName = Path.Combine(cwd, exeName);
            process.StartInfo.WorkingDirectory = cwd;
            process.StartInfo.Arguments = args;

            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;

            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;

            process.StartInfo.StandardOutputEncoding = System.Text.Encoding.UTF8;
            process.StartInfo.StandardErrorEncoding = System.Text.Encoding.UTF8;

            process.OutputDataReceived += (sender, args) => {
                if (!string.IsNullOrEmpty(args.Data))
                    Console.WriteLine(args.Data);
            };

            process.ErrorDataReceived += (sender, args) => {
                if (!string.IsNullOrEmpty(args.Data))
                    Console.WriteLine(args.Data);
            };

            process.Start();

            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            process.WaitForExit();
        }
    }
}
