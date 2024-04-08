using Sigmath.CodeGen;
using Sigmath.Lex;
using Sigmath.Parse;
using Sigmath.Parse.Concrete;

using System;

namespace Sigmath
{
	public static class Program
	{
		public static int Main(string[] args)
		{
			CodeGenerator g = CodeGenerator.CreateInGlobalContext();

			Fraction f1 = (3, 2), f2 = (1, 2), f3 = f1 % f2;

			Parser p = new(SourceReader.CreateFromText("1 / 2 + 2 * 3 - 2"));

			var v = p.ParseExpression().GetValue(g);

			return Environment.ExitCode;
		}
	}
}
