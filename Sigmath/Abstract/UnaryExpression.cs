using Sigmath.CodeGen;

using System;

namespace Sigmath.Abstract
{
	public sealed class UnaryExpression(UnaryExpressionOperator op, Expression arg) :
		Expression, IEquatable<UnaryExpression>
	{
		/* =---- Properties --------------------------------------------= */

		public UnaryExpressionOperator Operator => op;
		public Expression Argument => arg;

		/* =---- Methods -----------------------------------------------= */

		public override AbstractValue GetAbstractValue(Generator generator)
		{
			AbstractValue result;

			switch (op)
			{
			case UnaryExpressionOperator.Negative:
				result = this.Argument.BuildNeg(generator);
				break;

			default:
				throw new InvalidOperationException();
			}

			return result;
		}

		// --------------------------------------------------------------

		public bool Equals(UnaryExpression? other)
			=> (this.Operator == other?.Operator) && this.Argument.Equals(other?.Argument);

		public override bool Equals(object? obj)
			=> obj is UnaryExpression other ? this.Equals(other) : base.Equals(obj);

		public override int GetHashCode()
			=> HashCode.Combine(this.Operator, this.Argument);

		public override string ToString()
			=> $"{this.Operator}({this.Argument})";

		/* =------------------------------------------------------------= */
	}
}
