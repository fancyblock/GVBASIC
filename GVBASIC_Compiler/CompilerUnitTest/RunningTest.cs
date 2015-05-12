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
    public class RunningTest
    {
        [TestMethod]
        public void DoAssign()
        {
            string sourceCode =
                "10 A = 1                               \n" +
                "20 B% = 2                              \n" +
                "30 C$ = \"HJB\"                        \n" +
                "40 D% = 17.1                           \n";

            Tokenizer tokenizer = new Tokenizer(sourceCode);
            Parser parser = new Parser(tokenizer);

            parser.Parsing();

            Runtime rt = new Runtime(parser);
            rt.Run();
        }
    }
}
