namespace Tarkov_DMA_Backend.Tarkov.Ballistics
{
    public readonly struct G1DragModel(float mach, float ballist)
    {
        public readonly float Mach = mach;
        public readonly float Ballist = ballist;
    }
}
