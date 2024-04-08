using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;

namespace Sigmath.Parse.Concrete
{
	public readonly record struct Fraction(Integer Numerator, Integer Denominator) :
		IEquatable<Fraction>, IComparable<Fraction>, INumber<Fraction>
	{
		/* =---- Static Fields -----------------------------------------= */

		public static readonly Fraction MinValue = Integer.MinValue;
		public static readonly Fraction MaxValue = Integer.MaxValue;

		/* =---- Static Properties -------------------------------------= */

		public static int Radix => 10;

		// --------------------------------------------------------------

		public static Fraction One => 1;
		public static Fraction Zero => 0;
		public static Fraction AdditiveIdentity => Zero;
		public static Fraction MultiplicativeIdentity => One;

		/* =---- Static Methods ----------------------------------------= */

		public static Integer GetGreatestCommonDivisor(Integer a, Integer b)
		{
			while ((a != 0) && (b != 0))
			{
				if (a > b)
					a %= b;
				else
					b %= a;
			}

			return a | b;
		}

		public static Integer GetLeastCommonMultiple(Integer a, Integer b)
		{
			return a / GetGreatestCommonDivisor(a, b) * b;
		}

		// --------------------------------------------------------------

		public static bool IsCanonical(Fraction value)
			=> true;

		public static bool IsComplexNumber(Fraction value)
			=> false;

		public static bool IsEvenInteger(Fraction value)
			=> IsInteger(value) && ((value.Numerator % 2) == 0);

		public static bool IsFinite(Fraction value)
			=> true;

		public static bool IsImaginaryNumber(Fraction value)
			=> false;

		public static bool IsInfinity(Fraction value)
			=> false;

		public static bool IsInteger(Fraction value)
			=> value.Denominator == 1;

		public static bool IsNaN(Fraction value)
			=> value.Denominator == 0;

		public static bool IsNegative(Fraction value)
			=> value.Numerator < 0;

		public static bool IsNegativeInfinity(Fraction value)
			=> false;

		public static bool IsNormal(Fraction value)
			=> GetGreatestCommonDivisor(Integer.Abs(value.Numerator), value.Denominator) > 1;

		public static bool IsOddInteger(Fraction value)
			=> IsInteger(value) && ((value.Numerator % 2) != 0);

		public static bool IsPositive(Fraction value)
			=> value.Numerator > 0;

		public static bool IsPositiveInfinity(Fraction value)
			=> false;

		public static bool IsRealNumber(Fraction value)
			=> value.Denominator != 0;

		public static bool IsSubnormal(Fraction value)
			=> false;

		public static bool IsZero(Fraction value)
			=> value.Numerator == 0;

		// --------------------------------------------------------------

		public static Fraction Create(double value)
		{
			Fraction fraction;

			if ((value % 1) != 0)
			{
				bool sign = value < 0;

				if (sign)
					value = -value;

				long num = 0;

				while ((value % 1) != 0)
				{
					value *= 10;

					if (num == 0)
						num = 10;
					else
						num *= 10;
				}

				fraction = Reduce((long)value, num, sign ? Sign.Negative : Sign.Positive);
			}
			else
			{
				fraction = ((long)value, 1);
			}

			return fraction;
		}

		// --------------------------------------------------------------

		public static Fraction Reduce(Integer numerator, Integer denominator, Sign sign)
		{
			if (denominator == 0)
				throw new DivideByZeroException();

			Fraction result;

			if (sign != 0)
			{
				long gcd = GetGreatestCommonDivisor(numerator, denominator);

				if (gcd == 1)
					result = (sign * numerator, denominator);
				else
					result = (sign * numerator / gcd, denominator / gcd);
			}
			else
			{
				result = Zero;
			}

			return result;
		}

		public static Fraction Reduce(Integer numerator, Integer denominator)
			=> Reduce(numerator, denominator, Integer.GetSign(numerator));

		public static Fraction Reduce(Fraction fraction)
			=> Reduce(fraction.Numerator, fraction.Denominator);

		// --------------------------------------------------------------

		public static Fraction Abs(Fraction value)
			=> IsNegative(value) ? -value : value;

		public static Fraction MaxMagnitude(Fraction x, Fraction y)
			=> (x > y) ? x : y;

		public static Fraction MinMagnitude(Fraction x, Fraction y)
			=> (x < y) ? x : y;

		public static Fraction MaxMagnitudeNumber(Fraction x, Fraction y)
			=> IsNaN(x) ? y :
			   IsNaN(y) ? x : MaxMagnitude(x, y);

		public static Fraction MinMagnitudeNumber(Fraction x, Fraction y)
			=> IsNaN(x) ? y :
			   IsNaN(y) ? x : MinMagnitude(x, y);

		// --------------------------------------------------------------

		public static Fraction Parse(string s, NumberStyles style, IFormatProvider? provider)
		{
			if (String.IsNullOrWhiteSpace(s))
				throw new ArgumentNullException(nameof(s));

			int i = s.IndexOf('/');

			if ((i != -1) && (i < (s.Length - 1)))
				return (Int64.Parse(s[..i], style, provider), Int64.Parse(s[(i + 1)..], style, provider));
			else
				return Int64.Parse(s, style, provider);
		}

		public static Fraction Parse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider)
			=> Parse(s.ToString(), style, provider);

