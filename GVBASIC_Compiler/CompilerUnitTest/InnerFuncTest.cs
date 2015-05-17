using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CompilerUnitTest
{
    [TestClass]
    public class InnerFuncTest
    {
        [TestMethod]
        public void MathFuncions()
        {
            string sourceCode =
                "10 A = 1                               \n" +
                "20 B = -2                              \n" +
                "30 C = 3.7                             \n" +
                "40 D = -4.9                            \n" +
                "50 PRINT ABS(A),ABS(B),ABS(C),ABS(D)   \n";

            TestHelper.RunProgram(sourceCode);
        }

        [TestMethod]
        public void RandomGenerate()
        {
            string sourceCode =
                "10 A = RND(1)                          \n" +
                "20 PRINT A, RND(1)                     \n";

            TestHelper.RunProgram(sourceCode);
        }
    }
}
