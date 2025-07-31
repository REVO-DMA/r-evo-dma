using SharedMemory;
using System.Runtime.InteropServices;

namespace XPDB.Misc
{
    public unsafe struct PChar : IDisposable
    {
        public static implicit operator char*(PChar x) => x.Ptr;

        public readonly char* Ptr;

        public PChar(string str)
        {
            char* ptr = (char*)Marshal.StringToHGlobalAuto(str);

            Ptr = ptr;
        }

        public PChar(int length)
        {
            using SharedPinned<char> chars = new(length);
            string str = new(chars.Span);
            char* ptr = (char*)Marshal.StringToHGlobalAuto(str);

            Ptr = ptr;
        }

        public void Dispose()
        {
            Marshal.FreeHGlobal((nint)Ptr);
        }

        public override string ToString()
        {
            return Marshal.PtrToStringAuto((nint)Ptr);
        }
    }
}
