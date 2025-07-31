using System.Runtime.Serialization;

namespace Tarkov_DMA_Backend.LowLevel.Compression.Cab
{
	public class CabInfo : ArchiveInfo
	{
		public CabInfo(string path) : base(path) { }

		protected CabInfo(SerializationInfo info, StreamingContext context) : base(info, context) { }

		protected override CompressionEngine CreateCompressionEngine()
		{
			return new CabEngine();
		}

		public new IList<CabFileInfo> GetFiles()
		{
			IList<ArchiveFileInfo> files = base.GetFiles();
			List<CabFileInfo> list = new List<CabFileInfo>(files.Count);
			foreach (ArchiveFileInfo archiveFileInfo in files)
			{
				CabFileInfo cabFileInfo = (CabFileInfo)archiveFileInfo;
				list.Add(cabFileInfo);
			}
			return list.AsReadOnly();
		}

		public new IList<CabFileInfo> GetFiles(string searchPattern)
		{
			IList<ArchiveFileInfo> files = base.GetFiles(searchPattern);
			List<CabFileInfo> list = new List<CabFileInfo>(files.Count);
			foreach (ArchiveFileInfo archiveFileInfo in files)
			{
				CabFileInfo cabFileInfo = (CabFileInfo)archiveFileInfo;
				list.Add(cabFileInfo);
			}
			return list.AsReadOnly();
		}
	}
}
