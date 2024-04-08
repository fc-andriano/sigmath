namespace Sigmath.Lex
{
	public enum TokenCode :
		byte
	{
		Uknown,
		Ignore,

		EndOfFile,
		EndOfLine,

		/* =---- Punctuators -------------------------------------------= */

		// Brackets

		BrackLParent,
		BrackRParent,
		BrackLSquare,
		BrackRSquare,
		BrackLBraces,
		BrackRBraces,

		// Stray Operators

		PunctTilde,
		PunctQuestion,
		PunctExclaim,
		PunctAmp,
		PunctPipe,
		PunctCaret,
		PunctEqual,
		PunctPlus,
		PunctMinus,
		PunctStar,
		PunctSlash,
		PunctPercent,
		PunctGreater,
		PunctLess,
		PunctDot,
		PunctColon,
		PunctSemicolon,
		PunctComma,

		PunctAmpAmp,
		PunctPipePipe,
		PunctCaretCaret,
		PunctEqualEqual,
		PunctPlusPlus,
		PunctMinusMinus,
		PunctStarStar,
		PunctRight,
		PunctLeft,
		PunctDotDot,
		PunctColonColon,

		PunctPlusMinus,
		PunctMinusPlus,

		PunctTildeEqual,
		PunctExclaimEqual,
		PunctAmpEqual,
		PunctPipeEqual,
		PunctCaretEqual,
		PunctPlusEqual,
		PunctMinusEqual,
		PunctStarEqual,
		PunctSlashEqual,
		PunctPercentEqual,
		PunctGreaterEqual,
		PunctLessEqual,
		PunctColonEqual,

		PunctStarStarEqual,
		PunctRightEqual,
		PunctLeftEqual,

		PunctArrow,
		PunctLeftArrow,
		PunctBigArrow,
		PunctForall,
		PunctExists,
		PunctExistsOnly,
		PunctNotExists,

		PunctEllipsis,

		// --------------------------------------------------------------

		Identifier,

		/* =---- Constant Literals -------------------------------------= */

		ConstBinInteger,
		ConstOctInteger,
		ConstDecInteger,
		ConstHexInteger,
		ConstInteger,

		ConstExpReal,
		ConstReal

		/* =------------------------------------------------------------= */
	}
}
