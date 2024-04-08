using System;
using System.IO;

namespace Sigmath.Lex
{
	public sealed class SourceReader(TextReader textReader, int bufferSize = 1024) :
		Source(textReader)
	{
		private readonly char[] _buffer = new char[bufferSize];

		private int _len = 0;
		private int _pos = 0;

		/* =---- Static Methods ----------------------------------------= */

		public static SourceReader CreateFromText(string text)
			=> new(new StringReader(text));

		public static SourceReader CreateFromFile(string path)
			=> new(File.OpenText(path));

		/* =---- Properties --------------------------------------------= */

		public int BufferSize => bufferSize;
		public int BufferLength => _len % this.BufferSize;
		public override int Length => _len;
		public override int Position { get => _pos; protected internal set => _pos = value; }

		/* =---- Methods -----------------------------------------------= */

		private bool ChargeInternalBuffer()
		{
			int index = _len % this.BufferSize, count;

			if (index == 0)
				count = this.BaseReader.Read(_buffer, 0, this.BufferSize);
			else
				count = this.BaseReader.Read(_buffer, index, this.BufferSize - index);

			_len += count;
			_pos %= this.BufferSize;

			return count > 0;
		}

		// --------------------------------------------------------------

		private void ThrowOffsetIfOutOfRange(int offset)
		{
			if ((offset < 0) || (offset > (this.BufferSize - 1)))
				throw new ArgumentOutOfRangeException(nameof(offset), offset, $"Parameter {nameof(offset)} must be not negative and greater than {this.BufferSize - 1}");
			else
				return;
		}

		// --------------------------------------------------------------

		public override int Peek(int offset = 0)
		{
			this.ThrowOffsetIfOutOfRange(offset);

			if (((this.Position + offset) >= this.BufferLength) && !this.ChargeInternalBuffer())
				return EOF;
			else
				return _buffer[this.Position + offset];
		}

		public override int Read(int offset = 0)
		{
			this.ThrowOffsetIfOutOfRange(offset);

			if (((this.Position + offset) >= this.BufferLength) && !this.ChargeInternalBuffer())
				return EOF;
			else
				return _buffer[this.Position++ + offset];
		}

		public override void Skip(int count = 1)
		{
			this.ThrowOffsetIfOutOfRange(count);

			if (((this.Position + count) > this.BufferLength) && !this.ChargeInternalBuffer())
				throw new ArgumentOutOfRangeException(nameof(count));
			else
				this.Position += count;
		}

		/* =------------------------------------------------------------= */
	}
}
