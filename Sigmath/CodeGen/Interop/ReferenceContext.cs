using System;
using System.Diagnostics;

namespace Sigmath.CodeGen.Interop
{
	[DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(), nq}}")]
	public unsafe readonly struct ReferenceContext(void* ptr) :
		IDisposable, IEquatable<ReferenceContext>, IReference
	{
		private readonly void* _internalPtr = ptr;

		/* =---- Constructors ------------------------------------------= */

		public ReferenceContext() :
			this(LLVM.GetGlobalContext())
		{
		}

		/* =---- Static Fields -----------------------------------------= */

		public static readonly ReferenceContext Global = new();

		/* =---- Static Methods ----------------------------------------= */

		public static ReferenceContext Create()
			=> LLVM.ContextCreate();

		/* =---- Properties --------------------------------------------= */

		public nint Handle => (nint)_internalPtr;

		/* =---- Methods -----------------------------------------------= */

		public ReferenceModule CreateModule(string id)
			=> ReferenceModule.Create(id, this);

		public ReferenceBuilder CreateBuilder()
			=> ReferenceBuilder.Create(this);

		public ReferenceBasicBlock CreateBasicBlock(string name = "")
			=> ReferenceBasicBlock.Create(this, name);

		// --------------------------------------------------------------

		public void Dispose()
			=> LLVM.ContextDispose(_internalPtr);

		public bool Equals(ReferenceContext other)
			=> this.Handle.Equals(other.Handle);

		public override bool Equals(object? obj)
			=> obj is ReferenceContext other && this.Equals(other);

		public override int GetHashCode()
			=> this.Handle.GetHashCode();

		public override string ToString()
			=> IReference.GetRefName(this);

		// --------------------------------------------------------------

		internal string GetDebuggerDisplay()
			=> this.ToString();

		/* =---- Operators ---------------------------------------------= */

		public static bool operator ==(ReferenceContext left, ReferenceContext right)
			=> left.Equals(right);

		public static bool operator !=(ReferenceContext left, ReferenceContext right)
			=> !left.Equals(right);

		// --------------------------------------------------------------

		public static implicit operator void*(ReferenceContext that)
			=> that._internalPtr;

		public static implicit operator ReferenceContext(void* that)
			=> new(that);

		public static implicit operator nint(ReferenceContext that)
			=> that.Handle;

		public static implicit operator ReferenceContext(nint that)
			=> new(that.ToPointer());

		/* =------------------------------------------------------------= */
	}
}
