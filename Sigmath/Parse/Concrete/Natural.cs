using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;

namespace Sigmath.Parse.Concrete
{
	public readonly record struct Natural(ulong Value) :
		IEquatable<Natural>, IComparable<Natural>, INumber<Natural>
	{
		/* =---- Static Fields -----------------------------------------= */

		public static readonly Natural MinValue = UInt64.MinValue;
		public static readonly Natural MaxValue = UInt64.MaxValue;

		/* =---- Static Properties -------------------------------------= */

		public static int Radix => 10;

		// --------------------------------------------------------------

		public static Natural One => 1;
		public static Natural Zero => 0;
		public static Natural AdditiveIdentity => Zero;
		public static Natural MultiplicativeIdentity => One;

		/* =---- Static Methods ----------------------------------------= */

		public static bool IsCanonical(Natural value)
			=> true;

		public static bool IsComplexNumber(Natural value)
			=> false;

		public static bool IsEvenInteger(Natural value)
			=> (value.Value % 2) == 0;

		public static bool IsFinite(Natural value)
			=> true;

		public static bool IsImaginaryNumber(Natural value)
			=> false;

		public static bool IsInfinity(Natural value)
			=> false;

		public static bool IsInteger(Natural value)
			=> true;

		public static bool IsNaN(Natural value)
			=> false;

		public static bool IsNegative(Natural value)
			=> false;

		public static bool IsNegativeInfinity(Natural value)
			=> false;

		public static bool IsNormal(Natural value)
			=> true;

		public static bool IsOddInteger(Natural value)
			=> (value.Value % 2) != 0;

		public static bool IsPositive(Natural value)
			=> value.Value > 0;

		public static bool IsPositiveInfinity(Natural value)
			=> false;

		public static bool IsRealNumber(Natural value)
			=> true;

		public static bool IsSubnormal(Natural value)
			=> false;

		public static bool IsZero(Natural value)
			=> value.Value == 0;

		// --------------------------------------------------------------

		public static Natural Abs(Natural value)
			=> value;

		public static Natural MaxMagnitude(Natural x, Natural y)
			=> (x > y) ? x : y;

		public static Natural MinMagnitude(Natural x, Natural y)
			=> (x < y) ? x : y;

		public static Natural MaxMagnitudeNumber(Natural x, Natural y)
			=> MaxMagnitude(x, y);

		public static Natural MinMagnitudeNumber(Natural x, Natural y)
			=> MinMagnitude(x, y);

		// --------------------------------------------------------------

		public static Natural Parse(string s, NumberStyles style, IFormatProvider? provider)
			=> UInt64.Parse(s, style, provider);

		public static Natural Parse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider)
			=> UInt64.Parse(s, style, provider);

		public static Natural Parse(string s, IFormatProvider? provider)
			=> UInt64.Parse(s, provider);

		public static Natural Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
			=> UInt64.Parse(s, provider);

		public static Natural Parse(string s)
			=> UInt64.Parse(s);

		public static Natural Parse(ReadOnlySpan<char> s)
			=> UInt64.Parse(s);

		public static Natural Parse(string s, int radix)
			=> Convert.ToInt64(s, radix);

		public static Natural Parse(ReadOnlySpan<char> s, int radix)
			=> s.IsEmpty ? Zero : Parse(s.ToString(), radix);

		// --------------------------------------------------------------

		public static bool TryParse([NotNullWhen(true)] string? s, NumberStyles style, IFormatProvider? provider, [MaybeNullWhen(false)] out Natural result)
		{
			bool answer;

			if (answer = UInt64.TryParse(s, style, provider, out ulong r))
				result = r;
			else
				result = default;

			return answer;
		}

