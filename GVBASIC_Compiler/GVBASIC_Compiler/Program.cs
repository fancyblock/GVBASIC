using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GVBASIC_Compiler.Compiler;


namespace GVBASIC_Compiler
{
    class Program
    {
        static void Main(string[] args)
        {
			string path = null;
			if (args.Length > 0) 
				path = args [0];
			else 
				path = "../../../../Bas/_tank.bas";
            // print the file path 
            System.Console.WriteLine("Run file: " + path);

            // read the source
            string sourceCode = System.IO.File.ReadAllText(path);

            // lex  
            Tokenizer tok = new Tokenizer(sourceCode);

            // parser 
            Parser parser = new Parser(tok);
            // generate the IR
            parser.Parsing();

            // run 
            Runtime rt = new Runtime(parser);
            rt.SetAPI(new DebugAPI());
            rt.Run();

            // wait for key press 
            System.Console.ReadKey();
        }
    }
}
