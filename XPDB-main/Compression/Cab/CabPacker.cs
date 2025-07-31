using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;

namespace XPDB.Compression.Cab
{
    internal class CabPacker : CabWorker
	{
		public CabPacker(CabEngine cabEngine) : base(cabEngine)
		{
			this.fciAllocMemHandler = new NativeMethods.FCI.PFNALLOC(base.CabAllocMem);
			this.fciFreeMemHandler = new NativeMethods.FCI.PFNFREE(base.CabFreeMem);
			this.fciOpenStreamHandler = new NativeMethods.FCI.PFNOPEN(this.CabOpenStreamEx);
			this.fciReadStreamHandler = new NativeMethods.FCI.PFNREAD(this.CabReadStreamEx);
			this.fciWriteStreamHandler = new NativeMethods.FCI.PFNWRITE(this.CabWriteStreamEx);
			this.fciCloseStreamHandler = new NativeMethods.FCI.PFNCLOSE(this.CabCloseStreamEx);
			this.fciSeekStreamHandler = new NativeMethods.FCI.PFNSEEK(this.CabSeekStreamEx);
			this.fciFilePlacedHandler = new NativeMethods.FCI.PFNFILEPLACED(this.CabFilePlaced);
			this.fciDeleteFileHandler = new NativeMethods.FCI.PFNDELETE(this.CabDeleteFile);
			this.fciGetTempFileHandler = new NativeMethods.FCI.PFNGETTEMPFILE(this.CabGetTempFile);
			this.fciGetNextCabinet = new NativeMethods.FCI.PFNGETNEXTCABINET(this.CabGetNextCabinet);
			this.fciCreateStatus = new NativeMethods.FCI.PFNSTATUS(this.CabCreateStatus);
			this.fciGetOpenInfo = new NativeMethods.FCI.PFNGETOPENINFO(this.CabGetOpenInfo);
			this.tempStreams = new List<Stream>();
			this.CompressionLevel = ECompressionLevel.Normal;
		}

		public bool UseTempFiles
		{
			get
			{
				return !this.dontUseTempFiles;
			}

			set
			{
				this.dontUseTempFiles = !value;
			}
		}

		public ECompressionLevel CompressionLevel { get; set; }

		private void CreateFci(long maxArchiveSize)
		{
			NativeMethods.FCI.CCAB ccab = new NativeMethods.FCI.CCAB();
			if (maxArchiveSize > 0 && maxArchiveSize < ccab.cb)
			{
				ccab.cb = Math.Max(32768, (int)maxArchiveSize);
			}
			object option = this.context.GetOption("maxFolderSize", null);
			if (option != null)
			{
				long num = Convert.ToInt64(option, CultureInfo.InvariantCulture);
				if (num > 0 && num < ccab.cbFolderThresh)
				{
					ccab.cbFolderThresh = (int)num;
				}
			}
			this.maxCabBytes = ccab.cb;
			ccab.szCab = this.context.GetArchiveName(0);
			if (ccab.szCab == null)
			{
				throw new FileNotFoundException("Cabinet name not provided by stream context.");
			}
			ccab.setID = (short)new Random().Next(-32768, 32768);
			base.CabNumbers[ccab.szCab] = 0;
			this.currentArchiveName = ccab.szCab;
			this.totalArchives = 1;
			base.CabStream = null;
			base.Erf.Clear();
			this.fciHandle = NativeMethods.FCI.Create(base.ErfHandle.AddrOfPinnedObject(), this.fciFilePlacedHandler, this.fciAllocMemHandler, this.fciFreeMemHandler, this.fciOpenStreamHandler, this.fciReadStreamHandler, this.fciWriteStreamHandler, this.fciCloseStreamHandler, this.fciSeekStreamHandler, this.fciDeleteFileHandler, this.fciGetTempFileHandler, ccab, IntPtr.Zero);
			base.CheckError(false);
		}

