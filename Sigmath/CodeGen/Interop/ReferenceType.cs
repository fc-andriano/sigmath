using System;
using System.Diagnostics;
using System.IO;

namespace Sigmath.CodeGen.Interop
{
	[DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(), nq}}")]
	public unsafe readonly struct ReferenceType(void* ptr) :
		IEquatable<ReferenceType>, IReference, IReferenceDump
	{
		private readonly void* _internalPtr = ptr;

		/* =---- Static Methods ----------------------------------------= */

		#region IntTypeMethods

		public static ReferenceType GetInt1Type(ReferenceContext ctx)
			=> LLVM.Int1TypeInContext(ctx);

		public static ReferenceType GetInt1Type()
			=> LLVM.Int1Type();

		public static ReferenceType GetInt8Type(ReferenceContext ctx)
			=> LLVM.Int8TypeInContext(ctx);

		public static ReferenceType GetInt8Type()
			=> LLVM.Int8Type();

		public static ReferenceType GetInt16Type(ReferenceContext ctx)
			=> LLVM.Int16TypeInContext(ctx);

		public static ReferenceType GetInt16Type()
			=> LLVM.Int16Type();

		public static ReferenceType GetInt32Type(ReferenceContext ctx)
			=> LLVM.Int32TypeInContext(ctx);

		public static ReferenceType GetInt32Type()
			=> LLVM.Int32Type();

		public static ReferenceType GetInt64Type(ReferenceContext ctx)
			=> LLVM.Int64TypeInContext(ctx);

		public static ReferenceType GetInt64Type()
			=> LLVM.Int64Type();

		public static ReferenceType GetInt128Type(ReferenceContext ctx)
			=> LLVM.Int128TypeInContext(ctx);

		public static ReferenceType GetInt128Type()
			=> LLVM.Int128Type();

		// --------------------------------------------------------------

		public static ReferenceType GetIntType(ReferenceContext ctx, int bitsNumber)
			=> LLVM.IntTypeInContext(ctx, (uint)bitsNumber);

		public static ReferenceType GetIntType(int bitsNumber)
			=> LLVM.IntType((uint)bitsNumber);

		#endregion

		// --------------------------------------------------------------

		#region RealTypeMethods

		public static ReferenceType GetHalfType(ReferenceContext ctx)
			=> LLVM.HalfTypeInContext(ctx);

		public static ReferenceType GetHalfType()
			=> LLVM.HalfType();

		public static ReferenceType GetBFloatType(ReferenceContext ctx)
			=> LLVM.BFloatTypeInContext(ctx);

		public static ReferenceType GetBFloatType()
			=> LLVM.BFloatType();

		public static ReferenceType GetFloatType(ReferenceContext ctx)
			=> LLVM.FloatTypeInContext(ctx);

		public static ReferenceType GetFloatType()
			=> LLVM.FloatType();

		public static ReferenceType GetDoubleType(ReferenceContext ctx)
			=> LLVM.DoubleTypeInContext(ctx);

		public static ReferenceType GetDoubleType()
			=> LLVM.DoubleType();

		public static ReferenceType GetX86FP80Type(ReferenceContext ctx)
			=> LLVM.X86FP80TypeInContext(ctx);

		public static ReferenceType GetX86FP80Type()
			=> LLVM.X86FP80Type();

		public static ReferenceType GetFP128Type(ReferenceContext ctx)
			=> LLVM.FP128TypeInContext(ctx);

		public static ReferenceType GetFP128Type()
			=> LLVM.FP128Type();

		public static ReferenceType GetPPCFP128Type(ReferenceContext ctx)
			=> LLVM.PPCFP128TypeInContext(ctx);

		public static ReferenceType GetPPCFP128Type()
			=> LLVM.PPCFP128Type();

		#endregion

		// --------------------------------------------------------------

		#region FunctionTypeMethods

		public static ReferenceType GetFunctionType(ReferenceType returnType, ReferenceType[] paramTypes, bool isVarArg = false)
			=> LLVM.FunctionType(returnType, (void**)paramTypes.AsPointer(), (uint)paramTypes.Length, isVarArg ? 1 : 0);

		#endregion

		// --------------------------------------------------------------

		#region StructTypeMethods

		public static ReferenceType GetStructType(ReferenceContext context, ReferenceType[] elementTypes, bool isPacked = false)
			=> LLVM.StructTypeInContext(context, (void**)elementTypes.AsPointer(), (uint)elementTypes.Length, isPacked ? 1 : 0);

		public static ReferenceType GetStructType(ReferenceType[] elementTypes, bool isPacked = false)
			=> LLVM.StructType((void**)elementTypes.AsPointer(), (uint)elementTypes.Length, isPacked ? 1 : 0);

		#endregion

		// --------------------------------------------------------------

		public static ReferenceType TypeOf(ReferenceValue value)
			=> LLVM.TypeOf(value);

		/* =---- Properties --------------------------------------------= */

		public nint Handle => (nint)_internalPtr;

		/* =---- Methods -----------------------------------------------= */

		public string AsString()
			=> this.Handle.IsNotZero() ? ReferenceString.Unmarshal(LLVM.PrintTypeToString(_internalPtr)) : String.Empty;

		public void Dump()
			=> LLVM.DumpType(_internalPtr);

		public void Dump(TextWriter textWriter)
			=> textWriter.Write(this.AsString());

		public bool Equals(ReferenceType other)
			=> this.Handle.Equals(other.Handle);

		public override bool Equals(object? obj)
			=> obj is ReferenceType other && this.Equals(other);

		public override int GetHashCode()
			=> this.Handle.GetHashCode();

		public override string ToString()
			=> IReference.GetRefName(this);

		// --------------------------------------------------------------

		internal string GetDebuggerDisplay()
			=> this.AsString();

		/* =---- Operators ---------------------------------------------= */

		public static bool operator ==(ReferenceType left, ReferenceType right)
			=> left.Equals(right);

		public static bool operator !=(ReferenceType left, ReferenceType right)
			=> !left.Equals(right);

		// --------------------------------------------------------------

		public static implicit operator void*(ReferenceType that)
			=> that._internalPtr;

		public static implicit operator ReferenceType(void* that)
			=> new(that);

		public static implicit operator nint(ReferenceType that)
			=> that.Handle;

		public static implicit operator ReferenceType(nint that)
			=> new(that.ToPointer());

		/* =------------------------------------------------------------= */
	}
}
