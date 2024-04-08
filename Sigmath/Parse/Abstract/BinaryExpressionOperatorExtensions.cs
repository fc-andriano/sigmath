namespace Sigmath.Parse.Abstract
{
	public static class BinaryExpressionOperatorExtensions
	{
		/* =---- Extension Methods -------------------------------------= */

		public static ExpressionPrecedence GetPrecedence(this BinaryExpressionOperator binop)
			=> BinaryExpression.GetBinaryExpressionPrecedence(binop);

		/* =------------------------------------------------------------= */
	}
}
