using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;


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

            TestHelper.RunProgram(sourceCode);
        }

        [TestMethod]
        public void PrintTest2()
        {
            string sourceCode =
                "10 PRINT 1 + 1                                \n" +
                "20 PRINT \"HE\" + \"JIABIN\"                  \n";

            TestHelper.RunProgram(sourceCode);
        }

    }
}
