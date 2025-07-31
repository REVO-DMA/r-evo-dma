using Tarkov_DMA_Backend.IPC;
using Tarkov_DMA_Backend.MemDMA.EFT;
using Tarkov_DMA_Backend.MemDMA.ScatterAPI;
using Tarkov_DMA_Backend.UI;

namespace Tarkov_DMA_Backend.Tarkov.Toolkit.AlwaysActiveFeatures
{
    public class RamCleanerMonitor : AlwaysActiveFeature
    {
        private const string thisID = "ramCleanerMonitor";

        public RamCleanerMonitor(int delayMs) : base(delayMs, thisID) { }

        public override void Run(ref List<ScatterWriteEntry> writes, params object[] parameters)
        {
            ulong GameSettings = Memory.ReadPtr(EFTDMA.MonoClasses.GetSingleton(ClassNames.GameSettings.ClassName_ClassToken));

            ulong Game = Memory.ReadPtr(GameSettings + Offsets.GameSettingsContainer.Game);
            ulong Settings = Memory.ReadPtr(Game + Offsets.GameSettingsInnerContainer.Settings);
            ulong AutoEmptyWorkingSet = Memory.ReadPtr(Settings + Offsets.GameSettings.AutoEmptyWorkingSet);
            ulong SettingValueClass = Memory.ReadPtr(AutoEmptyWorkingSet + Offsets.BSGGameSetting.ValueClass);

            var newState = Memory.ReadValue<bool>(SettingValueClass + Offsets.BSGGameSettingValueClass.Value);

            if (newState && CurrentState != newState)
            {
                // Enabled

                Server.SendRadarStatus(Constants.RadarStatuses.DisableAutoRamCleaner);

                CurrentState = true;
            }
            else if (!newState) // Disabled
                CurrentState = false;
        }
    }
}