		public void Pack(IPackStreamContext streamContext, IEnumerable<string> files, long maxArchiveSize)
		{
            ArgumentNullException.ThrowIfNull(streamContext);
            ArgumentNullException.ThrowIfNull(files);
            lock (this)
            {
                try
                {
                    this.context = streamContext;
                    base.ResetProgressData();
                    this.CreateFci(maxArchiveSize);
                    foreach (string text in files)
                    {
                        FileAttributes fileAttributes;
                        DateTime dateTime;
                        Stream stream = this.context.OpenFileReadStream(text, out fileAttributes, out dateTime);
                        if (stream != null)
                        {
                            this.totalFileBytes += stream.Length;
                            this.totalFiles++;
                            this.context.CloseFileReadStream(text, stream);
                        }
                    }
                    long num = 0L;
                    this.currentFileNumber = -1;
                    foreach (string text2 in files)
                    {
                        FileAttributes fileAttributes2;
                        DateTime dateTime2;
                        Stream stream2 = this.context.OpenFileReadStream(text2, out fileAttributes2, out dateTime2);
                        if (stream2 != null)
                        {
                            if (stream2.Length >= 2147450880)
                            {
                                throw new NotSupportedException(string.Format(CultureInfo.InvariantCulture, "File {0} exceeds maximum file size for cabinet format.", new object[] { text2 }));
                            }
                            if (num > 0L)
                            {
                                bool flag = num + stream2.Length >= 2147450880;
                                if (!flag)
                                {
                                    flag = Convert.ToBoolean(streamContext.GetOption("nextFolder", new object[] { text2, this.currentFolderNumber }), CultureInfo.InvariantCulture);
                                }
                                if (flag)
                                {
                                    this.FlushFolder();
                                    num = 0L;
                                }
                            }
                            if (this.currentFolderTotalBytes > 0)
                            {
                                this.currentFolderTotalBytes = 0;
                                this.currentFolderNumber += 1;
                                num = 0;
                            }
                            this.currentFileName = text2;
                            this.currentFileNumber++;
                            this.currentFileTotalBytes = stream2.Length;
                            this.currentFileBytesProcessed = 0;
                            base.OnProgress(ArchiveProgressType.StartFile);
                            num += stream2.Length;
                            this.AddFile(text2, stream2, fileAttributes2, dateTime2, false, this.CompressionLevel);
                        }
                    }
                    this.FlushFolder();
                    this.FlushCabinet();
                }
                finally
                {
                    if (base.CabStream != null)
                    {
                        this.context.CloseArchiveWriteStream((int)this.currentArchiveNumber, this.currentArchiveName, base.CabStream);
                        base.CabStream = null;
                    }
                    if (base.FileStream != null)
                    {
                        this.context.CloseFileReadStream(this.currentFileName, base.FileStream);
                        base.FileStream = null;
                    }
                    this.context = null;
                    if (this.fciHandle != null)
                    {
                        this.fciHandle.Dispose();
                        this.fciHandle = null;
                    }
                }
            }
        }

		internal override int CabOpenStreamEx(string path, int openFlags, int shareMode, out int err, IntPtr pv)
		{
			if (base.CabNumbers.ContainsKey(path))
			{
				if (base.CabStream == null)
				{
					short num = base.CabNumbers[path];
					this.currentFolderTotalBytes = 0L;
					Stream stream = this.context.OpenArchiveWriteStream((int)num, path, true, base.CabEngine);
					if (stream == null)
					{
						throw new FileNotFoundException(string.Format(CultureInfo.InvariantCulture, "Cabinet {0} not provided.", new object[] { num }));
					}
					this.currentArchiveName = path;
					this.currentArchiveTotalBytes = Math.Min(this.totalFolderBytesProcessedInCurrentCab, (long)this.maxCabBytes);
					this.currentArchiveBytesProcessed = 0L;
					base.OnProgress(ArchiveProgressType.StartArchive);
					base.CabStream = stream;
				}
				path = "%%CAB%%";
			}
			else
			{
				if (path == "%%TEMP%%")
				{
					Stream stream2 = new MemoryStream();
					this.tempStreams.Add(stream2);
					int num2 = base.StreamHandles.AllocHandle(stream2);
					err = 0;
					return num2;
				}
				if (path != "%%CAB%%")
				{
					path = Path.Combine(Path.GetTempPath(), path);
					Stream stream3 = new FileStream(path, FileMode.Open, FileAccess.ReadWrite);
					this.tempStreams.Add(stream3);
					stream3 = new DuplicateStream(stream3);
					int num3 = base.StreamHandles.AllocHandle(stream3);
					err = 0;
					return num3;
				}
			}
			return base.CabOpenStreamEx(path, openFlags, shareMode, out err, pv);
		}

		internal override int CabWriteStreamEx(int streamHandle, IntPtr memory, int cb, out int err, IntPtr pv)
		{
			int num = base.CabWriteStreamEx(streamHandle, memory, cb, out err, pv);
            if (num > 0 && err == 0 && DuplicateStream.OriginalStream(base.StreamHandles[streamHandle]) == DuplicateStream.OriginalStream(base.CabStream))
            {
				this.currentArchiveBytesProcessed += cb;
                if (this.currentArchiveBytesProcessed > this.currentArchiveTotalBytes)
                {
                    this.currentArchiveBytesProcessed = this.currentArchiveTotalBytes;
                }
            }
            return num;
        }

