namespace pubg_dma_esp.PUBG.AC
{
    public sealed unsafe partial class Xenuine : SafeHandle
    {
        #region Static Singleton
        private static Xenuine? _xenuine;

        public static bool Initialize()
        {
            try
            {
                _xenuine?.Dispose();
                _xenuine = null;
                _xenuine = new();
                Console.WriteLine("[Xenuine] Initialized successfully!");
                return true;
            }
            catch
            {
                Console.WriteLine("[Xenuine] Init Error!");
                return false;
            }
        }

        public static ulong Decrypt(ulong encrypted) => _xenuine?.Decrypt_Internal(encrypted) ?? throw new Exception("Xenuine is not initialized!");

        #endregion

        private const uint MEM_COMMIT = 0x1000;
        private const uint MEM_RESERVE = 0x2000;
        private const uint MEM_RELEASE = 0x00008000;
        private const uint PAGE_EXECUTE_READWRITE = 0x40;

        private static readonly byte[] _sig = new byte[] { 0x48, 0x8D };
        private readonly delegate*<ulong, ulong, ulong> _decrypt;

        private Xenuine() : base(IntPtr.Zero, true)
        {
            try
            {
                var decrypt_ptr = Memory.ReadPtr(Memory.ModuleBase + Offsets.Xenuine.Decrypt, false);

                Span<byte> xeReadBuf = new byte[0x100];
                Memory.ReadBuffer(decrypt_ptr, xeReadBuf, false);
                for (int i = 0; i < _sig.Length; i++)
                    if (xeReadBuf[i] != _sig[i])
                        throw new Exception("Invalid Xenuine MemRead!");

                this.handle = VirtualAlloc(IntPtr.Zero, 0x100, MEM_COMMIT | MEM_RESERVE, PAGE_EXECUTE_READWRITE);
                if (this.IsInvalid)
                    throw new Exception("VirtualAlloc Fail!");
                try
                {
                    int n1 = MemoryMarshal.Read<int>(xeReadBuf.Slice(0x3, 4));
                    var rwxMem = new Span<byte>(this.handle.ToPointer(), 0x100);
                    xeReadBuf.CopyTo(rwxMem);

                    ushort v1 = 0xB848;
                    MemoryMarshal.Write(rwxMem, ref v1);
                    ulong v2 = decrypt_ptr + (uint)n1 + 7;
                    MemoryMarshal.Write(rwxMem.Slice(0x2), ref v2);

                    xeReadBuf.Slice(0x7, 0x100 - 0xA).CopyTo(rwxMem.Slice(0xA));
                    _decrypt = (delegate*<ulong, ulong, ulong>)this.handle.ToPointer();
                }
                catch
                {
                    VirtualFree(this.handle, 0, MEM_RELEASE);
                    throw;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("ERROR Initializing Xenuine!", ex);
            }
        }

        public override bool IsInvalid => this.handle == IntPtr.Zero;

        public ulong Decrypt_Internal(ulong encrypted)
        {
            bool entered = false;
            this.DangerousAddRef(ref entered);
            if (entered)
            {
                try
                {
                    return _decrypt(0, encrypted);
                }
                finally
                {
                    this.DangerousRelease();
                }
            }
            else
                throw new Exception("Xenuine is not initialized!");
        }

        protected override bool ReleaseHandle()
        {
            try
            {
                return VirtualFree(this.handle, 0, MEM_RELEASE);
            }
            catch
            {
                return false;
            }
        }

        [LibraryImport("kernel32.dll", SetLastError = true)]
        private static partial IntPtr VirtualAlloc(IntPtr lpAddress, nuint dwSize, uint flAllocationType, uint flProtect);

        [LibraryImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static partial bool VirtualFree(IntPtr lpAddress, nuint dwSize, uint dwFreeType);
    }
}
