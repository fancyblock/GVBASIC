using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GVBASIC_Compiler.Compiler;
using GVBASIC_Compiler;

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
                "40 D% = 17.1                           \n" +
                "50 EF% = 20.1                          \n" +
                "60 PRINT EF%                           \n" +
                "70 PRINT A                             \n" +
                "80 PRINT C$                            \n" +
                "90 PRINT A + B% + C$ + D%              \n";

            TestHelper.RunProgram(sourceCode);
        }
    }
}
