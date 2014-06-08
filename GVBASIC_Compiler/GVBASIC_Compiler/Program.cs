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
            int lineNum = 1;

            // parse to tokens 
            while( tok.IsFinish() == false )
            {
                Token t = tok.GetNextToken();

                if( t.m_type != TokenType.eEOL )
                {
                    tokenList.Add(t);

                    if( t.m_type == TokenType.eIntNum )
                    {
                        System.Console.Write(t.m_intVal + " ");
                    }
                    else if( t.m_type == TokenType.eRealNum )
                    {
                        System.Console.Write(t.m_realVal + " ");
                    }
                    else
                    {
                        System.Console.Write(t.m_type.ToString() + " ");
                    }
                }

                // skip the rem 
                if( t.m_type == TokenType.eRem )
                {
                    tok.SkipToNextLine();
                    lineNum++;
                }
                // end of line 
                else if( t.m_type == TokenType.eEOL )
                {
                    System.Console.Write("\n");
                    lineNum++;
                }
                // error handler 
                else if( t.m_type == TokenType.eError )
                {
                    System.Console.Write("Error char \'" + t.m_strVal + "\' in line " + lineNum + "\n" );
                    tok.SkipToNextLine();
                    lineNum++;

                    break;
                }
            }

            // grammer parse 
            //TODO 

            // wait for key press 
            System.Console.ReadKey();
        }
    }
}
