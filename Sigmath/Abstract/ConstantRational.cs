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
	public sealed class ConstantRational(int width, long numerator, ulong denominator) :
		Constant<(long Numerator, ulong Denominator)>((numerator, denominator))
	{
		/* =---- Constructors ------------------------------------------= */

		public ConstantRational(long numerator, ulong denominator) :
			this(Math.Max(ConstantInteger.GetWidth(numerator), ConstantNatural.GetWidth(denominator)), numerator, denominator)
		{
		}

		/* =---- Properties --------------------------------------------= */

		public int Width => width;

		/* =---- Methods -----------------------------------------------= */

		public override AbstractValue GetAbstractValue(Generator generator)
			=> generator.GetConstIntPair(this.Width, this.Value.Numerator, this.Value.Denominator, true);

		/* =------------------------------------------------------------= */
	}
}
