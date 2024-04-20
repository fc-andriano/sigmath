using Sigmath.CodeGen.Extensions;
using Sigmath.CodeGen.Internal;

using System;
using System.Diagnostics;

namespace Sigmath.CodeGen.Interop
{
    [DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(), nq}}")]
	public unsafe readonly struct ReferenceBuilder(void* ptr) :
		IDisposable, IEquatable<ReferenceBuilder>, IReference
	{
		private readonly void* _internalPtr = ptr;

		/* =---- Static Methods ----------------------------------------= */

		public static ReferenceBuilder Create()
			=> LLVM.BuilderCreate();

		public static ReferenceBuilder Create(ReferenceContext context)
			=> LLVM.BuilderCreateInContext(context);

		/* =---- Properties --------------------------------------------= */

		public nint Handle => (nint)_internalPtr;

		/* =---- Methods -----------------------------------------------= */

		#region NegBuilderMethods

		public ReferenceValue BuildNeg(ReferenceValue value, string name = "")
		{
			if (this.Handle.IsZero())
				throw new NullReferenceException();

			using (ReferenceString refString = ReferenceString.Marshal(name))
			{
				return LLVM.BuildNeg(_internalPtr, value, refString);
			}
		}

		public ReferenceValue BuildNUWNeg(ReferenceValue value, string name = "")
		{
			if (this.Handle.IsZero())
				throw new NullReferenceException();

			using (ReferenceString refString = ReferenceString.Marshal(name))
			{
				return LLVM.BuildNUWNeg(_internalPtr, value, refString);
			}
		}

		public ReferenceValue BuildNSWNeg(ReferenceValue value, string name = "")
		{
			if (this.Handle.IsZero())
				throw new NullReferenceException();

			using (ReferenceString refString = ReferenceString.Marshal(name))
			{
				return LLVM.BuildNSWNeg(_internalPtr, value, refString);
			}
		}

		public ReferenceValue BuildFNeg(ReferenceValue value, string name = "")
		{
			if (this.Handle.IsZero())
				throw new NullReferenceException();

			using (ReferenceString refString = ReferenceString.Marshal(name))
			{
				return LLVM.BuildFNeg(_internalPtr, value, refString);
			}
		}

		#endregion

		// --------------------------------------------------------------

		#region AddBuilderMethods

		public ReferenceValue BuildAdd(ReferenceValue lhs, ReferenceValue rhs, string name = "")
		{
			if (this.Handle.IsZero())
				throw new NullReferenceException();

			using (ReferenceString refString = ReferenceString.Marshal(name))
			{
				return LLVM.BuildAdd(_internalPtr, lhs, rhs, refString);
			}
		}

		public ReferenceValue BuildNUWAdd(ReferenceValue lhs, ReferenceValue rhs, string name = "")
		{
			if (this.Handle.IsZero())
				throw new NullReferenceException();

			using (ReferenceString refString = ReferenceString.Marshal(name))
			{
				return LLVM.BuildNUWAdd(_internalPtr, lhs, rhs, refString);
			}
		}

		public ReferenceValue BuildNSWAdd(ReferenceValue lhs, ReferenceValue rhs, string name = "")
		{
			if (this.Handle.IsZero())
				throw new NullReferenceException();

			using (ReferenceString refString = ReferenceString.Marshal(name))
			{
				return LLVM.BuildNSWAdd(_internalPtr, lhs, rhs, refString);
			}
		}

		public ReferenceValue BuildFAdd(ReferenceValue lhs, ReferenceValue rhs, string name = "")
		{
			if (this.Handle.IsZero())
				throw new NullReferenceException();

			using (ReferenceString refString = ReferenceString.Marshal(name))
			{
				return LLVM.BuildFAdd(_internalPtr, lhs, rhs, refString);
			}
		}

		#endregion

		// --------------------------------------------------------------

		#region SubBuilderMethods

		public ReferenceValue BuildSub(ReferenceValue lhs, ReferenceValue rhs, string name = "")
		{
			if (this.Handle.IsZero())
				throw new NullReferenceException();

			using (ReferenceString refString = ReferenceString.Marshal(name))
			{
				return LLVM.BuildSub(_internalPtr, lhs, rhs, refString);
			}
		}

		public ReferenceValue BuildNUWSub(ReferenceValue lhs, ReferenceValue rhs, string name = "")
		{
			if (this.Handle.IsZero())
				throw new NullReferenceException();

			using (ReferenceString refString = ReferenceString.Marshal(name))
			{
				return LLVM.BuildNUWSub(_internalPtr, lhs, rhs, refString);
			}
		}

		public ReferenceValue BuildNSWSub(ReferenceValue lhs, ReferenceValue rhs, string name = "")
		{
			if (this.Handle.IsZero())
				throw new NullReferenceException();

			using (ReferenceString refString = ReferenceString.Marshal(name))
			{
				return LLVM.BuildNSWSub(_internalPtr, lhs, rhs, refString);
			}
		}

		public ReferenceValue BuildFSub(ReferenceValue lhs, ReferenceValue rhs, string name = "")
		{
			if (this.Handle.IsZero())
				throw new NullReferenceException();

			using (ReferenceString refString = ReferenceString.Marshal(name))
			{
				return LLVM.BuildFSub(_internalPtr, lhs, rhs, refString);
			}
		}

		#endregion

		// --------------------------------------------------------------

		#region MulBuilderMethods

		public ReferenceValue BuildMul(ReferenceValue lhs, ReferenceValue rhs, string name = "")
		{
			if (this.Handle.IsZero())
				throw new NullReferenceException();

			using (ReferenceString refString = ReferenceString.Marshal(name))
			{
				return LLVM.BuildMul(_internalPtr, lhs, rhs, refString);
			}
		}

		public ReferenceValue BuildNUWMul(ReferenceValue lhs, ReferenceValue rhs, string name = "")
		{
			if (this.Handle.IsZero())
				throw new NullReferenceException();

			using (ReferenceString refString = ReferenceString.Marshal(name))
			{
				return LLVM.BuildNUWMul(_internalPtr, lhs, rhs, refString);
			}
		}

		public ReferenceValue BuildNSWMul(ReferenceValue lhs, ReferenceValue rhs, string name = "")
		{
			if (this.Handle.IsZero())
				throw new NullReferenceException();

			using (ReferenceString refString = ReferenceString.Marshal(name))
			{
				return LLVM.BuildNSWMul(_internalPtr, lhs, rhs, refString);
			}
		}

		public ReferenceValue BuildFMul(ReferenceValue lhs, ReferenceValue rhs, string name = "")
		{
			if (this.Handle.IsZero())
				throw new NullReferenceException();

			using (ReferenceString refString = ReferenceString.Marshal(name))
			{
				return LLVM.BuildFMul(_internalPtr, lhs, rhs, refString);
			}
		}

		#endregion

		// --------------------------------------------------------------

		#region DivBuilderMethods

		public ReferenceValue BuildUDiv(ReferenceValue lhs, ReferenceValue rhs, string name = "")
		{
			if (this.Handle.IsZero())
				throw new NullReferenceException();

			using (ReferenceString refString = ReferenceString.Marshal(name))
			{
				return LLVM.BuildUDiv(_internalPtr, lhs, rhs, refString);
			}
		}

		public ReferenceValue BuildExactUDiv(ReferenceValue lhs, ReferenceValue rhs, string name = "")
		{
			if (this.Handle.IsZero())
				throw new NullReferenceException();

			using (ReferenceString refString = ReferenceString.Marshal(name))
			{
				return LLVM.BuildExactUDiv(_internalPtr, lhs, rhs, refString);
			}
		}

		public ReferenceValue BuildSDiv(ReferenceValue lhs, ReferenceValue rhs, string name = "")
		{
			if (this.Handle.IsZero())
				throw new NullReferenceException();

			using (ReferenceString refString = ReferenceString.Marshal(name))
			{
				return LLVM.BuildSDiv(_internalPtr, lhs, rhs, refString);
			}
		}

		public ReferenceValue BuildExactSDiv(ReferenceValue lhs, ReferenceValue rhs, string name = "")
		{
			if (this.Handle.IsZero())
				throw new NullReferenceException();

			using (ReferenceString refString = ReferenceString.Marshal(name))
			{
				return LLVM.BuildExactSDiv(_internalPtr, lhs, rhs, refString);
			}
		}

		public ReferenceValue BuildFDiv(ReferenceValue lhs, ReferenceValue rhs, string name = "")
		{
			if (this.Handle.IsZero())
				throw new NullReferenceException();

			using (ReferenceString refString = ReferenceString.Marshal(name))
			{
				return LLVM.BuildFDiv(_internalPtr, lhs, rhs, refString);
			}
		}

		#endregion

		// --------------------------------------------------------------

		#region RemBuilderMethods

		public ReferenceValue BuildURem(ReferenceValue lhs, ReferenceValue rhs, string name = "")
		{
			if (this.Handle.IsZero())
				throw new NullReferenceException();

			using (ReferenceString refString = ReferenceString.Marshal(name))
			{
				return LLVM.BuildURem(_internalPtr, lhs, rhs, refString);
			}
		}

		public ReferenceValue BuildSRem(ReferenceValue lhs, ReferenceValue rhs, string name = "")
		{
			if (this.Handle.IsZero())
				throw new NullReferenceException();

			using (ReferenceString refString = ReferenceString.Marshal(name))
			{
				return LLVM.BuildSRem(_internalPtr, lhs, rhs, refString);
			}
		}

		public ReferenceValue BuildFRem(ReferenceValue lhs, ReferenceValue rhs, string name = "")
		{
			if (this.Handle.IsZero())
				throw new NullReferenceException();

			using (ReferenceString refString = ReferenceString.Marshal(name))
			{
				return LLVM.BuildFRem(_internalPtr, lhs, rhs, refString);
			}
		}

		#endregion

		// --------------------------------------------------------------

		#region ShiftBuilderMethods

		public ReferenceValue BuildShl(ReferenceValue lhs, ReferenceValue rhs, string name = "")
		{
			if (this.Handle.IsZero())
				throw new NullReferenceException();

			using (ReferenceString refString = ReferenceString.Marshal(name))
			{
				return LLVM.BuildShl(_internalPtr, lhs, rhs, refString);
			}
		}

		public ReferenceValue BuildLShr(ReferenceValue lhs, ReferenceValue rhs, string name = "")
		{
			if (this.Handle.IsZero())
				throw new NullReferenceException();

			using (ReferenceString refString = ReferenceString.Marshal(name))
			{
				return LLVM.BuildLShr(_internalPtr, lhs, rhs, refString);
			}
		}

		public ReferenceValue BuildAShr(ReferenceValue lhs, ReferenceValue rhs, string name = "")
		{
			if (this.Handle.IsZero())
				throw new NullReferenceException();

			using (ReferenceString refString = ReferenceString.Marshal(name))
			{
				return LLVM.BuildAShr(_internalPtr, lhs, rhs, refString);
			}
		}

		#endregion

		// --------------------------------------------------------------

		#region LogicBuilderMethods

		public ReferenceValue BuildNot(ReferenceValue value, string name = "")
		{
			if (this.Handle.IsZero())
				throw new NullReferenceException();

			using (ReferenceString refString = ReferenceString.Marshal(name))
			{
				return LLVM.BuildNot(_internalPtr, value, refString);
			}
		}

		public ReferenceValue BuildAnd(ReferenceValue lhs, ReferenceValue rhs, string name = "")
		{
			if (this.Handle.IsZero())
				throw new NullReferenceException();

			using (ReferenceString refString = ReferenceString.Marshal(name))
			{
				return LLVM.BuildAnd(_internalPtr, lhs, rhs, refString);
			}
		}

		public ReferenceValue BuildOr(ReferenceValue lhs, ReferenceValue rhs, string name = "")
		{
			if (this.Handle.IsZero())
				throw new NullReferenceException();

			using (ReferenceString refString = ReferenceString.Marshal(name))
			{
				return LLVM.BuildOr(_internalPtr, lhs, rhs, refString);
			}
		}

		public ReferenceValue BuildXOr(ReferenceValue lhs, ReferenceValue rhs, string name = "")
		{
			if (this.Handle.IsZero())
				throw new NullReferenceException();

			using (ReferenceString refString = ReferenceString.Marshal(name))
			{
				return LLVM.BuildXOr(_internalPtr, lhs, rhs, refString);
			}
		}

		#endregion

		// --------------------------------------------------------------

		internal ReferenceValue BuildBinOp(LLVMOpcode opcode, ReferenceValue lhs, ReferenceValue rhs, string name = "")
		{
			if (this.Handle.IsZero())
				throw new NullReferenceException();

			using (ReferenceString refString = ReferenceString.Marshal(name))
			{
				return LLVM.BuildBinOp(_internalPtr, opcode, lhs, rhs, refString);
			}
		}

		// --------------------------------------------------------------

		public void SetPosition(ReferenceBasicBlock block, ReferenceValue instr)
			=> LLVM.PositionBuilder(_internalPtr, block, instr);

		public void SetPosition(ReferenceBasicBlock block)
			=> LLVM.PositionBuilderAtEnd(_internalPtr, block);

		public void SetPositionBefore(ReferenceValue instr)
			=> LLVM.PositionBuilderBefore(_internalPtr, instr);

		// --------------------------------------------------------------

		public void Dispose()
			=> LLVM.ContextDispose(_internalPtr);

		public bool Equals(ReferenceBuilder other)
			=> this.Handle.Equals(other.Handle);

		public override bool Equals(object? obj)
			=> obj is ReferenceBuilder other && this.Equals(other);

		public override int GetHashCode()
			=> this.Handle.GetHashCode();

		public override string ToString()
			=> IReference.GetRefName(this);

		// --------------------------------------------------------------

		internal string GetDebuggerDisplay()
			=> this.ToString();

		/* =---- Operators ---------------------------------------------= */

		public static bool operator ==(ReferenceBuilder left, ReferenceBuilder right)
			=> left.Equals(right);

		public static bool operator !=(ReferenceBuilder left, ReferenceBuilder right)
			=> !left.Equals(right);

		// --------------------------------------------------------------

		public static implicit operator void*(ReferenceBuilder that)
			=> that._internalPtr;

		public static implicit operator ReferenceBuilder(void* that)
			=> new(that);

		public static implicit operator nint(ReferenceBuilder that)
			=> that.Handle;

		public static implicit operator ReferenceBuilder(nint that)
			=> new(that.ToPointer());

		/* =------------------------------------------------------------= */
	}
}
