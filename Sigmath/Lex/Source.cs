using System.IO;

namespace Sigmath.Lex
{
	public abstract class Source(TextReader textReader)
	{
		protected readonly TextReader _textReader = textReader;

		/* =---- Constants ---------------------------------------------= */

		public const int EOF = -1;

		/* =---- Properties --------------------------------------------= */

		public abstract int Length { get; }
		public abstract int Position { get; protected internal set; }

		public TextReader BaseReader => _textReader;

		/* =---- Methods -----------------------------------------------= */

		public abstract int Peek(int offset = 0);
		public abstract int Read(int offset = 0);

		public abstract void Skip(int count = 1);

		// --------------------------------------------------------------

		public virtual char PeekChar(int offset = 0)
		{
			int result = this.Peek(offset);

			if (result == EOF)
				return '\0';
			else
				return (char)result;
		}

		public virtual char ReadChar(int offset = 0)
		{
			int result = this.Read(offset);

			if (result == EOF)
				return '\0';
			else
				return (char)result;
		}

		/* =------------------------------------------------------------= */
	}
}
