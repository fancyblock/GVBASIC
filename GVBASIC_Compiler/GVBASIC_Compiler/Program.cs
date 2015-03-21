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
			{
				path = args [0];
			} 
			else 
			{
				path = "../../../../Bas/tank.bas";
			}
            System.Console.WriteLine("Run file: " + path);

            // read the source
            string sourceCode = System.IO.File.ReadAllText(path);

            // lex 
            Tokenizer tok = new Tokenizer(sourceCode);

            // for debug 
            printTokens(tok);
            tok.Reset();

            // parser 
            Parser parser = new Parser(tok);

            // generate the AST 
            parser.DoParse();

            // run 
            Runtime rt = new Runtime();
            rt.SetParser(parser);

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
            List<Token> tokenList = new List<Token>();
            int lineNum = 1;

            tok.Reset();

            // parse to tokens 
            while (tok.IsFinish() == false)
            {
                Token t = tok.GetNextToken();

                // skip the rem 
                if (t.m_type == TokenType.eRem)
                {
                    tok.SkipToNextLine();
                    lineNum++;
                }
                // end of line 
                else if (t.m_type == TokenType.eEOL)
                {
                    System.Console.Write("\n");
                    lineNum++;
                }
                // error handler 
                else if (t.m_type == TokenType.eError)
                {
                    System.Console.Write("Error char \'" + t.m_strVal + "\' in line " + lineNum + "\n");
                    tok.SkipToNextLine();
                    lineNum++;

                    break;
                }
                else
                {
                    tokenList.Add(t);

                    if (t.m_type == TokenType.eIntNum)
                    {
                        System.Console.Write(t.m_intVal + " ");
                    }
                    else if (t.m_type == TokenType.eRealNum)
                    {
                        System.Console.Write(t.m_realVal + " ");
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
            }
        }
    }
}
