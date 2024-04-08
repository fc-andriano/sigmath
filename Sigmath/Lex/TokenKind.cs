namespace Sigmath.Lex
{
	public enum TokenKind :
		byte
	{
		Invalid,
		Ignored,

		Constant,
		Variable,

		UnaryOperator,
		BinaryOperator,
		TernaryOperator,
	}
}
