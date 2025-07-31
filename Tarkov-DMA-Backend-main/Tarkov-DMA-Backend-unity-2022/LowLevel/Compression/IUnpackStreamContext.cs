namespace Tarkov_DMA_Backend.LowLevel.Compression
{
    public interface IUnpackStreamContext
	{
		Stream OpenArchiveReadStream(int archiveNumber, string archiveName, CompressionEngine compressionEngine);

		void CloseArchiveReadStream(int archiveNumber, string archiveName, Stream stream);

		Stream OpenFileWriteStream(string path, long fileSize, DateTime lastWriteTime);

		void CloseFileWriteStream(string path, Stream stream, FileAttributes attributes, DateTime lastWriteTime);
	}
}
