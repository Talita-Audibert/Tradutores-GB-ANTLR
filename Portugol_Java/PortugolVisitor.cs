﻿using Antlr4.Runtime;
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
			string a = context.ID().GetText();
			classFile.AppendFormat("public class {0}{{ \n\tpublic static void main(String[] args){{\n ", a);
			return base.VisitPrograma(context);
		}

		public override int VisitDecFuncoes([NotNull] PortugolParser.DecFuncoesContext context)
		{
			string a = context.GetText();
			return base.VisitDecFuncoes(context);
		}

		public override int VisitDecVars([NotNull] PortugolParser.DecVarsContext context)
		{
			string a = context.GetText();
			return base.VisitDecVars(context);
		}

		public override int VisitListaVar([NotNull] PortugolParser.ListaVarContext context)
		{
			string tipo = context.tipo().GetText();
			string variavel = context.ID(0).ToString();
			switch (tipo) {
				case "REAL":
					classFile.AppendFormat("double {0}", variavel);
					break;
				case "INTEIRO":
					classFile.AppendFormat("int {0}", variavel);
					break;
				case "BOOLEANO":
					classFile.AppendFormat("bool {0}", variavel);
					break;
				case "LOGICO":
					classFile.AppendFormat("bool {0}", variavel);
					break;
				case "STRING":
					classFile.AppendFormat("string {0}", variavel);
					break;
				case "CARACTERE":
					classFile.AppendFormat("char {0}", variavel);
					break;
			}

			int r = context.ID().Length;
			for(int i = 1; i<r; i++)
			{
				variavel = context.ID(i).ToString();
				classFile.AppendFormat(", {0}", variavel);
			}

			classFile.Append(";\n");
						
			return base.VisitListaVar(context);
		}

		public override int VisitStatement([NotNull] PortugolParser.StatementContext context)
		{
			string a = context.GetText();
			return base.VisitStatement(context);
		}

		public override int VisitTipo([NotNull] PortugolParser.TipoContext context)
		{
			string a = context.GetText();
			return base.VisitTipo(context);
		}

		public override int VisitListaPar([NotNull] PortugolParser.ListaParContext context)
		{
			string a = context.GetText();
			return base.VisitListaPar(context);
		}

		public override int VisitExpression([NotNull] PortugolParser.ExpressionContext context)
		{
			string a = context.GetText();
			return base.VisitExpression(context);
		}

		public override int VisitOperacao([NotNull] PortugolParser.OperacaoContext context)
		{
			string a = context.GetText();
			return base.VisitOperacao(context);
		}

		public override int VisitValor([NotNull] PortugolParser.ValorContext context)
		{
			string a = context.GetText();
			return base.VisitValor(context);
		}

	}
}
