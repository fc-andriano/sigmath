using Sigmath.CodeGen.Internal;
using Sigmath.CodeGen.Interop;

using System.Diagnostics;

namespace Sigmath.CodeGen
{
	[DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(), nq}}")]
	public sealed class Generator
	{
		private readonly ReferenceContext _context;
		private readonly ReferenceBuilder _builder;

		/* =---- Constructors ------------------------------------------= */

		private Generator(ReferenceContext context, ReferenceBuilder builder)
		{
			_context = context;
			_builder = builder;
		}

		/* =---- Static Methods ----------------------------------------= */

		public static Generator Create(ReferenceContext context)
			=> new(context, context.CreateBuilder());

		public static Generator Create()
			=> Create(ReferenceContext.Create());

		public static Generator CreateInGlobalContext()
			=> Create(ReferenceContext.Global);

		/* =---- Methods -----------------------------------------------= */

		public ReferenceModule CreateModule(string id)
			=> _context.CreateModule(id);

		public ReferenceBasicBlock CreateBasicBlock(string name = "")
			=> _context.CreateBasicBlock(name);

		// --------------------------------------------------------------

		public void SetPosition(ReferenceBasicBlock block, ReferenceValue instr)
			=> _builder.SetPosition(block, instr);

		public void SetPosition(ReferenceBasicBlock block)
			=> _builder.SetPosition(block);

		public void SetPositionBefore(ReferenceValue instr)
			=> _builder.SetPositionBefore(instr);

		// --------------------------------------------------------------

		#region TypeMethods

		// Type Methods

		#region IntTypeMethods

		public ReferenceType GetInt1Type()
			=> ReferenceType.GetInt1Type(_context);

		public ReferenceType GetInt8Type()
			=> ReferenceType.GetInt8Type(_context);

		public ReferenceType GetInt16Type()
			=> ReferenceType.GetInt16Type(_context);

		public ReferenceType GetInt32Type()
			=> ReferenceType.GetInt32Type(_context);

		public ReferenceType GetInt64Type()
			=> ReferenceType.GetInt64Type(_context);

		public ReferenceType GetInt128Type()
			=> ReferenceType.GetInt128Type(_context);

		// --------------------------------------------------------------

		public ReferenceType GetIntType(int bitsNumber)
			=> ReferenceType.GetIntType(bitsNumber);

		#endregion

		// --------------------------------------------------------------

		#region RealTypeMethods

		public ReferenceType GetHalfType()
			=> ReferenceType.GetHalfType(_context);

		public ReferenceType GetBFloatType()
			=> ReferenceType.GetBFloatType(_context);

		public ReferenceType GetFloatType()
			=> ReferenceType.GetFloatType(_context);

		public ReferenceType GetDoubleType()
			=> ReferenceType.GetDoubleType(_context);

		public ReferenceType GetX86FP80Type()
			=> ReferenceType.GetX86FP80Type(_context);

		public ReferenceType GetFP128Type()
			=> ReferenceType.GetFP128Type(_context);

		public ReferenceType GetPPCFP128Type()
			=> ReferenceType.GetPPCFP128Type(_context);

		#endregion

		// --------------------------------------------------------------

		public ReferenceType GetStructType(ReferenceType[] elementTypes, bool isPacked = false)
			=> ReferenceType.GetStructType(_context, elementTypes, isPacked);

		#endregion

		// --------------------------------------------------------------

		#region ValueMethods

		// Value Methods

		public ReferenceValue GetConstInt1(bool value)
			=> ReferenceValue.GetConstInt(this.GetInt1Type(), (ulong)(value ? 1 : 0), false);

		public ReferenceValue GetConstInt8(sbyte value, bool isSigned = true)
			=> ReferenceValue.GetConstInt(this.GetInt8Type(), (ulong)value, isSigned);

		public ReferenceValue GetConstInt8(byte value, bool isSigned = false)
			=> ReferenceValue.GetConstInt(this.GetInt8Type(), value, isSigned);

		public ReferenceValue GetConstInt16(short value, bool isSigned = true)
			=> ReferenceValue.GetConstInt(this.GetInt16Type(), (ulong)value, isSigned);

		public ReferenceValue GetConstInt16(ushort value, bool isSigned = false)
			=> ReferenceValue.GetConstInt(this.GetInt16Type(), value, isSigned);

		public ReferenceValue GetConstInt32(int value, bool isSigned = true)
			=> ReferenceValue.GetConstInt(this.GetInt32Type(), (ulong)value, isSigned);

		public ReferenceValue GetConstInt32(uint value, bool isSigned = false)
			=> ReferenceValue.GetConstInt(this.GetInt32Type(), value, isSigned);

		public ReferenceValue GetConstInt64(long value, bool isSigned = true)
			=> ReferenceValue.GetConstInt(this.GetInt64Type(), (ulong)value, isSigned);

		public ReferenceValue GetConstInt64(ulong value, bool isSigned = false)
			=> ReferenceValue.GetConstInt(this.GetInt64Type(), value, isSigned);

		public ReferenceValue GetConstInt128(long value, bool isSigned = true)
			=> ReferenceValue.GetConstInt(this.GetInt128Type(), (ulong)value, isSigned);

		public ReferenceValue GetConstInt128(ulong value, bool isSigned = false)
			=> ReferenceValue.GetConstInt(this.GetInt128Type(), value, isSigned);

		// --------------------------------------------------------------

		public ReferenceValue GetConstInt(int bitsNumber, ulong value, bool isSigned = false)
			=> ReferenceValue.GetConstInt(this.GetIntType(bitsNumber), value, isSigned);

		public ReferenceValue GetConstInt(int bitsNumber, long value, bool isSigned = true)
			=> ReferenceValue.GetConstInt(this.GetIntType(bitsNumber), (ulong)value, isSigned);

		// --------------------------------------------------------------

		public ReferenceValue GetConstIntPair(int bitsNumber, ulong value1, ulong value2, bool isSigned = false)
			=> ReferenceValue.GetConstStruct([this.GetConstInt(bitsNumber, value1, isSigned), this.GetConstInt(bitsNumber, value2, isSigned)], true);

		public ReferenceValue GetConstIntPair(int bitsNumber, long value1, long value2, bool isSigned = true)
			=> ReferenceValue.GetConstStruct([this.GetConstInt(bitsNumber, value1, isSigned), this.GetConstInt(bitsNumber, value2, isSigned)], true);

		// --------------------------------------------------------------

		public ReferenceValue GetConstHalf(double value)
			=> ReferenceValue.GetConstReal(this.GetHalfType(), value);

		public ReferenceValue GetConstBFloat(double value)
			=> ReferenceValue.GetConstReal(this.GetBFloatType(), value);

		public ReferenceValue GetConstFloat(double value)
			=> ReferenceValue.GetConstReal(this.GetFloatType(), value);

		public ReferenceValue GetConstDouble(double value)
			=> ReferenceValue.GetConstReal(this.GetDoubleType(), value);

		public ReferenceValue GetConstX86FP80(double value)
			=> ReferenceValue.GetConstReal(this.GetX86FP80Type(), value);

		public ReferenceValue GetConstFP128(double value)
			=> ReferenceValue.GetConstReal(this.GetFP128Type(), value);

		public ReferenceValue GetConstPPCFP128(double value)
			=> ReferenceValue.GetConstReal(this.GetPPCFP128Type(), value);

		// --------------------------------------------------------------

		public ReferenceValue GetConstStruct(ReferenceValue[] elements, bool isPacked = false)
			=> ReferenceValue.GetConstStruct(_context, elements, isPacked);

		// --------------------------------------------------------------

		public ReferenceValue GetConstPair(ReferenceValue value1, ReferenceValue value2)
			=> ReferenceValue.GetConstStruct(_context, [value1, value2], true);

		#endregion

		// --------------------------------------------------------------

		#region BuilderMethods

		// Builder Methods

		#region NegBuilderMethods

		public ReferenceValue BuildNeg(ReferenceValue value, string name = "")
			=> _builder.BuildNeg(value, name);

		public ReferenceValue BuildNUWNeg(ReferenceValue value, string name = "")
			=> _builder.BuildNUWNeg(value, name);

		public ReferenceValue BuildNSWNeg(ReferenceValue value, string name = "")
			=> _builder.BuildNSWNeg(value, name);

		public ReferenceValue BuildFNeg(ReferenceValue value, string name = "")
			=> _builder.BuildFNeg(value, name);

		#endregion

		// --------------------------------------------------------------

		#region AddBuilderMethods

		public ReferenceValue BuildAdd(ReferenceValue lhs, ReferenceValue rhs, string name = "")
			=> _builder.BuildAdd(lhs, rhs, name);

		public ReferenceValue BuildNUWAdd(ReferenceValue lhs, ReferenceValue rhs, string name = "")
			=> _builder.BuildNUWAdd(lhs, rhs, name);

		public ReferenceValue BuildNSWAdd(ReferenceValue lhs, ReferenceValue rhs, string name = "")
			=> _builder.BuildNSWAdd(lhs, rhs, name);

		public ReferenceValue BuildFAdd(ReferenceValue lhs, ReferenceValue rhs, string name = "")
			=> _builder.BuildFAdd(lhs, rhs, name);

		#endregion

		// --------------------------------------------------------------

		#region SubBuilderMethods

		public ReferenceValue BuildSub(ReferenceValue lhs, ReferenceValue rhs, string name = "")
			=> _builder.BuildSub(lhs, rhs, name);

		public ReferenceValue BuildNUWSub(ReferenceValue lhs, ReferenceValue rhs, string name = "")
			=> _builder.BuildNUWSub(lhs, rhs, name);

		public ReferenceValue BuildNSWSub(ReferenceValue lhs, ReferenceValue rhs, string name = "")
			=> _builder.BuildNSWSub(lhs, rhs, name);

		public ReferenceValue BuildFSub(ReferenceValue lhs, ReferenceValue rhs, string name = "")
			=> _builder.BuildFSub(lhs, rhs, name);

		#endregion

		// --------------------------------------------------------------

		#region MulBuilderMethods

		public ReferenceValue BuildMul(ReferenceValue lhs, ReferenceValue rhs, string name = "")
			=> _builder.BuildMul(lhs, rhs, name);

		public ReferenceValue BuildNUWMul(ReferenceValue lhs, ReferenceValue rhs, string name = "")
			=> _builder.BuildNUWMul(lhs, rhs, name);

		public ReferenceValue BuildNSWMul(ReferenceValue lhs, ReferenceValue rhs, string name = "")
			=> _builder.BuildNSWMul(lhs, rhs, name);

		public ReferenceValue BuildFMul(ReferenceValue lhs, ReferenceValue rhs, string name = "")
			=> _builder.BuildFMul(lhs, rhs, name);

		#endregion

		// --------------------------------------------------------------

		#region DivBuilderMethods

		public ReferenceValue BuildUDiv(ReferenceValue lhs, ReferenceValue rhs, string name = "")
			=> _builder.BuildUDiv(lhs, rhs, name);

		public ReferenceValue BuildExactUDiv(ReferenceValue lhs, ReferenceValue rhs, string name = "")
			=> _builder.BuildExactUDiv(lhs, rhs, name);

		public ReferenceValue BuildSDiv(ReferenceValue lhs, ReferenceValue rhs, string name = "")
			=> _builder.BuildSDiv(lhs, rhs, name);

		public ReferenceValue BuildExactSDiv(ReferenceValue lhs, ReferenceValue rhs, string name = "")
			=> _builder.BuildExactSDiv(lhs, rhs, name);

		public ReferenceValue BuildFDiv(ReferenceValue lhs, ReferenceValue rhs, string name = "")
			=> _builder.BuildFDiv(lhs, rhs, name);

		#endregion

		// --------------------------------------------------------------

		#region RemBuilderMethods

		public ReferenceValue BuildURem(ReferenceValue lhs, ReferenceValue rhs, string name = "")
			=> _builder.BuildURem(lhs, rhs, name);

		public ReferenceValue BuildSRem(ReferenceValue lhs, ReferenceValue rhs, string name = "")
			=> _builder.BuildSRem(lhs, rhs, name);

		public ReferenceValue BuildFRem(ReferenceValue lhs, ReferenceValue rhs, string name = "")
			=> _builder.BuildFRem(lhs, rhs, name);

		#endregion

		// --------------------------------------------------------------

		#region ShiftBuilderMethods

		public ReferenceValue BuildShl(ReferenceValue lhs, ReferenceValue rhs, string name = "")
			=> _builder.BuildShl(lhs, rhs, name);

		public ReferenceValue BuildLShr(ReferenceValue lhs, ReferenceValue rhs, string name = "")
			=> _builder.BuildLShr(lhs, rhs, name);

		public ReferenceValue BuildAShr(ReferenceValue lhs, ReferenceValue rhs, string name = "")
			=> _builder.BuildAShr(lhs, rhs, name);

		#endregion

		// --------------------------------------------------------------

		#region LogicBuilderMethods

		public ReferenceValue BuildNot(ReferenceValue value, string name = "")
			=> _builder.BuildNot(value, name);

		public ReferenceValue BuildAnd(ReferenceValue lhs, ReferenceValue rhs, string name = "")
			=> _builder.BuildAnd(lhs, rhs, name);

		public ReferenceValue BuildOr(ReferenceValue lhs, ReferenceValue rhs, string name = "")
			=> _builder.BuildOr(lhs, rhs, name);

		public ReferenceValue BuildXOr(ReferenceValue lhs, ReferenceValue rhs, string name = "")
			=> _builder.BuildXOr(lhs, rhs, name);

		#endregion

		// --------------------------------------------------------------

		internal ReferenceValue BuildBinOp(LLVMOpcode opcode, ReferenceValue lhs, ReferenceValue rhs, string name = "")
			=> _builder.BuildBinOp(opcode, lhs, rhs, name);

		#endregion

		// --------------------------------------------------------------

		internal string GetDebuggerDisplay()
			=> $"{{{_context.GetDebuggerDisplay()}, {_builder.GetDebuggerDisplay()}}}";

		/* =------------------------------------------------------------= */
	}
}
