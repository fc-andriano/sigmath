using Sigmath.CodeGen;

using System;
using System.Diagnostics;

namespace Sigmath.Parse.Abstract
{
	[DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(), nq}}")]
	public abstract class Constant :
		Expression, IEquatable<Constant>, IComparable<Constant>
	{
		/* =---- Properties --------------------------------------------= */

		public abstract double Value { get; }

		/* =---- Methods -----------------------------------------------= */

		public virtual Value BuildConstNeg(CodeGenerator generator)
			=> this.BuildNeg(generator);

		// --------------------------------------------------------------

		public virtual Value BuildConstAdd(CodeGenerator generator, Constant other)
			=> this.BuildAdd(generator, other);

		public virtual Value BuildConstSub(CodeGenerator generator, Constant other)
			=> this.BuildSub(generator, other);

		public virtual Value BuildConstMul(CodeGenerator generator, Constant other)
			=> this.BuildMul(generator, other);

		public virtual Value BuildConstDiv(CodeGenerator generator, Constant other)
			=> this.BuildDiv(generator, other);

		public virtual Value BuildConstRem(CodeGenerator generator, Constant other)
			=> this.BuildRem(generator, other);

		// --------------------------------------------------------------

		public virtual Value BuildConstShl(CodeGenerator generator, Constant other)
			=> this.BuildShl(generator, other);

		public virtual Value BuildConstLShr(CodeGenerator generator, Constant other)
			=> this.BuildLShr(generator, other);

		public virtual Value BuildConstAShr(CodeGenerator generator, Constant other)
			=> this.BuildAShr(generator, other);

		// --------------------------------------------------------------

		public virtual Value BuildConstNot(CodeGenerator generator)
			=> this.BuildNot(generator);

		// --------------------------------------------------------------

		public virtual Value BuildConstAnd(CodeGenerator generator, Constant other)
			=> this.BuildAnd(generator, other);

		public virtual Value BuildConstOr(CodeGenerator generator, Constant other)
			=> this.BuildOr(generator, other);

		public virtual Value BuildConstXOr(CodeGenerator generator, Constant other)
			=> this.BuildXOr(generator, other);

		// --------------------------------------------------------------

		public int CompareTo(Constant? other)
			=> this.Value.CompareTo(other?.Value);

		public bool Equals(Constant? other)
			=> this.Value == other?.Value;

		public override bool Equals(object? obj)
			=> obj is Constant other && base.Equals(other);

		public override int GetHashCode()
			=> base.GetHashCode();

		public override string ToString()
			=> $"{this.GetType().Name}: {this.Value}";

		// --------------------------------------------------------------

		internal protected string GetDebuggerDisplay()
			=> this.Value.ToString();

		/* =---- Operators ---------------------------------------------= */

		public static bool operator ==(Constant left, Constant right)
			=> left.Equals(right);

		public static bool operator !=(Constant left, Constant right)
			=> !left.Equals(right);

		// --------------------------------------------------------------

		public static bool operator <(Constant left, Constant right)
			=> left.CompareTo(right) < 0;

		public static bool operator <=(Constant left, Constant right)
			=> left.CompareTo(right) <= 0;

		public static bool operator >(Constant left, Constant right)
			=> left.CompareTo(right) > 0;

		public static bool operator >=(Constant left, Constant right)
			=> left.CompareTo(right) >= 0;

		/* =------------------------------------------------------------= */
	}
}
