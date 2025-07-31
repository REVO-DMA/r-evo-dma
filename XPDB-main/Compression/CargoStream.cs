namespace XPDB.Compression
{
    public class CargoStream : Stream
	{
		public CargoStream(Stream source, params IDisposable[] cargo)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			this.source = source;
			this.cargo = new List<IDisposable>(cargo);
		}

		public Stream Source
		{
			get
			{
				return this.source;
			}
		}

		public IList<IDisposable> Cargo
		{
			get
			{
				return this.cargo;
			}
		}

		public override bool CanRead
		{
			get
			{
				return this.source.CanRead;
			}
		}

		public override bool CanWrite
		{
			get
			{
				return this.source.CanWrite;
			}
		}

		public override bool CanSeek
		{
			get
			{
				return this.source.CanSeek;
			}
		}

		public override long Length
		{
			get
			{
				return this.source.Length;
			}
		}

		public override long Position
		{
			get
			{
				return this.source.Position;
			}
			set
			{
				this.source.Position = value;
			}
		}

		public override void Flush()
		{
			this.source.Flush();
		}

		public override void SetLength(long value)
		{
			this.source.SetLength(value);
		}

		public override void Close()
		{
			this.source.Close();
			foreach (IDisposable disposable in this.cargo)
			{
				disposable.Dispose();
			}
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			return this.source.Read(buffer, offset, count);
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			this.source.Write(buffer, offset, count);
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			return this.source.Seek(offset, origin);
		}

		private Stream source;

		private List<IDisposable> cargo;
	}
}
