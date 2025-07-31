using Tarkov_DMA_Backend.MemDMA.EFT;
using Tarkov_DMA_Backend.MemDMA.ScatterAPI;

namespace Tarkov_DMA_Backend.Tarkov.Toolkit.Features
{
    public class NoHeadBobbing : Feature
    {
        private const string thisID = "noHeadBobbing";

        public NoHeadBobbing(int delayMs) : base(delayMs, thisID) { }

        public override void Run(ref List<ScatterWriteEntry> writes, Player localPlayer, params object[] parameters)
        {
            bool newState = ToolkitManager.FeatureState[thisID];

            static ulong GetSettingValueClass()
            {
                ulong GameSettings = Memory.ReadPtr(EFTDMA.MonoClasses.GetSingleton(ClassNames.GameSettings.ClassName_ClassToken));

                ulong Game = Memory.ReadPtr(GameSettings + Offsets.GameSettingsContainer.Game);
                ulong Settings = Memory.ReadPtr(Game + Offsets.GameSettingsInnerContainer.Settings);
                ulong HeadBobbing = Memory.ReadPtr(Settings + Offsets.GameSettings.HeadBobbing);
                ulong SettingValueClass = Memory.ReadPtr(HeadBobbing + Offsets.BSGGameSetting.ValueClass);

                return SettingValueClass;
            }

            if (newState)
            {
                writes.Add(ScatterWriteEntry.Create(GetSettingValueClass() + Offsets.BSGGameSettingValueClass.Value, 0f));

                CurrentState = true;
            }
            else
            {
                writes.Add(ScatterWriteEntry.Create(GetSettingValueClass() + Offsets.BSGGameSettingValueClass.Value, 0.2f));

                CurrentState = false;
            }
        }
    }
}
