using Sigmath.CodeGen;

using System;

namespace Sigmath.Abstract
{
    public sealed class BinaryExpression(BinaryExpressionOperator op, Expression lhs, Expression rhs) :
		Expression, IEquatable<BinaryExpression>
	{
		/* =---- Properties --------------------------------------------= */

		public BinaryExpressionOperator Operator => op;

		public Expression LeftHandSide => lhs;
		public Expression RightHandSide => rhs;

		/* =---- Methods -----------------------------------------------= */

		public override AbstractValue GetAbstractValue(Generator generator)
		{
			AbstractValue result;

			switch (op)
			{
			case BinaryExpressionOperator.Add:
				result = this.LeftHandSide.BuildAdd(generator, this.RightHandSide);
				break;

			case BinaryExpressionOperator.Subtract:
				result = this.LeftHandSide.BuildSub(generator, this.RightHandSide);
				break;

			case BinaryExpressionOperator.Multiply:
				result = this.LeftHandSide.BuildMul(generator, this.RightHandSide);
				break;

			case BinaryExpressionOperator.Divide:
				result = this.LeftHandSide.BuildDiv(generator, this.RightHandSide);
				break;

			case BinaryExpressionOperator.Modulo:
				result = this.LeftHandSide.BuildRem(generator, this.RightHandSide);
				break;

			default:
				throw new InvalidOperationException();
			}

			return result;
		}

		// --------------------------------------------------------------

		public bool Equals(BinaryExpression? other)
			=> (this.Operator == other?.Operator) && this.LeftHandSide.Equals(other?.LeftHandSide) && this.RightHandSide.Equals(other?.RightHandSide);

		public override bool Equals(object? obj)
			=> obj is BinaryExpression other ? this.Equals(other) : base.Equals(obj);

		public override int GetHashCode()
			=> HashCode.Combine(this.Operator, this.LeftHandSide, this.RightHandSide);

		public override string ToString()
			=> $"{this.Operator}({this.LeftHandSide}, {this.RightHandSide})";

		/* =------------------------------------------------------------= */
	}
}