		public static Fraction Parse(string s, IFormatProvider? provider)
			=> Parse(s, NumberStyles.None, provider);

		public static Fraction Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
			=> Parse(s, NumberStyles.None, provider);

		public static Fraction Parse(string s)
			=> Parse(s, NumberStyles.None, null);

		public static Fraction Parse(ReadOnlySpan<char> s)
			=> Parse(s, NumberStyles.None, null);

		// --------------------------------------------------------------

		public static bool TryParse([NotNullWhen(true)] string? s, NumberStyles style, IFormatProvider? provider, [MaybeNullWhen(false)] out Fraction result)
		{
			bool answer;

			try
			{
				result = Parse(s ?? String.Empty, style, provider);
				answer = true;
			}
			catch
			{
				result = default;
				answer = false;
			}

			return answer;
		}

		public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider, [MaybeNullWhen(false)] out Fraction result)
			=> TryParse(s.ToString(), style, provider, out result);

		public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Fraction result)
			=> TryParse(s, NumberStyles.None, provider, out result);

		public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, [MaybeNullWhen(false)] out Fraction result)
			=> TryParse(s, NumberStyles.None, provider, out result);

		// --------------------------------------------------------------

		static bool INumberBase<Fraction>.TryConvertFromChecked<TOther>(TOther value, out Fraction result) => throw new NotImplementedException();

		static bool INumberBase<Fraction>.TryConvertFromSaturating<TOther>(TOther value, out Fraction result) => throw new NotImplementedException();

		static bool INumberBase<Fraction>.TryConvertFromTruncating<TOther>(TOther value, out Fraction result) => throw new NotImplementedException();

		static bool INumberBase<Fraction>.TryConvertToChecked<TOther>(Fraction value, out TOther result) => throw new NotImplementedException();

		static bool INumberBase<Fraction>.TryConvertToSaturating<TOther>(Fraction value, out TOther result) => throw new NotImplementedException();

		static bool INumberBase<Fraction>.TryConvertToTruncating<TOther>(Fraction value, out TOther result) => throw new NotImplementedException();

		/* =---- Methods -----------------------------------------------= */

		public Fraction Reduce()
			=> Reduce(this.Numerator, this.Denominator);

		// --------------------------------------------------------------

		public int CompareTo(Fraction other)
			=> (this.Numerator * other.Denominator).CompareTo(this.Denominator * other.Numerator);

		public int CompareTo(object? obj)
			=> (this.Numerator / this.Denominator).CompareTo(obj);

		public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
			=> this.ToDouble().TryFormat(destination, out charsWritten, format, provider);

		public double ToDouble()
			=> (double)this.Numerator / (double)this.Denominator;

		public long ToInteger()
			=> this.Numerator / this.Denominator;

		public string ToString(string? format, IFormatProvider? formatProvider)
			=> String.Format(formatProvider, format ?? String.Empty, this.ToDouble());

		public override string ToString()
			=> $"{this.Numerator}/{this.Denominator}";

		/* =---- Operators ---------------------------------------------= */

		public static Fraction operator -(Fraction value)
			=> (-value.Numerator, value.Denominator);

		public static Fraction operator +(Fraction value)
			=> (+value.Numerator, value.Denominator);

		// --------------------------------------------------------------

		public static Fraction operator +(Fraction left, int right)
			=> (left.Numerator + (right * left.Denominator), left.Denominator);

		public static Fraction operator -(Fraction left, int right)
			=> (left.Numerator - (right * left.Denominator), left.Denominator);

		public static Fraction operator *(Fraction left, int right)
			=> (left.Numerator * right, left.Denominator);

		public static Fraction operator /(Fraction left, int right)
			=> (left.Numerator, left.Denominator * right);

		public static Fraction operator %(Fraction left, int right)
			=> (left.Numerator % right, left.Denominator);

		// --------------------------------------------------------------

		public static Fraction operator +(Fraction left, long right)
			=> (left.Numerator + (right * left.Denominator), left.Denominator);

		public static Fraction operator -(Fraction left, long right)
			=> (left.Numerator - (right * left.Denominator), left.Denominator);

		public static Fraction operator *(Fraction left, long right)
			=> (left.Numerator * right, left.Denominator);

		public static Fraction operator /(Fraction left, long right)
			=> (left.Numerator, left.Denominator * right);

		public static Fraction operator %(Fraction left, long right)
			=> (left.Numerator % right, left.Denominator);

		// --------------------------------------------------------------

		public static Fraction operator +(Fraction left, Fraction right)
		{
			if (left.Denominator == right.Denominator)
				return (left.Numerator + right.Numerator, left.Denominator);
			else
				return ((left.Numerator * right.Denominator) + (left.Denominator * right.Numerator), left.Denominator * right.Denominator);
		}

		public static Fraction operator -(Fraction left, Fraction right)
		{
			if (left.Denominator == right.Denominator)
				return (left.Numerator - right.Numerator, left.Denominator);
			else
				return ((left.Numerator * right.Denominator) - (left.Denominator * right.Numerator), left.Denominator * right.Denominator);
		}

		public static Fraction operator *(Fraction left, Fraction right)
		{
			return (left.Numerator * right.Numerator, left.Denominator * right.Denominator);
		}

		public static Fraction operator /(Fraction left, Fraction right)
		{
			if (left.Denominator == right.Denominator)
				return (left.Numerator, right.Numerator);
			else
				return (left.Numerator * right.Denominator, left.Denominator * right.Numerator);
		}

		public static Fraction operator %(Fraction left, Fraction right)
		{
			if (left.Denominator == right.Denominator)
				return Zero;
			else
				return right * ((left / right) - (left / right).ToInteger());
		}

		// --------------------------------------------------------------

		public static Fraction operator --(Fraction value)
			=> (value.Numerator - value.Denominator, value.Denominator);

		public static Fraction operator ++(Fraction value)
			=> (value.Numerator + value.Denominator, value.Denominator);

		// --------------------------------------------------------------

		public static bool operator >(Fraction left, Fraction right)
			=> left.CompareTo(right) > 0;

		public static bool operator >=(Fraction left, Fraction right)
			=> left.CompareTo(right) >= 0;

		public static bool operator <(Fraction left, Fraction right)
			=> left.CompareTo(right) < 0;

		public static bool operator <=(Fraction left, Fraction right)
			=> left.CompareTo(right) <= 0;

		// --------------------------------------------------------------

		public static implicit operator Fraction((long Numerator, long Denominator) that)
			=> new(that.Numerator, that.Denominator);

		public static implicit operator (long Numerator, long Denominator)(Fraction that)
			=> (that.Numerator, that.Denominator);

		// --------------------------------------------------------------

		public static implicit operator Fraction(Natural that)
			=> new(that, 1);

		public static implicit operator Natural(Fraction that)
			=> (that.Denominator == 1) ? that.Numerator : (that.Numerator / that.Denominator);

		// --------------------------------------------------------------

		public static implicit operator Fraction(Integer that)
			=> new(that, 1);

		public static implicit operator Integer(Fraction that)
			=> (that.Denominator == 1) ? that.Numerator : (that.Numerator / that.Denominator);

		// --------------------------------------------------------------

		public static implicit operator Fraction(long that)
			=> new(that, 1);

		public static implicit operator long(Fraction that)
			=> (that.Denominator == 1) ? that.Numerator : (that.Numerator / that.Denominator);

		// --------------------------------------------------------------

		public static implicit operator Fraction(double that)
			=> Create(that);

		public static implicit operator double(Fraction that)
			=> (that.Denominator == 1) ? that.Numerator : ((double)that.Numerator / (double)that.Denominator);

		/* =------------------------------------------------------------= */
	}
}
