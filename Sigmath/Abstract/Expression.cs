using Sigmath.CodeGen;

using System;

namespace Sigmath.Abstract
{
	public abstract class Expression :
		Statement, IComparable<Expression>
	{
		/* =---- Methods -----------------------------------------------= */

		public abstract AbstractValue GetAbstractValue(Generator generator);

		// --------------------------------------------------------------

		public virtual AbstractValue BuildNeg(Generator generator)
			=> generator.BuildNeg(this.GetAbstractValue(generator));

		// --------------------------------------------------------------

		public virtual AbstractValue BuildAdd(Generator generator, Expression other)
			=> generator.BuildAdd(this.GetAbstractValue(generator), other.GetAbstractValue(generator));

		public virtual AbstractValue BuildSub(Generator generator, Expression other)
			=> generator.BuildSub(this.GetAbstractValue(generator), other.GetAbstractValue(generator));

		public virtual AbstractValue BuildMul(Generator generator, Expression other)
			=> generator.BuildMul(this.GetAbstractValue(generator), other.GetAbstractValue(generator));

		public virtual AbstractValue BuildDiv(Generator generator, Expression other)
			=> generator.BuildUDiv(this.GetAbstractValue(generator), other.GetAbstractValue(generator));

		public virtual AbstractValue BuildRem(Generator generator, Expression other)
			=> generator.BuildURem(this.GetAbstractValue(generator), other.GetAbstractValue(generator));

		// --------------------------------------------------------------

		public virtual AbstractValue BuildShl(Generator generator, Expression other)
			=> generator.BuildShl(this.GetAbstractValue(generator), other.GetAbstractValue(generator));

		public virtual AbstractValue BuildLShr(Generator generator, Expression other)
			=> generator.BuildLShr(this.GetAbstractValue(generator), other.GetAbstractValue(generator));

		public virtual AbstractValue BuildAShr(Generator generator, Expression other)
			=> generator.BuildAShr(this.GetAbstractValue(generator), other.GetAbstractValue(generator));

		// --------------------------------------------------------------

		public virtual AbstractValue BuildNot(Generator generator)
			=> generator.BuildNot(this.GetAbstractValue(generator));

		// --------------------------------------------------------------

		public virtual AbstractValue BuildAnd(Generator generator, Expression other)
			=> generator.BuildAnd(this.GetAbstractValue(generator), other.GetAbstractValue(generator));

		public virtual AbstractValue BuildOr(Generator generator, Expression other)
			=> generator.BuildOr(this.GetAbstractValue(generator), other.GetAbstractValue(generator));

		public virtual AbstractValue BuildXOr(Generator generator, Expression other)
			=> generator.BuildXOr(this.GetAbstractValue(generator), other.GetAbstractValue(generator));

		// --------------------------------------------------------------

		public virtual ExpressionPrecedence GetPrecedence()
			=> ExpressionPrecedence.None;

		// --------------------------------------------------------------

		public int CompareTo(Expression? other)
			=> this.GetPrecedence().CompareTo(other?.GetPrecedence());

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
