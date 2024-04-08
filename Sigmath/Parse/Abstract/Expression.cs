using Sigmath.CodeGen;

using System;

namespace Sigmath.Parse.Abstract
{
	public abstract class Expression :
		Statement, IComparable<Expression>
	{
		/* =---- Methods -----------------------------------------------= */

		public abstract Value GetValue(CodeGenerator generator);

		// --------------------------------------------------------------

		public virtual Value BuildNeg(CodeGenerator generator)
			=> generator.BuildNeg(this.GetValue(generator));

		// --------------------------------------------------------------

		public virtual Value BuildAdd(CodeGenerator generator, Expression other)
			=> generator.BuildAdd(this.GetValue(generator), other.GetValue(generator));

		public virtual Value BuildSub(CodeGenerator generator, Expression other)
			=> generator.BuildSub(this.GetValue(generator), other.GetValue(generator));

		public virtual Value BuildMul(CodeGenerator generator, Expression other)
			=> generator.BuildMul(this.GetValue(generator), other.GetValue(generator));

		public virtual Value BuildDiv(CodeGenerator generator, Expression other)
			=> generator.BuildUDiv(this.GetValue(generator), other.GetValue(generator));

		public virtual Value BuildRem(CodeGenerator generator, Expression other)
			=> generator.BuildURem(this.GetValue(generator), other.GetValue(generator));

		// --------------------------------------------------------------

		public virtual Value BuildShl(CodeGenerator generator, Expression other)
			=> generator.BuildShl(this.GetValue(generator), other.GetValue(generator));

		public virtual Value BuildLShr(CodeGenerator generator, Expression other)
			=> generator.BuildLShr(this.GetValue(generator), other.GetValue(generator));

		public virtual Value BuildAShr(CodeGenerator generator, Expression other)
			=> generator.BuildAShr(this.GetValue(generator), other.GetValue(generator));

		// --------------------------------------------------------------

		public virtual Value BuildNot(CodeGenerator generator)
			=> generator.BuildNot(this.GetValue(generator));

		// --------------------------------------------------------------

		public virtual Value BuildAnd(CodeGenerator generator, Expression other)
			=> generator.BuildAnd(this.GetValue(generator), other.GetValue(generator));

		public virtual Value BuildOr(CodeGenerator generator, Expression other)
			=> generator.BuildOr(this.GetValue(generator), other.GetValue(generator));

		public virtual Value BuildXOr(CodeGenerator generator, Expression other)
			=> generator.BuildXOr(this.GetValue(generator), other.GetValue(generator));

		// --------------------------------------------------------------

		public virtual ExpressionPrecedence GetExpressionPrecedence()
			=> ExpressionPrecedence.None;

		// --------------------------------------------------------------

		public virtual int CompareTo(Expression? other)
			=> this.GetExpressionPrecedence().CompareTo(other?.GetExpressionPrecedence());

		/* =---- Operators ---------------------------------------------= */

		public static bool operator <(Expression left, Expression right)
			=> left.CompareTo(right) < 0;

		public static bool operator <=(Expression left, Expression right)
			=> left.CompareTo(right) <= 0;

		public static bool operator >(Expression left, Expression right)
			=> left.CompareTo(right) > 0;

		public static bool operator >=(Expression left, Expression right)
			=> left.CompareTo(right) >= 0;

		/* =------------------------------------------------------------= */
	}
}
