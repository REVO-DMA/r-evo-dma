global using static TerraFX.Interop.Windows.Windows;
using cs2_dma_esp;
using Kokuban;
using Private_Overlay.Misc;
using Private_Overlay.Rendering;
using Private_Overlay.SharedMemory;
using SkiaSharp;
using System.Security.Principal;
using TerraFX.Interop.Windows;

namespace Private_Overlay
{
    internal class Program
    {
        private static readonly string startSLOBS = stackalloc char[] { '[', 'i', ']', ' ', 'P', 'l', 'e', 'a', 's', 'e', ' ', 's', 't', 'a', 'r', 't', ' ', 'S', 'L', 'O', 'B', 'S', ' ', 'a', 'n', 'd', ' ', 's', 'h', 'o', 'w', ' ', 't', 'h', 'e', ' ', 'o', 'v', 'e', 'r', 'l', 'a', 'y', ' ', 'v', 'i', 'a', ' ', 'y', 'o', 'u', 'r', ' ', 'h', 'o', 't', 'k', 'e', 'y', '.' }.Encrypt();
        
        private static readonly string setupInstructions = stackalloc char[] { 'S', 'e', 't', 'u', 'p', ' ', 'i', 'n', 's', 't', 'r', 'u', 'c', 't', 'i', 'o', 'n', 's', ':' }.Encrypt();
        private static readonly string step1 = stackalloc char[] { '1', '.', ' ', 'D', 'o', 'w', 'n', 'l', 'o', 'a', 'd', ' ', 'a', 'n', 'd', ' ',  'i', 'n', 's', 't', 'a', 'l', 'l', ' ', 'S', 'L', 'O', 'B', 'S', ' ', 'f', 'r', 'o', 'm', ':', ' ', 'h', 't', 't', 'p', 's', ':', '/', '/', 's', 't', 'r', 'e', 'a', 'm', 'l', 'a', 'b', 's', '.', 'c', 'o', 'm', '/', 's', 't', 'r', 'e', 'a', 'm', 'l', 'a', 'b', 's', '-', 'l', 'i', 'v', 'e', '-', 's', 't', 'r', 'e', 'a', 'm', 'i', 'n', 'g', '-', 's', 'o', 'f', 't', 'w', 'a', 'r', 'e' }.Encrypt();
        private static readonly string step2 = stackalloc char[] { '2', '.', ' ', 'S', 't', 'a', 'r', 't', ' ', 'S', 'L', 'O', 'B', 'S', ' ', 'a', 'n', 'd', ' ', 'c', 'l', 'i', 'c', 'k', ' ', 't', 'h', 'e', ' ', 'g', 'e', 'a', 'r', ' ', 'i', 'c', 'o', 'n', ' ', 'l', 'o', 'c', 'a', 't', 'e', 'd', ' ', 'i', 'n', ' ', 't', 'h', 'e', ' ', 'b', 'o', 't', 't', 'o', 'm', ' ', 'l', 'e', 'f', 't', ' ', 'c', 'o', 'r', 'n', 'e', 'r', '.' }.Encrypt();
        private static readonly string step3 = stackalloc char[] { '3', '.', ' ', 'C', 'l', 'i', 'c', 'k', ' ', 't', 'h', 'e', ' ', 'l', 'o', 'g', 'i', 'n', ' ', 'b', 'u', 't', 't', 'o', 'n', ' ', 'l', 'o', 'c', 'a', 't', 'e', 'd', ' ', 'i', 'n', ' ', 't', 'h', 'e', ' ', 'b', 'o', 't', 't', 'o', 'm', ' ', 'l', 'e', 'f', 't', ' ', 'c', 'o', 'r', 'n', 'e', 'r', ' ', 'a', 'n', 'd', ' ', 's', 'i', 'g', 'n', ' ', 'i', 'n', '.' }.Encrypt();
        private static readonly string thisStepIsRequired = stackalloc char[] { 'T', 'h', 'i', 's', ' ', 's', 't', 'e', 'p', ' ', 'i', 's', ' ', 'r', 'e', 'q', 'u', 'i', 'r', 'e', 'd', '!' }.Encrypt();
        private static readonly string step4 = stackalloc char[] { '4', '.', ' ', 'C', 'l', 'i', 'c', 'k', ' ', 't', 'h', 'e', ' ', 'g', 'e', 'a', 'r', ' ', 'i', 'c', 'o', 'n', ' ', 'l', 'o', 'c', 'a', 't', 'e', 'd', ' ', 'i', 'n', ' ', 't', 'h', 'e', ' ', 'b', 'o', 't', 't', 'o', 'm', ' ', 'l', 'e', 'f', 't', ' ', 'c', 'o', 'r', 'n', 'e', 'r', ' ', 'a', 'n', 'd', ' ', 'g', 'o', ' ', 't', 'o', ' ', 't', 'h', 'e', ' ', '"', 'G', 'a', 'm', 'e', ' ', 'O', 'v', 'e', 'r', 'l', 'a', 'y', '"', ' ', 't', 'a', 'b', ' ', '(', 'i', 't', '\'', 's', ' ', 'n', 'e', 'a', 'r', ' ', 't', 'h', 'e', ' ', 'b', 'o', 't', 't', 'o', 'm', ')', '.' }.Encrypt();
        private static readonly string step5 = stackalloc char[] { '5', '.', ' ', 'C', 'l', 'i', 'c', 'k', ' ', '"', 'E', 'n', 'a', 'b', 'l', 'e', ' ', 'i', 'n', '-', 'g', 'a', 'm', 'e', ' ', 'o', 'v', 'e', 'r', 'l', 'a', 'y', '"', ' ', 'a', 'n', 'd', ' ', 'd', 'i', 's', 'a', 'b', 'l', 'e', ' ', 'a', 'l', 'l', ' ', 'o', 'p', 't', 'i', 'o', 'n', 's', ' ', 'e', 'x', 'c', 'e', 'p', 't', ' ', '"', 'S', 'h', 'o', 'w', ' ', 'R', 'e', 'c', 'e', 'n', 't', ' ', 'E', 'v', 'e', 'n', 't', 's', '"', '.' }.Encrypt();
        private static readonly string step6 = stackalloc char[] { '6', '.', ' ', 'G', 'o', ' ', 't', 'o', ' ', 't', 'h', 'e', ' ', '"', 'H', 'o', 't', 'k', 'e', 'y', 's', '"', ' ', 't', 'a', 'b', ' ', '(', 'i', 't', '\'', 's', ' ', 'n', 'e', 'a', 'r', ' ', 't', 'h', 'e', ' ', 'm', 'i', 'd', 'd', 'l', 'e', ')', ' ', 'a', 'n', 'd', ' ', 'r', 'e', 'b', 'i', 'n', 'd', ' ', 't', 'h', 'e', ' ', '"', 'T', 'o', 'g', 'g', 'l', 'e', ' ', 'i', 'n', '-', 'g', 'a', 'm', 'e', ' ', 'o', 'v', 'e', 'r', 'l', 'a', 'y', '"', ' ', 'h', 'o', 't', 'k', 'e', 'y', ' ', 't', 'o', ' ', 'a', ' ', 'b', 'u', 't', 't', 'o', 'n', ' ', 'o', 'f', ' ', 'y', 'o', 'u', 'r', ' ', 'c', 'h', 'o', 'i', 'c', 'e', '.', ' ', 'T', 'h', 'i', 's', ' ', 'w', 'i', 'l', 'l', ' ', 'b', 'e', ' ', 'u', 's', 'e', 'd', ' ', 't', 'o', 'g', 'g', 'l', 'e', ' ', 't', 'h', 'e', ' ', 'o', 'v', 'e', 'r', 'l', 'a', 'y', ' ', 'v', 'i', 's', 'i', 'b', 'i', 'l', 'i', 't', 'y', '.' }.Encrypt();
        private static readonly string step7 = stackalloc char[] { '7', '.', ' ', 'Y', 'o', 'u', '\'', 'r', 'e', ' ', 'd', 'o', 'n', 'e', ',', ' ', 'j', 'u', 's', 't', ' ', 'p', 'r', 'e', 's', 's', ' ', 'y', 'o', 'u', 'r', ' ', 'c', 'h', 'o', 's', 'e', 'n', ' ', 'h', 'o', 't', 'k', 'e', 'y', ' ', 't', 'o', ' ', 's', 'h', 'o', 'w', ' ', 't', 'h', 'e', ' ', 'o', 'v', 'e', 'r', 'l', 'a', 'y', '!' }.Encrypt();