		internal override int CabCloseStreamEx(int streamHandle, out int err, IntPtr pv)
		{
			Stream stream = DuplicateStream.OriginalStream(base.StreamHandles[streamHandle]);
            if (stream == DuplicateStream.OriginalStream(base.FileStream))
            {
                this.context.CloseFileReadStream(this.currentFileName, stream);
                base.FileStream = null;
                long num = this.currentFileTotalBytes - this.currentFileBytesProcessed;
                this.currentFileBytesProcessed += num;
                this.fileBytesProcessed += num;
                base.OnProgress(ArchiveProgressType.FinishFile);
                this.currentFileTotalBytes = 0L;
                this.currentFileBytesProcessed = 0L;
                this.currentFileName = null;
            }
            else if (stream == DuplicateStream.OriginalStream(base.CabStream))
            {
                if (stream.CanWrite)
                {
                    stream.Flush();
                }
                this.currentArchiveBytesProcessed = this.currentArchiveTotalBytes;
                base.OnProgress(ArchiveProgressType.FinishArchive);
                this.currentArchiveNumber += 1;
                this.totalArchives += 1;
                this.context.CloseArchiveWriteStream((int)this.currentArchiveNumber, this.currentArchiveName, stream);
                this.currentArchiveName = base.NextCabinetName;
                this.currentArchiveBytesProcessed = (this.currentArchiveTotalBytes = 0L);
                this.totalFolderBytesProcessedInCurrentCab = 0L;
                base.CabStream = null;
            }
            else
            {
                stream.Close();
                this.tempStreams.Remove(stream);
            }
            return base.CabCloseStreamEx(streamHandle, out err, pv);
        }

		protected override void Dispose(bool disposing)
		{
			try
			{
				if (disposing && this.fciHandle != null)
				{
					this.fciHandle.Dispose();
					this.fciHandle = null;
				}
			}
			finally
			{
				base.Dispose(disposing);
			}
		}

		private static NativeMethods.FCI.TCOMP GetCompressionType(ECompressionLevel compLevel)
		{
			if (compLevel < ECompressionLevel.Min)
			{
				return NativeMethods.FCI.TCOMP.TYPE_NONE;
			}
			if (compLevel > ECompressionLevel.Max)
			{
				compLevel = ECompressionLevel.Max;
			}

            int num = 6 * (compLevel - ECompressionLevel.Min) / 9;
            return (NativeMethods.FCI.TCOMP)(3 | (3840 + (num << 8)));
        }

		private void AddFile(string name, Stream stream, FileAttributes attributes, DateTime lastWriteTime, bool execute, ECompressionLevel compLevel)
		{
			base.FileStream = stream;
			this.fileAttributes = attributes & (FileAttributes.ReadOnly | FileAttributes.Hidden | FileAttributes.System | FileAttributes.Archive);
			this.fileLastWriteTime = lastWriteTime;
			this.currentFileName = name;
			NativeMethods.FCI.TCOMP compressionType = CabPacker.GetCompressionType(compLevel);
			IntPtr intPtr = IntPtr.Zero;
			try
			{
				Encoding encoding = Encoding.ASCII;
				if (Encoding.UTF8.GetByteCount(name) > name.Length)
				{
					encoding = Encoding.UTF8;
					this.fileAttributes |= FileAttributes.Normal;
				}
				byte[] bytes = encoding.GetBytes(name);
				intPtr = Marshal.AllocHGlobal(bytes.Length + 1);
				Marshal.Copy(bytes, 0, intPtr, bytes.Length);
				Marshal.WriteByte(intPtr, bytes.Length, 0);
				base.Erf.Clear();
				NativeMethods.FCI.AddFile(this.fciHandle, string.Empty, intPtr, execute, this.fciGetNextCabinet, this.fciCreateStatus, this.fciGetOpenInfo, compressionType);
			}
			finally
			{
				if (intPtr != IntPtr.Zero)
				{
					Marshal.FreeHGlobal(intPtr);
				}
			}
			base.CheckError(false);
			base.FileStream = null;
			this.currentFileName = null;
		}

		private void FlushFolder()
		{
			base.Erf.Clear();
			NativeMethods.FCI.FlushFolder(this.fciHandle, this.fciGetNextCabinet, this.fciCreateStatus);
			base.CheckError(false);
		}

		private void FlushCabinet()
		{
			base.Erf.Clear();
			NativeMethods.FCI.FlushCabinet(this.fciHandle, false, this.fciGetNextCabinet, this.fciCreateStatus);
			base.CheckError(false);
		}

		private int CabGetOpenInfo(string path, out short date, out short time, out short attribs, out int err, IntPtr pv)
		{
			CompressionEngine.DateTimeToDosDateAndTime(this.fileLastWriteTime, out date, out time);
			attribs = (short)this.fileAttributes;
			Stream fileStream = base.FileStream;
			base.FileStream = new DuplicateStream(fileStream);
			int num = base.StreamHandles.AllocHandle(fileStream);
			err = 0;
			return num;
		}

