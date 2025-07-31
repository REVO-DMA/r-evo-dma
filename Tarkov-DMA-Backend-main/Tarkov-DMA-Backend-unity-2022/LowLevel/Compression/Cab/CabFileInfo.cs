using System.Runtime.Serialization;

namespace Tarkov_DMA_Backend.LowLevel.Compression.Cab
{
	public class CabFileInfo : ArchiveFileInfo
	{
		public CabFileInfo(CabInfo cabinetInfo, string filePath)
			: base(cabinetInfo, filePath)
		{
			if (cabinetInfo == null)
			{
				throw new ArgumentNullException("cabinetInfo");
			}
			this.cabFolder = -1;
		}

		internal CabFileInfo(string filePath, int cabFolder, int cabNumber, FileAttributes attributes, DateTime lastWriteTime, long length)
			: base(filePath, cabNumber, attributes, lastWriteTime, length)
		{
			this.cabFolder = cabFolder;
		}

		protected CabFileInfo(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			this.cabFolder = info.GetInt32("cabFolder");
		}

		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue("cabFolder", this.cabFolder);
		}

		public CabInfo Cabinet
		{
			get
			{
				return (CabInfo)base.Archive;
			}
		}

		public string CabinetName
		{
			get
			{
				return base.ArchiveName;
			}
		}

		public int CabinetFolderNumber
		{
			get
			{
				if (this.cabFolder < 0)
				{
					base.Refresh();
				}
				return this.cabFolder;
			}
		}

		protected override void Refresh(ArchiveFileInfo newFileInfo)
		{
			base.Refresh(newFileInfo);
			this.cabFolder = ((CabFileInfo)newFileInfo).cabFolder;
		}

		private int cabFolder;
	}
}
