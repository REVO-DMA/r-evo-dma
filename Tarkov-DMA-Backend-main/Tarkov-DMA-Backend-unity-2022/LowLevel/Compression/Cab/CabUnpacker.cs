using System.Globalization;

namespace Tarkov_DMA_Backend.LowLevel.Compression.Cab
{
    internal class CabUnpacker : CabWorker
	{
		public CabUnpacker(CabEngine cabEngine)
			: base(cabEngine)
		{
			this.fdiAllocMemHandler = new NativeMethods.FDI.PFNALLOC(base.CabAllocMem);
			this.fdiFreeMemHandler = new NativeMethods.FDI.PFNFREE(base.CabFreeMem);
			this.fdiOpenStreamHandler = new NativeMethods.FDI.PFNOPEN(base.CabOpenStream);
			this.fdiReadStreamHandler = new NativeMethods.FDI.PFNREAD(base.CabReadStream);
			this.fdiWriteStreamHandler = new NativeMethods.FDI.PFNWRITE(base.CabWriteStream);
			this.fdiCloseStreamHandler = new NativeMethods.FDI.PFNCLOSE(base.CabCloseStream);
			this.fdiSeekStreamHandler = new NativeMethods.FDI.PFNSEEK(base.CabSeekStream);
			this.fdiHandle = NativeMethods.FDI.Create(this.fdiAllocMemHandler, this.fdiFreeMemHandler, this.fdiOpenStreamHandler, this.fdiReadStreamHandler, this.fdiWriteStreamHandler, this.fdiCloseStreamHandler, this.fdiSeekStreamHandler, 1, base.ErfHandle.AddrOfPinnedObject());
			if (base.Erf.Error)
			{
				int oper = base.Erf.Oper;
				int type = base.Erf.Type;
				base.ErfHandle.Free();
				throw new CabException(oper, type, CabException.GetErrorMessage(oper, type, true));
			}
		}

		public bool IsArchive(Stream stream)
		{
            ArgumentNullException.ThrowIfNull(stream);
			
			lock (this)
			{
                return IsCabinet(stream, out short num, out int num2, out int num3);
			}
		}

		public List<ArchiveFileInfo> GetFileInfo(IUnpackStreamContext streamContext, Predicate<string> fileFilter)
		{
            ArgumentNullException.ThrowIfNull(streamContext);
            List<ArchiveFileInfo> list2;
            lock (this)
            {
                this.context = streamContext;
                this.filter = fileFilter;
                base.NextCabinetName = string.Empty;
                this.fileList = new List<ArchiveFileInfo>();
                bool suppressProgressEvents = base.SuppressProgressEvents;
                base.SuppressProgressEvents = true;
                try
                {
                    short num = 0;
                    while (base.NextCabinetName != null)
                    {
                        base.Erf.Clear();
                        base.CabNumbers[base.NextCabinetName] = num;
                        NativeMethods.FDI.Copy(this.fdiHandle, base.NextCabinetName, string.Empty, 0, new NativeMethods.FDI.PFNNOTIFY(this.CabListNotify), IntPtr.Zero, IntPtr.Zero);
                        base.CheckError(true);
                        num += 1;
                    }
                    List<ArchiveFileInfo> list = this.fileList;
                    this.fileList = null;
                    list2 = list;
                }
                finally
                {
                    base.SuppressProgressEvents = suppressProgressEvents;
                    if (base.CabStream != null)
                    {
                        this.context.CloseArchiveReadStream((int)this.currentArchiveNumber, this.currentArchiveName, base.CabStream);
                        base.CabStream = null;
                    }
                    this.context = null;
                }
            }
            return list2;
        }

