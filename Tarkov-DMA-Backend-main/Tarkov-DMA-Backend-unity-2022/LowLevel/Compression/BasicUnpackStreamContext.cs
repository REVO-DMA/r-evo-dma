namespace Tarkov_DMA_Backend.LowLevel.Compression
{
    public class BasicUnpackStreamContext : IUnpackStreamContext
	{
		public BasicUnpackStreamContext(Stream archiveStream)
		{
			ArchiveStream = archiveStream;
		}

		public Stream FileStream
		{
			get
			{
				return fileStream;
			}
		}

		public Stream OpenArchiveReadStream(int archiveNumber, string archiveName, CompressionEngine compressionEngine)
		{
			return new DuplicateStream(ArchiveStream);
		}

		public void CloseArchiveReadStream(int archiveNumber, string archiveName, Stream stream)
		{
		}

		public Stream OpenFileWriteStream(string path, long fileSize, DateTime lastWriteTime)
		{
			fileStream = new MemoryStream(new byte[fileSize], 0, (int)fileSize, true, true);
			return fileStream;
		}

		public void CloseFileWriteStream(string path, Stream stream, FileAttributes attributes, DateTime lastWriteTime)
		{
		}

		private Stream ArchiveStream;

		private Stream fileStream;
	}
}
