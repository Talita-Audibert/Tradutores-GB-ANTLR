using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using System;
using System.IO;
using System.Text;
using Antlr4.Runtime.Tree;

namespace Portugol_Java
{
	public static class Identacao
	{
		public const string Metodo = "\t";
		public const string CorpoMetodo = "\t\t";
		public const string CorpoIf = "\t\t\t";
	}
	public class PortugolVisitor : PortugolBaseVisitor<int>
	{
		StringBuilder classFile;
		string chavesFinais = "\t\tentrada.close(); \n \t } \n }";

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
					file.Write(classFile.ToString() + chavesFinais);
				}
			}
		}

		public override int VisitPrograma([NotNull] PortugolParser.ProgramaContext context)
		{
			string nomeClasse = context.ID().GetText();
			classFile.Append($"public class {nomeClasse}{{ {Environment.NewLine}{Identacao.Metodo} public static void main(String[] args){{ {Environment.NewLine} ");

			//sempre instancia uma variavel de IO mesmo se não é usado.
			//Se for instanciado no visitor das funções é sempre criado uma instancia nova
			classFile.Append($"{ Identacao.CorpoMetodo}Scanner entrada = new Scanner(System.in);{Environment.NewLine}");
			return base.VisitPrograma(context);
		}

		public override int VisitDecFuncoes([NotNull] PortugolParser.DecFuncoesContext context)
		{
			string metodos = context.ID().ToString();
			var metodo = context.parent.parent == null;

			if (metodo)
			{
				switch (metodos)
				{
					case "escreval":
						if (true)
						{
							string nomeSaida = "";
							if (context.listaPar() != null)
							{
								nomeSaida = context.listaPar().ID(0).ToString();
							}
							classFile.Append($"{Identacao.CorpoMetodo}System.out.println({nomeSaida});{Environment.NewLine}");
						}
						break;

					case "escreva":
						string texto = context.listaPar().STRING(0).ToString();
						classFile.Append($"{Identacao.CorpoMetodo}System.out.println({texto});{Environment.NewLine}");
						break;

					case "leia":
						string nomeVar = context.listaPar().ID(0).ToString();
						classFile.Append($"{Identacao.CorpoMetodo}{nomeVar} = entrada.next();{Environment.NewLine}");
						break;
				}
			}

			return base.VisitDecFuncoes(context);
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
					classFile.AppendFormat("boolean {0}", variavel);
					break;
				case "LOGICO":
					classFile.AppendFormat("boolean {0}", variavel);
					break;
				case "STRING":
					classFile.AppendFormat("String {0}", variavel);
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

		public override int VisitBlocos([NotNull] PortugolParser.BlocosContext context)
		{
			string bloco = context.GetText();
			if (bloco.StartsWith("ESCOLHA"))
			{
				string varSwitch = context.@switch().valor(0).GetText();
				classFile.Append($"{Identacao.CorpoMetodo}switch({varSwitch}){{{Environment.NewLine}");
				string caso;
				string sttm;
				for (int i = 1; i < context.@switch().valor().Length; i++)
				{
					caso = context.@switch().valor(i).GetText();
					classFile.Append($"{Identacao.CorpoMetodo}case{caso}:{Environment.NewLine}");

					sttm = context.@switch().statement(i - 1).GetText();

					if (sttm.StartsWith("SE"))
					{
						string expr = context.@switch().statement(i - 1).blocos().@if().expression().GetText();
						classFile.Append($"{Identacao.CorpoMetodo}if({expr}){{{Environment.NewLine}");

						int sttmCount = context.@switch().statement(i - 1).blocos().@if().statement().Length;
						for (int j = 0; j < sttmCount; j++)
						{
							string statement = context.@switch().statement(i - 1).blocos().@if().statement(j).decFuncoes().ID().ToString();
							switch (statement)
							{
								case "escreval":
									classFile.Append($"{Identacao.CorpoIf}System.out.println();{Environment.NewLine}");
									break;

								case "escreva":
									string texto = context.@switch().statement(i - 1).blocos().@if().statement(j).decFuncoes().listaPar().STRING(0).GetText();
									classFile.Append($"{Identacao.CorpoIf}System.out.println({texto});{Environment.NewLine}");
									break;

								case "leia":
									string nomeVar = context.@if().statement(0).decFuncoes().listaPar().GetText();
									classFile.Append($"{Identacao.CorpoIf}{nomeVar} = entrada.next();{Environment.NewLine}");
									break;

								default:
									classFile.Append($"{Identacao.CorpoIf}{statement};{Environment.NewLine}");
									break;
							}

						}
						classFile.Append($"{Identacao.CorpoIf}}}{Environment.NewLine}");

						int elseCount = context.@switch().statement(i - 1).blocos().@if().@else().statement().Length;
						for (int k = 0; k < elseCount; k++)
						{
							string elseStatement = context.@switch().statement(i - 1).blocos().@if().@else().statement(k).GetText();
							classFile.Append($"{Identacao.CorpoMetodo}else{{{Environment.NewLine}{Identacao.CorpoIf}{elseStatement};{Environment.NewLine}{Identacao.CorpoMetodo}}}{Environment.NewLine}");
						}

					}
					else
					{
						classFile.Append($"{Identacao.CorpoIf}{sttm};{Environment.NewLine}");
					}
					classFile.Append($"{Identacao.CorpoMetodo}break;{Environment.NewLine}");
				}
				classFile.Append($"{Identacao.CorpoMetodo}}}{Environment.NewLine}");
			}

			else if (bloco.StartsWith("SE") && (context.parent.parent.IsEmpty))
			{

				string expr = context.@if().expression().GetText();
				classFile.Append($"{Identacao.CorpoMetodo}if({expr}){{{Environment.NewLine}");

				int sttmCount = context.@if().statement().Length;
				for (int j = 0; j < sttmCount; j++)
				{
					string statement = context.@if().statement(j).GetText();
					classFile.Append($"{Identacao.CorpoIf}{statement};{Environment.NewLine}");
				}
				classFile.Append($"{Identacao.CorpoMetodo}}}{Environment.NewLine}");

				int elseCount = context.@if().@else().statement().Length;
				for (int k = 0; k < elseCount; k++)
				{
					string elseStatement = context.@if().@else().statement(k).GetText();
					classFile.Append($"{Identacao.CorpoMetodo}else{{{Environment.NewLine}{Identacao.CorpoIf}{elseStatement};{Environment.NewLine}{Identacao.CorpoMetodo}}}{Environment.NewLine}");
				}
			}

			return base.VisitBlocos(context);
		}
	}
}