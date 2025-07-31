namespace Tarkov_DMA_Backend.LowLevel.Compression
{
    public interface IPackStreamContext
	{
		string GetArchiveName(int archiveNumber);

		Stream OpenArchiveWriteStream(int archiveNumber, string archiveName, bool truncate, CompressionEngine compressionEngine);

		void CloseArchiveWriteStream(int archiveNumber, string archiveName, Stream stream);

		Stream OpenFileReadStream(string path, out FileAttributes attributes, out DateTime lastWriteTime);

		void CloseFileReadStream(string path, Stream stream);

		object GetOption(string optionName, object[] parameters);
	}
}
