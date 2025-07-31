namespace Tarkov_DMA_Backend.LowLevel.Compression.Cab
{
    public class CabEngine : CompressionEngine
	{
		public CabEngine() { }

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (this.packer != null)
				{
					this.packer.Dispose();
					this.packer = null;
				}
				if (this.unpacker != null)
				{
					this.unpacker.Dispose();
					this.unpacker = null;
				}
			}
			base.Dispose(disposing);
		}

		private CabPacker Packer
		{
			get
			{
				if (this.packer == null)
				{
					this.packer = new CabPacker(this);
				}
				return this.packer;
			}
		}

		private CabUnpacker Unpacker
		{
			get
			{
				if (this.unpacker == null)
				{
					this.unpacker = new CabUnpacker(this);
				}
				return this.unpacker;
			}
		}

		public override void Pack(IPackStreamContext streamContext, IEnumerable<string> files, long maxArchiveSize)
		{
			this.Packer.CompressionLevel = base.CompressionLevel;
			this.Packer.UseTempFiles = base.UseTempFiles;
			this.Packer.Pack(streamContext, files, maxArchiveSize);
		}

		public override bool IsArchive(Stream stream)
		{
			return this.Unpacker.IsArchive(stream);
		}

		public override List<ArchiveFileInfo> GetFileInfo(IUnpackStreamContext streamContext, Predicate<string> fileFilter)
		{
			return this.Unpacker.GetFileInfo(streamContext, fileFilter);
		}

		public override void Unpack(IUnpackStreamContext streamContext, Predicate<string> fileFilter)
		{
			this.Unpacker.Unpack(streamContext, fileFilter);
		}

		internal void ReportProgress(ArchiveProgressEventArgs e)
		{
			base.OnProgress(e);
		}

		private CabPacker packer;

		private CabUnpacker unpacker;
	}
}
