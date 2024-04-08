using System.IO;

namespace Sigmath.CodeGen.Interop
{
	internal interface IReferenceDump
	{
		/* =---- Methods -----------------------------------------------= */

		string AsString();

		void Dump();
		void Dump(TextWriter textWriter);

		/* =------------------------------------------------------------= */
	}
}
