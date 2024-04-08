namespace Sigmath.Lex
{
	public delegate bool CharsetPredicate(char c);

	public static class Charset
	{
		/* =---- Static Properties -------------------------------------= */

		public static int CharacterDelta => 'a' - 'A';

		public static CharsetPredicate Exponent => x => x is 'E' or 'e';
		public static CharsetPredicate Sign => x => x is '+' or '-';

		/* =---- Static Methods ----------------------------------------= */

		public static bool IsWhitespace(char c)
			=> c is ((> '\x00') and (<= '\x20')) or '\x7F';

		public static bool IsEndLine(char c)
			=> c is '\n';

		// --------------------------------------------------------------

		public static bool IsDigit(char c)
			=> c is (>= '0') and (<= '9');

		// --------------------------------------------------------------

		public static bool IsBinaryDigit(char c)
			=> c is '0' or '1';

		public static bool IsOctalDigit(char c)
			=> c is (>= '0') and (<= '7');

		public static bool IsHexadecimalDigit(char c)
			=> IsDigit(c) || (c is (>= 'A') and (<= 'F')) || (c is (>= 'a') and (<= 'f'));

		// --------------------------------------------------------------

		public static bool IsRadixSpecifier(char c)
			=> c is ((>= 'B') and (<= 'D')) or ((>= 'b') and (<= 'd')) or 'X' or 'x';

		// --------------------------------------------------------------

		public static bool IsUpperAlpha(char c)
			=> c is (>= 'A') and (<= 'Z');

		public static bool IsUpperAlnum(char c)
			=> IsUpperAlpha(c) || IsDigit(c);

		// --------------------------------------------------------------

		public static bool IsLowerAlpha(char c)
			=> c is (>= 'a') and (<= 'z');

		public static bool IsLowerAlnum(char c)
			=> IsLowerAlpha(c) || IsDigit(c);

		// --------------------------------------------------------------

		public static bool IsAlpha(char c)
			=> IsUpperAlpha(c) || IsLowerAlpha(c);

		public static bool IsAlnum(char c)
			=> IsAlpha(c) || IsDigit(c);

		// --------------------------------------------------------------

		public static bool IsIdentifierPrefix(char c)
			=> IsAlnum(c) || (c is '$' or '_');

		public static bool IsIdentifierInfix(char c)
			=> IsAlnum(c) || (c is '_');

		public static bool IsIdentifierPostfix(char c)
			=> c is '\'' or '\"';

		/* =------------------------------------------------------------= */
	}
}
