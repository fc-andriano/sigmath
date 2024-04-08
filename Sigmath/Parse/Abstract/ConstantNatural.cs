using Sigmath.CodeGen;
using Sigmath.Parse.Concrete;

using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;

namespace Sigmath.Parse.Abstract
{
	public sealed class ConstantNatural(int bitsNumber, Natural value) :
		Constant, IEquatable<ConstantNatural>, IComparable<ConstantNatural>
	{
		private readonly Natural _value = value;

		/* =---- Constructors ------------------------------------------= */

		public ConstantNatural(Natural value) :
			this(GetBitsNumber(value), value)
		{
		}

		/* =---- Static Methods ----------------------------------------= */

		public static bool IsCanonical(ConstantNatural value)
			=> true;

		public static bool IsComplexNumber(ConstantNatural value)
			=> false;

		public static bool IsEvenInteger(ConstantNatural value)
			=> (value._value % 2) == 0;

		public static bool IsFinite(ConstantNatural value)
			=> true;

		public static bool IsImaginaryNumber(ConstantNatural value)
			=> false;

		public static bool IsInfinity(ConstantNatural value)
			=> false;

		public static bool IsInteger(ConstantNatural value)
			=> true;

		public static bool IsNaN(ConstantNatural value)
			=> false;

		public static bool IsNegative(ConstantNatural value)
			=> false;

		public static bool IsNegativeInfinity(ConstantNatural value)
			=> false;

		public static bool IsNormal(ConstantNatural value)
			=> true;

		public static bool IsOddInteger(ConstantNatural value)
			=> (value._value % 2) != 0;

		public static bool IsPositive(ConstantNatural value)
			=> value._value != 0;

		public static bool IsPositiveInfinity(ConstantNatural value)
			=> false;

		public static bool IsRealNumber(ConstantNatural value)
			=> true;

		public static bool IsSubnormal(ConstantNatural value)
			=> false;

		public static bool IsZero(ConstantNatural value)
			=> value._value == 0;

		// --------------------------------------------------------------

		public static int GetBitsNumber(ulong value)
		{
			int result;

			switch (value)
			{
			case (> UInt32.MaxValue) and (<= UInt64.MaxValue):
				result = 64;
				break;

			default:
				result = 32;
				break;
			}

			return result;
		}

		// --------------------------------------------------------------

		public static ConstantNatural Abs(ConstantNatural value)
			=> value;

		public static ConstantNatural MaxMagnitude(ConstantNatural x, ConstantNatural y)
			=> (x._value > y._value) ? x : y;

		public static ConstantNatural MinMagnitude(ConstantNatural x, ConstantNatural y)
			=> (x._value < y._value) ? x : y;

		public static ConstantNatural MaxMagnitudeNumber(ConstantNatural x, ConstantNatural y)
			=> MaxMagnitude(x, y);

		public static ConstantNatural MinMagnitudeNumber(ConstantNatural x, ConstantNatural y)
			=> MinMagnitude(x, y);

		// --------------------------------------------------------------

		public static ConstantNatural Parse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider)
			=> UInt64.Parse(s, style, provider);

		public static ConstantNatural Parse(string s, NumberStyles style, IFormatProvider? provider)
			=> UInt64.Parse(s, style, provider);

		public static ConstantNatural Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
			=> UInt64.Parse(s, provider);

		public static ConstantNatural Parse(string s, IFormatProvider? provider)
			=> UInt64.Parse(s, provider);

		public static ConstantNatural Parse(ReadOnlySpan<char> s, int radix)
			=> Convert.ToUInt64(s.ToString(), radix);

		public static ConstantNatural Parse(string s, int radix)
			=> Convert.ToUInt64(s, radix);

		// --------------------------------------------------------------

