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
	public sealed class ConstantInteger(int width, long value) :
		Constant<long>(value)
	{
		/* =---- Constructors ------------------------------------------= */

		public ConstantInteger(long value) :
			this(GetWidth(value), value)
		{
		}

		/* =---- Static Methods ----------------------------------------= */

		public static int GetWidth(long value)
		{
			int result;

			switch (value)
			{
			case ((<= Int64.MaxValue) and (> Int32.MaxValue))
			  or ((>= Int64.MinValue) and (< Int32.MinValue)):
				result = 64;
				break;

			case (<= Int32.MaxValue)
			  or (>= Int32.MinValue):
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
			=> generator.GetConstInt(this.Width, this.Value, isSigned: true);

		/* =------------------------------------------------------------= */
	}
}
