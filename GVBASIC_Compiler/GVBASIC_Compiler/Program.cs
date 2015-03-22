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

            // tokenizer 
            Tokenizer tok = new Tokenizer(sourceCode);

            printTokens(tok);  //<<<<<<<<<<<<<<< for debug

            // parser 
            Parser parser = new Parser(tok);
            // generate the IR
            //parser.Parsing();

            // run 
            Runtime rt = new Runtime( parser );
            rt.Run();

            // wait for key press 
            System.Console.ReadKey();
        }


        //--------------------------- private functions -------------------------- 

        /// <summary>
        /// print tokenizer 
        /// </summary>
        /// <param name="tokenizer"></param>
        static protected void printTokens( Tokenizer tok )
        {
            tok.Reset();

            int lineNum = 1;
            Token t = tok.GetNextToken();
            // parse to tokens 
            while( t.m_type != TokenType.eEOF )
            {
                // end of line 
                if (t.m_type == TokenType.eEOL)
                {
                    System.Console.Write("\n");
                    lineNum++;
                }
                // error handler 
                else if (t.m_type == TokenType.eError)
                {
                    System.Console.Write("Error char \'" + t.m_strVal + "\' in line " + lineNum + "\n");

                    break;
                }
                else
                {
                    if (t.m_type == TokenType.eIntNum)
                    {
                        System.Console.Write(t.m_intVal + " ");
                    }
                    else if (t.m_type == TokenType.eRealNum)
                    {
                        System.Console.Write(t.m_realVal + " ");
                    }
                    else if( t.m_type == TokenType.eSymbol )
                    {
                        System.Console.Write(t.m_strVal + " ");
                    }
                    else if (t.m_type == TokenType.eUndefine)
                    {
                        System.Console.Write("[error]");
                    }
                    else
                    {
                        System.Console.Write(t.m_type.ToString() + " ");
                    }
                }

                t = tok.GetNextToken();
            }
        }
    }
}
