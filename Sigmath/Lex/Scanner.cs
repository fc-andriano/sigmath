using System.Text;

namespace Sigmath.Lex
{
	public sealed class Scanner(Source source)
	{
		private readonly StringBuilder _lexemeBuilder = new();

		/* =---- Properties --------------------------------------------= */

		public Source Source => source;

		/* =---- Methods -----------------------------------------------= */

		public string GetLexeme(int startIndex, int length)
			=> _lexemeBuilder.ToString(startIndex, length);

		public string GetLexeme()
			=> _lexemeBuilder.ToString();

		// --------------------------------------------------------------

		public void ClearLexeme()
			=> _lexemeBuilder.Clear();

		// --------------------------------------------------------------

		public void Append(char c)
			=> _lexemeBuilder.Append(c);

		public void Append(string s)
			=> _lexemeBuilder.Append(s);

		// --------------------------------------------------------------

		public void Consume(int offset = 0)
			=> this.Append(source.ReadChar(offset));

		public void Consume(int count, int offset = 0)
		{
			do
				this.Append(source.ReadChar(offset));
			while (--count > 0);
		}

		// --------------------------------------------------------------

		public void Skip(int count = 1)
			=> source.Skip(count);

		// --------------------------------------------------------------

		public bool Match(char c, int offset = 0)
			=> c == source.PeekChar(offset);

		public bool Match(CharsetPredicate predicate, int offset = 0)
			=> predicate(source.PeekChar(offset));

		// --------------------------------------------------------------

		public bool Scan(char c, int offset = 0)
		{
			bool result = true;

			if (this.Match(c, offset))
				this.Consume(offset);
			else
				result = false;

			return result;
		}

		public bool Scan(CharsetPredicate predicate, int offset = 0, bool once = false)
		{
			bool result = true;

			if (this.Match(predicate, offset))
			{
				int count = 0;

				do
					count++;
				while (!once && this.Match(predicate, offset + count));

				this.Consume(count, offset);
			}
			else
			{
				result = false;
			}

			return result;
		}

		// --------------------------------------------------------------

		public bool Pass(char c, int offset = 0)
		{
			bool result = true;

			if (this.Match(c, offset))
				this.Skip(offset + 1);
			else
				result = false;

			return result;
		}

		public bool Pass(CharsetPredicate predicate, int offset = 0, bool once = false)
		{
			bool result = true;

			if (this.Match(predicate, offset))
			{
				int count = 0;

				do
					count++;
				while (!once && this.Match(predicate, offset + count));

				this.Skip(count);
			}
			else
			{
				result = false;
			}

			return result;
		}

		// --------------------------------------------------------------

		public bool PassNumberWithBasePrefix(char lowerPrefixLetter, CharsetPredicate predicate, int offset = 0)
		{
			bool result;
			char upperPrefixLetter = (char)(lowerPrefixLetter - Charset.CharacterDelta);

			if (this.Pass(c => c == upperPrefixLetter || c == lowerPrefixLetter, offset, once: true))
				result = this.Scan(predicate, offset);
			else
				result = false;

			return result;
		}

		public bool PassNumberExponentSuffix(CharsetPredicate predicate, int offset = 0)
		{
			bool result;

			if (this.Pass(Charset.Exponent, offset, once: true))
			{
				this.Scan(Charset.Sign, offset, once: true);
				result = this.Scan(predicate, offset);
			}
			else
			{
				result = false;
			}

			return result;
		}

		/* =------------------------------------------------------------= */
	}
}
