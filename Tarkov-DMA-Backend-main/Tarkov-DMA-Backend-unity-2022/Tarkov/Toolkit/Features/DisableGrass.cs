using Tarkov_DMA_Backend.MemDMA.EFT;
using Tarkov_DMA_Backend.MemDMA.ScatterAPI;
using Tarkov_DMA_Backend.Unity.Collections;
using Tarkov_DMA_Backend.Unity.LowLevel;

namespace Tarkov_DMA_Backend.Tarkov.Toolkit.Features
{
    public class DisableGrass : Feature
    {
        private const string thisID = "disableGrass";

        private readonly struct Bounds(Vector3 p, Vector3 e)
        {
            public readonly Vector3 P = p;
            public readonly Vector3 E = e;
        }

        private static readonly Bounds HiddenBounds = new(new(0f, 0f, 0f), new(0f, 0f, 0f));
        private static readonly Bounds ShownBounds = new(new(0.5f, 0.5f, 0.5f), new(0.5f, 0.5f, 0.5f));

        public DisableGrass(int delayMs) : base(delayMs, thisID) { }

        public override void Run(ref List<ScatterWriteEntry> writes, Player localPlayer, params object[] parameters)
        {
            bool newState = ToolkitManager.FeatureState[thisID];
            if (newState == CurrentState) return;

            ulong activeManagersListAddr = Memory.ReadPtr(EFTDMA.MonoClasses.GetStaticClass("GPUInstancer.GPUInstancerManager"));

            if (newState)
            {
                MemList<ulong> activeManagersList = new(activeManagersListAddr);
                foreach (var manager in activeManagersList.Items)
                {
                    ulong runtimeDataListAddr = Memory.ReadPtr(manager + Offsets.GPUInstancerManager.runtimeDataList);
                    MemList<ulong> runtimeDataList = new(runtimeDataListAddr);
                    foreach (var data in runtimeDataList.Items)
                        writes.Add(ScatterWriteEntry.Create(data + Offsets.RuntimeDataList.instanceBounds, HiddenBounds));
                }

                CurrentState = true;
            }
            else
            {
                MemList<ulong> activeManagersList = new(activeManagersListAddr);
                foreach (var manager in activeManagersList.Items)
                {
                    ulong runtimeDataListAddr = Memory.ReadPtr(manager + Offsets.GPUInstancerManager.runtimeDataList);
                    MemList<ulong> runtimeDataList = new(runtimeDataListAddr);
                    foreach (var data in runtimeDataList.Items)
                        writes.Add(ScatterWriteEntry.Create(data + Offsets.RuntimeDataList.instanceBounds, ShownBounds));
                }

                CurrentState = false;
            }
        }
    }
}
