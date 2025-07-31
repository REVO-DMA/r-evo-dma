using System.Runtime.Serialization;
using System.Security.Permissions;

namespace XPDB.Compression
{
    [Serializable]
	public abstract class ArchiveFileInfo : FileSystemInfo
	{
		protected ArchiveFileInfo(ArchiveInfo archiveInfo, string filePath)
		{
			if (filePath == null)
			{
				throw new ArgumentNullException("filePath");
			}
			this.Archive = archiveInfo;
			this.name = global::System.IO.Path.GetFileName(filePath);
			this.path = global::System.IO.Path.GetDirectoryName(filePath);
			this.attributes = FileAttributes.Normal;
			this.lastWriteTime = DateTime.MinValue;
		}

		protected ArchiveFileInfo(string filePath, int archiveNumber, FileAttributes attributes, DateTime lastWriteTime, long length)
			: this(null, filePath)
		{
			this.exists = true;
			this.archiveNumber = archiveNumber;
			this.attributes = attributes;
			this.lastWriteTime = lastWriteTime;
			this.length = length;
			this.initialized = true;
		}

		protected ArchiveFileInfo(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			this.archiveInfo = (ArchiveInfo)info.GetValue("archiveInfo", typeof(ArchiveInfo));
			this.name = info.GetString("name");
			this.path = info.GetString("path");
			this.initialized = info.GetBoolean("initialized");
			this.exists = info.GetBoolean("exists");
			this.archiveNumber = info.GetInt32("archiveNumber");
			this.attributes = (FileAttributes)info.GetValue("attributes", typeof(FileAttributes));
			this.lastWriteTime = info.GetDateTime("lastWriteTime");
			this.length = info.GetInt64("length");
		}

		public override string Name
		{
			get
			{
				return this.name;
			}
		}

		public string Path
		{
			get
			{
				return this.path;
			}
		}

		public override string FullName
		{
			get
			{
				string text = global::System.IO.Path.Combine(this.Path, this.Name);
				if (this.Archive != null)
				{
					text = global::System.IO.Path.Combine(this.ArchiveName, text);
				}
				return text;
			}
		}

		public ArchiveInfo Archive
		{
			get
			{
				return this.archiveInfo;
			}
			internal set
			{
				this.archiveInfo = value;
				this.OriginalPath = ((value != null) ? value.FullName : null);
				this.FullPath = this.OriginalPath;
			}
		}

		public string ArchiveName
		{
			get
			{
				if (this.Archive == null)
				{
					return null;
				}
				return this.Archive.FullName;
			}
		}

		public int ArchiveNumber
		{
			get
			{
				return this.archiveNumber;
			}
		}

		public override bool Exists
		{
			get
			{
				if (!this.initialized)
				{
					this.Refresh();
				}
				return this.exists;
			}
		}

		public long Length
		{
			get
			{
				if (!this.initialized)
				{
					this.Refresh();
				}
				return this.length;
			}
		}

		public new FileAttributes Attributes
		{
			get
			{
				if (!this.initialized)
				{
					this.Refresh();
				}
				return this.attributes;
			}
		}

		public new DateTime LastWriteTime
		{
			get
			{
				if (!this.initialized)
				{
					this.Refresh();
				}
				return this.lastWriteTime;
			}
		}

		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue("archiveInfo", this.archiveInfo);
			info.AddValue("name", this.name);
			info.AddValue("path", this.path);
			info.AddValue("initialized", this.initialized);
			info.AddValue("exists", this.exists);
			info.AddValue("archiveNumber", this.archiveNumber);
			info.AddValue("attributes", this.attributes);
			info.AddValue("lastWriteTime", this.lastWriteTime);
			info.AddValue("length", this.length);
		}

		public override string ToString()
		{
			return this.FullName;
		}

		public override void Delete()
		{
			throw new NotSupportedException();
		}

		public new void Refresh()
		{
			base.Refresh();
			if (this.Archive != null)
			{
				string text = global::System.IO.Path.Combine(this.Path, this.Name);
				ArchiveFileInfo file = this.Archive.GetFile(text);
				if (file == null)
				{
					throw new FileNotFoundException("File not found in archive.", text);
				}
				this.Refresh(file);
			}
		}

		public void CopyTo(string destFileName)
		{
			this.CopyTo(destFileName, false);
		}

		public void CopyTo(string destFileName, bool overwrite)
		{
			if (destFileName == null)
			{
				throw new ArgumentNullException("destFileName");
			}
			if (!overwrite && File.Exists(destFileName))
			{
				throw new IOException();
			}
			if (this.Archive == null)
			{
				throw new InvalidOperationException();
			}
			this.Archive.UnpackFile(global::System.IO.Path.Combine(this.Path, this.Name), destFileName);
		}

		public Stream OpenRead()
		{
			return this.Archive.OpenRead(global::System.IO.Path.Combine(this.Path, this.Name));
		}

		public StreamReader OpenText()
		{
			return this.Archive.OpenText(global::System.IO.Path.Combine(this.Path, this.Name));
		}

		protected virtual void Refresh(ArchiveFileInfo newFileInfo)
		{
			this.exists = newFileInfo.exists;
			this.length = newFileInfo.length;
			this.attributes = newFileInfo.attributes;
			this.lastWriteTime = newFileInfo.lastWriteTime;
		}

		private ArchiveInfo archiveInfo;

		private string name;

		private string path;

		private bool initialized;

		private bool exists;

		private int archiveNumber;

		private FileAttributes attributes;

		private DateTime lastWriteTime;

		private long length;
	}
}
