using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace Sigmath.CodeGen.Interop
{
	public unsafe delegate sbyte* UnmarshalerFunc<T>(T* arg, nuint* length)
		where T : unmanaged;

	[DebuggerDisplay($"{{{nameof(this.GetDebuggerDisplay)}(), nq}}")]
	public unsafe readonly struct ReferenceString(sbyte* ptr) :
		IDisposable, IEquatable<ReferenceString>, IReference
	{
		private readonly sbyte* _internalPtr = ptr;

		/* =---- Constructors ------------------------------------------= */

		public ReferenceString(nint handle) :
			this((sbyte*)handle.ToPointer())
		{
		}

		/* =---- Static Fields -----------------------------------------= */

		public static readonly ReferenceString Null = new(IntPtr.Zero);
		public static readonly ReferenceString Empty = Marshal(String.Empty);

		/* =---- Static Methods ----------------------------------------= */

		public static bool IsNullOrEmpty(ReferenceString referencedString)
			=> (referencedString == Null) || (referencedString == Empty);

		// --------------------------------------------------------------

		public static ReferenceString Marshal(string str)
		{
			nuint length = str.Length > 0 ? (nuint)Encoding.UTF8.GetMaxByteCount(str.Length) : 0;
			sbyte* value;

#if NET6_0_OR_GREATER
			value = (sbyte*)NativeMemory.Alloc(length + 1);
#else
			value = (sbyte*)Marsahl.AllocHGlobal(length + 1);
#endif

			if (length > 0)
			{
				fixed (char* ptr = str)
					length = (nuint)Encoding.UTF8.GetBytes(ptr, str.Length, (byte*)value, (int)length);
			}

			value[length] = 0;

			return value;
		}

		public static ReferenceString[] Marshal(string[] strings)
		{
			ReferenceString[] result = new ReferenceString[strings.Length];

			for (int i = 0; i < strings.Length; i++)
				result[i] = Marshal(strings[i]);

			return result;
		}

		public static string Unmarshal(sbyte* message)
			=> ((nint)message).IsNotZero() ? new(message) : String.Empty;

		public static string Unmarshal<T>(T* arg, UnmarshalerFunc<T> unmarshaler)
			where T : unmanaged
		{
			nuint length;
			void* result = unmarshaler(arg, &length);

			return ((nint)result).IsNotZero() && (length > 0) ? new((sbyte*)result, 0, (int)length) : String.Empty;
		}

		/* =---- Properties --------------------------------------------= */

		public nint Handle => (nint)_internalPtr;
		public sbyte* Value => _internalPtr;

		/* =---- Methods -----------------------------------------------= */

		public bool IsNullOrEmpty()
			=> this.Handle.IsZero() || IsNullOrEmpty(this);

		// --------------------------------------------------------------

		public void Dispose()
		{
			if (!this.IsNullOrEmpty())
#if NET6_0_OR_GREATER
				NativeMemory.Free(_internalPtr);
#else
				Marshal.FreeHGlobal((nint)_ref);
#endif
		}

		public bool Equals(ReferenceString other)
			=> this.Handle == other.Handle;

		public override bool Equals(object? obj)
			=> obj is ReferenceString other && this.Equals(other);

		public override int GetHashCode()
			=> this.Handle.GetHashCode();

		public override string ToString()
			=> IReference.GetRefName(this);

		private string GetDebuggerDisplay()
			=> !this.IsNullOrEmpty() ? $"\"{new(_internalPtr)}\"" : nameof(Empty);

		/* =---- Operators ---------------------------------------------= */

		public static bool operator ==(ReferenceString left, ReferenceString right)
			=> left.Equals(right);

		public static bool operator !=(ReferenceString left, ReferenceString right)
			=> !left.Equals(right);

		// --------------------------------------------------------------

		public static implicit operator ReferenceString(sbyte* ptr)
			=> new(ptr);

		public static implicit operator ReferenceString(nint handle)
			=> new(handle);

		public static implicit operator sbyte*(ReferenceString that)
			=> that._internalPtr;

		public static implicit operator nint(ReferenceString that)
			=> that.Handle;

		public static implicit operator string(ReferenceString that)
			=> Unmarshal(that);

		/* =------------------------------------------------------------= */
	}
}
