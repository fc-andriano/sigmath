namespace Sigmath.Lex
{
	public static class TokenCodeExtensions
	{
		/* =---- Extension Methods -------------------------------------= */

		public static TokenKind GetTokenKind(this TokenCode code)
		{
			TokenKind kind;

			switch (code)
			{
			case TokenCode.Ignore
			  or TokenCode.EndOfFile
			  or TokenCode.EndOfLine:
				kind = TokenKind.Ignored;
				break;

			case TokenCode.ConstBinInteger
			  or TokenCode.ConstOctInteger
			  or TokenCode.ConstDecInteger
			  or TokenCode.ConstHexInteger
			  or TokenCode.ConstInteger
			  or TokenCode.ConstExpReal
			  or TokenCode.ConstReal:
				kind = TokenKind.Constant;
				break;

			case TokenCode.PunctQuestion
			  or TokenCode.PunctAmp
			  or TokenCode.PunctPipe
			  or TokenCode.PunctCaret
			  or TokenCode.PunctEqual
			  or TokenCode.PunctPlus
			  or TokenCode.PunctMinus
			  or TokenCode.PunctStar
			  or TokenCode.PunctSlash
			  or TokenCode.PunctPercent
			  or TokenCode.PunctGreater
			  or TokenCode.PunctLess
			  or TokenCode.PunctDot
			  or TokenCode.PunctColon
			  or TokenCode.PunctAmpAmp
			  or TokenCode.PunctPipePipe
			  or TokenCode.PunctCaretCaret
			  or TokenCode.PunctEqualEqual
			  or TokenCode.PunctStarStar
			  or TokenCode.PunctRight
			  or TokenCode.PunctLeft
			  or TokenCode.PunctColonColon
			  or TokenCode.PunctPlusMinus
			  or TokenCode.PunctMinusPlus
			  or TokenCode.PunctTildeEqual
			  or TokenCode.PunctExclaimEqual
			  or TokenCode.PunctAmpEqual
			  or TokenCode.PunctPipeEqual
			  or TokenCode.PunctCaretEqual
			  or TokenCode.PunctPlusEqual
			  or TokenCode.PunctMinusEqual
			  or TokenCode.PunctStarEqual
			  or TokenCode.PunctSlashEqual
			  or TokenCode.PunctPercentEqual
			  or TokenCode.PunctGreaterEqual
			  or TokenCode.PunctLessEqual
			  or TokenCode.PunctColonEqual
			  or TokenCode.PunctStarStarEqual
			  or TokenCode.PunctRightEqual
			  or TokenCode.PunctLeftEqual
			  or TokenCode.PunctArrow
			  or TokenCode.PunctLeftArrow
			  or TokenCode.PunctBigArrow:
				kind = TokenKind.BinaryOperator;
				break;

			default:
				kind = TokenKind.Invalid;
				break;
			}

			return kind;
		}

		/* =------------------------------------------------------------= */
	}
}
