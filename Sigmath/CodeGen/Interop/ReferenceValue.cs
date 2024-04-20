using System;
using System.Diagnostics;
using System.IO;
using Sigmath.CodeGen.Extensions;

namespace Sigmath.CodeGen.Interop
{
    [DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(), nq}}")]
	public unsafe readonly struct ReferenceValue(void* ptr) :
		IEquatable<ReferenceValue>, IReference, IReferenceDump
	{
		private readonly void* _internalPtr = ptr;

		/* =---- Static Methods ----------------------------------------= */

		public static ReferenceValue GetConstInt(ReferenceType type, ulong value, bool isSigned = false)
			=> LLVM.ConstInt(type, value, isSigned ? 1 : 0);

		// --------------------------------------------------------------

		public static ReferenceValue GetConstReal(ReferenceType type, double value)
			=> LLVM.ConstReal(type, value);

		// --------------------------------------------------------------

		public static ReferenceValue GetConstStruct(ReferenceContext context, ReferenceValue[] constValues, bool isPacked = false)
			=> LLVM.ConstStructInContext(context, (void**)constValues.AsPointer(), (uint)constValues.Length, isPacked ? 1 : 0);

		public static ReferenceValue GetConstStruct(ReferenceValue[] constValues, bool isPacked = false)
			=> LLVM.ConstStruct((void**)constValues.AsPointer(), (uint)constValues.Length, isPacked ? 1 : 0);

		/* =---- Properties --------------------------------------------= */

		public nint Handle => (nint)_internalPtr;

		/* =---- Methods -----------------------------------------------= */

		public string AsString()
			=> this.Handle.IsNotZero() ? ReferenceString.Unmarshal(LLVM.PrintValueToString(_internalPtr)) : String.Empty;

		public void Dump()
			=> LLVM.DumpValue(_internalPtr);

		public void Dump(TextWriter textWriter)
			=> textWriter.Write(this.AsString());

		public bool Equals(ReferenceValue other)
			=> this.Handle.Equals(other.Handle);

		public override bool Equals(object? obj)
			=> obj is ReferenceValue other && this.Equals(other);

		public override int GetHashCode()
			=> this.Handle.GetHashCode();

		public override string ToString()
			=> IReference.GetRefName(this);

		// --------------------------------------------------------------

		internal string GetDebuggerDisplay()
			=> this.AsString();

		/* =---- Operators ---------------------------------------------= */

		public static bool operator ==(ReferenceValue left, ReferenceValue right)
			=> left.Equals(right);

		public static bool operator !=(ReferenceValue left, ReferenceValue right)
			=> !left.Equals(right);

		// --------------------------------------------------------------

		public static implicit operator void*(ReferenceValue that)
			=> that._internalPtr;

		public static implicit operator ReferenceValue(void* that)
			=> new(that);

		public static implicit operator nint(ReferenceValue that)
			=> that.Handle;

		public static implicit operator ReferenceValue(nint that)
			=> new(that.ToPointer());

		/* =------------------------------------------------------------= */
	}
}
