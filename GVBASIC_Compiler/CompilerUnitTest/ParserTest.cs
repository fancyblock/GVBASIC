using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GVBASIC_Compiler.Compiler;

namespace CompilerUnitTest
{
    [TestClass]
    public class ParserTest
    {
        [TestMethod]
        public void ParserExpAdd()
        {
            string sourceCode =
                "10 A = 1 + 1                           \n" +
                "20 A = A + 1                           \n" +
                "30 A = 1 + A                           \n" +
                "40 A = A + A                           \n" +
                "50 A = A + B                           \n" +
                "60 A = \"TT\" + \"FF\"                 \n" +
                "70 A = 7 + B : B = 1 + 9               \n" +
                "80 A = 1 + 1 + 2 + A:B = 11 + 9";

            Tokenizer tokenizer = new Tokenizer(sourceCode);
            Parser parser = new Parser(tokenizer);

            parser.Parsing();
            
            // check statement
            List<Statement> statements = parser.STATEMENTS;

            if (statements.Count != 8)
                throw new Exception("statement count incorrect.");

            //TODO 
        }

        [TestMethod]
        public void ParserExpCollection()
        {
            string sourceCode =
                "10 A = 1 + 1 - 2                       \n" +
                "20 A = A + 1 * 7                       \n" +
                "30 A = 1 + A / 8 - ( 11 + 2 )          \n" +
                "40 A = -(A + A) * C                    \n" +
                "50 A = A - C * 1 + B                   \n" +
                "60 A = \"TT\" + \"FF\"                 \n" +
                "70 A = 7 + B : B = 1 + 9               \n" +
                "80 A = 1 + 1 + 2 + A:B = 11 + 9";

            Tokenizer tokenizer = new Tokenizer(sourceCode);
            Parser parser = new Parser(tokenizer);

            parser.Parsing();

            // check statement
            List<Statement> statements = parser.STATEMENTS;

            if (statements.Count != 8)
                throw new Exception("statement count incorrect.");

            //TODO 
        }

        [TestMethod]
        public void ParserIf()
        {
            string sourceCode =
                @"10 IF A>1 THEN PRINT C
                  20 IF C=2 THEN PRINT 11 ELSE PRINT 22
                  30 IF B<=1 GOTO 13
                  40 IF E>0 GOTO 11 ELSE 70
                  50 IF F < 6 AND G > 8 THEN PRINT theStr:C=17 ELSE 70";

            Tokenizer tokenizer = new Tokenizer(sourceCode);
            Parser parser = new Parser(tokenizer);

            parser.Parsing();

            // check statement
            List<Statement> statements = parser.STATEMENTS;

            if (statements.Count != 5)
                throw new Exception("statement count incorrect.");
            
            //TODO 

        }
    }
}
