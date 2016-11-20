using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using System;
using System.IO;
using System.Text;

namespace Portugol_Java
{
	public static class Identacao
	{
		public const string Metodo = "\t";
		public const string CorpoMetodo = "\t\t";
	}
	public class PortugolVisitor : PortugolBaseVisitor<int>
	{
		StringBuilder classFile;

		public PortugolVisitor()
		{
			classFile = new StringBuilder();
		}

		public void SaveToFile(string filePath)
		{
			if (classFile != null)
			{
				using (var file = new StreamWriter(filePath, false))
				{
					file.Write(classFile.ToString());
				}
			}
		}

		public override int VisitPrograma([NotNull] PortugolParser.ProgramaContext context)
		{
			string nomeClasse = context.ID().GetText();
			classFile.Append($"public class {nomeClasse}{{ {Environment.NewLine}{Identacao.Metodo} public static void main(String[] args){{ {Environment.NewLine} ");
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
			classFile.Append($"{Identacao.CorpoMetodo}");

			switch (tipo)
			{
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

			for (int i = 1; i < r; i++)
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
