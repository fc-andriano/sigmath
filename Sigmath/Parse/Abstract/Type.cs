using Sigmath.CodeGen.Interop;

using System;
using System.Diagnostics;

namespace Sigmath.Parse.Abstract
{
	[DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(), nq}}")]
	public sealed class Type(ReferenceType value) :
		IEquatable<Type>
	{
		/* =---- Static Methods ----------------------------------------= */

		public static Type GetTypeOfValue(Value value)
			=> ReferenceType.TypeOf(value.RefValue);

		/* =---- Properties --------------------------------------------= */

		public ReferenceType RefType => value;

		/* =---- Methods -----------------------------------------------= */

		public bool Equals(Type? other)
			=> this.RefType == other?.RefType;

		public override bool Equals(object? obj)
			=> obj is Type other && this.Equals(other);

		public override int GetHashCode()
			=> this.RefType.GetHashCode();

		public override string ToString()
			=> this.RefType.ToString();

		// --------------------------------------------------------------

		internal string GetDebuggerDisplay()
			=> this.RefType.GetDebuggerDisplay();

		/* =---- Operators ---------------------------------------------= */

		public static bool operator ==(Type left, Type right)
			=> left.Equals(right);

		public static bool operator !=(Type left, Type right)
			=> !left.Equals(right);

		// --------------------------------------------------------------

		public static implicit operator Type(ReferenceType value)
			=> new(value);

		public static implicit operator ReferenceType(Type value)
			=> value.RefType;

		/* =------------------------------------------------------------= */
	}
}
