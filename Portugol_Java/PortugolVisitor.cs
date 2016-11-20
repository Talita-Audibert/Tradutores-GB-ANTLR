using Antlr4.Runtime.Misc;
using System.Text;

namespace Portugol_Java
{
	public class PortugolVisitor : PortugolBaseVisitor<int>
	{
		StringBuilder classFile;

		public PortugolVisitor()
		{
			classFile = new StringBuilder();
		}

		public override int VisitPrograma([NotNull] PortugolParser.ProgramaContext context)
		{
			return base.VisitPrograma(context);
		}
	}
}
