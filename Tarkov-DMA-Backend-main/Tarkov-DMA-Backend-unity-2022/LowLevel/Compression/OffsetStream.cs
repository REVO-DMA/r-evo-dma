namespace Tarkov_DMA_Backend.LowLevel.Compression
{
    public class OffsetStream : Stream
	{
		public OffsetStream(Stream source, long offset)
		{
            ArgumentNullException.ThrowIfNull(source);
            Source = source;
			SourceOffset = offset;
			Source.Seek(SourceOffset, SeekOrigin.Current);
		}

		public Stream Source { get; private set; }

		public long SourceOffset { get; private set; }

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
				return Source.Length - SourceOffset;
			}
		}

		public override long Position
		{
			get
			{
				return Source.Position - SourceOffset;
			}

			set
			{
				Source.Position = value + SourceOffset;
			}
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			return Source.Read(buffer, offset, count);
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			Source.Write(buffer, offset, count);
		}

		public override int ReadByte()
		{
			return Source.ReadByte();
		}

		public override void WriteByte(byte value)
		{
			Source.WriteByte(value);
		}

		public override void Flush()
		{
			Source.Flush();
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			return Source.Seek(offset + ((origin == SeekOrigin.Begin) ? SourceOffset : 0L), origin) - SourceOffset;
		}

		public override void SetLength(long value)
		{
			Source.SetLength(value + SourceOffset);
		}

		public override void Close()
		{
			Source.Close();
		}
	}
}
