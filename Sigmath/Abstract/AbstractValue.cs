using Sigmath.CodeGen.Interop;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Sigmath.Abstract
{
	[DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(), nq}}")]
	public sealed class AbstractValue(ReferenceValue value) :
		IEquatable<AbstractValue>
	{
		/* =---- Properties --------------------------------------------= */

		public ReferenceValue Value => value;

		/* =---- Methods -----------------------------------------------= */

		public bool Equals(AbstractValue? other)
			=> this.Value == other?.Value;

		public override bool Equals(object? obj)
			=> obj is AbstractValue other && this.Equals(other);

		public override int GetHashCode()
			=> this.Value.GetHashCode();

		public override string ToString()
			=> this.Value.ToString();

		// --------------------------------------------------------------

		internal string GetDebuggerDisplay()
			=> this.Value.GetDebuggerDisplay();

		/* =---- Operators ---------------------------------------------= */

		public static bool operator ==(AbstractValue left, AbstractValue right)
			=> left.Equals(right);

		public static bool operator !=(AbstractValue left, AbstractValue right)
			=> !left.Equals(right);

		// --------------------------------------------------------------

		public static implicit operator AbstractValue(ReferenceValue value)
			=> new(value);

		public static implicit operator ReferenceValue(AbstractValue value)
			=> value.Value;

		/* =------------------------------------------------------------= */
	}
}
