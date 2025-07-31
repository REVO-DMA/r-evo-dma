namespace arena_dma_backend.Ballistics
{
    public readonly struct BallisticSimulationOutput(float dropCompensation, float travelTime)
    {
        public readonly float DropCompensation = dropCompensation;
        public readonly float TravelTime = travelTime;
    }
}
