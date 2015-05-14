using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GVBASIC_Compiler.Compiler;

namespace CompilerUnitTest
{
    [TestClass]
    public class TokenizerTest
    {
        [TestMethod]
        public void Tokenizer()
        {
            string sourceCode =
            "testSymbol #1 123 17.0 \"hjb\" \r\n"   +
            "+-*/^"                                 +
            "=>< >=<=<>"                            +
            "AND OR NOT"                            +
            ";,:()"                                 +
            "LET DIM READ DATA RESTORE GOTO "       +
            "IF THEN ELSE FOR NEXT "                +
            "WHILE WEND TO STEP "                   +
            "DEF FN GOSUB RETURN ON "               +
            "REM this is comment \r\n"              +             // REM will be skip , replace with a EOL
            "PRINT OPEN CLOSE FIELD "               +
            "GET LSET PUT RSET WRITE "              +
            "INPUT INKEY$ "                         +
            ""                                      +    //TODO 
            "@";

            int[] typeList = new int[] 
            {
            Token.SYMBOL, Token.FILE_NUM, Token.INT, Token.FLOAT, Token.STRING, Token.EOL,
            Token.PLUS, Token.MINUS, Token.MUL, Token.DIV, Token.POWER,
            Token.EQUAL, Token.GTR, Token.LT, Token.GTE, Token.LTE, Token.NEG,
            Token.AND, Token.OR, Token.NOT,
            Token.SEMI, Token.COMMA, Token.COLON, Token.LEFT_BRA, Token.RIGHT_BRA,
            Token.LET, Token.DIM, Token.READ, Token.DATA, Token.RESTORE, Token.GOTO,
            Token.IF, Token.THEN, Token.ELSE, Token.FOR, Token.NEXT,
            Token.WHILE, Token.WEND, Token.TO, Token.STEP,
            Token.DEF, Token.FN, Token.GOSUB, Token.RETURN, Token.ON, 
            Token.EOL,
            Token.PRINT, Token.OPEN, Token.CLOSE, Token.FIELD,
            Token.GET, Token.LSET, Token.PUT, Token.RSET, Token.WRITE,
            Token.INPUT, Token.INKEY,
            //TODO 
            Token.ERROR, Token.FILE_END
            };

            Tokenizer tokenizer = new Tokenizer( sourceCode );

            for (int i = 0; i < typeList.Length; i++ )
            {
                Token tok = tokenizer.GetToken();

                if (tok.m_type != typeList[i])
                    throw new Exception("Token type incorrect. Must be " + typeList[i].ToString());
            }
        }


        [TestMethod]
        public void ShowTokens()
        {
            string path = "../../../../Bas/_tank.bas";
            // read the source
            string sourceCode = System.IO.File.ReadAllText(path);

            // tokenizer 
            Tokenizer tok = new Tokenizer(sourceCode);

            printTokens(tok);
        }


        /// <summary>
        /// print tokenizer 
        /// </summary>
        /// <param name="tokenizer"></param>
        protected void printTokens(Tokenizer tok)
        {
            tok.Reset();

            int lineNum = 1;
            Token t = tok.GetToken();
            // parse to tokens 
            while (t.m_type != Token.FILE_END)
            {
                // end of line 
                if (t.m_type == Token.EOL)
                {
                    System.Console.Write("\n");
                    lineNum++;
                }
                // error handler 
                else if (t.m_type == Token.ERROR)
                {
                    throw new Exception("Error token in line " + lineNum);
                }
                else
                {
                    if (t.m_type == Token.INT)
                        System.Console.Write(t.m_intVal + " ");
                    else if (t.m_type == Token.FLOAT)
                        System.Console.Write(t.m_floatVal + " ");
                    else if (t.m_type == Token.SYMBOL)
                        System.Console.Write(t.m_strVal + " ");
                    else
                        System.Console.Write(t.m_type.ToString() + " ");
                }

                t = tok.GetToken();
            }
        }

    }
}
