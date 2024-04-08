using System;
using System.Diagnostics;

namespace Sigmath.CodeGen.Interop
{
	[DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(), nq}}")]
	public unsafe readonly struct ReferenceBasicBlock(void* ptr) :
		IDisposable, IEquatable<ReferenceBasicBlock>, IReference
	{
		private readonly void* _internalPtr = ptr;

		/* =---- Static Methods ----------------------------------------= */

		public static ReferenceBasicBlock Create(ReferenceContext context, string name = "")
		{
			if (context.Handle.IsZero())
				throw new NullReferenceException();

			using (ReferenceString refString = ReferenceString.Marshal(name))
			{
				return LLVM.CreateBasicBlockInContext(context, refString);
			}
		}

		/* =---- Properties --------------------------------------------= */

		public nint Handle => (nint)_internalPtr;

		/* =---- Methods -----------------------------------------------= */

		public void Dispose()
			=> LLVM.DeleteBasicBlock(_internalPtr);

		public bool Equals(ReferenceBasicBlock other)
			=> this.Handle.Equals(other.Handle);

		public override bool Equals(object? obj)
			=> obj is ReferenceBasicBlock other && this.Equals(other);

		public override int GetHashCode()
			=> this.Handle.GetHashCode();

		public override string ToString()
			=> IReference.GetRefName(this);

		// --------------------------------------------------------------

		internal string GetDebuggerDisplay()
			=> this.ToString();

		/* =---- Operators ---------------------------------------------= */

		public static bool operator ==(ReferenceBasicBlock left, ReferenceBasicBlock right)
			=> left.Equals(right);

		public static bool operator !=(ReferenceBasicBlock left, ReferenceBasicBlock right)
			=> !left.Equals(right);

		// --------------------------------------------------------------

		public static implicit operator void*(ReferenceBasicBlock that)
			=> that._internalPtr;

		public static implicit operator ReferenceBasicBlock(void* that)
			=> new(that);

		public static implicit operator nint(ReferenceBasicBlock that)
			=> that.Handle;

		public static implicit operator ReferenceBasicBlock(nint that)
			=> new(that.ToPointer());

		/* =------------------------------------------------------------= */
	}
}
