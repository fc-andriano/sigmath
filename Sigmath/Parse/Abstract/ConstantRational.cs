using Sigmath.CodeGen;
using Sigmath.Parse.Concrete;

using System;

namespace Sigmath.Parse.Abstract
{
	public sealed class ConstantRational(int width, Fraction value) :
		Constant, IEquatable<ConstantRational>, IComparable<ConstantRational>
	{
		private readonly Fraction _value = value;

		/* =---- Constructors ------------------------------------------= */

		public ConstantRational(Fraction value) :
			this(GetBitsNumber(value), value)
		{
		}

		/* =---- Static Methods ----------------------------------------= */

		public static int GetBitsNumber(Fraction value)
			=> Math.Max(ConstantInteger.GetBitsNumber(value.Numerator), ConstantInteger.GetBitsNumber(value.Denominator));

		/* =---- Properties --------------------------------------------= */

		public override double Value => _value.ToDouble();

		/* =---- Methods -----------------------------------------------= */

		public override Value GetValue(CodeGenerator generator)
			=> generator.GetConstIntPair(width, _value.Numerator, _value.Denominator, true);

		// --------------------------------------------------------------

		public Value BuildConstAdd(CodeGenerator generator, ConstantRational other)
			=> (this + other).GetValue(generator);

		public Value BuildConstSub(CodeGenerator generator, ConstantRational other)
			=> (this - other).GetValue(generator);

		public Value BuildConstMul(CodeGenerator generator, ConstantRational other)
			=> (this * other).GetValue(generator);

		public Value BuildConstDiv(CodeGenerator generator, ConstantRational other)
			=> (this / other).GetValue(generator);

		public Value BuildConstRem(CodeGenerator generator, ConstantRational other)
			=> (this % other).GetValue(generator);

		// --------------------------------------------------------------

		public int CompareTo(ConstantRational? other)
			=> this._value.CompareTo(other?._value);

		public int CompareTo(object? obj)
			=> this._value.CompareTo(obj);

		public bool Equals(ConstantRational? other)
			=> this._value == other?._value;

		public override bool Equals(object? obj)
			=> obj is ConstantRational other && this.Equals(other);

		public override int GetHashCode()
			=> base.GetHashCode();

		public override string ToString()
			=> this._value.ToString();

		/* =---- Operators ---------------------------------------------= */

		public static ConstantRational operator +(ConstantRational value)
			=> +value._value;

		public static ConstantRational operator -(ConstantRational value)
			=> -value._value;

		// --------------------------------------------------------------

		public static ConstantRational operator +(ConstantRational left, ConstantRational right)
			=> left._value + right._value;

		public static ConstantRational operator -(ConstantRational left, ConstantRational right)
			=> left._value - right._value;

		public static ConstantRational operator *(ConstantRational left, ConstantRational right)
			=> left._value * right._value;

		public static ConstantRational operator /(ConstantRational left, ConstantRational right)
			=> left._value / right._value;

		public static ConstantRational operator %(ConstantRational left, ConstantRational right)
			=> left._value % right._value;

		// --------------------------------------------------------------

		public static ConstantRational operator ++(ConstantRational value)
			=> value._value + 1;

		public static ConstantRational operator --(ConstantRational value)
			=> value._value - 1;

		// --------------------------------------------------------------

		public static bool operator ==(ConstantRational? left, ConstantRational? right)
			=> left?.Equals(right) ?? false;

		public static bool operator !=(ConstantRational? left, ConstantRational? right)
			=> !(left == right);

		// --------------------------------------------------------------

		public static bool operator <(ConstantRational left, ConstantRational right)
			=> left.CompareTo(right) < 0;

		public static bool operator <=(ConstantRational left, ConstantRational right)
			=> left.CompareTo(right) <= 0;

		public static bool operator >(ConstantRational left, ConstantRational right)
			=> left.CompareTo(right) > 0;

		public static bool operator >=(ConstantRational left, ConstantRational right)
			=> left.CompareTo(right) >= 0;

		// --------------------------------------------------------------

		public static implicit operator ConstantRational(Fraction that)
			=> new(that);

		public static implicit operator Fraction(ConstantRational that)
			=> that._value;

		/* =------------------------------------------------------------= */
	}
}
