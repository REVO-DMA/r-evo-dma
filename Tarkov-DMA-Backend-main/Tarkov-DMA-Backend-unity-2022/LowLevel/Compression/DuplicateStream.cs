namespace Tarkov_DMA_Backend.LowLevel.Compression
{
    public class DuplicateStream : Stream
	{
        private long position;

        public DuplicateStream(Stream source)
		{
            ArgumentNullException.ThrowIfNull(source);
            Source = OriginalStream(source);
		}

		public Stream Source {  get; private set; }

		public override bool CanRead
		{
			get
			{
				return Source.CanRead;
			}
		}

		public override bool CanWrite
		{
			get
			{
				return Source.CanWrite;
			}
		}

		public override bool CanSeek
		{
			get
			{
				return Source.CanSeek;
			}
		}

		public override long Length
		{
			get
			{
				return Source.Length;
			}
		}

		public override long Position
		{
			get
			{
				return position;
			}

			set
			{
				position = value;
			}
		}

		public static Stream OriginalStream(Stream stream)
		{
			DuplicateStream duplicateStream = stream as DuplicateStream;
			if (duplicateStream == null)
			{
				return stream;
			}
			return duplicateStream.Source;
		}

		public override void Flush()
		{
			Source.Flush();
		}

		public override void SetLength(long value)
		{
			Source.SetLength(value);
		}

		public override void Close()
		{
			Source.Close();
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			long num = Source.Position;
			Source.Position = position;
			int num2 = Source.Read(buffer, offset, count);
			position = Source.Position;
			Source.Position = num;
			return num2;
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			long num = Source.Position;
			Source.Position = position;
			Source.Write(buffer, offset, count);
			position = Source.Position;
			Source.Position = num;
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			long num = 0L;
			if (origin == SeekOrigin.Current)
			{
				num = position;
			}
			else if (origin == SeekOrigin.End)
			{
				num = Length;
			}
			position = num + offset;
			return position;
		}
	}
}
