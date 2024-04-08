using System;
using System.Runtime.CompilerServices;

namespace Sigmath.CodeGen
{
	public static unsafe partial class UnsafeExtensions
	{
		/* =---- Extension Methods -------------------------------------= */

		// IntPtr Extensions

		public static bool IsZero(this nint value)
			=> value == IntPtr.Zero;

		public static bool IsNotZero(this nint value)
			=> value != IntPtr.Zero;

		public static void* AsPointer(this nint value)
			=> (void*)value;

		// Array Extensions

		public static TArray* AsPointer<TArray>(this TArray[] value)
			where TArray : unmanaged => (TArray*)(value.Length > 0 ? Unsafe.AsPointer(ref value[0]) : null);

		// Span Extensions

		public static TArray* AsPointer<TArray>(this Span<TArray> value)
			where TArray : unmanaged => (TArray*)(!value.IsEmpty ? Unsafe.AsPointer(ref value[0]) : null);

		/* =------------------------------------------------------------= */
	}
}
