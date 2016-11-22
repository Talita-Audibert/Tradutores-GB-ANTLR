﻿using Antlr4.Runtime;
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
		int contadorLinhas = 0;

		public PortugolVisitor()
		{
			classFile = new StringBuilder();
		}

		public string ChavesFinais(int contador)
		{
            string auxiliar = "";
            for (int i = 0; i < contador; i++)
            {
                auxiliar += "}";
            }

            return auxiliar;	
		}

		public void SaveToFile(string filePath)
		{
			if (classFile != null)
			{
				using (var file = new StreamWriter(filePath, false))
				{
					file.Write(classFile.ToString() + ChavesFinais(contadorLinhas));
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
			contadorLinhas += 2;
			return base.VisitPrograma(context);
		}

		public override int VisitDecFuncoes([NotNull] PortugolParser.DecFuncoesContext context)
		{
			string metodos = context.ID().ToString();

			switch (metodos)
			{
				case "escreval":
					classFile.Append($"{Identacao.CorpoMetodo}System.out.println();{Environment.NewLine}");
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

		public override int VisitBlocos([NotNull] PortugolParser.BlocosContext context)
		{
			string bloco = context.GetText();
			if (bloco.StartsWith("ESCOLHA"))
			{
				string varSwitch = context.@switch().valor(0).GetText();
				classFile.Append($"{Identacao.CorpoMetodo}switch({varSwitch}){{{Environment.NewLine}");
                contadorLinhas++;
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
							string statement = context.@switch().statement(i - 1).blocos().@if().statement(j).GetText();
							classFile.Append($"{Identacao.CorpoIf}{statement};{Environment.NewLine}");
						}
						classFile.Append($"{Identacao.CorpoIf}}}{Environment.NewLine}");

						int elseCount = context.@switch().statement(i - 1).blocos().@if().@else().statement().Length;
						for (int k = 0; k < elseCount; k++)
						{
							string elseStatement = context.@switch().statement(i - 1).blocos().@if().@else().statement(k).GetText();
							classFile.Append($"{Identacao.CorpoMetodo}else{{{elseStatement};{Environment.NewLine}}}{Environment.NewLine}");
						}

					}
					else
					{
						classFile.Append($"{Identacao.CorpoIf}{sttm};{Environment.NewLine}");
					}
					classFile.Append($"{Identacao.CorpoMetodo}break;{Environment.NewLine}");
				}
			}

			else if (bloco.StartsWith("SE"))
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