		public void Unpack(IUnpackStreamContext streamContext, Predicate<string> fileFilter)
		{
            lock (this)
            {
                List<ArchiveFileInfo> fileInfo = this.GetFileInfo(streamContext, fileFilter);
                base.ResetProgressData();
                if (fileInfo != null)
                {
                    this.totalFiles = fileInfo.Count;
                    for (int i = 0; i < fileInfo.Count; i++)
                    {
                        this.totalFileBytes += fileInfo[i].Length;
                        if (fileInfo[i].ArchiveNumber >= (int)this.totalArchives)
                        {
                            int num = fileInfo[i].ArchiveNumber + 1;
                            this.totalArchives = (short)num;
                        }
                    }
                }
                this.context = streamContext;
                this.fileList = null;
                base.NextCabinetName = string.Empty;
                this.folderId = -1;
                this.currentFileNumber = -1;
                try
                {
                    short num2 = 0;
                    while (base.NextCabinetName != null)
                    {
                        base.Erf.Clear();
                        base.CabNumbers[base.NextCabinetName] = num2;
                        NativeMethods.FDI.Copy(this.fdiHandle, base.NextCabinetName, string.Empty, 0, new NativeMethods.FDI.PFNNOTIFY(this.CabExtractNotify), IntPtr.Zero, IntPtr.Zero);
                        base.CheckError(true);
                        num2 += 1;
                    }
                }
                finally
                {
                    if (base.CabStream != null)
                    {
                        this.context.CloseArchiveReadStream((int)this.currentArchiveNumber, this.currentArchiveName, base.CabStream);
                        base.CabStream = null;
                    }
                    if (base.FileStream != null)
                    {
                        this.context.CloseFileWriteStream(this.currentFileName, base.FileStream, FileAttributes.Normal, DateTime.Now);
                        base.FileStream = null;
                    }
                    this.context = null;
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
                    Stream stream = this.context.OpenArchiveReadStream((int)num, path, base.CabEngine);
                    if (stream == null)
                    {
                        throw new FileNotFoundException(string.Format(CultureInfo.InvariantCulture, "Cabinet {0} not provided.", new object[] { num }));
                    }
                    this.currentArchiveName = path;
                    this.currentArchiveNumber = num;
                    if (this.totalArchives <= this.currentArchiveNumber)
                    {
                        int num2 = (int)(this.currentArchiveNumber + 1);
                        this.totalArchives = (short)num2;
                    }
                    this.currentArchiveTotalBytes = stream.Length;
                    this.currentArchiveBytesProcessed = 0L;
                    if (this.folderId != -3)
                    {
                        base.OnProgress(ArchiveProgressType.StartArchive);
                    }
                    base.CabStream = stream;
                }
                path = "%%CAB%%";
            }
            return base.CabOpenStreamEx(path, openFlags, shareMode, out err, pv);
        }

		internal override int CabReadStreamEx(int streamHandle, IntPtr memory, int cb, out int err, IntPtr pv)
		{
			int num = base.CabReadStreamEx(streamHandle, memory, cb, out err, pv);
            if (err == 0 && base.CabStream != null && this.fileList == null && DuplicateStream.OriginalStream(base.StreamHandles[streamHandle]) == DuplicateStream.OriginalStream(base.CabStream))
            {
                this.currentArchiveBytesProcessed += cb;
                if (this.currentArchiveBytesProcessed > this.currentArchiveTotalBytes)
                {
                    this.currentArchiveBytesProcessed = this.currentArchiveTotalBytes;
                }
            }
            return num;
        }

		internal override int CabWriteStreamEx(int streamHandle, IntPtr memory, int cb, out int err, IntPtr pv)
		{
			int num = base.CabWriteStreamEx(streamHandle, memory, cb, out err, pv);
            if (num > 0 && err == 0)
            {
                this.currentFileBytesProcessed += cb;
                this.fileBytesProcessed += cb;
                base.OnProgress(ArchiveProgressType.PartialFile);
            }
            return num;
        }

		internal override int CabCloseStreamEx(int streamHandle, out int err, IntPtr pv)
		{
			Stream stream = DuplicateStream.OriginalStream(base.StreamHandles[streamHandle]);
			if (stream == DuplicateStream.OriginalStream(base.CabStream))
			{
				if (this.folderId != -3)
				{
					base.OnProgress(ArchiveProgressType.FinishArchive);
				}
				this.context.CloseArchiveReadStream((int)this.currentArchiveNumber, this.currentArchiveName, stream);
				this.currentArchiveName = base.NextCabinetName;
				this.currentArchiveBytesProcessed = (this.currentArchiveTotalBytes = 0L);
				base.CabStream = null;
			}
			return base.CabCloseStreamEx(streamHandle, out err, pv);
		}

