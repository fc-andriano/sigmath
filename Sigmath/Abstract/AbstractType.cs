using Sigmath.CodeGen.Interop;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sigmath.Abstract
{
	[DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(), nq}}")]
	public sealed class AbstractType(ReferenceType value) :
		IEquatable<AbstractType>
	{
		/* =---- Static Methods ----------------------------------------= */

		public static AbstractType GetTypeOfValue(AbstractValue value)
			=> (AbstractType)ReferenceType.TypeOf(value.Value);

		/* =---- Properties --------------------------------------------= */

		public ReferenceType Value => value;

		/* =---- Methods -----------------------------------------------= */

		public bool Equals(AbstractType? other)
			=> this.Value == other?.Value;

		public override bool Equals(object? obj)
			=> obj is AbstractType other && this.Equals(other);

		public override int GetHashCode()
			=> this.Value.GetHashCode();

		public override string ToString()
			=> this.Value.ToString();

		// --------------------------------------------------------------

		internal string GetDebuggerDisplay()
			=> this.Value.GetDebuggerDisplay();

		/* =---- Operators ---------------------------------------------= */

		public static bool operator ==(AbstractType left, AbstractType right)
			=> left.Equals(right);

		public static bool operator !=(AbstractType left, AbstractType right)
			=> !left.Equals(right);

		// --------------------------------------------------------------

		public static implicit operator AbstractType(ReferenceType value)
			=> new(value);

		public static implicit operator ReferenceType(AbstractType value)
			=> value.Value;

		/* =------------------------------------------------------------= */
	}
}
