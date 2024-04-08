using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace Sigmath.Parse.Concrete
{
	public readonly record struct Integer(long Value) :
		IEquatable<Integer>, IComparable<Integer>, INumber<Integer>
	{
		/* =---- Static Fields -----------------------------------------= */

		public static readonly Integer MinValue = Int64.MinValue;
		public static readonly Integer MaxValue = Int64.MaxValue;

		/* =---- Static Properties -------------------------------------= */

		public static int Radix => 10;

		// --------------------------------------------------------------

		public static Integer One => 1;
		public static Integer Zero => 0;
		public static Integer AdditiveIdentity => Zero;
		public static Integer MultiplicativeIdentity => One;

		/* =---- Static Methods ----------------------------------------= */

		public static bool IsCanonical(Integer value)
			=> true;

		public static bool IsComplexNumber(Integer value)
			=> false;

		public static bool IsEvenInteger(Integer value)
			=> (value.Value % 2) == 0;

		public static bool IsFinite(Integer value)
			=> true;

		public static bool IsImaginaryNumber(Integer value)
			=> false;

		public static bool IsInfinity(Integer value)
			=> false;

		public static bool IsInteger(Integer value)
			=> true;

		public static bool IsNaN(Integer value)
			=> false;

		public static bool IsNegative(Integer value)
			=> value.Value < 0;

		public static bool IsNegativeInfinity(Integer value)
			=> false;

		public static bool IsNormal(Integer value)
			=> true;

		public static bool IsOddInteger(Integer value)
			=> (value.Value % 2) != 0;

		public static bool IsPositive(Integer value)
			=> value.Value > 0;

		public static bool IsPositiveInfinity(Integer value)
			=> false;

		public static bool IsRealNumber(Integer value)
			=> true;

		public static bool IsSubnormal(Integer value)
			=> false;

		public static bool IsZero(Integer value)
			=> value.Value == 0;

		// --------------------------------------------------------------

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Sign GetSign(Integer value)
		{
			Sign result;

			switch (value.Value)
			{
			case > 0:
				result = Sign.Positive;
				break;

			case 0:
				result = Sign.Unsigned;
				break;

			case < 0:
				result = Sign.Negative;
				break;

			default:
				throw new InvalidOperationException();
			}

			return result;
		}

		// --------------------------------------------------------------

		public static Integer Abs(Integer value)
			=> IsNegative(value) ? -value : value;

		public static Integer MaxMagnitude(Integer x, Integer y)
			=> (x > y) ? x : y;

		public static Integer MinMagnitude(Integer x, Integer y)
			=> (x < y) ? x : y;

		public static Integer MaxMagnitudeNumber(Integer x, Integer y)
			=> MaxMagnitude(x, y);

		public static Integer MinMagnitudeNumber(Integer x, Integer y)
			=> MinMagnitude(x, y);

		// --------------------------------------------------------------

		public static Integer Parse(string s, NumberStyles style, IFormatProvider? provider)
			=> Int64.Parse(s, style, provider);

		public static Integer Parse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider)
			=> Int64.Parse(s, style, provider);

		public static Integer Parse(string s, IFormatProvider? provider)
			=> Int64.Parse(s, provider);

		public static Integer Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
			=> Int64.Parse(s, provider);

		public static Integer Parse(string s)
			=> Int64.Parse(s);

		public static Integer Parse(ReadOnlySpan<char> s)
			=> Int64.Parse(s);

		public static Integer Parse(string s, int radix)
			=> Convert.ToInt64(s, radix);

		public static Integer Parse(ReadOnlySpan<char> s, int radix)
			=> s.IsEmpty ? Zero : Parse(s.ToString(), radix);

		// --------------------------------------------------------------

		public static bool TryParse([NotNullWhen(true)] string? s, NumberStyles style, IFormatProvider? provider, [MaybeNullWhen(false)] out Integer result)
		{
			bool answer;

			if (answer = Int64.TryParse(s, style, provider, out long r))
				result = r;
			else
				result = default;

			return answer;
		}

		public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider, [MaybeNullWhen(false)] out Integer result)

		{
			bool answer;

			if (answer = Int64.TryParse(s, style, provider, out long r))
				result = r;
			else
				result = default;

			return answer;
		}

		public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Integer result)
		{
			bool answer;

			if (answer = Int64.TryParse(s, provider, out long r))
				result = r;
			else
				result = default;

			return answer;
		}

		public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, [MaybeNullWhen(false)] out Integer result)
		{
			bool answer;

			if (answer = Int64.TryParse(s, provider, out long r))
				result = r;
			else
				result = default;

			return answer;
		}

		public static bool TryParse([NotNullWhen(true)] string? s, [MaybeNullWhen(false)] out Integer result)
		{
			bool answer;

			if (answer = Int64.TryParse(s, out long r))
				result = r;
			else
				result = default;

			return answer;
		}

		public static bool TryParse(ReadOnlySpan<char> s, [MaybeNullWhen(false)] out Integer result)
		{
			bool answer;

			if (answer = Int64.TryParse(s, out long r))
				result = r;
			else
				result = default;

			return answer;
		}

		// --------------------------------------------------------------

		internal static bool TryConvertFromChecked<TOther>(TOther value, out Integer result)
			where TOther : INumberBase<TOther>
		{
			bool answer = true;

			switch (Convert.GetTypeCode(typeof(TOther)))
			{
			case TypeCode.Single:
				result = checked((long)(float)(object)value);
				break;

			case TypeCode.Double:
				result = checked((long)(double)(object)value);
				break;

			case TypeCode.SByte:
				result = (sbyte)(object)value;
				break;

			case TypeCode.Int16:
				result = (short)(object)value;
				break;

			case TypeCode.Int32:
				result = (int)(object)value;
				break;

			case TypeCode.Int64:
				result = (long)(object)value;
				break;

			case TypeCode.Object when typeof(TOther) == typeof(Half):
				result = checked((long)(Half)(object)value);
				break;

			case TypeCode.Object when typeof(TOther) == typeof(Int128):
				result = checked((long)(Int128)(object)value);
				break;

			case TypeCode.Object when typeof(TOther) == typeof(IntPtr):
				result = (IntPtr)(object)value;
				break;

			default:
				result = Zero;
				answer = false;
				break;
			}

			return answer;
		}

		internal static bool TryConvertFromSaturating<TOther>(TOther value, out Integer result)
			where TOther : INumberBase<TOther>
		{
			bool answer = true;

			switch (Convert.GetTypeCode(typeof(TOther)))
			{
			case TypeCode.Single:
				float actualValue1 = (float)(object)value;

				if (actualValue1 >= MaxValue.Value)
					result = MaxValue;
				else if (actualValue1 <= MinValue.Value)
					result = MinValue;
				else
					result = (long)actualValue1;

				break;

			case TypeCode.Double:
				double actualValue2 = (double)(object)value;

				if (actualValue2 >= MaxValue.Value)
					result = MaxValue;
				else if (actualValue2 <= MinValue.Value)
					result = MinValue;
				else
					result = (long)actualValue2;

				break;

			case TypeCode.SByte:
				result = (sbyte)(object)value;
				break;

			case TypeCode.Int16:
				result = (short)(object)value;
				break;

			case TypeCode.Int32:
				result = (int)(object)value;
				break;

			case TypeCode.Int64:
				result = (long)(object)value;
				break;

			case TypeCode.Object when typeof(TOther) == typeof(Half):
				Half actualValue3 = (Half)(object)value;

				if (actualValue3 == Half.PositiveInfinity)
					result = MaxValue;
				else if (actualValue3 == Half.NegativeInfinity)
					result = MinValue;
				else
					result = (long)actualValue3;

				break;

			case TypeCode.Object when typeof(TOther) == typeof(Int128):
				Int128 actualValue4 = (Int128)(object)value;

				if (actualValue4 >= MaxValue.Value)
					result = MaxValue;
				else if (actualValue4 <= MinValue.Value)
					result = MinValue;
				else
					result = (long)actualValue4;

				break;

			case TypeCode.Object when typeof(TOther) == typeof(IntPtr):
				result = (IntPtr)(object)value;
				break;

			default:
				result = Zero;
				answer = false;
				break;
			}

			return answer;
		}

		internal static bool TryConvertFromTruncating<TOther>(TOther value, out Integer result)
			where TOther : INumberBase<TOther>
		{
			bool answer = true;

			switch (Convert.GetTypeCode(typeof(TOther)))
			{
			case TypeCode.Single:
				float actualValue1 = (float)(object)value;

				if (actualValue1 >= MaxValue.Value)
					result = MaxValue;
				else if (actualValue1 <= MinValue.Value)
					result = MinValue;
				else
					result = (long)actualValue1;

				break;

			case TypeCode.Double:
				double actualValue2 = (double)(object)value;

				if (actualValue2 >= MaxValue.Value)
					result = MaxValue;
				else if (actualValue2 <= MinValue.Value)
					result = MinValue;
				else
					result = (long)actualValue2;

				break;

			case TypeCode.SByte:
				result = (sbyte)(object)value;
				break;

			case TypeCode.Int16:
				result = (short)(object)value;
				break;

			case TypeCode.Int32:
				result = (int)(object)value;
				break;

			case TypeCode.Int64:
				result = (long)(object)value;
				break;

			case TypeCode.Object when typeof(TOther) == typeof(Half):
				Half actualValue3 = (Half)(object)value;

				if (actualValue3 == Half.PositiveInfinity)
					result = MaxValue;
				else if (actualValue3 == Half.NegativeInfinity)
					result = MinValue;
				else
					result = (long)actualValue3;

				break;

			case TypeCode.Object when typeof(TOther) == typeof(Int128):
				Int128 actualValue4 = (Int128)(object)value;

				if (actualValue4 >= MaxValue.Value)
					result = MaxValue;
				else if (actualValue4 <= MinValue.Value)
					result = MinValue;
				else
					result = (long)actualValue4;

				break;

			case TypeCode.Object when typeof(TOther) == typeof(IntPtr):
				result = (IntPtr)(object)value;
				break;

			default:
				result = Zero;
				answer = false;
				break;
			}

			return answer;
		}

		internal static bool TryConvertToChecked<TOther>(Integer value, [MaybeNullWhen(false)] out TOther result)
		{
			bool answer = true;

			switch (Convert.GetTypeCode(typeof(TOther)))
			{
			case TypeCode.Byte:
				result = (TOther)(object)checked((byte)value.Value);
				break;

			case TypeCode.Char:
				result = (TOther)(object)checked((char)value.Value);
				break;

			case TypeCode.Decimal:
				result = (TOther)(object)(decimal)value.Value;
				break;

			case TypeCode.UInt16:
				result = (TOther)(object)checked((ushort)value.Value);
				break;

			case TypeCode.UInt32:
				result = (TOther)(object)checked((uint)value.Value);
				break;

			case TypeCode.UInt64:
				result = (TOther)(object)checked((ulong)value.Value);
				break;

			case TypeCode.Object when typeof(TOther) == typeof(UInt128):
				result = (TOther)(object)checked((Int128)value.Value);
				break;

			case TypeCode.Object when typeof(TOther) == typeof(UIntPtr):
				result = (TOther)(object)checked((IntPtr)value.Value);
				break;

			default:
				result = default;
				answer = false;
				break;
			}

			return answer;
		}

		internal static bool TryConvertToSaturating<TOther>(Integer value, [MaybeNullWhen(false)] out TOther result)
		{
			bool answer = true;

			switch (Convert.GetTypeCode(typeof(TOther)))
			{
			case TypeCode.Byte:
				result = (TOther)(object)((value >= MaxValue) ? Byte.MaxValue : (value <= MinValue) ? Byte.MinValue : (byte)value);
				break;

			case TypeCode.Char:
				result = (TOther)(object)((value >= MaxValue) ? Byte.MaxValue : (value <= MinValue) ? Byte.MinValue : (byte)value);
				break;

			case TypeCode.Decimal:
				result = (TOther)(object)(decimal)value;
				break;

			case TypeCode.UInt16:
				result = (TOther)(object)((value >= MaxValue) ? Byte.MaxValue : (value <= MinValue) ? Byte.MinValue : (byte)value);
				break;

			case TypeCode.UInt32:
				result = (TOther)(object)((value >= MaxValue) ? Byte.MaxValue : (value <= MinValue) ? Byte.MinValue : (byte)value);
				break;

			case TypeCode.UInt64:
				result = (TOther)(object)((value >= MaxValue) ? Byte.MaxValue : (value <= MinValue) ? Byte.MinValue : (byte)value);
				break;

			case TypeCode.Object when typeof(TOther) == typeof(UInt128):
				result = (TOther)(object)((value >= MaxValue) ? Byte.MaxValue : (value <= MinValue) ? Byte.MinValue : (byte)value);
				break;

			case TypeCode.Object when typeof(TOther) == typeof(UIntPtr):
				result = (TOther)(object)((value >= MaxValue) ?
#if TARGET_32BIT
					UInt32.MaxValue
#else
					UInt64.MaxValue
#endif
					: (value <= MinValue) ?
#if TARGET_32BIT
					UInt32.MinValue
#else
					UInt64.MinValue
#endif
					: (byte)value);
				break;

			default:
				result = default;
				answer = false;
				break;
			}

			return answer;
		}

		internal static bool TryConvertToTruncating<TOther>(Integer value, [MaybeNullWhen(false)] out TOther result)
		{
			bool answer = true;

			switch (Convert.GetTypeCode(typeof(TOther)))
			{
			case TypeCode.Byte:
				result = (TOther)(object)((value >= MaxValue) ? Byte.MaxValue : (value <= MinValue) ? Byte.MinValue : (byte)value);
				break;

			case TypeCode.Char:
				result = (TOther)(object)((value >= MaxValue) ? Byte.MaxValue : (value <= MinValue) ? Byte.MinValue : (byte)value);
				break;

			case TypeCode.Decimal:
				result = (TOther)(object)(decimal)value;
				break;

			case TypeCode.UInt16:
				result = (TOther)(object)((value >= MaxValue) ? Byte.MaxValue : (value <= MinValue) ? Byte.MinValue : (byte)value);
				break;

			case TypeCode.UInt32:
				result = (TOther)(object)((value >= MaxValue) ? Byte.MaxValue : (value <= MinValue) ? Byte.MinValue : (byte)value);
				break;

			case TypeCode.UInt64:
				result = (TOther)(object)((value >= MaxValue) ? Byte.MaxValue : (value <= MinValue) ? Byte.MinValue : (byte)value);
				break;

			case TypeCode.Object when typeof(TOther) == typeof(UInt128):
				result = (TOther)(object)((value >= MaxValue) ? Byte.MaxValue : (value <= MinValue) ? Byte.MinValue : (byte)value);
				break;

			case TypeCode.Object when typeof(TOther) == typeof(UIntPtr):
				result = (TOther)(object)((value >= MaxValue) ?
#if TARGET_32BIT
					UInt32.MaxValue
#else
					UInt64.MaxValue
#endif
					: (value <= MinValue) ?
#if TARGET_32BIT
					UInt32.MinValue
#else
					UInt64.MinValue
#endif
					: (byte)value);
				break;

			default:
				result = default;
				answer = false;
				break;
			}

			return answer;
		}

		// --------------------------------------------------------------

		static bool INumberBase<Integer>.TryConvertFromChecked<TOther>(TOther value, out Integer result)
			=> TryConvertFromChecked(value, out result);

		static bool INumberBase<Integer>.TryConvertFromSaturating<TOther>(TOther value, out Integer result)
			=> TryConvertFromSaturating(value, out result);

		static bool INumberBase<Integer>.TryConvertFromTruncating<TOther>(TOther value, out Integer result)
			=> TryConvertFromTruncating(value, out result);

		static bool INumberBase<Integer>.TryConvertToChecked<TOther>(Integer value, [MaybeNullWhen(false)] out TOther result)
			=> TryConvertToChecked(value, out result);

		static bool INumberBase<Integer>.TryConvertToSaturating<TOther>(Integer value, [MaybeNullWhen(false)] out TOther result)
			=> TryConvertToSaturating(value, out result);

		static bool INumberBase<Integer>.TryConvertToTruncating<TOther>(Integer value, [MaybeNullWhen(false)] out TOther result)
			=> TryConvertToTruncating(value, out result);

		/* =---- Methods -----------------------------------------------= */

		public int CompareTo(Natural other)
			=> this.Value.CompareTo(other.Value);

		public int CompareTo(Integer other)
			=> this.Value.CompareTo(other.Value);

		public int CompareTo(object? obj)
			=> this.Value.CompareTo(obj);

		public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
			=> this.Value.TryFormat(destination, out charsWritten, format, provider);

		public string ToString(string? format, IFormatProvider? formatProvider)
			=> String.Format(formatProvider, format ?? String.Empty, this.Value);

		public override string ToString()
			=> $"{this.Value}";

		/* =---- Operators ---------------------------------------------= */

		public static Integer operator -(Integer value)
			=> -value.Value;

		public static Integer operator +(Integer value)
			=> +value.Value;

		// --------------------------------------------------------------

		public static Integer operator +(Integer left, Natural right)
			=> left.Value + (long)right.Value;

		public static Integer operator -(Integer left, Natural right)
			=> left.Value - (long)right.Value;

		public static Integer operator *(Integer left, Natural right)
			=> left.Value * (long)right.Value;

		public static Integer operator /(Integer left, Natural right)
			=> left.Value / (long)right.Value;

		public static Integer operator %(Integer left, Natural right)
			=> left.Value % (long)right.Value;

		// --------------------------------------------------------------

		public static Integer operator +(Natural left, Integer right)
			=> (long)left.Value + right.Value;

		public static Integer operator -(Natural left, Integer right)
			=> (long)left.Value - right.Value;

		public static Integer operator *(Natural left, Integer right)
			=> (long)left.Value * right.Value;

		public static Integer operator /(Natural left, Integer right)
			=> (long)left.Value / right.Value;

		public static Integer operator %(Natural left, Integer right)
			=> (long)left.Value % right.Value;

		// --------------------------------------------------------------

		public static Integer operator +(Integer left, Integer right)
			=> left.Value + right.Value;

		public static Integer operator -(Integer left, Integer right)
			=> left.Value - right.Value;

		public static Integer operator *(Integer left, Integer right)
			=> left.Value * right.Value;

		public static Integer operator /(Integer left, Integer right)
			=> left.Value / right.Value;

		public static Integer operator %(Integer left, Integer right)
			=> left.Value % right.Value;

		// --------------------------------------------------------------

		public static Integer operator +(Integer left, int right)
			=> left.Value + right;

		public static Integer operator -(Integer left, int right)
			=> left.Value - right;

		public static Integer operator *(Integer left, int right)
			=> left.Value * right;

		public static Integer operator /(Integer left, int right)
			=> left.Value / right;

		public static Integer operator %(Integer left, int right)
			=> left.Value % right;

		// --------------------------------------------------------------

		public static Integer operator +(int left, Integer right)
			=> left + right.Value;

		public static Integer operator -(int left, Integer right)
			=> left - right.Value;

		public static Integer operator *(int left, Integer right)
			=> left * right.Value;

		public static Integer operator /(int left, Integer right)
			=> left / right.Value;

		public static Integer operator %(int left, Integer right)
			=> left % right.Value;

		/// --------------------------------------------------------------

		public static Integer operator +(Integer left, long right)
			=> left.Value + right;

		public static Integer operator -(Integer left, long right)
			=> left.Value - right;

		public static Integer operator *(Integer left, long right)
			=> left.Value * right;

		public static Integer operator /(Integer left, long right)
			=> left.Value / right;

		public static Integer operator %(Integer left, long right)
			=> left.Value % right;

		// --------------------------------------------------------------

		public static Integer operator +(long left, Integer right)
			=> left + right.Value;

		public static Integer operator -(long left, Integer right)
			=> left - right.Value;

		public static Integer operator *(long left, Integer right)
			=> left * right.Value;

		public static Integer operator /(long left, Integer right)
			=> left / right.Value;

		public static Integer operator %(long left, Integer right)
			=> left % right.Value;

		// --------------------------------------------------------------

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Integer operator *(Integer left, Sign right)
		{
			switch (right)
			{
			case Sign.Positive:
				return +left;

			case Sign.Negative:
				return -left;

			case Sign.Unsigned:
				return Zero;

			default:
				throw new InvalidOperationException();
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Integer operator *(Sign left, Integer right)
			=> right * left;

		// --------------------------------------------------------------

		public static Integer operator --(Integer value)
			=> value.Value - 1;

		public static Integer operator ++(Integer value)
			=> value.Value + 1;

		// --------------------------------------------------------------

		public static bool operator >(Integer left, Natural right)
			=> left.CompareTo(right) > 0;

		public static bool operator >=(Integer left, Natural right)
			=> left.CompareTo(right) >= 0;

		public static bool operator <(Integer left, Natural right)
			=> left.CompareTo(right) < 0;

		public static bool operator <=(Integer left, Natural right)
			=> left.CompareTo(right) <= 0;

		// --------------------------------------------------------------

		public static bool operator >(Natural left, Integer right)
			=> left.CompareTo(right) > 0;

		public static bool operator >=(Natural left, Integer right)
			=> left.CompareTo(right) >= 0;

		public static bool operator <(Natural left, Integer right)
			=> left.CompareTo(right) < 0;

		public static bool operator <=(Natural left, Integer right)
			=> left.CompareTo(right) <= 0;

		// --------------------------------------------------------------

		public static bool operator >(Integer left, Integer right)
			=> left.CompareTo(right) > 0;

		public static bool operator >=(Integer left, Integer right)
			=> left.CompareTo(right) >= 0;

		public static bool operator <(Integer left, Integer right)
			=> left.CompareTo(right) < 0;

		public static bool operator <=(Integer left, Integer right)
			=> left.CompareTo(right) <= 0;

		// --------------------------------------------------------------

		public static bool operator >(Integer left, int right)
			=> left.CompareTo((Integer)right) > 0;

		public static bool operator >=(Integer left, int right)
			=> left.CompareTo((Integer)right) >= 0;

		public static bool operator <(Integer left, int right)
			=> left.CompareTo((Integer)right) < 0;

		public static bool operator <=(Integer left, int right)
			=> left.CompareTo((Integer)right) <= 0;

		// --------------------------------------------------------------

		public static bool operator >(int left, Integer right)
			=> left.CompareTo(right.Value) > 0;

		public static bool operator >=(int left, Integer right)
			=> left.CompareTo(right.Value) >= 0;

		public static bool operator <(int left, Integer right)
			=> left.CompareTo(right.Value) < 0;

		public static bool operator <=(int left, Integer right)
			=> left.CompareTo(right.Value) <= 0;

		// --------------------------------------------------------------

		public static bool operator >(Integer left, long right)
			=> left.CompareTo((Integer)right) > 0;

		public static bool operator >=(Integer left, long right)
			=> left.CompareTo((Integer)right) >= 0;

		public static bool operator <(Integer left, long right)
			=> left.CompareTo((Integer)right) < 0;

		public static bool operator <=(Integer left, long right)
			=> left.CompareTo((Integer)right) <= 0;

		// --------------------------------------------------------------

		public static bool operator >(long left, Integer right)
			=> left.CompareTo(right.Value) > 0;

		public static bool operator >=(long left, Integer right)
			=> left.CompareTo(right.Value) >= 0;

		public static bool operator <(long left, Integer right)
			=> left.CompareTo(right.Value) < 0;

		public static bool operator <=(long left, Integer right)
			=> left.CompareTo(right.Value) <= 0;


		// --------------------------------------------------------------

		public static implicit operator Integer(Natural that)
			=> new((long)that.Value);

		public static implicit operator Natural(Integer that)
			=> that.Value;

		// --------------------------------------------------------------

		public static implicit operator Integer(long that)
			=> new(that);

		public static implicit operator long(Integer that)
			=> that.Value;

		// --------------------------------------------------------------

		public static implicit operator Integer(double that)
			=> new((long)that);

		public static implicit operator double(Integer that)
			=> that.Value;

		/* =------------------------------------------------------------= */
	}
}
