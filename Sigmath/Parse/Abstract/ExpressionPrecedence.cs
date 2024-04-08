namespace Sigmath.Parse.Abstract
{
	public enum ExpressionPrecedence :
		byte
	{
		None = 0x00,

		/* =---- Unary -------------------------------------------------= */

		UnaryArithmetic = 0x24,
		UnaryBitwise = 0x22,
		UnaryLogical = 0x20,

		/* =---- Binary ------------------------------------------------= */

		BinaryArithmeticProduct = 0x4F,
		BinaryArithmeticSum = 0x4E,
		BinaryBitwise = 0x48,
		BinaryRelational = 0x44,
		BinaryComparison = 0x42,
		BinaryLogical = 0x40,

		/* =------------------------------------------------------------= */
	}
}
