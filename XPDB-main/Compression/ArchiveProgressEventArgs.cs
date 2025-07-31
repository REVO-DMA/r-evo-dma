namespace XPDB.Compression
{
    public class ArchiveProgressEventArgs : EventArgs
	{
		public ArchiveProgressEventArgs(ArchiveProgressType progressType, string currentFileName, int currentFileNumber, int totalFiles, long currentFileBytesProcessed, long currentFileTotalBytes, string currentArchiveName, int currentArchiveNumber, int totalArchives, long currentArchiveBytesProcessed, long currentArchiveTotalBytes, long fileBytesProcessed, long totalFileBytes)
		{
			ProgressType = progressType;
			CurrentFileName = currentFileName;
			CurrentFileNumber = currentFileNumber;
			TotalFiles = totalFiles;
			CurrentFileBytesProcessed = currentFileBytesProcessed;
			CurrentFileTotalBytes = currentFileTotalBytes;
			CurrentArchiveName = currentArchiveName;
            CurrentArchiveNumber = (short)currentArchiveNumber;
            TotalArchives = (short)totalArchives;
            CurrentArchiveBytesProcessed = currentArchiveBytesProcessed;
            CurrentArchiveTotalBytes = currentArchiveTotalBytes;
            FileBytesProcessed = fileBytesProcessed;
            TotalFileBytes = totalFileBytes;
        }

		public ArchiveProgressType ProgressType { get; private set; }

		public string CurrentFileName { get; private set; }

        public int CurrentFileNumber { get; private set; }

        public int TotalFiles { get; private set; }

        public long CurrentFileBytesProcessed { get; private set; }

        public long CurrentFileTotalBytes { get; private set; }

        public string CurrentArchiveName { get; private set; }

        public int CurrentArchiveNumber { get; private set; }

        public int TotalArchives { get; private set; }

        public long CurrentArchiveBytesProcessed { get; private set; }

        public long CurrentArchiveTotalBytes { get; private set; }

        public long FileBytesProcessed { get; private set; }

        public long TotalFileBytes { get; private set; }
	}
}
