using System.Diagnostics;

namespace UpdateHelper
{
    internal static class Program
    {
        private static readonly List<string> SillyMessages = new()
        {
            "Loading the flux capacitor...",
            "Optimizing the bit-shifting matrix...",
            "Compiling bytecode with extra sarcasm...",
            "Re-calibrating the warp drive...",
            "Aligning the phase variance modulator...",
            "Reversing the polarity of the neutron flow...",
            "Boosting signal-to-noise ratio by 42%...",
            "Shuffling the priorities of the task list...",
            "Double-checking Schrodinger's cat...",
            "Adding more RAM to the coffee maker...",
            "Hiding jelly beans in the buffer...",
            "Defragmenting the space-time continuum...",
            "Tuning the flux capacitor with a monkey wrench...",
            "Overclocking the quantum processor...",
            "Disarming the self-destruct sequence... maybe...",
            "Initializing the neural net with positive vibes...",
            "Calculating the meaning of life, the universe, and everything...",
            "Filing bug reports with the intergalactic council...",
            "Counting all the zeroes and ones...",
            "Upgrading the sarcasm module...",
            "Decompressing the reality distortion field...",
            "Inverting the polarity of the bitstream...",
            "Synchronizing with parallel dimensions...",
            "Patching the time dilation bug...",
            "Mapping the uncharted sectors of the codebase...",
        };

        static void Main(string[] args)
        {
            try
            {
                List<string> messages = new();

                while (true)
                {
                    if (EvoRunning())
                    {
                        if (messages.Count == 0)
                        {
                            messages = SillyMessages.ToList();
                            Shuffle(messages);
                        }

                        Console.WriteLine(messages[0]);
                        messages.RemoveAt(0);
                    }
                    else
                    {
                        string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                        string evoEXE = Path.Combine(localAppData, @"Programs\eft-dma-radar-ui\EVO DMA Tarkov.exe");

                        StartEVO(evoEXE);
                        break;
                    }

                    Thread.Sleep(2000);
                }
            }
            catch { }
        }

        private static bool EvoRunning()
        {
            var processNames = Process.GetProcesses().Select(x => x.ProcessName);

            const string searchString = "EVO DMA Tarkov";
            const StringComparison sc = StringComparison.OrdinalIgnoreCase;
            bool isProcessRunning = processNames.Any(p => p.Contains(searchString, sc));

            return isProcessRunning;
        }

        private static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = Random.Shared.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        private static void StartEVO(string exeLocation)
        {
            ProcessStartInfo startInfo = new()
            {
                FileName = exeLocation,
                UseShellExecute = true,
                CreateNoWindow = true,
            };

            Process process = new()
            {
                StartInfo = startInfo
            };

            process.Start();
            process.Dispose();
        }
    }
}
