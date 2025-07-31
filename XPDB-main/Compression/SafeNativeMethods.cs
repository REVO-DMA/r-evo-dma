using System.Runtime.InteropServices;
using System.Security;

namespace XPDB.Compression
{
    [SuppressUnmanagedCodeSecurity]
	internal static class SafeNativeMethods
	{
		[DllImport("kernel32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool DosDateTimeToFileTime(short wFatDate, short wFatTime, out long fileTime);

		[DllImport("kernel32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool FileTimeToDosDateTime(ref long fileTime, out short wFatDate, out short wFatTime);
	}
}
