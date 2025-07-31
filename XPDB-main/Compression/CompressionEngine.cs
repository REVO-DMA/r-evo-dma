using System.Globalization;

namespace XPDB.Compression
{
    public abstract class CompressionEngine : IDisposable
	{
		protected CompressionEngine()
		{
			compressionLevel = ECompressionLevel.Normal;
		}

		~CompressionEngine()
		{
			Dispose(false);
		}

		public event EventHandler<ArchiveProgressEventArgs> Progress
		{
			add
			{
				EventHandler<ArchiveProgressEventArgs> eventHandler = ProgressField;
				EventHandler<ArchiveProgressEventArgs> eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					EventHandler<ArchiveProgressEventArgs> eventHandler3 = (EventHandler<ArchiveProgressEventArgs>)Delegate.Combine(eventHandler2, value);
					eventHandler = Interlocked.CompareExchange<EventHandler<ArchiveProgressEventArgs>>(ref ProgressField, eventHandler3, eventHandler2);
				}
				while (eventHandler != eventHandler2);
			}
			remove
			{
				EventHandler<ArchiveProgressEventArgs> eventHandler = ProgressField;
				EventHandler<ArchiveProgressEventArgs> eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					EventHandler<ArchiveProgressEventArgs> eventHandler3 = (EventHandler<ArchiveProgressEventArgs>)Delegate.Remove(eventHandler2, value);
					eventHandler = Interlocked.CompareExchange<EventHandler<ArchiveProgressEventArgs>>(ref ProgressField, eventHandler3, eventHandler2);
				}
				while (eventHandler != eventHandler2);
			}
		}

		public bool UseTempFiles
		{
			get
			{
				return !dontUseTempFiles;
			}
			set
			{
				dontUseTempFiles = !value;
			}
		}

		public ECompressionLevel CompressionLevel
		{
			get
			{
				return compressionLevel;
			}
			set
			{
				compressionLevel = value;
			}
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		public void Pack(IPackStreamContext streamContext, IEnumerable<string> files)
		{
            ArgumentNullException.ThrowIfNull(files);
            Pack(streamContext, files, 0);
		}

		public abstract void Pack(IPackStreamContext streamContext, IEnumerable<string> files, long maxArchiveSize);

		public abstract bool IsArchive(Stream stream);

		public virtual long FindArchiveOffset(Stream stream)
		{
            ArgumentNullException.ThrowIfNull(stream);
            long num = 4;
			long length = stream.Length;
            for (long num2 = 0; num2 <= length - num; num2 += num)
            {
                stream.Seek(num2, SeekOrigin.Begin);
                if (IsArchive(stream))
                {
                    return num2;
                }
            }
            return -1;
        }

		public List<ArchiveFileInfo> GetFileInfo(Stream stream)
		{
			return GetFileInfo(new BasicUnpackStreamContext(stream), null);
		}

		public abstract List<ArchiveFileInfo> GetFileInfo(IUnpackStreamContext streamContext, Predicate<string> fileFilter);

		public List<string> GetFiles(Stream stream)
		{
			return GetFiles(new BasicUnpackStreamContext(stream), null);
		}

		public List<string> GetFiles(IUnpackStreamContext streamContext, Predicate<string> fileFilter)
		{
            ArgumentNullException.ThrowIfNull(streamContext);
            List<ArchiveFileInfo> fileInfo = GetFileInfo(streamContext, fileFilter);
			List<string> list = new(fileInfo.Count);

            for (int i = 0; i < fileInfo.Count; i++)
            {
                list.Add(fileInfo[i].Name);
            }
            return list;
        }

		public Stream Unpack(Stream stream, string path)
		{
            ArgumentNullException.ThrowIfNull(stream);
            ArgumentNullException.ThrowIfNull(path);
            BasicUnpackStreamContext basicUnpackStreamContext = new BasicUnpackStreamContext(stream);
			Unpack(basicUnpackStreamContext, (string match) => string.Compare(match, path, true, CultureInfo.InvariantCulture) == 0);
			Stream fileStream = basicUnpackStreamContext.FileStream;
			if (fileStream != null)
			{
				fileStream.Position = 0;
			}
			return fileStream;
		}

		public abstract void Unpack(IUnpackStreamContext streamContext, Predicate<string> fileFilter);

		protected void OnProgress(ArchiveProgressEventArgs e)
		{
            ProgressField?.Invoke(this, e);
        }

		protected virtual void Dispose(bool disposing) { }

		public static void DosDateAndTimeToDateTime(short dosDate, short dosTime, out DateTime dateTime)
		{
			if (dosDate == 0 && dosTime == 0)
			{
				dateTime = DateTime.MinValue;
				return;
			}

			long num;
			SafeNativeMethods.DosDateTimeToFileTime(dosDate, dosTime, out num);
			dateTime = DateTime.FromFileTimeUtc(num);
			dateTime = new DateTime(dateTime.Ticks, DateTimeKind.Local);
		}

		public static void DateTimeToDosDateAndTime(DateTime dateTime, out short dosDate, out short dosTime)
		{
			dateTime = new DateTime(dateTime.Ticks, DateTimeKind.Utc);
			long num = dateTime.ToFileTimeUtc();
			SafeNativeMethods.FileTimeToDosDateTime(ref num, out dosDate, out dosTime);
		}

		private ECompressionLevel compressionLevel;

		private bool dontUseTempFiles;

		private EventHandler<ArchiveProgressEventArgs> ProgressField;
	}
}
