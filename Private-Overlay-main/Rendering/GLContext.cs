using Silk.NET.Core.Contexts;
using Silk.NET.Core.Loader;
using System.Text;

namespace Private_Overlay.Rendering
{
    public class GLContext : INativeContext
    {
        private readonly UnmanagedLibrary _l;

        public GLContext()
        {
            _l = new UnmanagedLibrary("opengl32.dll");
        }

        public nint GetProcAddress(string proc, int? slot = null)
        {
            if (TryGetProcAddress(proc, out var address, slot))
            {
                return address;
            }

            throw new InvalidOperationException("No function was found with the name " + proc + ".");
        }

        public unsafe bool TryGetProcAddress(string proc, out nint addr, int? slot = null)
        {
            if (_l.TryLoadFunction(proc, out addr))
            {
                return true;
            }

            // + 1 for null terminated string
            var asciiName = new byte[proc.Length + 1];

            Encoding.ASCII.GetBytes(proc, asciiName);

            fixed (byte* name = asciiName)
            {
                addr = wglGetProcAddress((sbyte*)name);
                if (addr != nint.Zero)
                {
                    return true;
                }
            }

            return false;
        }

        public void Dispose()
        {
            _l.Dispose();
        }
    }
}