		public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider, [MaybeNullWhen(false)] out Natural result)

		{
			bool answer;

			if (answer = UInt64.TryParse(s, style, provider, out ulong r))
				result = r;
			else
				result = default;

			return answer;
		}

		public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Natural result)
		{
			bool answer;

			if (answer = UInt64.TryParse(s, provider, out ulong r))
				result = r;
			else
				result = default;

			return answer;
		}

		public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, [MaybeNullWhen(false)] out Natural result)
		{
			bool answer;

			if (answer = UInt64.TryParse(s, provider, out ulong r))
				result = r;
			else
				result = default;

			return answer;
		}

		public static bool TryParse([NotNullWhen(true)] string? s, [MaybeNullWhen(false)] out Natural result)
		{
			bool answer;

			if (answer = Int64.TryParse(s, out long r))
				result = r;
			else
				result = default;

			return answer;
		}

		public static bool TryParse(ReadOnlySpan<char> s, [MaybeNullWhen(false)] out Natural result)
		{
			bool answer;

			if (answer = Int64.TryParse(s, out long r))
				result = r;
			else
				result = default;

			return answer;
		}

		// --------------------------------------------------------------

		internal static bool TryConvertFromChecked<TOther>(TOther value, out Natural result)
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

		internal static bool TryConvertFromSaturating<TOther>(TOther value, out Natural result)
			where TOther : INumberBase<TOther>
		{
			bool answer = true;

			switch (Convert.GetTypeCode(typeof(TOther)))
			{
			case TypeCode.Single:
				float actualValue1 = (float)(object)value;

				if (actualValue1 >= MaxValue)
					result = MaxValue;
				else if (actualValue1 <= MinValue)
					result = MinValue;
				else
					result = (long)actualValue1;

				break;

			case TypeCode.Double:
				double actualValue2 = (double)(object)value;

				if (actualValue2 >= MaxValue)
					result = MaxValue;
				else if (actualValue2 <= MinValue)
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

		internal static bool TryConvertFromTruncating<TOther>(TOther value, out Natural result)
			where TOther : INumberBase<TOther>
		{
			bool answer = true;

			switch (Convert.GetTypeCode(typeof(TOther)))
			{
			case TypeCode.Single:
				float actualValue1 = (float)(object)value;

				if (actualValue1 >= MaxValue)
					result = MaxValue;
				else if (actualValue1 <= MinValue)
					result = MinValue;
				else
					result = (long)actualValue1;

				break;

			case TypeCode.Double:
				double actualValue2 = (double)(object)value;

				if (actualValue2 >= MaxValue)
					result = MaxValue;
				else if (actualValue2 <= MinValue)
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

		internal static bool TryConvertToChecked<TOther>(Natural value, [MaybeNullWhen(false)] out TOther result)
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

		internal static bool TryConvertToSaturating<TOther>(Natural value, [MaybeNullWhen(false)] out TOther result)
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

		internal static bool TryConvertToTruncating<TOther>(Natural value, [MaybeNullWhen(false)] out TOther result)
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

		static bool INumberBase<Natural>.TryConvertFromChecked<TOther>(TOther value, out Natural result)
			=> TryConvertFromChecked(value, out result);

		static bool INumberBase<Natural>.TryConvertFromSaturating<TOther>(TOther value, out Natural result)
			=> TryConvertFromSaturating(value, out result);

		static bool INumberBase<Natural>.TryConvertFromTruncating<TOther>(TOther value, out Natural result)
			=> TryConvertFromTruncating(value, out result);

		static bool INumberBase<Natural>.TryConvertToChecked<TOther>(Natural value, [MaybeNullWhen(false)] out TOther result)
			=> TryConvertToChecked(value, out result);

		static bool INumberBase<Natural>.TryConvertToSaturating<TOther>(Natural value, [MaybeNullWhen(false)] out TOther result)
			=> TryConvertToSaturating(value, out result);

		static bool INumberBase<Natural>.TryConvertToTruncating<TOther>(Natural value, [MaybeNullWhen(false)] out TOther result)
			=> TryConvertToTruncating(value, out result);

		/* =---- Methods -----------------------------------------------= */

		public int CompareTo(Natural other)
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

		public static Natural operator -(Natural value)
			=> throw new InvalidOperationException();

		public static Natural operator +(Natural value)
			=> throw new InvalidOperationException();

		// --------------------------------------------------------------

		public static Natural operator +(Natural left, Natural right)
			=> left.Value + right.Value;

		public static Natural operator -(Natural left, Natural right)
			=> left.Value - right.Value;

		public static Natural operator *(Natural left, Natural right)
			=> left.Value * right.Value;

		public static Natural operator /(Natural left, Natural right)
			=> left.Value / right.Value;

		public static Natural operator %(Natural left, Natural right)
			=> left.Value % right.Value;

		// --------------------------------------------------------------

		public static Natural operator +(Natural left, uint right)
			=> left.Value + right;

		public static Natural operator -(Natural left, uint right)
			=> left.Value - right;

		public static Natural operator *(Natural left, uint right)
			=> left.Value * right;

		public static Natural operator /(Natural left, uint right)
			=> left.Value / right;

		public static Natural operator %(Natural left, uint right)
			=> left.Value % right;

		// --------------------------------------------------------------

		public static Natural operator +(uint left, Natural right)
			=> left + right.Value;

		public static Natural operator -(uint left, Natural right)
			=> left - right.Value;

		public static Natural operator *(uint left, Natural right)
			=> left * right.Value;

		public static Natural operator /(uint left, Natural right)
			=> left / right.Value;

		public static Natural operator %(uint left, Natural right)
			=> left % right.Value;

		/// --------------------------------------------------------------

		public static Natural operator +(Natural left, ulong right)
			=> left.Value + right;

		public static Natural operator -(Natural left, ulong right)
			=> left.Value - right;

		public static Natural operator *(Natural left, ulong right)
			=> left.Value * right;

		public static Natural operator /(Natural left, ulong right)
			=> left.Value / right;

		public static Natural operator %(Natural left, ulong right)
			=> left.Value % right;

		// --------------------------------------------------------------

		public static Natural operator +(ulong left, Natural right)
			=> left + right.Value;

		public static Natural operator -(ulong left, Natural right)
			=> left - right.Value;

		public static Natural operator *(ulong left, Natural right)
			=> left * right.Value;

		public static Natural operator /(ulong left, Natural right)
			=> left / right.Value;

		public static Natural operator %(ulong left, Natural right)
			=> left % right.Value;

		// --------------------------------------------------------------

		public static Natural operator --(Natural value)
			=> value.Value - 1;

		public static Natural operator ++(Natural value)
			=> value.Value + 1;

		// --------------------------------------------------------------

		public static bool operator >(Natural left, Natural right)
			=> left.CompareTo(right) > 0;

		public static bool operator >=(Natural left, Natural right)
			=> left.CompareTo(right) >= 0;

		public static bool operator <(Natural left, Natural right)
			=> left.CompareTo(right) < 0;

		public static bool operator <=(Natural left, Natural right)
			=> left.CompareTo(right) <= 0;

		// --------------------------------------------------------------

		public static bool operator >(Natural left, uint right)
			=> left.CompareTo(right) > 0;

		public static bool operator >=(Natural left, uint right)
			=> left.CompareTo(right) >= 0;

		public static bool operator <(Natural left, uint right)
			=> left.CompareTo(right) < 0;

		public static bool operator <=(Natural left, uint right)
			=> left.CompareTo(right) <= 0;

		// --------------------------------------------------------------

		public static bool operator >(uint left, Natural right)
			=> left.CompareTo(right.Value) > 0;

		public static bool operator >=(uint left, Natural right)
			=> left.CompareTo(right.Value) >= 0;

		public static bool operator <(uint left, Natural right)
			=> left.CompareTo(right.Value) < 0;

		public static bool operator <=(uint left, Natural right)
			=> left.CompareTo(right.Value) <= 0;

		// --------------------------------------------------------------

		public static bool operator >(Natural left, ulong right)
			=> left.CompareTo(right) > 0;

		public static bool operator >=(Natural left, ulong right)
			=> left.CompareTo(right) >= 0;

		public static bool operator <(Natural left, ulong right)
			=> left.CompareTo(right) < 0;

		public static bool operator <=(Natural left, ulong right)
			=> left.CompareTo(right) <= 0;

		// --------------------------------------------------------------

		public static bool operator >(ulong left, Natural right)
			=> left.CompareTo(right.Value) > 0;

		public static bool operator >=(ulong left, Natural right)
			=> left.CompareTo(right.Value) >= 0;

		public static bool operator <(ulong left, Natural right)
			=> left.CompareTo(right.Value) < 0;

		public static bool operator <=(ulong left, Natural right)
			=> left.CompareTo(right.Value) <= 0;

		// --------------------------------------------------------------

		public static implicit operator Natural(ulong that)
			=> new(that);

		public static implicit operator ulong(Natural that)
			=> that.Value;

		// --------------------------------------------------------------

		public static implicit operator Natural(double that)
			=> new((ulong)that);

		public static implicit operator double(Natural that)
			=> that.Value;

		/* =------------------------------------------------------------= */
	}
}
