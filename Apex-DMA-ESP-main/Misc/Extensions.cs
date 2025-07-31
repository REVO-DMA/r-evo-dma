namespace apex_dma_esp.Misc
{
    public static class Extensions
    {
        public static string? ToManagedStringAnsi(this nint stringPtr)
        {
            string? managedString = Marshal.PtrToStringAnsi(stringPtr);
            Marshal.FreeHGlobal(stringPtr);

            return managedString;
        }

        public static string? ToManagedStringUni(this nint stringPtr)
        {
            string? managedString = Marshal.PtrToStringUni(stringPtr);
            Marshal.FreeHGlobal(stringPtr);

            return managedString;
        }
    }
}
