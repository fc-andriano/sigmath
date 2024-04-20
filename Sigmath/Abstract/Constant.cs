using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Sigmath.Abstract
{
	public abstract class Constant<TValue>(TValue value) :
		Expression, IEquatable<Constant<TValue>> where TValue : struct
	{
		/* =---- Properties --------------------------------------------= */

		public TValue Value => value;

		/* =---- Methods -----------------------------------------------= */

		public bool Equals(Constant<TValue>? other)
			=> this.Value.Equals(other?.Value);

		public override bool Equals(object? obj)
			=> obj is Constant<TValue> other ? this.Equals(other) : this.Value.Equals(obj);

		public override int GetHashCode()
			=> base.GetHashCode();

		public override string ToString()
			=> $"{this.Value}";

		/* =---- Operators ---------------------------------------------= */

		public static bool operator ==(Constant<TValue> left, Constant<TValue> right)
			=> left.Equals(right);

		public static bool operator !=(Constant<TValue> left, Constant<TValue> right)
			=> !left.Equals(right);

		/* =------------------------------------------------------------= */
	}
}
