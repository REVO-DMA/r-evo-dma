namespace arena_dma_backend.Ballistics
{
    public readonly struct TrajectoryDataPoint(int index, float time, Vector3 position, Vector3 velocity)
    {
        public readonly int Index = index;
        public readonly float Time = time;
        public readonly Vector3 Position = position;
        public readonly Vector3 Velocity = velocity;
    }
}
