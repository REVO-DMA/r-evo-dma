using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;

namespace XPDB.Compression.Cab
{
    internal static class NativeMethods
	{
		internal static class FCI
		{
			[DllImport("cabinet.dll", BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "FCICreate", ThrowOnUnmappableChar = true)]
			internal static extern Handle Create(IntPtr perf, PFNFILEPLACED pfnfcifp, PFNALLOC pfna, PFNFREE pfnf, PFNOPEN pfnopen, PFNREAD pfnread, PFNWRITE pfnwrite, PFNCLOSE pfnclose, PFNSEEK pfnseek, PFNDELETE pfndelete, PFNGETTEMPFILE pfnfcigtf, [MarshalAs(UnmanagedType.LPStruct)] CCAB pccab, IntPtr pv);

			[DllImport("cabinet.dll", BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "FCIAddFile", ThrowOnUnmappableChar = true)]
			internal static extern int AddFile(Handle hfci, string pszSourceFile, IntPtr pszFileName, [MarshalAs(UnmanagedType.Bool)] bool fExecute, PFNGETNEXTCABINET pfnfcignc, PFNSTATUS pfnfcis, PFNGETOPENINFO pfnfcigoi, TCOMP typeCompress);

			[DllImport("cabinet.dll", BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "FCIFlushCabinet", ThrowOnUnmappableChar = true)]
			internal static extern int FlushCabinet(Handle hfci, [MarshalAs(UnmanagedType.Bool)] bool fGetNextCab, PFNGETNEXTCABINET pfnfcignc, PFNSTATUS pfnfcis);

			[DllImport("cabinet.dll", BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "FCIFlushFolder", ThrowOnUnmappableChar = true)]
			internal static extern int FlushFolder(Handle hfci, PFNGETNEXTCABINET pfnfcignc, PFNSTATUS pfnfcis);

			[SuppressUnmanagedCodeSecurity]
			[DllImport("cabinet.dll", BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "FCIDestroy", ThrowOnUnmappableChar = true)]
			[return: MarshalAs(UnmanagedType.Bool)]
			internal static extern bool Destroy(IntPtr hfci);

			internal const int MIN_DISK = 32768;

			internal const int MAX_DISK = 2147483647;

			internal const int MAX_FOLDER = 2147450880;

			internal const int MAX_FILENAME = 256;

			internal const int MAX_CABINET_NAME = 256;

			internal const int MAX_CAB_PATH = 256;

			internal const int MAX_DISK_NAME = 256;

			internal const int CPU_80386 = 1;

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate IntPtr PFNALLOC(int cb);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void PFNFREE(IntPtr pv);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate int PFNOPEN(string path, int oflag, int pmode, out int err, IntPtr pv);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate int PFNREAD(int fileHandle, IntPtr memory, int cb, out int err, IntPtr pv);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate int PFNWRITE(int fileHandle, IntPtr memory, int cb, out int err, IntPtr pv);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate int PFNCLOSE(int fileHandle, out int err, IntPtr pv);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate int PFNSEEK(int fileHandle, int dist, int seekType, out int err, IntPtr pv);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate int PFNDELETE(string path, out int err, IntPtr pv);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate int PFNGETNEXTCABINET(IntPtr pccab, uint cbPrevCab, IntPtr pv);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate int PFNFILEPLACED(IntPtr pccab, string path, long fileSize, int continuation, IntPtr pv);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate int PFNGETOPENINFO(string path, out short date, out short time, out short pattribs, out int err, IntPtr pv);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate int PFNSTATUS(NativeMethods.FCI.STATUS typeStatus, uint cb1, uint cb2, IntPtr pv);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate int PFNGETTEMPFILE(IntPtr tempNamePtr, int tempNameSize, IntPtr pv);

			internal enum ERROR
			{
				NONE,
				OPEN_SRC,
				READ_SRC,
				ALLOC_FAIL,
				TEMP_FILE,
				BAD_COMPR_TYPE,
				CAB_FILE,
				USER_ABORT,
				MCI_FAIL
			}

			internal enum TCOMP : ushort
			{
				MASK_TYPE = 15,
				TYPE_NONE = 0,
				TYPE_MSZIP,
				TYPE_QUANTUM,
				TYPE_LZX,
				BAD = 15,
				MASK_LZX_WINDOW = 7936,
				LZX_WINDOW_LO = 3840,
				LZX_WINDOW_HI = 5376,
				SHIFT_LZX_WINDOW = 8,
				MASK_QUANTUM_LEVEL = 240,
				QUANTUM_LEVEL_LO = 16,
				QUANTUM_LEVEL_HI = 112,
				SHIFT_QUANTUM_LEVEL = 4,
				MASK_QUANTUM_MEM = 7936,
				QUANTUM_MEM_LO = 2560,
				QUANTUM_MEM_HI = 5376,
				SHIFT_QUANTUM_MEM = 8,
				MASK_RESERVED = 57344
			}

			internal enum STATUS : uint
			{
				FILE,
				FOLDER,
				CABINET
			}

			[StructLayout(LayoutKind.Sequential)]
			internal class CCAB
			{
				public CCAB() { }

				internal int cb = int.MaxValue;

				internal int cbFolderThresh = 2147450880;

				internal int cbReserveCFHeader;

				internal int cbReserveCFFolder;

				internal int cbReserveCFData;

				internal int iCab;

				internal int iDisk;

