namespace Tarkov_DMA_Backend.LowLevel.Compression.Cab
{
    internal abstract class CabWorker : IDisposable
	{
		protected CabWorker(CabEngine cabEngine)
		{
			this.cabEngine = cabEngine;
			this.streamHandles = new HandleManager<Stream>();
			this.erf = new NativeMethods.ERF();
			this.erfHandle = GCHandle.Alloc(this.erf, GCHandleType.Pinned);
			this.cabNumbers = new Dictionary<string, short>(1);
			this.buf = new byte[32768];
		}

		~CabWorker()
		{
			this.Dispose(false);
		}

		public CabEngine CabEngine
		{
			get
			{
				return this.cabEngine;
			}
		}

		internal NativeMethods.ERF Erf
		{
			get
			{
				return this.erf;
			}
		}

		internal GCHandle ErfHandle
		{
			get
			{
				return this.erfHandle;
			}
		}

		internal HandleManager<Stream> StreamHandles
		{
			get
			{
				return this.streamHandles;
			}
		}

		internal bool SuppressProgressEvents
		{
			get
			{
				return this.suppressProgressEvents;
			}
			set
			{
				this.suppressProgressEvents = value;
			}
		}

		internal IDictionary<string, short> CabNumbers
		{
			get
			{
				return this.cabNumbers;
			}
		}

		internal string NextCabinetName
		{
			get
			{
				return this.nextCabinetName;
			}
			set
			{
				this.nextCabinetName = value;
			}
		}

		internal Stream CabStream
		{
			get
			{
				return this.cabStream;
			}
			set
			{
				this.cabStream = value;
			}
		}

		internal Stream FileStream
		{
			get
			{
				return this.fileStream;
			}
			set
			{
				this.fileStream = value;
			}
		}

		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected void ResetProgressData()
		{
			this.currentFileName = null;
			this.currentFileNumber = 0;
			this.totalFiles = 0;
			this.currentFileBytesProcessed = 0L;
			this.currentFileTotalBytes = 0L;
			this.currentFolderNumber = 0;
			this.currentFolderTotalBytes = 0L;
			this.currentArchiveName = null;
			this.currentArchiveNumber = 0;
			this.totalArchives = 0;
			this.currentArchiveBytesProcessed = 0L;
			this.currentArchiveTotalBytes = 0L;
			this.fileBytesProcessed = 0L;
			this.totalFileBytes = 0L;
		}

		protected void OnProgress(ArchiveProgressType progressType)
		{
			if (!this.suppressProgressEvents)
			{
				ArchiveProgressEventArgs archiveProgressEventArgs = new ArchiveProgressEventArgs(progressType, this.currentFileName, (this.currentFileNumber >= 0) ? this.currentFileNumber : 0, this.totalFiles, this.currentFileBytesProcessed, this.currentFileTotalBytes, this.currentArchiveName, (int)this.currentArchiveNumber, (int)this.totalArchives, this.currentArchiveBytesProcessed, this.currentArchiveTotalBytes, this.fileBytesProcessed, this.totalFileBytes);
				this.CabEngine.ReportProgress(archiveProgressEventArgs);
			}
		}

		internal IntPtr CabAllocMem(int byteCount)
		{
			return Marshal.AllocHGlobal((IntPtr)byteCount);
		}

		internal void CabFreeMem(IntPtr memPointer)
		{
			Marshal.FreeHGlobal(memPointer);
		}

		internal int CabOpenStream(string path, int openFlags, int shareMode)
		{
			int num;
			return this.CabOpenStreamEx(path, openFlags, shareMode, out num, IntPtr.Zero);
		}

		internal virtual int CabOpenStreamEx(string path, int openFlags, int shareMode, out int err, IntPtr pv)
		{
			path = path.Trim();
			Stream stream = this.cabStream;
			this.cabStream = new DuplicateStream(stream);
			int num = this.streamHandles.AllocHandle(stream);
			err = 0;
			return num;
		}

		internal int CabReadStream(int streamHandle, IntPtr memory, int cb)
		{
			int num;
			return this.CabReadStreamEx(streamHandle, memory, cb, out num, IntPtr.Zero);
		}

		internal virtual int CabReadStreamEx(int streamHandle, IntPtr memory, int cb, out int err, IntPtr pv)
		{
			Stream stream = this.streamHandles[streamHandle];
			if (cb > this.buf.Length)
			{
				this.buf = new byte[cb];
			}
			int num = stream.Read(this.buf, 0, cb);
			Marshal.Copy(this.buf, 0, memory, num);
			err = 0;
			return num;
		}

		internal int CabWriteStream(int streamHandle, IntPtr memory, int cb)
		{
			int num;
			return this.CabWriteStreamEx(streamHandle, memory, cb, out num, IntPtr.Zero);
		}

		internal virtual int CabWriteStreamEx(int streamHandle, IntPtr memory, int cb, out int err, IntPtr pv)
		{
			Stream stream = this.streamHandles[streamHandle];
			if (cb > this.buf.Length)
			{
				this.buf = new byte[cb];
			}
			Marshal.Copy(memory, this.buf, 0, cb);
			stream.Write(this.buf, 0, cb);
			err = 0;
			return cb;
		}

		internal int CabCloseStream(int streamHandle)
		{
			int num;
			return this.CabCloseStreamEx(streamHandle, out num, IntPtr.Zero);
		}

		internal virtual int CabCloseStreamEx(int streamHandle, out int err, IntPtr pv)
		{
			this.streamHandles.FreeHandle(streamHandle);
			err = 0;
			return 0;
		}

		internal int CabSeekStream(int streamHandle, int offset, int seekOrigin)
		{
			int num;
			return this.CabSeekStreamEx(streamHandle, offset, seekOrigin, out num, IntPtr.Zero);
		}

		internal virtual int CabSeekStreamEx(int streamHandle, int offset, int seekOrigin, out int err, IntPtr pv)
		{
            offset = (int)this.streamHandles[streamHandle].Seek(offset, (SeekOrigin)seekOrigin);
            err = 0;
            return offset;
        }

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (this.cabStream != null)
				{
					this.cabStream.Close();
					this.cabStream = null;
				}
				if (this.fileStream != null)
				{
					this.fileStream.Close();
					this.fileStream = null;
				}
			}
			if (this.erfHandle.IsAllocated)
			{
				this.erfHandle.Free();
			}
		}

		protected void CheckError(bool extracting)
		{
			if (this.Erf.Error)
			{
				throw new CabException(this.Erf.Oper, this.Erf.Type, CabException.GetErrorMessage(this.Erf.Oper, this.Erf.Type, extracting));
			}
		}

		internal const string CabStreamName = "%%CAB%%";

		private CabEngine cabEngine;

		private HandleManager<Stream> streamHandles;

		private Stream cabStream;

		private Stream fileStream;

		private NativeMethods.ERF erf;

		private GCHandle erfHandle;

		private IDictionary<string, short> cabNumbers;

		private string nextCabinetName;

		private bool suppressProgressEvents;

		private byte[] buf;

		protected string currentFileName;

		protected int currentFileNumber;

		protected int totalFiles;

		protected long currentFileBytesProcessed;

		protected long currentFileTotalBytes;

		protected short currentFolderNumber;

		protected long currentFolderTotalBytes;

		protected string currentArchiveName;

		protected short currentArchiveNumber;

		protected short totalArchives;

		protected long currentArchiveBytesProcessed;

		protected long currentArchiveTotalBytes;

		protected long fileBytesProcessed;

		protected long totalFileBytes;
	}
}
