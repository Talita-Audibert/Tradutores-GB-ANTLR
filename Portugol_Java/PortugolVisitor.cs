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
        int contadorLinhas = 0;

        public PortugolVisitor()
        {
            classFile = new StringBuilder();
        }

        public string ChavesFinais(int contador)
        {
            var auxiliar = "";

            for (int i = 0; i < contador; i++)
            {
                auxiliar += Environment.NewLine + "}";
            }

            return auxiliar;

        }

        public void SaveToFile(string filePath)
        {
            if (classFile != null)
            {
                using (var file = new StreamWriter(filePath, false))
                {
                    file.Write(classFile.ToString()+ChavesFinais(contadorLinhas));
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

        //In progress...
        public override int VisitBlocos([NotNull] PortugolParser.BlocosContext context)
        {
            string blocos = context.GetText();
            int tst = context.ChildCount;

            if (blocos.Contains("ESCOLHA"))
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
                    if (sttm.Contains("SE"))
                    {
                        classFile.Append($"{Identacao.CorpoMetodo}if(){{{Environment.NewLine}");
                    }
                    else
                    {
                        classFile.Append($"{Identacao.CorpoMetodo}{sttm}{Environment.NewLine}");
                    }
                    classFile.Append($"{Identacao.CorpoMetodo}break;{Environment.NewLine}");
                }


            }



            return base.VisitBlocos(context);
        }


    }
}
