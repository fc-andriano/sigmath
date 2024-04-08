using Sigmath.CodeGen;
using Sigmath.Parse.Concrete;

using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace Sigmath.Parse.Abstract
{
	public sealed class ConstantInteger(int bitsNumber, Integer value) :
		Constant, IEquatable<ConstantInteger>, IComparable<ConstantInteger>
	{
		private readonly Integer _value = value;

		/* =---- Constructors ------------------------------------------= */

		public ConstantInteger(Integer value) :
			this(GetBitsNumber(value), value)
		{
		}

		/* =---- Static Methods ----------------------------------------= */

		public static int GetBitsNumber(long value)
		{
			int result;

			switch (value)
			{
			case ((< Int32.MinValue) and (>= Int64.MinValue))
			  or ((> Int32.MaxValue) and (<= Int64.MaxValue)):
				result = 64;
				break;

			default:
				result = 32;
				break;
			}

			return result;
		}

		// --------------------------------------------------------------

		public static ConstantInteger Parse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider)
			=> Integer.Parse(s, style, provider);

		public static ConstantInteger Parse(string s, NumberStyles style, IFormatProvider? provider)
			=> Integer.Parse(s, style, provider);

		public static ConstantInteger Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
			=> Integer.Parse(s, provider);

		public static ConstantInteger Parse(string s, IFormatProvider? provider)
			=> Integer.Parse(s, provider);

		public static ConstantInteger Parse(ReadOnlySpan<char> s, int radix)
			=> Integer.Parse(s, radix);

		public static ConstantInteger Parse(string s, int radix)
			=> Integer.Parse(s, radix);

		// --------------------------------------------------------------

		public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider, [MaybeNullWhen(false)] out ConstantInteger result)
		{
			bool r;

			if (r = Integer.TryParse(s, style, provider, out Integer value))
				result = value;
			else
				result = Integer.Zero;

			return r;
		}

		public static bool TryParse([NotNullWhen(true)] string? s, NumberStyles style, IFormatProvider? provider, [MaybeNullWhen(false)] out ConstantInteger result)
		{
			bool r;

			if (r = Integer.TryParse(s, style, provider, out Integer value))
				result = value;
			else
				result = Integer.Zero;

			return r;
		}

		public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, [MaybeNullWhen(false)] out ConstantInteger result)
		{
			bool r;

			if (r = Integer.TryParse(s, provider, out Integer value))
				result = value;
			else
				result = Integer.Zero;

			return r;
		}

		public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out ConstantInteger result)
		{
			bool r;

			if (r = Integer.TryParse(s, provider, out Integer value))
				result = value;
			else
				result = Integer.Zero;

			return r;
		}

		/* =---- Properties --------------------------------------------= */

		public Integer IntegerValue => _value;
		public override double Value => _value;

		/* =---- Methods -----------------------------------------------= */

		public override Value GetValue(CodeGenerator generator)
			=> generator.GetConstInt(bitsNumber, (ulong)_value, true);

		// --------------------------------------------------------------

		public override Value BuildDiv(CodeGenerator generator, Expression other)
			=> generator.BuildSDiv(this.GetValue(generator), other.GetValue(generator));

		public override Value BuildRem(CodeGenerator generator, Expression other)
			=> generator.BuildSRem(this.GetValue(generator), other.GetValue(generator));

		// --------------------------------------------------------------

		public int CompareTo(ConstantInteger? other)
			=> this._value.CompareTo(other?._value);

		public bool Equals(ConstantInteger? other)
			=> this._value == other?._value;

		public override bool Equals(object? obj)
			=> obj is ConstantInteger other && this.Equals(other);

		public override int GetHashCode()
			=> base.GetHashCode();

		public override string ToString()
			=> this._value.ToString();

		/* =---- Operators ---------------------------------------------= */

		public static bool operator ==(ConstantInteger? left, ConstantInteger? right)
			=> left?.Equals(right) ?? false;

		public static bool operator !=(ConstantInteger? left, ConstantInteger? right)
			=> !(left == right);

		// --------------------------------------------------------------

		public static bool operator <(ConstantInteger left, ConstantInteger right)
			=> left.CompareTo(right) < 0;

		public static bool operator <=(ConstantInteger left, ConstantInteger right)
			=> left.CompareTo(right) <= 0;

		public static bool operator >(ConstantInteger left, ConstantInteger right)
			=> left.CompareTo(right) > 0;

		public static bool operator >=(ConstantInteger left, ConstantInteger right)
			=> left.CompareTo(right) >= 0;

		// --------------------------------------------------------------

		public static implicit operator ConstantInteger(Integer that)
			=> new(that);

		public static implicit operator Integer(ConstantInteger that)
			=> that._value;

		/* =------------------------------------------------------------= */
	}
}
