using System;

namespace Sigmath.Lex
{
	public class LexerException(string? message, Exception? innerException) :
		Exception(message, innerException)
	{
		/* =---- Static Methods ----------------------------------------= */

		public static LexerException GetInvalidError(Lexer lex, string? what, Exception? innerException = null)
		{
			string message;

			if (what is null)
				message = $"Invalid character sequence '{lex.GetLexeme()}'";
			else
				message = $"Invalid {what} (got '{lex.GetLexeme()}')";

			return new(message, innerException);
		}

		/* =------------------------------------------------------------= */
	}
}