        private static readonly string infoString = stackalloc char[] { 'I', 'n', 'f', 'o', ':' }.Encrypt();
        private static readonly string exitInstructions = stackalloc char[] { 'T', 'o', ' ', 'e', 'x', 'i', 't', ' ', 't', 'h', 'e', ' ', 'o', 'v', 'e', 'r', 'l', 'a', 'y', ' ', 'c', 'l', 'o', 's', 'e', ' ', 'S', 'L', 'O', 'B', 'S', ' ', 'a', 'n', 'd', ' ', 't', 'h', 'e', 'n', ' ', 'c', 'l', 'o', 's', 'e', ' ', 't', 'h', 'i', 's', ' ', 'c', 'o', 'n', 's', 'o', 'l', 'e', ' ', 'w', 'i', 'n', 'd', 'o', 'w', '.' }.Encrypt();

        private static readonly string slobsWindowFound = stackalloc char[] { 'S', 'L', 'O', 'B', 'S', ' ', 'w', 'i', 'n', 'd', 'o', 'w', ' ', 'f', 'o', 'u', 'n', 'd', ',', ' ', 's', 't', 'a', 'r', 't', 'i', 'n', 'g', ' ', 'o', 'v', 'e', 'r', 'l', 'a', 'y', '.', '.', '.' }.Encrypt();

