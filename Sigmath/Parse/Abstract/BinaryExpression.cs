using Sigmath.CodeGen;

using System;

namespace Sigmath.Parse.Abstract
{
	public sealed class BinaryExpression(BinaryExpressionOperator op, Expression lhs, Expression rhs) :
		Expression, IEquatable<BinaryExpression>, IComparable<BinaryExpression>
	{
		/* =---- Static Methods ----------------------------------------= */

		public static ExpressionPrecedence GetBinaryExpressionPrecedence(BinaryExpressionOperator binop)
		{
			ExpressionPrecedence result;

			switch (binop)
			{
			case BinaryExpressionOperator.Add
			  or BinaryExpressionOperator.Subtract:
				result = ExpressionPrecedence.BinaryArithmeticSum;
				break;

			case BinaryExpressionOperator.Multiply
			  or BinaryExpressionOperator.Divide
			  or BinaryExpressionOperator.Modulo:
				result = ExpressionPrecedence.BinaryArithmeticProduct;
				break;

			case BinaryExpressionOperator.Equals
			  or BinaryExpressionOperator.NotEquals:
				result = ExpressionPrecedence.BinaryComparison;
				break;

			case BinaryExpressionOperator.GreaterThan
			  or BinaryExpressionOperator.GreaterThanOrEquals
			  or BinaryExpressionOperator.LessThan
			  or BinaryExpressionOperator.LessThanOrEquals:
				result = ExpressionPrecedence.BinaryRelational;
				break;

			default:
				throw new InvalidOperationException();
			}

			return result;
		}

		/* =---- Properties --------------------------------------------= */

		public BinaryExpressionOperator Operator => op;

		public Expression LeftHandSide => lhs;
		public Expression RightHandSide => rhs;

		/* =---- Methods -----------------------------------------------= */

		public override Value GetValue(CodeGenerator generator)
		{
			Value result;

			switch (this.Operator)
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

		public override ExpressionPrecedence GetExpressionPrecedence()
			=> GetBinaryExpressionPrecedence(this.Operator);

		// --------------------------------------------------------------

		public int CompareTo(BinaryExpression? other)
			=> this.GetExpressionPrecedence().CompareTo(other?.GetExpressionPrecedence());

		public bool Equals(BinaryExpression? other)
			=> (this.Operator == other?.Operator) && (this.LeftHandSide == other?.LeftHandSide) && (this.RightHandSide == other?.RightHandSide);

		public override bool Equals(object? obj)
			=> obj is BinaryExpression other && this.Equals(other);

		public override int GetHashCode()
			=> base.GetHashCode();

		public override string ToString()
			=> $"{this.Operator}({this.LeftHandSide}, {this.RightHandSide})";

		/* =---- Operators ---------------------------------------------= */

		public static bool operator ==(BinaryExpression? left, BinaryExpression? right)
			=> left?.Equals(right) ?? false;

		public static bool operator !=(BinaryExpression? left, BinaryExpression? right)
			=> !(left == right);

		// --------------------------------------------------------------

		public static bool operator <(BinaryExpression left, BinaryExpression right)
			=> left.CompareTo(right) < 0;

		public static bool operator <=(BinaryExpression left, BinaryExpression right)
			=> left.CompareTo(right) <= 0;

		public static bool operator >(BinaryExpression left, BinaryExpression right)
			=> left.CompareTo(right) > 0;

		public static bool operator >=(BinaryExpression left, BinaryExpression right)
			=> left.CompareTo(right) >= 0;

		/* =------------------------------------------------------------= */
	}
}