		protected override void Dispose(bool disposing)
		{
			try
			{
				if (disposing && this.fdiHandle != null)
				{
					this.fdiHandle.Dispose();
					this.fdiHandle = null;
				}
			}
			finally
			{
				base.Dispose(disposing);
			}
		}

		private static string GetFileName(NativeMethods.FDI.NOTIFICATION notification)
		{
			Encoding encoding = (((notification.attribs & 128) != 0) ? Encoding.UTF8 : Encoding.Default);
			int num = 0;
            while (Marshal.ReadByte(notification.psz1, num) != 0)
            {
                num++;
            }
            byte[] array = new byte[num];
            Marshal.Copy(notification.psz1, array, 0, num);
            string text = encoding.GetString(array);
            if (Path.IsPathRooted(text))
            {
                text = text.Replace(Path.VolumeSeparatorChar.ToString() ?? "", "");
            }
            return text;
        }

		private bool IsCabinet(Stream cabStream, out short id, out int cabFolderCount, out int fileCount)
		{
			int num = base.StreamHandles.AllocHandle(cabStream);
			bool flag2;
			try
			{
				base.Erf.Clear();
				NativeMethods.FDI.CABINFO cabinfo;
				bool flag = NativeMethods.FDI.IsCabinet(this.fdiHandle, num, out cabinfo) != 0;
				if (base.Erf.Error)
				{
					if (base.Erf.Oper != 3)
					{
						throw new CabException(base.Erf.Oper, base.Erf.Type, CabException.GetErrorMessage(base.Erf.Oper, base.Erf.Type, true));
					}
					flag = false;
				}
				id = cabinfo.setID;
				cabFolderCount = (int)cabinfo.cFolders;
				fileCount = (int)cabinfo.cFiles;
				flag2 = flag;
			}
			finally
			{
				base.StreamHandles.FreeHandle(num);
			}
			return flag2;
		}

		private int CabListNotify(NativeMethods.FDI.NOTIFICATIONTYPE notificationType, NativeMethods.FDI.NOTIFICATION notification)
		{
            switch (notificationType)
            {
                case NativeMethods.FDI.NOTIFICATIONTYPE.CABINET_INFO:
                    {
                        string text = Marshal.PtrToStringAnsi(notification.psz1);
                        base.NextCabinetName = ((text.Length != 0) ? text : null);
                        return 0;
                    }
                case NativeMethods.FDI.NOTIFICATIONTYPE.PARTIAL_FILE:
                    return 0;
                case NativeMethods.FDI.NOTIFICATIONTYPE.COPY_FILE:
                    {
                        string fileName = CabUnpacker.GetFileName(notification);
                        if ((this.filter == null || this.filter(fileName)) && this.fileList != null)
                        {
                            FileAttributes fileAttributes = (FileAttributes)(notification.attribs & 39);
                            if (fileAttributes == (FileAttributes)0)
                            {
                                fileAttributes = FileAttributes.Normal;
                            }
                            DateTime dateTime;
                            CompressionEngine.DosDateAndTimeToDateTime(notification.date, notification.time, out dateTime);
                            long num = notification.cb;
                            CabFileInfo cabFileInfo = new CabFileInfo(fileName, (int)notification.iFolder, (int)notification.iCabinet, fileAttributes, dateTime, num);
                            this.fileList.Add(cabFileInfo);
                            this.currentFileNumber = this.fileList.Count - 1;
                            this.fileBytesProcessed += notification.cb;
                        }
                        this.totalFiles++;
                        this.totalFileBytes += notification.cb;
                        return 0;
                    }
                default:
                    return 0;
            }
        }

