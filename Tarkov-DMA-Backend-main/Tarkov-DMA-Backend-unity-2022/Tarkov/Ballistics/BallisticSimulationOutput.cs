namespace Tarkov_DMA_Backend.Tarkov.Ballistics
{
    public readonly struct BallisticSimulationOutput(float dropCompensation, float travelTime)
    {
        public readonly float DropCompensation = dropCompensation;
        public readonly float TravelTime = travelTime;
    }
}
