using Sigmath.CodeGen;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Sigmath.Abstract
{
	public sealed class ConstantNatural(int width, ulong value) :
		Constant<ulong>(value)
	{
		/* =---- Constructors ------------------------------------------= */

		public ConstantNatural(ulong value) :
			this(GetWidth(value), value)
		{
		}

		/* =---- Static Methods ----------------------------------------= */

		public static int GetWidth(ulong value)
		{
			int result;

			switch (value)
			{
			case (<= UInt64.MaxValue) and (> UInt32.MaxValue):
				result = 64;
				break;

			case <= UInt32.MaxValue:
				result = 32;
				break;

			default:
				throw new Exception();
			}	

			return result;
		}

		/* =---- Properties --------------------------------------------= */

		public int Width => width;

		/* =---- Methods -----------------------------------------------= */

		public override AbstractValue GetAbstractValue(Generator generator)
			=> generator.GetConstInt(this.Width, this.Value, isSigned: false);

		/* =------------------------------------------------------------= */
	}
}
