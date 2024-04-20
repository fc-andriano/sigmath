using Sigmath.CodeGen.Extensions;

namespace Sigmath.CodeGen.Interop
{
    internal interface IReference
	{
		/* =---- Static Methods ----------------------------------------= */

		public static string GetRefName(IReference opaque)
			=> opaque.Handle.IsNotZero() ? $"{opaque.GetType().Name}: 0x{opaque.Handle:X16}" : "NULL";

		/* =---- Properties --------------------------------------------= */

		nint Handle { get; }

		/* =---- Methods -----------------------------------------------= */

		public virtual bool Equals(IReference other)
			=> this.Handle.Equals(other.Handle);

		/* =------------------------------------------------------------= */
	}
}
