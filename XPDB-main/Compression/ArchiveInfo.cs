using System.Globalization;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;

namespace XPDB.Compression
{
	public abstract class ArchiveInfo : FileSystemInfo
	{
		protected ArchiveInfo(string path)
		{
            ArgumentNullException.ThrowIfNull(path);
            OriginalPath = path;
			FullPath = Path.GetFullPath(path);
		}

		protected ArchiveInfo(SerializationInfo info, StreamingContext context) : base(info, context) { }

		public DirectoryInfo Directory
		{
			get
			{
				return new DirectoryInfo(Path.GetDirectoryName(FullName));
			}
		}

		public string DirectoryName
		{
			get
			{
				return Path.GetDirectoryName(FullName);
			}
		}

		public long Length
		{
			get
			{
				return new FileInfo(FullName).Length;
			}
		}

		public override string Name
		{
			get
			{
				return Path.GetFileName(FullName);
			}
		}

		public override bool Exists
		{
			get
			{
				return File.Exists(FullName);
			}
		}

		public override string ToString()
		{
			return FullName;
		}

		public override void Delete()
		{
			File.Delete(FullName);
		}

		public void CopyTo(string destFileName)
		{
			File.Copy(FullName, destFileName);
		}

		public void CopyTo(string destFileName, bool overwrite)
		{
			File.Copy(FullName, destFileName, overwrite);
		}

		public void MoveTo(string destFileName)
		{
			File.Move(FullName, destFileName);
			FullPath = Path.GetFullPath(destFileName);
		}

		public bool IsValid()
		{
			bool flag;
			using (Stream stream = File.OpenRead(FullName))
			{
				using (CompressionEngine compressionEngine = CreateCompressionEngine())
				{
					flag = compressionEngine.FindArchiveOffset(stream) >= 0L;
				}
			}
			return flag;
		}

		public IList<ArchiveFileInfo> GetFiles()
		{
			return InternalGetFiles(null);
		}

