using Tarkov_DMA_Backend.MemDMA.ScatterAPI;
using Tarkov_DMA_Backend.UI;

namespace Tarkov_DMA_Backend.Tarkov.Toolkit.Features
{
    public class AlwaysDay : Feature
    {
        private const string thisID = "alwaysDay";

        public AlwaysDay(int delayMs) : base(delayMs, thisID) { }

        public override void Run(ref List<ScatterWriteEntry> writes, Player localPlayer, params object[] parameters)
        {
            bool newState = ToolkitManager.FeatureState[thisID];
            if (newState == CurrentState && RunImmediately == false) return;

            static (ulong, ulong) GetTOD_Time()
            {
                ulong TOD_Scattering = CameraManager.GetTOD_Scattering();
                ulong TOD_Sky = Memory.ReadPtr(TOD_Scattering + Offsets.TOD_Scattering.sky);
                ulong TOD_Cycle = Memory.ReadPtr(TOD_Sky + Offsets.TOD_Sky.Cycle);
                ulong TOD_Components = Memory.ReadPtr(TOD_Sky + Offsets.TOD_Sky.TOD_Components);
                ulong TOD_Time = Memory.ReadPtr(TOD_Components + Offsets.TOD_Components.TOD_Time);

                return (TOD_Cycle, TOD_Time);
            }

            string currentMap = UICache.CurrentMap;
            if (currentMap == string.Empty || currentMap == "laboratory" || currentMap == "factory")
                return;

            if (newState)
            {
                var TOD_Time = GetTOD_Time();

                // Lock time
                writes.Add(ScatterWriteEntry.Create(TOD_Time.Item2 + Offsets.TOD_Time.LockCurrentTime, true));

                // Set hour
                writes.Add(ScatterWriteEntry.Create(TOD_Time.Item1 + Offsets.TOD_CycleParameters.Hour, ToolkitManager.FeatureSettings_float["alwaysDay_Hour"]));

                CurrentState = true;
            }
            else
            {
                // Unlock time
                writes.Add(ScatterWriteEntry.Create(GetTOD_Time().Item2 + Offsets.TOD_Time.LockCurrentTime, false));

                CurrentState = false;
            }
        }
    }
}
