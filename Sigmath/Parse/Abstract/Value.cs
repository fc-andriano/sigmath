using Sigmath.CodeGen.Interop;

using System;
using System.Diagnostics;

namespace Sigmath.Parse.Abstract
{
	[DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(), nq}}")]
	public sealed class Value(ReferenceValue value) :
		IEquatable<Value>
	{
		/* =---- Properties --------------------------------------------= */

		public ReferenceValue RefValue => value;

		/* =---- Methods -----------------------------------------------= */

		public bool Equals(Value? other)
			=> this.RefValue == other?.RefValue;

		public override bool Equals(object? obj)
			=> obj is Value other && this.Equals(other);

		public override int GetHashCode()
			=> this.RefValue.GetHashCode();

		public override string ToString()
			=> this.RefValue.ToString();

		// --------------------------------------------------------------

		internal string GetDebuggerDisplay()
			=> this.RefValue.GetDebuggerDisplay();

		/* =---- Operators ---------------------------------------------= */

		public static bool operator ==(Value left, Value right)
			=> left.Equals(right);

		public static bool operator !=(Value left, Value right)
			=> !left.Equals(right);

		// --------------------------------------------------------------

		public static implicit operator Value(ReferenceValue value)
			=> new(value);

		public static implicit operator ReferenceValue(Value value)
			=> value.RefValue;

		/* =------------------------------------------------------------= */
	}
}
