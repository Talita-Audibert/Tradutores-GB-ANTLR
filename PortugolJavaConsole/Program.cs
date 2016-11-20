using Antlr4.Runtime;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portugol_Java;
using Antlr4.Runtime.Tree;

namespace PortugolJavaConsole
{
	class Program
	{
		static void Main(string[] args)
		{
			StreamReader inputStream = new StreamReader(@"C:\Users\talia_000\Desktop\Trabalho_tradutores\Portugol_Java\Samples\Teste1.txt");
			AntlrInputStream input = new AntlrInputStream(inputStream.ReadToEnd());
			PortugolLexer lexer = new PortugolLexer(input);
			CommonTokenStream tokens = new CommonTokenStream(lexer);
			PortugolParser parser = new PortugolParser(tokens);
			IParseTree tree = parser.programa();
			Console.WriteLine(tree.ToStringTree(parser));
			PortugolVisitor visitor = new PortugolVisitor();
			visitor.Visit(tree);
			//Console.WriteLine(visitor.Visit(tree));

			Console.Read();
		}
	}
}
