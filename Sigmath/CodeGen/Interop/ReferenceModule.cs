using System;
using System.Diagnostics;
using System.IO;

namespace Sigmath.CodeGen.Interop
{
	[DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(), nq}}")]
	public unsafe readonly struct ReferenceModule(void* ptr) :
		IDisposable, IEquatable<ReferenceModule>, IReference, IReferenceDump
	{
		private readonly void* _internalPtr = ptr;

		/* =---- Static Methods ----------------------------------------= */

		public static ReferenceModule Create(string id)
			=> LLVM.ModuleCreate(ReferenceString.Marshal(id));

		public static ReferenceModule Create(string id, ReferenceContext context)
			=> LLVM.ModuleCreateInContext(ReferenceString.Marshal(id), context);

		/* =---- Properties --------------------------------------------= */

		public nint Handle => (nint)_internalPtr;

		/* =---- Methods -----------------------------------------------= */

		public ReferenceContext GetContext()
			=> this.Handle.IsNotZero() ? LLVM.GetModuleContext(_internalPtr) : default;

		// --------------------------------------------------------------

		public string AsString()
			=> this.Handle.IsNotZero() ? ReferenceString.Unmarshal(LLVM.PrintModuleToString(_internalPtr)) : String.Empty;

		public void Dispose()
			=> LLVM.ModuleDispose(_internalPtr);

		public void Dump()
			=> LLVM.DumpModule(_internalPtr);

		public void Dump(TextWriter textWriter)
			=> textWriter.Write(this.AsString());

		public bool Equals(ReferenceModule other)
			=> this.Handle.Equals(other.Handle);

		public override bool Equals(object? obj)
			=> obj is ReferenceContext other && this.Equals(other);

		public override int GetHashCode()
			=> this.Handle.GetHashCode();

		public override string ToString()
			=> IReference.GetRefName(this);

		// --------------------------------------------------------------

		internal string GetDebuggerDisplay()
			=> this.AsString();

		/* =---- Operators ---------------------------------------------= */

		public static bool operator ==(ReferenceModule left, ReferenceModule right)
			=> left.Equals(right);

		public static bool operator !=(ReferenceModule left, ReferenceModule right)
			=> !left.Equals(right);

		// --------------------------------------------------------------

		public static implicit operator void*(ReferenceModule that)
			=> that._internalPtr;

		public static implicit operator ReferenceModule(void* that)
			=> new(that);

		public static implicit operator nint(ReferenceModule that)
			=> that.Handle;

		public static implicit operator ReferenceModule(nint that)
			=> new(that.ToPointer());

		/* =------------------------------------------------------------= */
	}
}
