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

            // 10
            // HEJIABN
            // 27.3
            // ------------
            // 1
            // 2
            // 3
            // 4
            // 5
            // 246810TAIL
            // -1
            // -77
            // -23.8

            string result =
                "10" + "\n" +
                "HEJIABIN" + "\n" +
                "27.3" + "\n" +
                "-------------" + "\n" +
                "1" + "\n" +
                "2" + "\n" +
                "3" + "\n" +
                "4" + "\n" +
                "5" + "\n" +
                "246810TAIL" + "\n" +
                "-1" + "\n" +
                "-77" + "\n" +
                "-23.8" + "\n";

            TestHelper.TestProgram(sourceCode, result);
        }

        [TestMethod]
        public void PrintTest2()
        {
            string sourceCode =
                "10 PRINT 1 + 1                                \n" +
                "20 PRINT \"HE\" + \"JIABIN\"                  \n" +
                "30 A = 3                                      \n" +
                "40 PRINT A                                    \n";

            // 2
            // HEJIABIN
            // 3

            string result =
                "2" + "\n" +
                "HEJIABIN" + "\n" +
                "3" + "\n";

            TestHelper.TestProgram(sourceCode, result);
        }

    }
}
