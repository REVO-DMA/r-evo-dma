using cs2_dma_esp.CS2;
using cs2_dma_esp.CS2.Features;
using cs2_dma_esp.ESP;

namespace cs2_dma_esp.MemDMA.CS2
{
    public sealed class DMA_Main : MemDMA
    {
        #region Fields/Properties/Constructor
        public volatile bool Ready = false;
        
        private readonly Thread _tPrimary;

        public new ulong ModuleBase => base.ModuleBase;

        public DMA_Main(bool isDebug) : base(isDebug)
        {
            _tPrimary = new Thread(MemoryPrimaryWorker)
            {
                IsBackground = true
            };

            _tPrimary.Start();
        }
        #endregion

        #region Primary Memory Thread

        /// <summary>
        /// Main worker thread to perform DMA Reads on.
        /// </summary>
        private void MemoryPrimaryWorker()
        {
            try
            {
                ESP_Manager.Start();
                //RCS.Start();

                Logger.WriteLine("Memory thread starting...");
                while (true)
                {
                    Logger.WriteLine("Searching for the CS2 Process...");

                    while (true)
                    {
                        uint[] pids = GetPidsForProcess("cs2.exe");
                        bool validPidFound = false;
                        foreach (uint pid in pids)
                        {
                            PID = pid;

                            try
                            {
                                base.ModuleBase = GetModuleBase("cs2.exe");
                                ClientDLL = GetModuleBase("client.dll");
                                Tier0DLL = GetModuleBase("tier0.dll");
                                InputSystemDLL = GetModuleBase("inputsystem.dll");

                                if (base.ModuleBase != 0x0 && ClientDLL != 0x0 && Tier0DLL != 0x0 && InputSystemDLL != 0x0)
                                {
                                    validPidFound = true;
                                    break;
                                }
                            }
                            catch { }
                        }

                        if (validPidFound)
                        {
                            Logger.WriteLine($"CS2 Startup [OK]");
                            Logger.WriteLine($"PID: {PID} \n cs2.exe: 0x{ModuleBase:X} \n client.dll: 0x{ClientDLL:X} \n tier0.dll 0x{Tier0DLL:X} \n inputsystem.dll 0x{InputSystemDLL:X}");

                            Game.Init(ClientDLL, Tier0DLL, InputSystemDLL);

                            break;
                        }
                        else
                        {
                            Logger.WriteLine("CS2 Startup [FAIL]");
                            Thread.Sleep(1000);
                        }
                    }

                    while (true)
                    {
                        try
                        {
                            if (Memory.ReadPtr(Game.GlobalVars, false) == 0x0)
                                break;
                        }
                        catch
                        {
                            break;
                        }

                        Game.UpdateData();
                        Game.UpdatePlayers();

                        Thread.Sleep(1000);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLine($"FATAL ERROR on Memory Thread: {ex}"); // Log fatal error
                throw; // State is corrupt, program will need to restart
            }
        }
        #endregion
    }
}
