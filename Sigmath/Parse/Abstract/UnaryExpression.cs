using Sigmath.CodeGen;

using System;

namespace Sigmath.Parse.Abstract
{
	public sealed class UnaryExpression(UnaryExpressionOperator op, Expression value, bool isPostUnary = false) :
		Expression, IEquatable<UnaryExpression>, IComparable<UnaryExpression>
	{
		/* =---- Properties --------------------------------------------= */

		public bool IsPostUnary => isPostUnary;

		public UnaryExpressionOperator Operator => op;
		public Expression Value => value;

		/* =---- Methods -----------------------------------------------= */

		public override Value GetValue(CodeGenerator generator)
		{
			Value result;

			switch (this.Operator)
			{
			case UnaryExpressionOperator.Positive:
				result = this.Value.GetValue(generator);
				break;

			case UnaryExpressionOperator.Negative:
				result = this.Value.BuildNeg(generator);
				break;

			default:
				throw new InvalidOperationException();
			}

			return result;
		}

		// --------------------------------------------------------------

		public int CompareTo(UnaryExpression? other)
			=> this.GetExpressionPrecedence().CompareTo(other?.GetExpressionPrecedence());

		public bool Equals(UnaryExpression? other)
			=> (this.Operator == other?.Operator) && (this.Value == other?.Value);

		public override bool Equals(object? obj)
			=> obj is UnaryExpression other && this.Equals(other);

		public override int GetHashCode()
			=> base.GetHashCode();

		public override string ToString()
			=> $"{this.Operator}({this.Value})";

		/* =---- Operators ---------------------------------------------= */

		public static bool operator ==(UnaryExpression? left, UnaryExpression? right)
			=> left?.Equals(right) ?? false;

		public static bool operator !=(UnaryExpression? left, UnaryExpression? right)
			=> !(left == right);

		// --------------------------------------------------------------

		public static bool operator <(UnaryExpression left, UnaryExpression right)
			=> left.CompareTo(right) < 0;

		public static bool operator <=(UnaryExpression left, UnaryExpression right)
			=> left.CompareTo(right) <= 0;

		public static bool operator >(UnaryExpression left, UnaryExpression right)
			=> left.CompareTo(right) > 0;

		public static bool operator >=(UnaryExpression left, UnaryExpression right)
			=> left.CompareTo(right) >= 0;

		/* =------------------------------------------------------------= */
	}
}