		public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider, [MaybeNullWhen(false)] out ConstantNatural result)
		{
			bool r;

			if (r = UInt64.TryParse(s, style, provider, out ulong value))
				result = value;
			else
				result = Zero;

			return r;
		}

		public static bool TryParse([NotNullWhen(true)] string? s, NumberStyles style, IFormatProvider? provider, [MaybeNullWhen(false)] out ConstantNatural result)
		{
			bool r;

			if (r = UInt64.TryParse(s, style, provider, out ulong value))
				result = value;
			else
				result = Zero;

			return r;
		}

		public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, [MaybeNullWhen(false)] out ConstantNatural result)
		{
			bool r;

			if (r = UInt64.TryParse(s, provider, out ulong value))
				result = value;
			else
				result = Zero;

			return r;
		}

		public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out ConstantNatural result)
		{
			bool r;

			if (r = UInt64.TryParse(s, provider, out ulong value))
				result = value;
			else
				result = Zero;

			return r;
		}

		// --------------------------------------------------------------

		static bool INumberBase<ConstantNatural>.TryConvertFromChecked<TOther>(TOther value, out ConstantNatural result)
		{
			bool answer = true;

			switch (Convert.GetTypeCode(typeof(TOther)))
			{
			case TypeCode.Byte:
				result = (byte)(object)value;
				break;

			case TypeCode.Char:
				result = (char)(object)value;
				break;

			case TypeCode.Decimal:
				result = checked((ulong)(decimal)(object)value);
				break;

			case TypeCode.UInt16:
				result = (ushort)(object)value;
				break;

			case TypeCode.UInt32:
				result = (uint)(object)value;
				break;

			case TypeCode.UInt64:
				result = (ulong)(object)value;
				break;

			case TypeCode.Object when typeof(TOther) == typeof(UInt128):
				result = checked((ulong)(UInt128)(object)value);
				break;

			case TypeCode.Object when typeof(TOther) == typeof(UIntPtr):
				result = (UIntPtr)(object)value;
				break;

			default:
				result = Zero;
				answer = false;
				break;
			}

			return answer;
		}

		static bool INumberBase<ConstantNatural>.TryConvertFromSaturating<TOther>(TOther value, out ConstantNatural result)
		{
			bool answer = true;

			switch (Convert.GetTypeCode(typeof(TOther)))
			{
			case TypeCode.Byte:
				result = (byte)(object)value;
				break;

			case TypeCode.Char:
				result = (char)(object)value;
				break;

			case TypeCode.Decimal:
				decimal actualValue1 = (decimal)(object)value;

				if (actualValue1 >= MaxValue._value)
					result = MaxValue;
				else if (actualValue1 <= MinValue._value)
					result = MinValue;
				else
					result = (ulong)actualValue1;

				break;

			case TypeCode.UInt16:
				result = (ushort)(object)value;
				break;

			case TypeCode.UInt32:
				result = (uint)(object)value;
				break;

			case TypeCode.UInt64:
				result = (ulong)(object)value;
				break;

			case TypeCode.Object when typeof(TOther) == typeof(UInt128):
				UInt128 actualValue2 = (UInt128)(object)value;

				if (actualValue2 >= MaxValue._value)
					result = MaxValue;
				else
					result = (ulong)actualValue2;

				break;

			case TypeCode.Object when typeof(TOther) == typeof(UIntPtr):
				result = (UIntPtr)(object)value;
				break;

			default:
				result = Zero;
				answer = false;
				break;
			}

			return answer;
		}

		static bool INumberBase<ConstantNatural>.TryConvertFromTruncating<TOther>(TOther value, out ConstantNatural result)
		{
			bool answer = true;

			switch (Convert.GetTypeCode(typeof(TOther)))
			{
			case TypeCode.Byte:
				result = (byte)(object)value;
				break;

			case TypeCode.Char:
				result = (char)(object)value;
				break;

			case TypeCode.Decimal:
				decimal actualValue = (decimal)(object)value;

				if (actualValue >= MaxValue._value)
					result = MaxValue;
				else if (actualValue <= MinValue._value)
					result = MinValue;
				else
					result = (ulong)actualValue;

				break;

			case TypeCode.UInt16:
				result = (ushort)(object)value;
				break;

			case TypeCode.UInt32:
				result = (uint)(object)value;
				break;

			case TypeCode.UInt64:
				result = (ulong)(object)value;
				break;

			case TypeCode.Object when typeof(TOther) == typeof(UInt128):
				result = (ulong)(UInt128)(object)value;
				break;

			case TypeCode.Object when typeof(TOther) == typeof(UIntPtr):
				result = (UIntPtr)(object)value;
				break;

			default:
				result = Zero;
				answer = false;
				break;
			}

			return answer;
		}

		static bool INumberBase<ConstantNatural>.TryConvertToChecked<TOther>(ConstantNatural value, [MaybeNullWhen(false)] out TOther result)
		{
			bool answer = true;

			switch (Convert.GetTypeCode(typeof(TOther)))
			{
			case TypeCode.Single:
				result = (TOther)(object)(float)value._value;
				break;

			case TypeCode.Double:
				result = (TOther)(object)(double)value._value;
				break;

			case TypeCode.SByte:
				result = (TOther)(object)checked((sbyte)value._value);
				break;

			case TypeCode.Int16:
				result = (TOther)(object)checked((short)value._value);
				break;

			case TypeCode.Int32:
				result = (TOther)(object)checked((int)value._value);
				break;

			case TypeCode.Int64:
				result = (TOther)(object)checked((long)value._value);
				break;

			case TypeCode.Object when typeof(TOther) == typeof(Half):
				result = (TOther)(object)(Half)value._value;
				break;

			case TypeCode.Object when typeof(TOther) == typeof(Int128):
				result = (TOther)(object)checked((Int128)value._value);
				break;

			case TypeCode.Object when typeof(TOther) == typeof(IntPtr):
				result = (TOther)(object)checked((IntPtr)value._value);
				break;

			default:
				result = default;
				answer = false;
				break;
			}

			return answer;
		}

		static bool INumberBase<ConstantNatural>.TryConvertToSaturating<TOther>(ConstantNatural value, [MaybeNullWhen(false)] out TOther result)
		{
			bool answer = true;

			switch (Convert.GetTypeCode(typeof(TOther)))
			{
			case TypeCode.Single:
				result = (TOther)(object)(float)value._value;
				break;

			case TypeCode.Double:
				result = (TOther)(object)(double)value._value;
				break;

			case TypeCode.SByte:
				result = (TOther)(object)((value >= (ulong)SByte.MaxValue) ? SByte.MaxValue : (sbyte)value._value);
				break;

			case TypeCode.Int16:
				result = (TOther)(object)((value >= (ulong)Int16.MaxValue) ? Int16.MaxValue : (short)value._value);
				break;

			case TypeCode.Int32:
				result = (TOther)(object)((value >= Int32.MaxValue) ? Int32.MaxValue : (int)value._value);
				break;

			case TypeCode.Int64:
				result = (TOther)(object)((value >= Int64.MaxValue) ? Int64.MaxValue : (long)value._value);
				break;

			case TypeCode.Object when typeof(TOther) == typeof(Half):
				result = (TOther)(object)(Half)value._value;
				break;

			case TypeCode.Object when typeof(TOther) == typeof(Int128):
				result = (TOther)(object)((value >= (ulong)Int128.MaxValue) ? Int128.MaxValue : (Int128)value._value);
				break;

			case TypeCode.Object when typeof(TOther) == typeof(IntPtr):
				result = (TOther)(object)((value >= (ulong)IntPtr.MaxValue) ? IntPtr.MaxValue : (IntPtr)value._value);
				break;

			default:
				result = default;
				answer = false;
				break;
			}

			return answer;
		}

		static bool INumberBase<ConstantNatural>.TryConvertToTruncating<TOther>(ConstantNatural value, [MaybeNullWhen(false)] out TOther result)
		{
			bool answer = true;

			switch (Convert.GetTypeCode(typeof(TOther)))
			{
			case TypeCode.Single:
				result = (TOther)(object)(float)value._value;
				break;

			case TypeCode.Double:
				result = (TOther)(object)(double)value._value;
				break;

			case TypeCode.SByte:
				result = (TOther)(object)(sbyte)value._value;
				break;

			case TypeCode.Int16:
				result = (TOther)(object)(short)value._value;
				break;

			case TypeCode.Int32:
				result = (TOther)(object)(int)value._value;
				break;

			case TypeCode.Int64:
				result = (TOther)(object)(long)value._value;
				break;

			case TypeCode.Object when typeof(TOther) == typeof(Half):
				result = (TOther)(object)(Half)value._value;
				break;

			case TypeCode.Object when typeof(TOther) == typeof(Int128):
				result = (TOther)(object)(Int128)value._value;
				break;

			case TypeCode.Object when typeof(TOther) == typeof(IntPtr):
				result = (TOther)(object)(IntPtr)value._value;
				break;

			default:
				result = default;
				answer = false;
				break;
			}

			return answer;
		}

		/* =---- Properties --------------------------------------------= */

		public override double Value => _value;

		/* =---- Methods -----------------------------------------------= */

		public override Value GetValue(CodeGenerator generator)
			=> generator.GetConstInt(bitsNumber, _value, false);

		// --------------------------------------------------------------

		public override Value BuildDiv(CodeGenerator generator, Expression other)
			=> generator.BuildUDiv(this.GetValue(generator), other.GetValue(generator));

		public override Value BuildRem(CodeGenerator generator, Expression other)
			=> generator.BuildURem(this.GetValue(generator), other.GetValue(generator));

		// --------------------------------------------------------------

		public Value BuildConstAdd(CodeGenerator generator, ConstantNatural other)
			=> (this + other).GetValue(generator);

		public Value BuildConstSub(CodeGenerator generator, ConstantNatural other)
			=> (this - other).GetValue(generator);

		public Value BuildConstMul(CodeGenerator generator, ConstantNatural other)
			=> (this * other).GetValue(generator);

		public Value BuildConstDiv(CodeGenerator generator, ConstantNatural other)
			=> (this / other).GetValue(generator);

		public Value BuildConstRem(CodeGenerator generator, ConstantNatural other)
			=> (this % other).GetValue(generator);

		// --------------------------------------------------------------

		public int CompareTo(ConstantNatural? other)
			=> this._value.CompareTo(other?._value);

		public int CompareTo(object? obj)
			=> this._value.CompareTo(obj);

		public bool Equals(ConstantNatural? other)
			=> this._value == other?._value;

		public override bool Equals(object? obj)
			=> obj is ConstantNatural other && this.Equals(other);

		public override int GetHashCode()
			=> base.GetHashCode();

		public string ToString(string? format, IFormatProvider? formatProvider)
			=> this._value.ToString(format, formatProvider);

		public override string ToString()
			=> this._value.ToString();

		public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
			=> this._value.TryFormat(destination, out charsWritten, format, provider);

		/* =---- Operators ---------------------------------------------= */

		public static ConstantNatural operator +(ConstantNatural value)
			=> value._value;

		public static ConstantNatural operator -(ConstantNatural value)
			=> throw new InvalidOperationException();

		// --------------------------------------------------------------

		public static ConstantNatural operator +(ConstantNatural left, ConstantNatural right)
			=> left._value + right._value;

		public static ConstantNatural operator -(ConstantNatural left, ConstantNatural right)
			=> left._value - right._value;

		public static ConstantNatural operator *(ConstantNatural left, ConstantNatural right)
			=> left._value * right._value;

		public static ConstantNatural operator /(ConstantNatural left, ConstantNatural right)
			=> left._value / right._value;

		public static ConstantNatural operator %(ConstantNatural left, ConstantNatural right)
			=> left._value % right._value;

		// --------------------------------------------------------------

		public static ConstantNatural operator ++(ConstantNatural value)
			=> value._value + 1;

		public static ConstantNatural operator --(ConstantNatural value)
			=> value._value - 1;

		// --------------------------------------------------------------

		public static bool operator ==(ConstantNatural? left, ConstantNatural? right)
			=> left?.Equals(right) ?? false;

		public static bool operator !=(ConstantNatural? left, ConstantNatural? right)
			=> !(left == right);

		// --------------------------------------------------------------

		public static bool operator <(ConstantNatural left, ConstantNatural right)
			=> left.CompareTo(right) < 0;

		public static bool operator <=(ConstantNatural left, ConstantNatural right)
			=> left.CompareTo(right) <= 0;

		public static bool operator >(ConstantNatural left, ConstantNatural right)
			=> left.CompareTo(right) > 0;

		public static bool operator >=(ConstantNatural left, ConstantNatural right)
			=> left.CompareTo(right) >= 0;

		// --------------------------------------------------------------

		public static implicit operator ConstantNatural(ulong that)
			=> new(that);

		public static implicit operator ulong(ConstantNatural that)
			=> that._value;

		/* =------------------------------------------------------------= */
	}
}