        public static bool slobsInfoShown = false;

        static void Main(string[] args)
        {
            if (!IsRunAsAdmin())
            {
                Console.WriteLine("Please run this program as Admin. Press any key to exit...");
                Console.ReadKey();
                Environment.Exit(0);
            }

            try
            {
                InitTypes();

                Timing.MakePrecise();

                PoolManager.Run();

                static void printInfo()
                {
                    Console.WriteLine(Chalk.Red + startSLOBS.Decrypt());

                    ShowSetup();

                    Console.WriteLine();
                    Console.WriteLine(Chalk.Underline.White + infoString.Decrypt());
                    Console.WriteLine(Chalk.Green + exitInstructions.Decrypt());
                }

                while (true)
                {
                    HGLRC? hglrc = null;

                    try
                    {
                        if (!slobsInfoShown)
                        {
                            Console.Clear();
                            printInfo();
                            slobsInfoShown = true;
                        }

                        hglrc = SLOBS.Init();
                        if (hglrc != null)
                            Console.Clear();
                        else
                        {
                            Thread.Sleep(100);
                            continue;
                        }

                        Console.WriteLine(Chalk.Green + slobsWindowFound.Decrypt());
                        Thread.Sleep(2500);
                        Console.Clear();
                        slobsInfoShown = false;
                        ShowWindow(GetConsoleWindow(), 0); // SW_HIDE = 0

                        SLOBS.RenderESP();
                    }
                    catch { throw; }
                    finally
                    {
                        if (hglrc != null)
                            SLOBS.Cleanup((HGLRC)hglrc);

                        ShowWindow(GetConsoleWindow(), 1); // SW_NORMAL = 1
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("The application has crashed. Please report this crash!");
                Console.WriteLine("=============================Start Crash Log=============================\n\n");
                Console.WriteLine(ex);
                Console.WriteLine("\n\n==============================End Crash Log==============================");
                Console.WriteLine("\n\nPress any key to exit...");
                Console.ReadKey();
            }
        }

        public static void ShowSetup()
        {
            Console.WriteLine();
            Console.WriteLine(Chalk.Underline.White + setupInstructions.Decrypt());
            Console.WriteLine(Chalk.Green + step1.Decrypt());
            Console.WriteLine(Chalk.Green + step2.Decrypt());
            Console.Write(Chalk.Green + step3.Decrypt());
            Console.WriteLine(Chalk.Underline.Red + $" {thisStepIsRequired.Decrypt()}");
            Console.WriteLine(Chalk.Green + step4.Decrypt());
            Console.WriteLine(Chalk.Green + step5.Decrypt());
            Console.WriteLine(Chalk.Green + step6.Decrypt());
            Console.WriteLine(Chalk.Green + step7.Decrypt());
        }

        private static void InitTypes()
        {
            _ = new ESP_RenderPacket(Array.Empty<PlayerESP_Data>());
            var boneBounds = new BoneBounds(1, 1, 1, 1);
            _ = new PlayerESP_Data(Array.Empty<SKPoint>(), new(), boneBounds, "", false, 1);
        }

        private static bool IsRunAsAdmin()
        {
            WindowsIdentity id = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new(id);

            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }
    }
}