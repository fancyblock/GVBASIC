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
            // print the arguments
            System.Console.WriteLine("[Command line arguments]");
            foreach (string str in args)
            {
                System.Console.WriteLine(str);
            }

            string path = args[0];

            // read the source
            string sourceCode = System.IO.File.ReadAllText(path);

            List<Token> tokenList = new List<Token>();

            // compiler code 
            Tokenizer tok = new Tokenizer();
            tok.SetSource(sourceCode);
            tok.Reset();

            // parse to tokens 
            while( tok.IsFinish() == false )
            {
                Token t = tok.GetNextToken();

                System.Console.Write(t.m_type.ToString() + ",");

                //TODO 

                if( t.m_type == TokenType.eIntNum )
                {
                    //System.Console.Write(t.m_type.ToString() + ",");
                    //tok.SkipToNextLine();
                }
                else
                {
                    tokenList.Add(t);
                }
            }

            // grammer parse 
            //TODO 

            // wait for key press 
            System.Console.ReadKey();
        }
    }
}
