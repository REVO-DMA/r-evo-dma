namespace arena_dma_backend.Ballistics
{
    public readonly struct G1DragModel(float mach, float ballist)
    {
        public readonly float Mach = mach;
        public readonly float Ballist = ballist;
    }
}
