using cs2_dma_esp.MemDMA.Collections;

namespace cs2_dma_esp.CS2
{
    public static class CS_Helper
    {
        public static ulong Cvar { get; set; }

        public static ulong GetInterface(ulong baseAddress, string name)
        {
            if (baseAddress == 0x0)
                return 0x0;

            ulong exportAddress = Memory.GetModuleExport("tier0.dll", "CreateInterface");
            if (exportAddress == 0x0)
                return 0x0;

            ulong interfaceEntry = Memory.ReadValue<ulong>((exportAddress + 7) + Memory.ReadValue<uint>(exportAddress + 3));
            if (interfaceEntry == 0x0)
                return 0x0;

            int nameLength = name.Length;

            while (true)
            {
                ulong interfaceNameAddr = Memory.ReadValue<ulong>(interfaceEntry + 8);
                string interfaceName = ReadString(interfaceNameAddr, nameLength);

                if (string.Equals(interfaceName, name, StringComparison.OrdinalIgnoreCase))
                {
                    ulong vfunc = Memory.ReadValue<ulong>(interfaceEntry);
                    ulong addr = (vfunc + 7) + Memory.ReadValue<uint>(vfunc + 3);
                    return addr;
                }

                interfaceEntry = Memory.ReadValue<ulong>(interfaceEntry + 16);
                if (interfaceEntry == 0)
                    break;
            }
            return 0;
        }

        public static ulong GetInterfaceFunction(ulong interfaceAddress, uint index)
        {
            return Memory.ReadValue<ulong>(Memory.ReadValue<ulong>(interfaceAddress) + (index * 8));
        }

        public static ulong GetConvar(string name)
        {
            if (Cvar == 0x0)
                return 0x0;

            ulong objs = Memory.ReadValue<ulong>(Cvar + 64, false);
            int convarLength = name.Length;

            int gg = Memory.ReadValue<int>(Cvar + 160, false);
            for (int i = 0; i < gg; i++)
            {
                ulong entry = Memory.ReadValue<ulong>(objs + (uint)(16 * i), false);
                if (entry == 0)
                    break;

                var convarName = ReadString(Memory.ReadValue<ulong>(entry, false), convarLength);

                if (string.Equals(convarName, name, StringComparison.OrdinalIgnoreCase))
                {
                    Logger.WriteLine($"GetConvar(): {convarName} -> 0x{entry:X}");
                    return entry;
                }
            }

            return 0x0;
        }

        private static string? ReadString(ulong address, int length)
        {
            try
            {
                using var hBuf = new SharedMemory<byte>(length);
                var buffer = hBuf.Span;

                Memory.ReadBuffer(address, buffer, false);

                return Encoding.ASCII.GetString(buffer).TrimEnd('\0');
            }
            catch
            {
                Logger.WriteLine($"Failed to read string at 0x{address:X}");
            }

            return null;
        }
    }
}