		private int CabFilePlaced(IntPtr pccab, string filePath, long fileSize, int continuation, IntPtr pv)
		{
			return 0;
		}

		private int CabGetNextCabinet(IntPtr pccab, uint prevCabSize, IntPtr pv)
		{
			NativeMethods.FCI.CCAB ccab = new();
			Marshal.PtrToStructure(pccab, ccab);
			ccab.szDisk = string.Empty;
			ccab.szCab = this.context.GetArchiveName(ccab.iCab);
			base.CabNumbers[ccab.szCab] = (short)ccab.iCab;
			base.NextCabinetName = ccab.szCab;
			Marshal.StructureToPtr(ccab, pccab, false);
			return 1;
		}

		private int CabCreateStatus(NativeMethods.FCI.STATUS typeStatus, uint cb1, uint cb2, IntPtr pv)
		{
            switch (typeStatus)
            {
                case NativeMethods.FCI.STATUS.FILE:
                    if (cb2 > 0U && this.currentFileBytesProcessed < this.currentFileTotalBytes)
                    {
                        if (this.currentFileBytesProcessed + (long)((ulong)cb2) > this.currentFileTotalBytes)
                        {
                            cb2 = (uint)this.currentFileTotalBytes - (uint)this.currentFileBytesProcessed;
                        }
                        this.currentFileBytesProcessed += (long)((ulong)cb2);
                        this.fileBytesProcessed += (long)((ulong)cb2);
                        base.OnProgress(ArchiveProgressType.PartialFile);
                    }
                    break;
                case NativeMethods.FCI.STATUS.FOLDER:
                    if (cb1 == 0U)
                    {
                        this.currentFolderTotalBytes = (long)(cb2 - (ulong)this.totalFolderBytesProcessedInCurrentCab);
                        this.totalFolderBytesProcessedInCurrentCab = (long)((ulong)cb2);
                    }
                    else if (this.currentFolderTotalBytes == 0L)
                    {
                        base.OnProgress(ArchiveProgressType.PartialArchive);
                    }
                    break;
            }

            return 0;
        }

		private int CabGetTempFile(IntPtr tempNamePtr, int tempNameSize, IntPtr pv)
		{
			string text;
			if (this.UseTempFiles)
			{
				text = Path.GetFileName(Path.GetTempFileName());
			}
			else
			{
				text = "%%TEMP%%";
			}
			byte[] bytes = Encoding.ASCII.GetBytes(text);
			if (bytes.Length >= tempNameSize)
			{
				return -1;
			}
			Marshal.Copy(bytes, 0, tempNamePtr, bytes.Length);
			Marshal.WriteByte(tempNamePtr, bytes.Length, 0);
			return 1;
		}

		private int CabDeleteFile(string path, out int err, IntPtr pv)
		{
			try
			{
				if (path != "%%TEMP%%")
				{
					path = Path.Combine(Path.GetTempPath(), path);
					File.Delete(path);
				}
			}
			catch (IOException)
			{
			}
			err = 0;
			return 1;
		}

		private const string TempStreamName = "%%TEMP%%";

		private NativeMethods.FCI.Handle fciHandle;

		private NativeMethods.FCI.PFNALLOC fciAllocMemHandler;

		private NativeMethods.FCI.PFNFREE fciFreeMemHandler;

		private NativeMethods.FCI.PFNOPEN fciOpenStreamHandler;

		private NativeMethods.FCI.PFNREAD fciReadStreamHandler;

		private NativeMethods.FCI.PFNWRITE fciWriteStreamHandler;

		private NativeMethods.FCI.PFNCLOSE fciCloseStreamHandler;

		private NativeMethods.FCI.PFNSEEK fciSeekStreamHandler;

		private NativeMethods.FCI.PFNFILEPLACED fciFilePlacedHandler;

		private NativeMethods.FCI.PFNDELETE fciDeleteFileHandler;

		private NativeMethods.FCI.PFNGETTEMPFILE fciGetTempFileHandler;

		private NativeMethods.FCI.PFNGETNEXTCABINET fciGetNextCabinet;

		private NativeMethods.FCI.PFNSTATUS fciCreateStatus;

		private NativeMethods.FCI.PFNGETOPENINFO fciGetOpenInfo;

		private IPackStreamContext context;

		private FileAttributes fileAttributes;

		private DateTime fileLastWriteTime;

		private int maxCabBytes;

		private long totalFolderBytesProcessedInCurrentCab;

		private bool dontUseTempFiles;

		private IList<Stream> tempStreams;
	}
}
