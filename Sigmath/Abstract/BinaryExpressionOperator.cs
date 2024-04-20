namespace Sigmath.Parse.Abstract
{
	public enum BinaryExpressionOperator :
		byte
	{
		/* =---- Arithmetic Operators ----------------------------------= */

		Add,
		Subtract,
		Multiply,
		Divide,
		Modulo,

		/* =---- Comparison Operators ----------------------------------= */

		Equals,
		NotEquals,

		// --------------------------------------------------------------

		GreaterThan,
		GreaterThanOrEquals,
		LessThan,
		LessThanOrEquals,

		/* =------------------------------------------------------------= */
	}
}
