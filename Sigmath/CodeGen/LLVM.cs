using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Sigmath.CodeGen
{
	/// <summary>Defines the type of a member as it was used in the native signature.</summary>
	/// <remarks>Initializes a new instance of the <see cref="NativeTypeNameAttribute" /> class.</remarks>
	/// <param name="name">The name of the type that was used in the native signature.</param>
	[AttributeUsage(AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.ReturnValue, AllowMultiple = false, Inherited = true), Conditional("DEBUG")]
	internal sealed class NativeTypeNameAttribute(string name) :
		Attribute
	{
		/* =---- Properties --------------------------------------------= */

		/// <summary>Gets the name of the type that was used in the native signature.</summary>
		public string Name => name;

		/* =------------------------------------------------------------= */
	}

	[SuppressMessage("Interoperability", "SYSLIB1054")]
	internal unsafe static partial class LLVM
	{
		/* =---- Constructors ------------------------------------------= */

		static LLVM()
			=> NativeLibrary.SetDllImportResolver(Assembly.GetExecutingAssembly(), OnDllImport);

		/* =---- Static Events -----------------------------------------= */

		public static event DllImportResolver? ResolveLibrary;

		/* =---- Static Methods ----------------------------------------= */

		private static bool TryResolveLibrary(string libraryName, Assembly assembly, DllImportSearchPath? searchPath, out nint nativeLibrary)
		{
			DllImportResolver? resolveLibrary = ResolveLibrary;

			if (resolveLibrary != null)
			{
				Delegate[] resolvers = resolveLibrary.GetInvocationList();

				foreach (DllImportResolver resolver in resolvers.Cast<DllImportResolver>())
				{
					nativeLibrary = resolver(libraryName, assembly, searchPath);

					if (nativeLibrary != IntPtr.Zero)
						return true;
				}
			}

			nativeLibrary = IntPtr.Zero;
			return false;
		}

		private static bool TryResolveLLVM(Assembly assembly, DllImportSearchPath? searchPath, out nint nativeLibrary)
			=> (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) && NativeLibrary.TryLoad("libLLVM.so.14", assembly, searchPath, out nativeLibrary))
			|| (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) && NativeLibrary.TryLoad("libLLVM-14", assembly, searchPath, out nativeLibrary))
			|| (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) && NativeLibrary.TryLoad("libLLVM.so.1", assembly, searchPath, out nativeLibrary))
			|| (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) && NativeLibrary.TryLoad("LLVM-C.dll", assembly, searchPath, out nativeLibrary))
			|| NativeLibrary.TryLoad("libLLVM", assembly, searchPath, out nativeLibrary);

		// --------------------------------------------------------------

		private static nint OnDllImport(string libraryName, Assembly assembly, DllImportSearchPath? searchPath)
			=> TryResolveLibrary(libraryName, assembly, searchPath, out var nativeLibrary)
			? nativeLibrary : libraryName.Equals("libLLVM") && TryResolveLLVM(assembly, searchPath, out nativeLibrary)
			? nativeLibrary : IntPtr.Zero;

		/* =---- Methods -----------------------------------------------= */

		#region ContextBindings

		// Context

		[DllImport("libLLVM", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LLVMContextCreate", ExactSpelling = true)]
		[return: NativeTypeName("LLVMContextRef")]
		public static extern void* ContextCreate();

		[DllImport("libLLVM", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LLVMContextDispose", ExactSpelling = true)]
		public static extern void ContextDispose([NativeTypeName("LLVMContextRef")] void* context);

		// --------------------------------------------------------------

		[DllImport("libLLVM", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LLVMGetGlobalContext", ExactSpelling = true)]
		[return: NativeTypeName("LLVMContextRef")]
		public static extern void* GetGlobalContext();

		[DllImport("libLLVM", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LLVMGetModuleContext", ExactSpelling = true)]
		[return: NativeTypeName("LLVMContextRef")]
		public static extern void* GetModuleContext([NativeTypeName("LLVMModuleRef")] void* module);

		#endregion

		// --------------------------------------------------------------

		#region MuduleBindings

		// Module

		[DllImport("libLLVM", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LLVMModuleCreateWithName", ExactSpelling = true)]
		[return: NativeTypeName("LLVMModuleRef")]
		public static extern void* ModuleCreate([NativeTypeName("const char *")] sbyte* id);

		[DllImport("libLLVM", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LLVMModuleCreateWithNameInContext", ExactSpelling = true)]
		[return: NativeTypeName("LLVMModuleRef")]
		public static extern void* ModuleCreateInContext([NativeTypeName("const char *")] sbyte* id, [NativeTypeName("LLVMContextRef")] void* context);

		[DllImport("libLLVM", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LLVMCloneModule", ExactSpelling = true)]
		[return: NativeTypeName("LLVMModuleRef")]
		public static extern void* ModuleClone([NativeTypeName("LLVMModuleRef")] void* module);

		[DllImport("libLLVM", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LLVMDisposeModule", ExactSpelling = true)]
		public static extern void ModuleDispose([NativeTypeName("LLVMModuleRef")] void* module);

		// --------------------------------------------------------------

		[DllImport("libLLVM", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LLVMDumpModule", ExactSpelling = true)]
		public static extern void DumpModule([NativeTypeName("LLVMModuleRef")] void* module);

		[DllImport("libLLVM", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LLVMPrintModuleToFile", ExactSpelling = true)]
		[return: NativeTypeName("LLVMBool")]
		public static extern int PrintModuleToFile([NativeTypeName("LLVMModuleRef")] void* module, [NativeTypeName("const char *")] sbyte* path, [NativeTypeName("char **")] sbyte** errorMessage);

		[DllImport("libLLVM", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LLVMPrintModuleToString", ExactSpelling = true)]
		[return: NativeTypeName("char *")]
		public static extern sbyte* PrintModuleToString([NativeTypeName("LLVMModuleRef")] void* module);

		#endregion

		// --------------------------------------------------------------

		#region BuilderBindings

		// Builder

		[DllImport("libLLVM", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LLVMCreateBuilder", ExactSpelling = true)]
		[return: NativeTypeName("LLVMBuilderRef")]
		public static extern void* BuilderCreate();

		[DllImport("libLLVM", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LLVMCreateBuilderInContext", ExactSpelling = true)]
		[return: NativeTypeName("LLVMBuilderRef")]
		public static extern void* BuilderCreateInContext([NativeTypeName("LLVMContextRef")] void* context);

		// --------------------------------------------------------------

		#region NegBuilderBindings

		[DllImport("libLLVM", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LLVMBuildNeg", ExactSpelling = true)]
		[return: NativeTypeName("LLVMValueRef")]
		public static extern void* BuildNeg([NativeTypeName("LLVMBuilderRef")] void* builder, [NativeTypeName("LLVMValueRef")] void* value, [NativeTypeName("const char *")] sbyte* name);

		[DllImport("libLLVM", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LLVMBuildNSWNeg", ExactSpelling = true)]
		[return: NativeTypeName("LLVMValueRef")]
		public static extern void* BuildNSWNeg([NativeTypeName("LLVMBuilderRef")] void* builder, [NativeTypeName("LLVMValueRef")] void* value, [NativeTypeName("const char *")] sbyte* name);

		[DllImport("libLLVM", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LLVMBuildNUWNeg", ExactSpelling = true)]
		[return: NativeTypeName("LLVMValueRef")]
		public static extern void* BuildNUWNeg([NativeTypeName("LLVMBuilderRef")] void* builder, [NativeTypeName("LLVMValueRef")] void* value, [NativeTypeName("const char *")] sbyte* name);

		[DllImport("libLLVM", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LLVMBuildFNeg", ExactSpelling = true)]
		[return: NativeTypeName("LLVMValueRef")]
		public static extern void* BuildFNeg([NativeTypeName("LLVMBuilderRef")] void* builder, [NativeTypeName("LLVMValueRef")] void* value, [NativeTypeName("const char *")] sbyte* name);

		#endregion

		// --------------------------------------------------------------

		#region AddBuilderBindings

		[DllImport("libLLVM", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LLVMBuildAdd", ExactSpelling = true)]
		[return: NativeTypeName("LLVMValueRef")]
		public static extern void* BuildAdd([NativeTypeName("LLVMBuilderRef")] void* builder, [NativeTypeName("LLVMValueRef")] void* lhs, [NativeTypeName("LLVMValueRef")] void* rhs, [NativeTypeName("const char *")] sbyte* name);

		[DllImport("libLLVM", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LLVMBuildNUWAdd", ExactSpelling = true)]
		[return: NativeTypeName("LLVMValueRef")]
		public static extern void* BuildNUWAdd([NativeTypeName("LLVMBuilderRef")] void* builder, [NativeTypeName("LLVMValueRef")] void* lhs, [NativeTypeName("LLVMValueRef")] void* rhs, [NativeTypeName("const char *")] sbyte* name);

		[DllImport("libLLVM", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LLVMBuildNSWAdd", ExactSpelling = true)]
		[return: NativeTypeName("LLVMValueRef")]
		public static extern void* BuildNSWAdd([NativeTypeName("LLVMBuilderRef")] void* builder, [NativeTypeName("LLVMValueRef")] void* lhs, [NativeTypeName("LLVMValueRef")] void* rhs, [NativeTypeName("const char *")] sbyte* name);

		[DllImport("libLLVM", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LLVMBuildFAdd", ExactSpelling = true)]
		[return: NativeTypeName("LLVMValueRef")]
		public static extern void* BuildFAdd([NativeTypeName("LLVMBuilderRef")] void* builder, [NativeTypeName("LLVMValueRef")] void* lhs, [NativeTypeName("LLVMValueRef")] void* rhs, [NativeTypeName("const char *")] sbyte* name);

		#endregion

		// --------------------------------------------------------------

		#region SubBuilderBindings

		[DllImport("libLLVM", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LLVMBuildSub", ExactSpelling = true)]
		[return: NativeTypeName("LLVMValueRef")]
		public static extern void* BuildSub([NativeTypeName("LLVMBuilderRef")] void* builder, [NativeTypeName("LLVMValueRef")] void* lhs, [NativeTypeName("LLVMValueRef")] void* rhs, [NativeTypeName("const char *")] sbyte* name);

		[DllImport("libLLVM", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LLVMBuildNUWSub", ExactSpelling = true)]
		[return: NativeTypeName("LLVMValueRef")]
		public static extern void* BuildNUWSub([NativeTypeName("LLVMBuilderRef")] void* builder, [NativeTypeName("LLVMValueRef")] void* lhs, [NativeTypeName("LLVMValueRef")] void* rhs, [NativeTypeName("const char *")] sbyte* name);

		[DllImport("libLLVM", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LLVMBuildNSWSub", ExactSpelling = true)]
		[return: NativeTypeName("LLVMValueRef")]
		public static extern void* BuildNSWSub([NativeTypeName("LLVMBuilderRef")] void* builder, [NativeTypeName("LLVMValueRef")] void* lhs, [NativeTypeName("LLVMValueRef")] void* rhs, [NativeTypeName("const char *")] sbyte* name);

		[DllImport("libLLVM", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LLVMBuildFSub", ExactSpelling = true)]
		[return: NativeTypeName("LLVMValueRef")]
		public static extern void* BuildFSub([NativeTypeName("LLVMBuilderRef")] void* builder, [NativeTypeName("LLVMValueRef")] void* lhs, [NativeTypeName("LLVMValueRef")] void* rhs, [NativeTypeName("const char *")] sbyte* name);

		#endregion

		// --------------------------------------------------------------

		#region MulBuilderBindings

		[DllImport("libLLVM", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LLVMBuildMul", ExactSpelling = true)]
		[return: NativeTypeName("LLVMValueRef")]
		public static extern void* BuildMul([NativeTypeName("LLVMBuilderRef")] void* builder, [NativeTypeName("LLVMValueRef")] void* lhs, [NativeTypeName("LLVMValueRef")] void* rhs, [NativeTypeName("const char *")] sbyte* name);

		[DllImport("libLLVM", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LLVMBuildNUWMul", ExactSpelling = true)]
		[return: NativeTypeName("LLVMValueRef")]
		public static extern void* BuildNUWMul([NativeTypeName("LLVMBuilderRef")] void* builder, [NativeTypeName("LLVMValueRef")] void* lhs, [NativeTypeName("LLVMValueRef")] void* rhs, [NativeTypeName("const char *")] sbyte* name);

		[DllImport("libLLVM", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LLVMBuildNSWMul", ExactSpelling = true)]
		[return: NativeTypeName("LLVMValueRef")]
		public static extern void* BuildNSWMul([NativeTypeName("LLVMBuilderRef")] void* builder, [NativeTypeName("LLVMValueRef")] void* lhs, [NativeTypeName("LLVMValueRef")] void* rhs, [NativeTypeName("const char *")] sbyte* name);

		[DllImport("libLLVM", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LLVMBuildFMul", ExactSpelling = true)]
		[return: NativeTypeName("LLVMValueRef")]
		public static extern void* BuildFMul([NativeTypeName("LLVMBuilderRef")] void* builder, [NativeTypeName("LLVMValueRef")] void* lhs, [NativeTypeName("LLVMValueRef")] void* rhs, [NativeTypeName("const char *")] sbyte* name);

		#endregion

		// --------------------------------------------------------------

		#region DivBuilderBindings

		[DllImport("libLLVM", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LLVMBuildUDiv", ExactSpelling = true)]
		[return: NativeTypeName("LLVMValueRef")]
		public static extern void* BuildUDiv([NativeTypeName("LLVMBuilderRef")] void* builder, [NativeTypeName("LLVMValueRef")] void* lhs, [NativeTypeName("LLVMValueRef")] void* rhs, [NativeTypeName("const char *")] sbyte* name);

		[DllImport("libLLVM", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LLVMBuildExactUDiv", ExactSpelling = true)]
		[return: NativeTypeName("LLVMValueRef")]
		public static extern void* BuildExactUDiv([NativeTypeName("LLVMBuilderRef")] void* builder, [NativeTypeName("LLVMValueRef")] void* lhs, [NativeTypeName("LLVMValueRef")] void* rhs, [NativeTypeName("const char *")] sbyte* name);

		[DllImport("libLLVM", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LLVMBuildSDiv", ExactSpelling = true)]
		[return: NativeTypeName("LLVMValueRef")]
		public static extern void* BuildSDiv([NativeTypeName("LLVMBuilderRef")] void* builder, [NativeTypeName("LLVMValueRef")] void* lhs, [NativeTypeName("LLVMValueRef")] void* rhs, [NativeTypeName("const char *")] sbyte* name);

		[DllImport("libLLVM", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LLVMBuildExactSDiv", ExactSpelling = true)]
		[return: NativeTypeName("LLVMValueRef")]
		public static extern void* BuildExactSDiv([NativeTypeName("LLVMBuilderRef")] void* builder, [NativeTypeName("LLVMValueRef")] void* lhs, [NativeTypeName("LLVMValueRef")] void* rhs, [NativeTypeName("const char *")] sbyte* name);

		[DllImport("libLLVM", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LLVMBuildFDiv", ExactSpelling = true)]
		[return: NativeTypeName("LLVMValueRef")]
		public static extern void* BuildFDiv([NativeTypeName("LLVMBuilderRef")] void* builder, [NativeTypeName("LLVMValueRef")] void* lhs, [NativeTypeName("LLVMValueRef")] void* rhs, [NativeTypeName("const char *")] sbyte* name);

		#endregion

		// --------------------------------------------------------------

		#region RemBuilderBindings

		[DllImport("libLLVM", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LLVMBuildURem", ExactSpelling = true)]
		[return: NativeTypeName("LLVMValueRef")]
		public static extern void* BuildURem([NativeTypeName("LLVMBuilderRef")] void* builder, [NativeTypeName("LLVMValueRef")] void* lhs, [NativeTypeName("LLVMValueRef")] void* rhs, [NativeTypeName("const char *")] sbyte* name);

		[DllImport("libLLVM", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LLVMBuildSRem", ExactSpelling = true)]
		[return: NativeTypeName("LLVMValueRef")]
		public static extern void* BuildSRem([NativeTypeName("LLVMBuilderRef")] void* builder, [NativeTypeName("LLVMValueRef")] void* lhs, [NativeTypeName("LLVMValueRef")] void* rhs, [NativeTypeName("const char *")] sbyte* name);

		[DllImport("libLLVM", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LLVMBuildFRem", ExactSpelling = true)]
		[return: NativeTypeName("LLVMValueRef")]
		public static extern void* BuildFRem([NativeTypeName("LLVMBuilderRef")] void* builder, [NativeTypeName("LLVMValueRef")] void* lhs, [NativeTypeName("LLVMValueRef")] void* rhs, [NativeTypeName("const char *")] sbyte* name);

		#endregion

		// --------------------------------------------------------------

		#region ShiftBuilderBindings

		[DllImport("libLLVM", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LLVMBuildShl", ExactSpelling = true)]
		[return: NativeTypeName("LLVMValueRef")]
		public static extern void* BuildShl([NativeTypeName("LLVMBuilderRef")] void* builder, [NativeTypeName("LLVMValueRef")] void* lhs, [NativeTypeName("LLVMValueRef")] void* rhs, [NativeTypeName("const char *")] sbyte* name);

		[DllImport("libLLVM", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LLVMBuildLShr", ExactSpelling = true)]
		[return: NativeTypeName("LLVMValueRef")]
		public static extern void* BuildLShr([NativeTypeName("LLVMBuilderRef")] void* builder, [NativeTypeName("LLVMValueRef")] void* lhs, [NativeTypeName("LLVMValueRef")] void* rhs, [NativeTypeName("const char *")] sbyte* name);

		[DllImport("libLLVM", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LLVMBuildAShr", ExactSpelling = true)]
		[return: NativeTypeName("LLVMValueRef")]
		public static extern void* BuildAShr([NativeTypeName("LLVMBuilderRef")] void* builder, [NativeTypeName("LLVMValueRef")] void* lhs, [NativeTypeName("LLVMValueRef")] void* rhs, [NativeTypeName("const char *")] sbyte* name);

		#endregion

		// --------------------------------------------------------------

		#region LogicBuilderBindings

		[DllImport("libLLVM", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LLVMBuildNot", ExactSpelling = true)]
		[return: NativeTypeName("LLVMValueRef")]
		public static extern void* BuildNot([NativeTypeName("LLVMBuilderRef")] void* builder, [NativeTypeName("LLVMValueRef")] void* value, [NativeTypeName("const char *")] sbyte* name);

		[DllImport("libLLVM", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LLVMBuildAnd", ExactSpelling = true)]
		[return: NativeTypeName("LLVMValueRef")]
		public static extern void* BuildAnd([NativeTypeName("LLVMBuilderRef")] void* builder, [NativeTypeName("LLVMValueRef")] void* lhs, [NativeTypeName("LLVMValueRef")] void* rhs, [NativeTypeName("const char *")] sbyte* name);

		[DllImport("libLLVM", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LLVMBuildOr", ExactSpelling = true)]
		[return: NativeTypeName("LLVMValueRef")]
		public static extern void* BuildOr([NativeTypeName("LLVMBuilderRef")] void* builder, [NativeTypeName("LLVMValueRef")] void* lhs, [NativeTypeName("LLVMValueRef")] void* rhs, [NativeTypeName("const char *")] sbyte* name);

		[DllImport("libLLVM", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LLVMBuildXor", ExactSpelling = true)]
		[return: NativeTypeName("LLVMValueRef")]
		public static extern void* BuildXOr([NativeTypeName("LLVMBuilderRef")] void* builder, [NativeTypeName("LLVMValueRef")] void* lhs, [NativeTypeName("LLVMValueRef")] void* rhs, [NativeTypeName("const char *")] sbyte* name);

		#endregion

		// --------------------------------------------------------------

		[DllImport("libLLVM", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LLVMBuildBinOp", ExactSpelling = true)]
		[return: NativeTypeName("LLVMValueRef")]
		public static extern void* BuildBinOp([NativeTypeName("LLVMBuilderRef")] void* builder, LLVMOpcode opcode, [NativeTypeName("LLVMValueRef")] void* lhs, [NativeTypeName("LLVMValueRef")] void* rhs, [NativeTypeName("const char *")] sbyte* name);

		// --------------------------------------------------------------

		[DllImport("libLLVM", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LLVMPositionBuilder", ExactSpelling = true)]
		public static extern void PositionBuilder([NativeTypeName("LLVMBuilderRef")] void* builder, [NativeTypeName("LLVMBasicBlockRef")] void* block, [NativeTypeName("LLVMValueRef")] void* instr);

		[DllImport("libLLVM", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LLVMPositionBuilderBefore", ExactSpelling = true)]
		public static extern void PositionBuilderBefore([NativeTypeName("LLVMBuilderRef")] void* builder, [NativeTypeName("LLVMValueRef")] void* instr);

		[DllImport("libLLVM", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LLVMPositionBuilderAtEnd", ExactSpelling = true)]
		public static extern void PositionBuilderAtEnd([NativeTypeName("LLVMBuilderRef")] void* builder, [NativeTypeName("LLVMBasicBlockRef")] void* block);

		#endregion

		// --------------------------------------------------------------

		#region BasicBlockBindings

		// Basic Block

		[DllImport("libLLVM", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LLVMCreateBasicBlockInContext", ExactSpelling = true)]
		[return: NativeTypeName("LLVMBasicBlockRef")]
		public static extern void* CreateBasicBlockInContext([NativeTypeName("LLVMContextRef")] void* context, [NativeTypeName("const char *")] sbyte* name);

		[DllImport("libLLVM", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LLVMDeleteBasicBlock", ExactSpelling = true)]
		public static extern void DeleteBasicBlock([NativeTypeName("LLVMBasicBlockRef")] void* block);

		// --------------------------------------------------------------

		[DllImport("libLLVM", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LLVMInsertBasicBlockInContext", ExactSpelling = true)]
		[return: NativeTypeName("LLVMBasicBlockRef")]
		public static extern void AppendExistingBasicBlock([NativeTypeName("LLVMValueRef")] void* fn, [NativeTypeName("LLVMBasicBlockRef")] void* block);

		[DllImport("libLLVM", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LLVMAppendBasicBlockInContext", ExactSpelling = true)]
		[return: NativeTypeName("LLVMBasicBlockRef")]
		public static extern void* AppendBasicBlockInContext([NativeTypeName("LLVMContextRef")] void* context, [NativeTypeName("LLVMValueRef")] void* fn, [NativeTypeName("const char *")] sbyte* name);

		[DllImport("libLLVM", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LLVMAppendBasicBlock", ExactSpelling = true)]
		[return: NativeTypeName("LLVMBasicBlockRef")]
		public static extern void* AppendBasicBlock([NativeTypeName("LLVMValueRef")] void* fn, [NativeTypeName("const char *")] sbyte* name);

		// --------------------------------------------------------------

		[DllImport("libLLVM", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LLVMInsertExistingBasicBlockAfterInsertBlock", ExactSpelling = true)]
		public static extern void InsertExistingBasicBlockAfterInsertBlock([NativeTypeName("LLVMBuilderRef")] void* builder, [NativeTypeName("LLVMBasicBlockRef")] void* block);

		[DllImport("libLLVM", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LLVMAppendExistingBasicBlock", ExactSpelling = true)]
		public static extern void* InsertBasicBlockInContext([NativeTypeName("LLVMContextRef")] void* context, [NativeTypeName("LLVMBasicBlockRef")] void* block, [NativeTypeName("const char *")] sbyte* name);

		[DllImport("libLLVM", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LLVMInsertBasicBlock", ExactSpelling = true)]
		[return: NativeTypeName("LLVMBasicBlockRef")]
		public static extern void* InsertBasicBlock([NativeTypeName("LLVMBasicBlockRef")] void* previousBlock, [NativeTypeName("const char *")] sbyte* name);

		// --------------------------------------------------------------

		[DllImport("libLLVM", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LLVMRemoveBasicBlockFromParent", ExactSpelling = true)]
		public static extern void RemoveBasicBlockFromParent([NativeTypeName("LLVMBasicBlockRef")] void* block);

		[DllImport("libLLVM", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LLVMMoveBasicBlockBefore", ExactSpelling = true)]
		public static extern void MoveBasicBlockBefore([NativeTypeName("LLVMBasicBlockRef")] void* block, [NativeTypeName("LLVMBasicBlockRef")] void* destinationBlock);

		[DllImport("libLLVM", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LLVMMoveBasicBlockAfter", ExactSpelling = true)]
		public static extern void MoveBasicBlockAfter([NativeTypeName("LLVMBasicBlockRef")] void* block, [NativeTypeName("LLVMBasicBlockRef")] void* destinationBlock);

		// --------------------------------------------------------------

		[DllImport("libLLVM", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LLVMGetFirstInstruction", ExactSpelling = true)]
		[return: NativeTypeName("LLVMValueRef")]
		public static extern void* GetFirstInstruction([NativeTypeName("LLVMBasicBlockRef")] void* block);

		[DllImport("libLLVM", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LLVMGetLastInstruction", ExactSpelling = true)]
		[return: NativeTypeName("LLVMValueRef")]
		public static extern void* GetLastInstruction([NativeTypeName("LLVMBasicBlockRef")] void* block);

		#endregion

		// --------------------------------------------------------------

		#region ValueBindings

		// Value

		[DllImport("libLLVM", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LLVMConstPointerNull", ExactSpelling = true)]
		[return: NativeTypeName("LLVMValueRef")]
		public static extern void* ConstPointerNull([NativeTypeName("LLVMTypeRef")] void* type);

		[DllImport("libLLVM", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LLVMConstInt", ExactSpelling = true)]
		[return: NativeTypeName("LLVMValueRef")]
		public static extern void* ConstInt([NativeTypeName("LLVMTypeRef")] void* intType, [NativeTypeName("unsigned long long")] ulong value, [NativeTypeName("LLVMBool")] int signExtend);

		[DllImport("libLLVM", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LLVMConstIntOfArbitraryPrecision", ExactSpelling = true)]
		[return: NativeTypeName("LLVMValueRef")]
		public static extern void* ConstIntOfArbitraryPrecision([NativeTypeName("LLVMTypeRef")] void* intType, [NativeTypeName("unsigned int")] uint numWords, [NativeTypeName("const uint64_t[]")] ulong* words);

		[DllImport("libLLVM", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LLVMConstIntOfString", ExactSpelling = true)]
		[return: NativeTypeName("LLVMValueRef")]
		public static extern void* ConstIntOfString([NativeTypeName("LLVMTypeRef")] void* intType, [NativeTypeName("const char *")] sbyte* text, [NativeTypeName("uint8_t")] byte radix);

		[DllImport("libLLVM", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LLVMConstIntOfStringAndSize", ExactSpelling = true)]
		[return: NativeTypeName("LLVMValueRef")]
		public static extern void* ConstIntOfStringAndSize([NativeTypeName("LLVMTypeRef")] void* intType, [NativeTypeName("const char *")] sbyte* text, [NativeTypeName("unsigned int")] uint length, [NativeTypeName("uint8_t")] byte radix);

		[DllImport("libLLVM", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LLVMConstIntGetZExtValue", ExactSpelling = true)]
		[return: NativeTypeName("unsigned long long")]
		public static extern ulong ConstIntGetZExtValue([NativeTypeName("LLVMValueRef")] void* value);

		[DllImport("libLLVM", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LLVMConstIntGetSExtValue", ExactSpelling = true)]
		[return: NativeTypeName("long long")]
		public static extern long ConstIntGetSExtValue([NativeTypeName("LLVMValueRef")] void* value);

		// --------------------------------------------------------------

		[DllImport("libLLVM", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LLVMConstReal", ExactSpelling = true)]
		[return: NativeTypeName("LLVMValueRef")]
		public static extern void* ConstReal([NativeTypeName("LLVMTypeRef")] void* realType, double value);

		[DllImport("libLLVM", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LLVMConstRealOfString", ExactSpelling = true)]
		[return: NativeTypeName("LLVMValueRef")]
		public static extern void* ConstRealOfString([NativeTypeName("LLVMTypeRef")] void* realType, [NativeTypeName("const char *")] sbyte* text);

		[DllImport("libLLVM", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LLVMConstRealOfStringAndSize", ExactSpelling = true)]
		[return: NativeTypeName("LLVMValueRef")]
		public static extern void* ConstRealOfStringAndSize([NativeTypeName("LLVMTypeRef")] void* realType, [NativeTypeName("const char *")] sbyte* text, [NativeTypeName("unsigned int")] uint length);

		[DllImport("libLLVM", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LLVMConstRealGetDouble", ExactSpelling = true)]
		public static extern double ConstRealGetDouble([NativeTypeName("LLVMValueRef")] void* value, [NativeTypeName("LLVMBool *")] int* losesInfo);

		// --------------------------------------------------------------

		[DllImport("libLLVM", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LLVMConstStringInContext", ExactSpelling = true)]
		[return: NativeTypeName("LLVMValueRef")]
		public static extern void* ConstStringInContext([NativeTypeName("LLVMContextRef")] void* context, [NativeTypeName("const char *")] sbyte* str, [NativeTypeName("unsigned int")] uint length, [NativeTypeName("LLVMBool")] int dontNullTerminate);

		[DllImport("libLLVM", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LLVMConstString", ExactSpelling = true)]
		[return: NativeTypeName("LLVMValueRef")]
		public static extern void* ConstString([NativeTypeName("const char *")] sbyte* str, [NativeTypeName("unsigned int")] uint length, [NativeTypeName("LLVMBool")] int dontNullTerminate);

		// --------------------------------------------------------------

		[DllImport("libLLVM", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LLVMConstStructInContext", ExactSpelling = true)]
		[return: NativeTypeName("LLVMValueRef")]
		public static extern void* ConstStructInContext([NativeTypeName("LLVMContextRef")] void* context, [NativeTypeName("LLVMValueRef *")] void** constantValues, [NativeTypeName("unsigned int")] uint count, [NativeTypeName("LLVMBool")] int isPacked);

		[DllImport("libLLVM", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LLVMConstStruct", ExactSpelling = true)]
		[return: NativeTypeName("LLVMValueRef")]
		public static extern void* ConstStruct([NativeTypeName("LLVMValueRef *")] void** constantValues, [NativeTypeName("unsigned int")] uint count, [NativeTypeName("LLVMBool")] int isPacked);

		// --------------------------------------------------------------

		[DllImport("libLLVM", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LLVMTypeOf", ExactSpelling = true)]
		[return: NativeTypeName("LLVMTypeRef")]
		public static extern void* TypeOf([NativeTypeName("LLVMValueRef")] void* value);

		// --------------------------------------------------------------

		[DllImport("libLLVM", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LLVMDumpValue", ExactSpelling = true)]
		public static extern void DumpValue([NativeTypeName("LLVMValueRef")] void* value);

		[DllImport("libLLVM", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LLVMPrintValueToString", ExactSpelling = true)]
		[return: NativeTypeName("char *")]
		public static extern sbyte* PrintValueToString([NativeTypeName("LLVMValueRef")] void* value);

		#endregion

		// --------------------------------------------------------------

		#region TypeBindings

		// Type

		// --------------------------------------------------------------

		#region IntTypeBindings

		[DllImport("libLLVM", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LLVMInt1Type", ExactSpelling = true)]
		[return: NativeTypeName("LLVMTypeRef")]
		public static extern void* Int1Type();

		[DllImport("libLLVM", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LLVMInt8Type", ExactSpelling = true)]
		[return: NativeTypeName("LLVMTypeRef")]
		public static extern void* Int8Type();

		[DllImport("libLLVM", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LLVMInt16Type", ExactSpelling = true)]
		[return: NativeTypeName("LLVMTypeRef")]
		public static extern void* Int16Type();

		[DllImport("libLLVM", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LLVMInt32Type", ExactSpelling = true)]
		[return: NativeTypeName("LLVMTypeRef")]
		public static extern void* Int32Type();

		[DllImport("libLLVM", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LLVMInt64Type", ExactSpelling = true)]
		[return: NativeTypeName("LLVMTypeRef")]
		public static extern void* Int64Type();

		[DllImport("libLLVM", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LLVMInt128Type", ExactSpelling = true)]
		[return: NativeTypeName("LLVMTypeRef")]
		public static extern void* Int128Type();

		// --------------------------------------------------------------

		[DllImport("libLLVM", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LLVMIntType", ExactSpelling = true)]
		[return: NativeTypeName("LLVMTypeRef")]
		public static extern void* IntType([NativeTypeName("unsigned int")] uint bitsNumber);

		// --------------------------------------------------------------

		[DllImport("libLLVM", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LLVMInt1TypeInContext", ExactSpelling = true)]
		[return: NativeTypeName("LLVMTypeRef")]
		public static extern void* Int1TypeInContext([NativeTypeName("LLVMContextRef")] void* context);

		[DllImport("libLLVM", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LLVMInt8TypeInContext", ExactSpelling = true)]
		[return: NativeTypeName("LLVMTypeRef")]
		public static extern void* Int8TypeInContext([NativeTypeName("LLVMContextRef")] void* context);

		[DllImport("libLLVM", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LLVMInt16TypeInContext", ExactSpelling = true)]
		[return: NativeTypeName("LLVMTypeRef")]
		public static extern void* Int16TypeInContext([NativeTypeName("LLVMContextRef")] void* context);

		[DllImport("libLLVM", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LLVMInt32TypeInContext", ExactSpelling = true)]
		[return: NativeTypeName("LLVMTypeRef")]
		public static extern void* Int32TypeInContext([NativeTypeName("LLVMContextRef")] void* context);

		[DllImport("libLLVM", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LLVMInt64TypeInContext", ExactSpelling = true)]
		[return: NativeTypeName("LLVMTypeRef")]
		public static extern void* Int64TypeInContext([NativeTypeName("LLVMContextRef")] void* context);

		[DllImport("libLLVM", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LLVMInt128TypeInContext", ExactSpelling = true)]
		[return: NativeTypeName("LLVMTypeRef")]
		public static extern void* Int128TypeInContext([NativeTypeName("LLVMContextRef")] void* context);

		// --------------------------------------------------------------

		[DllImport("libLLVM", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LLVMIntTypeInContext", ExactSpelling = true)]
		[return: NativeTypeName("LLVMTypeRef")]
		public static extern void* IntTypeInContext([NativeTypeName("LLVMContextRef")] void* context, [NativeTypeName("unsigned int")] uint bitsNumber);

		// --------------------------------------------------------------

		[DllImport("libLLVM", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LLVMGetIntTypeWidth", ExactSpelling = true)]
		[return: NativeTypeName("unsigned int")]
		public static extern uint GetIntTypeWidth([NativeTypeName("LLVMTypeRef")] void* intType);

		#endregion

		// --------------------------------------------------------------

		#region RealTypeBindings

		[DllImport("libLLVM", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LLVMHalfType", ExactSpelling = true)]
		[return: NativeTypeName("LLVMTypeRef")]
		public static extern void* HalfType();

		[DllImport("libLLVM", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LLVMBFloatType", ExactSpelling = true)]
		[return: NativeTypeName("LLVMTypeRef")]
		public static extern void* BFloatType();

		[DllImport("libLLVM", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LLVMFloatType", ExactSpelling = true)]
		[return: NativeTypeName("LLVMTypeRef")]
		public static extern void* FloatType();

		[DllImport("libLLVM", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LLVMDoubleType", ExactSpelling = true)]
		[return: NativeTypeName("LLVMTypeRef")]
		public static extern void* DoubleType();

		[DllImport("libLLVM", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LLVMX86FP80Type", ExactSpelling = true)]
		[return: NativeTypeName("LLVMTypeRef")]
		public static extern void* X86FP80Type();

		[DllImport("libLLVM", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LLVMFP128Type", ExactSpelling = true)]
		[return: NativeTypeName("LLVMTypeRef")]
		public static extern void* FP128Type();

		[DllImport("libLLVM", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LLVMPPCFP128Type", ExactSpelling = true)]
		[return: NativeTypeName("LLVMTypeRef")]
		public static extern void* PPCFP128Type();

		// --------------------------------------------------------------

		[DllImport("libLLVM", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LLVMHalfTypeInContext", ExactSpelling = true)]
		[return: NativeTypeName("LLVMTypeRef")]
		public static extern void* HalfTypeInContext([NativeTypeName("LLVMContextRef")] void* context);

		[DllImport("libLLVM", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LLVMBFloatTypeInContext", ExactSpelling = true)]
		[return: NativeTypeName("LLVMTypeRef")]
		public static extern void* BFloatTypeInContext([NativeTypeName("LLVMContextRef")] void* context);

		[DllImport("libLLVM", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LLVMFloatTypeInContext", ExactSpelling = true)]
		[return: NativeTypeName("LLVMTypeRef")]
		public static extern void* FloatTypeInContext([NativeTypeName("LLVMContextRef")] void* context);

		[DllImport("libLLVM", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LLVMDoubleTypeInContext", ExactSpelling = true)]
		[return: NativeTypeName("LLVMTypeRef")]
		public static extern void* DoubleTypeInContext([NativeTypeName("LLVMContextRef")] void* context);

		[DllImport("libLLVM", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LLVMX86FP80TypeInContext", ExactSpelling = true)]
		[return: NativeTypeName("LLVMTypeRef")]
		public static extern void* X86FP80TypeInContext([NativeTypeName("LLVMContextRef")] void* context);

		[DllImport("libLLVM", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LLVMFP128TypeInContext", ExactSpelling = true)]
		[return: NativeTypeName("LLVMTypeRef")]
		public static extern void* FP128TypeInContext([NativeTypeName("LLVMContextRef")] void* context);

		[DllImport("libLLVM", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LLVMPPCFP128TypeInContext", ExactSpelling = true)]
		[return: NativeTypeName("LLVMTypeRef")]
		public static extern void* PPCFP128TypeInContext([NativeTypeName("LLVMContextRef")] void* context);

		#endregion

		// --------------------------------------------------------------

		#region FunctionTypeBindings

		[DllImport("libLLVM", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LLVMFunctionType", ExactSpelling = true)]
		[return: NativeTypeName("LLVMTypeRef")]
		public static extern void* FunctionType([NativeTypeName("LLVMTypeRef")] void* returnType, [NativeTypeName("LLVMTypeRef *")] void** paramTypes, [NativeTypeName("unsigned int")] uint paramCount, [NativeTypeName("LLVMBool")] int isVarArg);

		// --------------------------------------------------------------

		[DllImport("libLLVM", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LLVMIsFunctionVarArg", ExactSpelling = true)]
		[return: NativeTypeName("LLVMBool")]
		public static extern int IsFunctionVarArg([NativeTypeName("LLVMTypeRef")] void* functionType);

		[DllImport("libLLVM", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LLVMGetReturnType", ExactSpelling = true)]
		[return: NativeTypeName("LLVMTypeRef")]
		public static extern void* GetReturnType([NativeTypeName("LLVMTypeRef")] void* functionType);

		[DllImport("libLLVM", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LLVMCountParamTypes", ExactSpelling = true)]
		[return: NativeTypeName("unsigned int")]
		public static extern uint CountParamTypes([NativeTypeName("LLVMTypeRef")] void* functionType);

		[DllImport("libLLVM", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LLVMGetParamTypes", ExactSpelling = true)]
		public static extern void GetParamTypes([NativeTypeName("LLVMTypeRef")] void* functionType, [NativeTypeName("LLVMTypeRef *")] void** dest);

		#endregion

		// --------------------------------------------------------------

		#region StructTypeBindings

		[DllImport("libLLVM", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LLVMStructType", ExactSpelling = true)]
		[return: NativeTypeName("LLVMTypeRef")]
		public static extern void* StructType([NativeTypeName("LLVMTypeRef *")] void** elementTypes, [NativeTypeName("unsigned int")] uint elementCount, [NativeTypeName("LLVMBool")] int isPacked);

		// --------------------------------------------------------------

		[DllImport("libLLVM", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LLVMStructTypeInContext", ExactSpelling = true)]
		[return: NativeTypeName("LLVMTypeRef")]
		public static extern void* StructTypeInContext([NativeTypeName("LLVMContextRef")] void* context, [NativeTypeName("LLVMTypeRef *")] void** elementTypes, [NativeTypeName("unsigned int")] uint elementCount, [NativeTypeName("LLVMBool")] int isPacked);

		[DllImport("libLLVM", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LLVMStructCreateNamed", ExactSpelling = true)]
		[return: NativeTypeName("LLVMTypeRef")]
		public static extern void* StructCreateNamed([NativeTypeName("LLVMContextRef")] void* context, [NativeTypeName("const char *")] sbyte* name);

		// --------------------------------------------------------------

		[DllImport("libLLVM", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LLVMGetStructName", ExactSpelling = true)]
		[return: NativeTypeName("const char *")]
		public static extern sbyte* GetStructName([NativeTypeName("LLVMTypeRef")] void* structType);

		[DllImport("libLLVM", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LLVMStructSetBody", ExactSpelling = true)]
		public static extern void StructSetBody([NativeTypeName("LLVMTypeRef")] void* structType, [NativeTypeName("LLVMTypeRef *")] void** elementTypes, [NativeTypeName("unsigned int")] uint elementCount, [NativeTypeName("LLVMBool")] int isPacked);

		[DllImport("libLLVM", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LLVMCountStructElementTypes", ExactSpelling = true)]
		[return: NativeTypeName("unsigned int")]
		public static extern uint CountStructElementTypes([NativeTypeName("LLVMTypeRef")] void* structType);

		[DllImport("libLLVM", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LLVMGetStructElementTypes", ExactSpelling = true)]
		public static extern void GetStructElementTypes([NativeTypeName("LLVMTypeRef")] void* structType, [NativeTypeName("LLVMTypeRef *")] void** dest);

		[DllImport("libLLVM", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LLVMStructGetTypeAtIndex", ExactSpelling = true)]
		[return: NativeTypeName("LLVMTypeRef")]
		public static extern void* StructGetTypeAtIndex([NativeTypeName("LLVMTypeRef")] void* structType, [NativeTypeName("unsigned int")] uint i);

		[DllImport("libLLVM", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LLVMIsPackedStruct", ExactSpelling = true)]
		[return: NativeTypeName("LLVMBool")]
		public static extern int IsPackedStruct([NativeTypeName("LLVMTypeRef")] void* structType);

		[DllImport("libLLVM", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LLVMIsOpaqueStruct", ExactSpelling = true)]
		[return: NativeTypeName("LLVMBool")]
		public static extern int IsOpaqueStruct([NativeTypeName("LLVMTypeRef")] void* structType);

		[DllImport("libLLVM", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LLVMIsLiteralStruct", ExactSpelling = true)]
		[return: NativeTypeName("LLVMBool")]
		public static extern int IsLiteralStruct([NativeTypeName("LLVMTypeRef")] void* structType);

		#endregion

		// --------------------------------------------------------------

		[DllImport("libLLVM", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LLVMDumpType", ExactSpelling = true)]
		public static extern void DumpType([NativeTypeName("LLVMTypeRef")] void* type);

		[DllImport("libLLVM", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LLVMPrintTypeToString", ExactSpelling = true)]
		[return: NativeTypeName("char *")]
		public static extern sbyte* PrintTypeToString([NativeTypeName("LLVMTypeRef")] void* type);

		#endregion

		/* =------------------------------------------------------------= */
	}
}