		public IList<ArchiveFileInfo> GetFiles(string searchPattern)
		{
			if (searchPattern == null)
			{
				throw new ArgumentNullException("searchPattern");
			}
			string text = string.Format(CultureInfo.InvariantCulture, "^{0}$", new object[] { Regex.Escape(searchPattern).Replace("\\*", ".*").Replace("\\?", ".") });
			Regex regex = new Regex(text, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
			return InternalGetFiles((string match) => regex.IsMatch(match));
		}

		public void Unpack(string destDirectory)
		{
			Unpack(destDirectory, null);
		}

		public void Unpack(string destDirectory, EventHandler<ArchiveProgressEventArgs> progressHandler)
		{
			using CompressionEngine compressionEngine = CreateCompressionEngine();
            compressionEngine.Progress += progressHandler;
            compressionEngine.Unpack(new ArchiveFileStreamContext(FullName, destDirectory, null)
            {
                EnableOffsetOpen = true
            }, null);
        }

		public void UnpackFile(string fileName, string destFileName)
		{
			ArgumentNullException.ThrowIfNull(fileName);
			ArgumentNullException.ThrowIfNull(destFileName);

			UnpackFiles(new string[] { fileName }, null, new string[] { destFileName });
		}

		public void UnpackFiles(IList<string> fileNames, string destDirectory, IList<string> destFileNames)
		{
			UnpackFiles(fileNames, destDirectory, destFileNames, null);
		}

		public void UnpackFiles(IList<string> fileNames, string destDirectory, IList<string> destFileNames, EventHandler<ArchiveProgressEventArgs> progressHandler)
		{
			ArgumentNullException.ThrowIfNull(fileNames);
			if (destFileNames == null)
			{
				ArgumentNullException.ThrowIfNull(destDirectory);
				destFileNames = fileNames;
			}
			if (destFileNames.Count != fileNames.Count)
			{
				throw new ArgumentOutOfRangeException("destFileNames");
			}
			IDictionary<string, string> dictionary = ArchiveInfo.CreateStringDictionary(fileNames, destFileNames);
			UnpackFileSet(dictionary, destDirectory, progressHandler);
		}

		public void UnpackFileSet(IDictionary<string, string> fileNames, string destDirectory)
		{
			UnpackFileSet(fileNames, destDirectory, null);
		}

		public void UnpackFileSet(IDictionary<string, string> fileNames, string destDirectory, EventHandler<ArchiveProgressEventArgs> progressHandler)
		{
			ArgumentNullException.ThrowIfNull(fileNames);
			using CompressionEngine compressionEngine = CreateCompressionEngine();
			compressionEngine.Progress += progressHandler;
			compressionEngine.Unpack(new ArchiveFileStreamContext(FullName, destDirectory, fileNames)
			{
				EnableOffsetOpen = true
			}, (string match) => fileNames.ContainsKey(match));
		}

		public Stream OpenRead(string fileName)
		{
			Stream stream = File.OpenRead(FullName);
			CompressionEngine compressionEngine = CreateCompressionEngine();
			return new CargoStream(compressionEngine.Unpack(stream, fileName), new IDisposable[] { stream, compressionEngine });
		}

		public StreamReader OpenText(string fileName)
		{
			return new StreamReader(OpenRead(fileName));
		}

		public void Pack(string sourceDirectory)
		{
			Pack(sourceDirectory, false, ECompressionLevel.Max, null);
		}

		public void Pack(string sourceDirectory, bool includeSubdirectories, ECompressionLevel compLevel, EventHandler<ArchiveProgressEventArgs> progressHandler)
		{
			IList<string> relativeFilePathsInDirectoryTree = GetRelativeFilePathsInDirectoryTree(sourceDirectory, includeSubdirectories);
			IList<string> list = relativeFilePathsInDirectoryTree;
			PackFiles(sourceDirectory, list, list, compLevel, progressHandler);
		}

		public void PackFiles(string sourceDirectory, IList<string> sourceFileNames, IList<string> fileNames)
		{
			PackFiles(sourceDirectory, sourceFileNames, fileNames, ECompressionLevel.Max, null);
		}

		public void PackFiles(string sourceDirectory, IList<string> sourceFileNames, IList<string> fileNames, ECompressionLevel compLevel, EventHandler<ArchiveProgressEventArgs> progressHandler)
		{
			ArgumentNullException.ThrowIfNull(sourceFileNames);

			if (fileNames == null)
			{
				string[] array = new string[sourceFileNames.Count];
				for (int i = 0; i < sourceFileNames.Count; i++)
				{
					array[i] = Path.GetFileName(sourceFileNames[i]);
				}
				fileNames = array;
			}
			else if (fileNames.Count != sourceFileNames.Count)
			{
				throw new ArgumentOutOfRangeException(nameof(fileNames));
			}
			using (CompressionEngine compressionEngine = CreateCompressionEngine())
			{
				compressionEngine.Progress += progressHandler;
				IDictionary<string, string> dictionary = ArchiveInfo.CreateStringDictionary(fileNames, sourceFileNames);
				ArchiveFileStreamContext archiveFileStreamContext = new ArchiveFileStreamContext(FullName, sourceDirectory, dictionary);
				archiveFileStreamContext.EnableOffsetOpen = true;
				compressionEngine.CompressionLevel = compLevel;
				compressionEngine.Pack(archiveFileStreamContext, fileNames);
			}
		}

		public void PackFileSet(string sourceDirectory, IDictionary<string, string> fileNames)
		{
			PackFileSet(sourceDirectory, fileNames, ECompressionLevel.Max, null);
		}

		public void PackFileSet(string sourceDirectory, IDictionary<string, string> fileNames, ECompressionLevel compLevel, EventHandler<ArchiveProgressEventArgs> progressHandler)
		{
            ArgumentNullException.ThrowIfNull(fileNames);
            string[] array = new string[fileNames.Count];
			fileNames.Keys.CopyTo(array, 0);
			using (CompressionEngine compressionEngine = CreateCompressionEngine())
			{
				compressionEngine.Progress += progressHandler;
				ArchiveFileStreamContext archiveFileStreamContext = new ArchiveFileStreamContext(FullName, sourceDirectory, fileNames);
				archiveFileStreamContext.EnableOffsetOpen = true;
				compressionEngine.CompressionLevel = compLevel;
				compressionEngine.Pack(archiveFileStreamContext, array);
			}
		}

		internal IList<string> GetRelativeFilePathsInDirectoryTree(string dir, bool includeSubdirectories)
		{
			IList<string> list = new List<string>();
			RecursiveGetRelativeFilePathsInDirectoryTree(dir, string.Empty, includeSubdirectories, list);
			return list;
		}

		internal ArchiveFileInfo GetFile(string path)
		{
			IList<ArchiveFileInfo> list = InternalGetFiles((string match) => string.Compare(match, path, true, CultureInfo.InvariantCulture) == 0);
			if (list == null || list.Count <= 0)
			{
				return null;
			}
			return list[0];
		}

		protected abstract CompressionEngine CreateCompressionEngine();

		private static IDictionary<string, string> CreateStringDictionary(IList<string> keys, IList<string> values)
		{
			Dictionary<string, string> dictionary = new(StringComparer.OrdinalIgnoreCase);
            for (int i = 0; i < keys.Count; i++)
            {
                dictionary.Add(keys[i], values[i]);
            }
            return dictionary;
        }

		private void RecursiveGetRelativeFilePathsInDirectoryTree(string dir, string relativeDir, bool includeSubdirectories, IList<string> fileList)
		{
			string[] array = global::System.IO.Directory.GetFiles(dir);
			for (int i = 0; i < array.Length; i++)
			{
				string fileName = Path.GetFileName(array[i]);
				fileList.Add(Path.Combine(relativeDir, fileName));
			}
			if (includeSubdirectories)
			{
				array = global::System.IO.Directory.GetDirectories(dir);
				for (int i = 0; i < array.Length; i++)
				{
					string fileName2 = Path.GetFileName(array[i]);
					RecursiveGetRelativeFilePathsInDirectoryTree(Path.Combine(dir, fileName2), Path.Combine(relativeDir, fileName2), includeSubdirectories, fileList);
				}
			}
		}

		private List<ArchiveFileInfo> InternalGetFiles(Predicate<string> fileFilter)
		{
            List<ArchiveFileInfo> list;
            using (CompressionEngine compressionEngine = CreateCompressionEngine())
            {
                List<ArchiveFileInfo> fileInfo = compressionEngine.GetFileInfo(new ArchiveFileStreamContext(FullName, null, null)
                {
                    EnableOffsetOpen = true
                }, fileFilter);
                for (int i = 0; i < fileInfo.Count; i++)
                {
                    fileInfo[i].Archive = this;
                }
                list = fileInfo;
            }
            return list;
        }
	}
}