		private int CabExtractNotify(NativeMethods.FDI.NOTIFICATIONTYPE notificationType, NativeMethods.FDI.NOTIFICATION notification)
		{
			switch (notificationType)
			{
			case NativeMethods.FDI.NOTIFICATIONTYPE.CABINET_INFO:
				if (base.NextCabinetName != null && base.NextCabinetName.StartsWith("?", StringComparison.Ordinal))
				{
					base.NextCabinetName = base.NextCabinetName.Substring(1);
				}
				else
				{
					string text = Marshal.PtrToStringAnsi(notification.psz1);
					base.NextCabinetName = ((text.Length != 0) ? text : null);
				}
				return 0;
			case NativeMethods.FDI.NOTIFICATIONTYPE.COPY_FILE:
				return this.CabExtractCopyFile(notification);
			case NativeMethods.FDI.NOTIFICATIONTYPE.CLOSE_FILE_INFO:
				return this.CabExtractCloseFile(notification);
			case NativeMethods.FDI.NOTIFICATIONTYPE.NEXT_CABINET:
			{
				string text2 = Marshal.PtrToStringAnsi(notification.psz1);
				base.CabNumbers[text2] = notification.iCabinet;
				base.NextCabinetName = "?" + base.NextCabinetName;
				return 0;
			}
			}
			return 0;
		}

		private int CabExtractCopyFile(NativeMethods.FDI.NOTIFICATION notification)
		{
            if ((int)notification.iFolder != this.folderId)
            {
                if (notification.iFolder != -3 && this.folderId != -1)
                {
                    this.currentFolderNumber += 1;
                }
                this.folderId = (int)notification.iFolder;
            }
            string fileName = CabUnpacker.GetFileName(notification);
            if (this.filter == null || this.filter(fileName))
            {
                this.currentFileNumber++;
                this.currentFileName = fileName;
                this.currentFileBytesProcessed = 0L;
                this.currentFileTotalBytes = (long)notification.cb;
                base.OnProgress(ArchiveProgressType.StartFile);
                DateTime dateTime;
                CompressionEngine.DosDateAndTimeToDateTime(notification.date, notification.time, out dateTime);
                Stream stream = this.context.OpenFileWriteStream(fileName, (long)notification.cb, dateTime);
                if (stream != null)
                {
                    base.FileStream = stream;
                    return base.StreamHandles.AllocHandle(stream);
                }
                this.fileBytesProcessed += notification.cb;
                base.OnProgress(ArchiveProgressType.FinishFile);
                this.currentFileName = null;
            }
            return 0;
        }

		private int CabExtractCloseFile(NativeMethods.FDI.NOTIFICATION notification)
		{
			Stream stream = base.StreamHandles[notification.hf];
			base.StreamHandles.FreeHandle(notification.hf);
			string fileName = CabUnpacker.GetFileName(notification);
			FileAttributes fileAttributes = (FileAttributes)(notification.attribs & 39);
			if (fileAttributes == (FileAttributes)0)
			{
				fileAttributes = FileAttributes.Normal;
			}
			DateTime dateTime;
			CompressionEngine.DosDateAndTimeToDateTime(notification.date, notification.time, out dateTime);
			stream.Flush();
			this.context.CloseFileWriteStream(fileName, stream, fileAttributes, dateTime);
			base.FileStream = null;
            long num = this.currentFileTotalBytes - this.currentFileBytesProcessed;
            this.currentFileBytesProcessed += num;
            this.fileBytesProcessed += num;
            base.OnProgress(ArchiveProgressType.FinishFile);
            this.currentFileName = null;
            return 1;
        }

		private NativeMethods.FDI.Handle fdiHandle;

		private NativeMethods.FDI.PFNALLOC fdiAllocMemHandler;

		private NativeMethods.FDI.PFNFREE fdiFreeMemHandler;

		private NativeMethods.FDI.PFNOPEN fdiOpenStreamHandler;

		private NativeMethods.FDI.PFNREAD fdiReadStreamHandler;

		private NativeMethods.FDI.PFNWRITE fdiWriteStreamHandler;

		private NativeMethods.FDI.PFNCLOSE fdiCloseStreamHandler;

		private NativeMethods.FDI.PFNSEEK fdiSeekStreamHandler;

		private IUnpackStreamContext context;

		private List<ArchiveFileInfo> fileList;

		private int folderId;

		private Predicate<string> filter;
	}
}
