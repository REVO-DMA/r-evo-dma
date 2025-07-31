namespace Tarkov_DMA_Backend.Tarkov.Ballistics
{
    public static class G1
    {
        private static readonly G1DragModel[] G1Coefficents = new G1DragModel[]
        {
            new(0f, 0.2629f),
            new(0.05f, 0.2558f),
            new(0.1f, 0.2487f),
            new(0.15f, 0.2413f),
            new(0.2f, 0.2344f),
            new(0.25f, 0.2278f),
            new(0.3f, 0.2214f),
            new(0.35f, 0.2155f),
            new(0.4f, 0.2104f),
            new(0.45f, 0.2061f),
            new(0.5f, 0.2032f),
            new(0.55f, 0.202f),
            new(0.6f, 0.2034f),
            new(0.7f, 0.2165f),
            new(0.725f, 0.223f),
            new(0.75f, 0.2313f),
            new(0.775f, 0.2417f),
            new(0.8f, 0.2546f),
            new(0.825f, 0.2706f),
            new(0.85f, 0.2901f),
            new(0.875f, 0.3136f),
            new(0.9f, 0.3415f),
            new(0.925f, 0.3734f),
            new(0.95f, 0.4084f),
            new(0.975f, 0.4448f),
            new(1f, 0.4805f),
            new(1.025f, 0.5136f),
            new(1.05f, 0.5427f),
            new(1.075f, 0.5677f),
            new(1.1f, 0.5883f),
            new(1.125f, 0.6053f),
            new(1.15f, 0.6191f),
            new(1.2f, 0.6393f),
            new(1.25f, 0.6518f),
            new(1.3f, 0.6589f),
            new(1.35f, 0.6621f),
            new(1.4f, 0.6625f),
            new(1.45f, 0.6607f),
            new(1.5f, 0.6573f),
            new(1.55f, 0.6528f),
            new(1.6f, 0.6474f),
            new(1.65f, 0.6413f),
            new(1.7f, 0.6347f),
            new(1.75f, 0.628f),
            new(1.8f, 0.621f),
            new(1.85f, 0.6141f),
            new(1.9f, 0.6072f),
            new(1.95f, 0.6003f),
            new(2f, 0.5934f),
            new(2.05f, 0.5867f),
            new(2.1f, 0.5804f),
            new(2.15f, 0.5743f),
            new(2.2f, 0.5685f),
            new(2.25f, 0.563f),
            new(2.3f, 0.5577f),
            new(2.35f, 0.5527f),
            new(2.4f, 0.5481f),
            new(2.45f, 0.5438f),
            new(2.5f, 0.5397f),
            new(2.6f, 0.5325f),
            new(2.7f, 0.5264f),
            new(2.8f, 0.5211f),
            new(2.9f, 0.5168f),
            new(3f, 0.5133f),
            new(3.1f, 0.5105f),
            new(3.2f, 0.5084f),
            new(3.3f, 0.5067f),
            new(3.4f, 0.5054f),
            new(3.5f, 0.504f),
            new(3.6f, 0.503f),
            new(3.7f, 0.5022f),
            new(3.8f, 0.5016f),
            new(3.9f, 0.501f),
            new(4f, 0.5006f),
            new(4.2f, 0.4998f),
            new(4.4f, 0.4995f),
            new(4.6f, 0.4992f),
            new(4.8f, 0.499f),
            new(5f, 0.4988f)
        };

        public static float CalculateDragCoefficient(float velocity)
        {
            int num = (int)MathF.Floor(velocity / 343f / 0.05f);

            if (num <= 0) return 0f;

            if (num <= G1Coefficents.Length - 1)
            {
                var nm1 = G1Coefficents[num - 1];
                var n = G1Coefficents[num];

                float num2 = nm1.Mach * 343f;
                float num3 = n.Mach * 343f;
                float ballist = nm1.Ballist;

                return (n.Ballist - ballist) / (num3 - num2) * (velocity - num2) + ballist;
            }

            return G1Coefficents[G1Coefficents.Length - 1].Ballist;
        }
    }
}