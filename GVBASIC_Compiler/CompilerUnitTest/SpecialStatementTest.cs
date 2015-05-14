using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GVBASIC_Compiler;
using GVBASIC_Compiler.Compiler;

namespace CompilerUnitTest
{
    [TestClass]
    public class SpecialStatementTest
    {
        [TestMethod]
        public void PrintTest1()
        {
            string sourceCode =
                "10 PRINT 10                                \n" +
                "20 PRINT \"HEJIABIN\"                      \n" +
                "30 PRINT 27.3                              \n" +
                "40 PRINT \"-------------\"                 \n" +
                "50 PRINT 1,2,3,4,5                         \n" +
                "60 PRINT 2;4;6;8;10;                       \n" +
                "70 PRINT \"TAIL\"                          \n" +
                "80 PRINT -1,-77,-23.8                      \n";

            Tokenizer tokenizer = new Tokenizer(sourceCode);
            Parser parser = new Parser(tokenizer);

            parser.Parsing();

            Runtime rt = new Runtime(parser);
            rt.SetAPI(new DebugAPI());
            rt.Run();
        }

        [TestMethod]
        public void PrintTest2()
        {
            string sourceCode =
                "10 PRINT 1 + 1                                \n" +
                "20 PRINT \"HE\" + \"JIABIN\"                  \n";

            Tokenizer tokenizer = new Tokenizer(sourceCode);
            Parser parser = new Parser(tokenizer);

            parser.Parsing();

            Runtime rt = new Runtime(parser);
            rt.SetAPI(new DebugAPI());
            rt.Run();
        }

    }
}