				internal int fFailOnIncompressible;

				internal short setID;

				[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
				internal string szDisk = string.Empty;

				[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
				internal string szCab = string.Empty;

				[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
				internal string szCabPath = string.Empty;
			}

			internal class Handle : SafeHandle
			{
				internal Handle()
					: base(IntPtr.Zero, true)
				{
				}

				public override bool IsInvalid
				{
					get
					{
						return this.handle == IntPtr.Zero;
					}
				}

				[SecurityPermission(SecurityAction.Assert, UnmanagedCode = true)]
				protected override bool ReleaseHandle()
				{
					return NativeMethods.FCI.Destroy(this.handle);
				}
			}
		}

		internal static class FDI
		{
			[DllImport("cabinet.dll", BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "FDICreate", ThrowOnUnmappableChar = true)]
			internal static extern Handle Create([MarshalAs(UnmanagedType.FunctionPtr)] PFNALLOC pfnalloc, [MarshalAs(UnmanagedType.FunctionPtr)] PFNFREE pfnfree, PFNOPEN pfnopen, PFNREAD pfnread, PFNWRITE pfnwrite, PFNCLOSE pfnclose, PFNSEEK pfnseek, int cpuType, IntPtr perf);

			[DllImport("cabinet.dll", BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "FDICopy", ThrowOnUnmappableChar = true)]
			internal static extern int Copy(Handle hfdi, string pszCabinet, string pszCabPath, int flags, PFNNOTIFY pfnfdin, IntPtr pfnfdid, IntPtr pvUser);

			[SuppressUnmanagedCodeSecurity]
			[DllImport("cabinet.dll", BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "FDIDestroy", ThrowOnUnmappableChar = true)]
			[return: MarshalAs(UnmanagedType.Bool)]
			internal static extern bool Destroy(IntPtr hfdi);

			[DllImport("cabinet.dll", BestFitMapping = false, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "FDIIsCabinet", ThrowOnUnmappableChar = true)]
			internal static extern int IsCabinet(Handle hfdi, int hf, out CABINFO pfdici);

			internal const int MAX_DISK = 2147483647;

			internal const int MAX_FILENAME = 256;

			internal const int MAX_CABINET_NAME = 256;

			internal const int MAX_CAB_PATH = 256;

			internal const int MAX_DISK_NAME = 256;

			internal const int CPU_80386 = 1;

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate IntPtr PFNALLOC(int cb);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate void PFNFREE(IntPtr pv);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate int PFNOPEN(string path, int oflag, int pmode);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate int PFNREAD(int hf, IntPtr pv, int cb);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate int PFNWRITE(int hf, IntPtr pv, int cb);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate int PFNCLOSE(int hf);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate int PFNSEEK(int hf, int dist, int seektype);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			internal delegate int PFNNOTIFY(NOTIFICATIONTYPE fdint, NOTIFICATION fdin);

			internal enum ERROR
			{
				NONE,
				CABINET_NOT_FOUND,
				NOT_A_CABINET,
				UNKNOWN_CABINET_VERSION,
				CORRUPT_CABINET,
				ALLOC_FAIL,
				BAD_COMPR_TYPE,
				MDI_FAIL,
				TARGET_FILE,
				RESERVE_MISMATCH,
				WRONG_CABINET,
				USER_ABORT
			}

			internal enum NOTIFICATIONTYPE
			{
				CABINET_INFO,
				PARTIAL_FILE,
				COPY_FILE,
				CLOSE_FILE_INFO,
				NEXT_CABINET,
				ENUMERATE
			}

			internal struct CABINFO
			{
				internal int cbCabinet;

				internal short cFolders;

				internal short cFiles;

				internal short setID;

				internal short iCabinet;

				internal int fReserve;

				internal int hasprev;

				internal int hasnext;
			}

			[StructLayout(LayoutKind.Sequential)]
			internal class NOTIFICATION
			{
				internal int hf
				{
					get
					{
						return (int)this.hf_ptr;
					}
				}

				public NOTIFICATION() { }

				internal int cb;

				internal IntPtr psz1;

				internal IntPtr psz2;

				internal IntPtr psz3;

				internal IntPtr pv;

				internal IntPtr hf_ptr;

				internal short date;

				internal short time;

				internal short attribs;

				internal short setID;

				internal short iCabinet;

				internal short iFolder;

				internal int fdie;
			}

			internal class Handle : SafeHandle
			{
				internal Handle() : base(IntPtr.Zero, true) { }

				public override bool IsInvalid
				{
					get
					{
						return this.handle == IntPtr.Zero;
					}
				}

				protected override bool ReleaseHandle()
				{
					return NativeMethods.FDI.Destroy(this.handle);
				}
			}
		}

		[StructLayout(LayoutKind.Sequential)]
		internal class ERF
		{
			internal int Oper
			{
				get
				{
					return this.erfOper;
				}
				set
				{
					this.erfOper = value;
				}
			}

			internal int Type
			{
				get
				{
					return this.erfType;
				}
				set
				{
					this.erfType = value;
				}
			}

			internal bool Error
			{
				get
				{
					return this.fError != 0;
				}
				set
				{
					this.fError = (value ? 1 : 0);
				}
			}

			internal void Clear()
			{
				this.Oper = 0;
				this.Type = 0;
				this.Error = false;
			}

			public ERF() { }

			private int erfOper;

			private int erfType;

			private int fError;
		}
	}
}
