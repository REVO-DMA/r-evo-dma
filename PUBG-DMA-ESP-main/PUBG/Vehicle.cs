namespace pubg_dma_esp.PUBG
{
    public class Vehicle
    {
        #region Fields

        public ulong UpdateIteration;

        public readonly ulong Base;
        public readonly ulong RootComponent;

        public readonly string Name;

        public Vector3 Position = Vector3.Zero;
        public int Distance;

        #endregion

        public Vehicle(ulong vehicleBase, ulong rootComponent, string name, ulong updateIteration)
        {
            UpdateIteration = updateIteration;

            Base = vehicleBase;
            RootComponent = rootComponent;

            Name = name;
        }

        public void SetPosition(Vector3 newPosition) => Position = newPosition;
    }
}
