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
                "10 A = 1 + 1" + "\r\n" +
                "20 A = A + 1" + "\r\n" +
                "30 A = 1 + A" + "\r\n" +
                "40 A = A + A" + "\r\n" +
                "50 A = A + B" + "\r\n" +
                "60 A = \"TT\" + \"FF\"" + "\r\n" +
                "70 A = 7 + B : B = 1 + 9";

            Tokenizer tokenizer = new Tokenizer(sourceCode);
            Parser parser = new Parser(tokenizer);

            parser.Parsing();
            
            // check statement
            List<Statement> statements = parser.STATEMENTS;

            if (statements.Count != 7)
                throw new Exception("statement count incorrect.");

            //TODO 
        }

        [TestMethod]
        public void Parser2()
        {
            //TODO 
        }
    }
}
