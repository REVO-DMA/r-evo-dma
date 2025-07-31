using Tarkov_DMA_Backend.MemDMA.EFT;
using Tarkov_DMA_Backend.MemDMA.ScatterAPI;
using Tarkov_DMA_Backend.Misc;
using Tarkov_DMA_Backend.Unity;

namespace Tarkov_DMA_Backend.Tarkov.Toolkit.Features
{
    public class Chams : Feature
    {
        private const string thisID = "chams";

        public static readonly object _chamsLock = new object();

        public static bool CanRun = true;

        private int _currentMode = 0;

        public Chams(int delayMs) : base(delayMs, thisID) { }

        public override void Run(ref List<ScatterWriteEntry> writes, Player localPlayer, params object[] parameters)
        {
            if (!ToolkitManager.FeatureState[thisID])
                return;

            if (!CameraManager.PlayerIsInRaid)
                return;

            bool lockTaken = false;

            try
            {
                lockTaken = Monitor.TryEnter(_chamsLock, TimeSpan.Zero);
                if (!lockTaken)
                    return;

                if (!EFTDMA.InitChams())
                    return;

                int chamsMode = ToolkitManager.FeatureSettings_int["chams_Mode"];

                // Force wall chams if vis check is not loaded
                if (chamsMode == 3 && ChamsManager.ChamsStatus != ChamsManager.ChamsLoadStatus.FullyLoaded)
                    chamsMode = 1;

                Player[] Players = EFTDMA.DisplayPlayers;

                if (Players == null) return;

                // Mark chams as unset so everyone gets reapplied on mode change
                if (_currentMode != chamsMode)
                {
                    foreach (Player player in Players)
                    {
                        if (player == null) continue;

                        player.ChamsSet = false;
                    }
                }

                int? chamsMaterialID = null;

                if (chamsMode == 1) // Wall Chams
                {
                    ulong NightVision = CameraManager.GetNightVision();
                    if (NightVision == 0x0)
                        throw new ValueOutOfRangeException("NightVision");

                    ulong thermalVisionMaterial = Memory.ReadPtr(NightVision + Offsets.ThermalVision.Material);
                    ulong thermalVisionMaterialInstance = Memory.ReadPtr(thermalVisionMaterial + UnityOffsets.Material.Instance);
                    chamsMaterialID = Memory.ReadValue<int>(thermalVisionMaterialInstance + UnityOffsets.UnityObject.InstanceID);
                }
                else if (chamsMode == 2) // Thermal Chams
                {
                    ulong SSAA = CameraManager.GetSSAA();
                    if (SSAA == 0x0)
                        throw new ValueOutOfRangeException("SSAA");

                    ulong ssaaMaterial = Memory.ReadPtr(SSAA + Offsets.SSAA.OpticMaskMaterial);
                    ulong ssaaMaterialInstance = Memory.ReadPtr(ssaaMaterial + UnityOffsets.Material.Instance);
                    chamsMaterialID = Memory.ReadValue<int>(ssaaMaterialInstance + UnityOffsets.UnityObject.InstanceID);
                }
                else if (chamsMode == 3) // Vis Check Chams
                {
                    chamsMaterialID = 999;
                }

                if (chamsMaterialID == null || chamsMaterialID == 0)
                    throw new ValueOutOfRangeException("chamsMaterialID");

                foreach (Player player in Players)
                {
                    if (player == null || player.ChamsSet) continue;

                    // Dynamically fetch the correct cham for this player type
                    if (chamsMode == 3)
                    {
                        var tmpMaterial = ChamsManager.GetMaterialForPlayerType(player.Type);
                        ChamsMaterial material;
                        if (tmpMaterial != null)
                        {
                            material = (ChamsMaterial)tmpMaterial;
                            chamsMaterialID = material.InstanceID;
                        }
                    }

                    player.SetChams(ref writes, (int)chamsMaterialID);
                }

                // Update internal mode
                _currentMode = chamsMode;
            }
            finally
            {
                if (lockTaken)
                    Monitor.Exit(_chamsLock);
            }
        }
    }
}
