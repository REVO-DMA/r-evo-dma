using Spectre.Console;
using System.Diagnostics;

namespace TarkovDumper
{
    internal class Program
    {
        private const string DefaultSDKPath = @"EFTOffsets.cs";

#if DEBUG
        private const string DefaultAssemblyPath = @"Z:\Assembly-CSharp.dll";
        private const string DefaultDumpPath = @"C:\Users\microPower\Documents\GitHub\UnispectEx\Unispect\bin\Release\dump.txt";
#else
        //private const string DefaultAssemblyPath = @"C:\Battlestate Games\Escape from Tarkov\EscapeFromTarkov_Data\Managed\Assembly-CSharp.dll";
        //private const string DefaultDumpPath = null;
        private const string DefaultAssemblyPath = @"Z:\Assembly-CSharp.dll";
        private const string DefaultDumpPath = @"C:\Users\microPower\Documents\GitHub\UnispectEx\Unispect\bin\Release\dump.txt";
#endif

        static void Main(string[] args)
        {
            StructureGenerator structGenerator_gameData = new("GameData");
            StructureGenerator structGenerator_classNames = new("ClassNames");
            StructureGenerator structGenerator_offsets = new("Offsets");
            StructureGenerator structGenerator_enums = new("Enums");

            AnsiConsole.Profile.Width = 420;

            string assemblyPath;
            string dumpPath;
            string sdkOutputPath;
            if (args.Length == 2)
            {
                assemblyPath = args[0];
                dumpPath = args[1];
                sdkOutputPath = args[2];
            }
            else
            {
                assemblyPath = AnsiConsole.Prompt(
                    new TextPrompt<string>("Assembly-CSharp.dll Path")
                        .PromptStyle("green")
                        .DefaultValue(DefaultAssemblyPath)
                        .ValidationErrorMessage("[red]That's not a valid file path[/]")
                        .Validate(path =>
                        {
                            if (Path.Exists(path))
                                return ValidationResult.Success();
                            else
                                return ValidationResult.Error("[red]You must enter a valid file path[/]");
                        })
                    );

                TextPrompt<string> dumpPathPrompt = new TextPrompt<string>("UnispectEx Dump Path")
                        .PromptStyle("green")
                        .ValidationErrorMessage("[red]That's not a valid file path[/]")
                        .Validate(path =>
                        {
                            if (Path.Exists(path))
                                return ValidationResult.Success();
                            else
                                return ValidationResult.Error("[red]You must enter a valid file path[/]");
                        });
                if (DefaultDumpPath != null)
                    dumpPathPrompt = dumpPathPrompt.DefaultValue(DefaultDumpPath);

                dumpPath = AnsiConsole.Prompt(dumpPathPrompt);

                sdkOutputPath = AnsiConsole.Prompt(
                new TextPrompt<string>("SDK Output Path")
                    .PromptStyle("green")
                    .DefaultValue(DefaultSDKPath)
                    .ValidationErrorMessage("[red]That's not a valid file path[/]")
                    .Validate(path =>
                    {
                        string fileName = Path.GetFileName(path);

                        if (fileName == null || fileName == string.Empty)
                            return ValidationResult.Error("[red]You must enter a valid file path that includes the file name[/]");

                        if (Path.Exists(Path.GetDirectoryName(path)))
                            return ValidationResult.Success();
                        else
                            return ValidationResult.Error("[red]You must enter the name of an already existing directory[/]");
                    })
                );
            }

            AnsiConsole.Status().Start("Starting...", ctx => {
                ctx.Spinner(Spinner.Known.Dots);
                ctx.SpinnerStyle(Style.Parse("green"));

                AnsiConsole.WriteLine();
                AnsiConsole.MarkupLine("Processing entries...");

                Processor processor = null;

                try
                {
                    processor = new(assemblyPath, dumpPath);

                    processor.ProcessGameData(ctx, structGenerator_gameData);
                    processor.ProcessClassNames(ctx, structGenerator_classNames);
                    processor.ProcessOffsets(ctx, structGenerator_offsets);
                    processor.ProcessEnums(ctx, structGenerator_enums);

                    AnsiConsole.Clear();

                    List<StructureGenerator> sgList = new()
                    {
                        structGenerator_gameData,
                        structGenerator_classNames,
                        structGenerator_offsets,
                        structGenerator_enums,
                    };
                    AnsiConsole.WriteLine(StructureGenerator.GenerateNamespace("SDK", sgList));
                    AnsiConsole.WriteLine(StructureGenerator.GenerateReports(sgList));

                    string plainSDK = StructureGenerator.GenerateNamespace("SDK", sgList, false);
                    File.WriteAllText(sdkOutputPath, plainSDK);

                    // Show it in explorer
                    Process.Start("explorer.exe", $"/select,\"{sdkOutputPath}\"");
                }
                catch (Exception ex)
                {
                    AnsiConsole.WriteLine();

                    if (processor != null)
                        AnsiConsole.MarkupLine($"[bold yellow]Exception thrown while processing step -> {processor.LastStepName}[/]");

                    AnsiConsole.MarkupLine($"[red]{ex.Message}[/]");
                    if (ex.StackTrace != null)
                    {
                        AnsiConsole.MarkupLine("[bold yellow]==========================Begin Stack Trace==========================[/]");
                        AnsiConsole.WriteLine(ex.StackTrace);
                        AnsiConsole.MarkupLine("[bold yellow]===========================End Stack Trace===========================[/]");
                    }
                }
            });

            AnsiConsole.WriteLine();
            AnsiConsole.WriteLine("Press the space bar to exit...");
            Console.ReadKey();
        }
    }
}
