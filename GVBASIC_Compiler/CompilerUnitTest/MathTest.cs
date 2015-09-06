using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CompilerUnitTest
{
    [TestClass]
    public class MathTest
    {
        [TestMethod]
        public void SGNTest()
        {
            string code =
                "10 PRINT SGN(5)                                    \n" +
                "20 PRINT SGN(-9)                                   \n" +
                "30 PRINT SGN(0)                                    \n";

            string result =
                "1" + "\n" +
                "-1" + "\n" +
                "0" + "\n";

            TestHelper.TestProgram(code, result);
        }

        [TestMethod]
        public void INTTest()
        {
            string code =
                "10 PRINT INT(5)                                    \n" +
                "20 PRINT INT(-9)                                   \n" +
                "30 PRINT INT(0)                                    \n" +
                "40 PRINT INT(3.8)                                  \n" +
                "50 PRINT INT(-3.8)                                 \n";

            string result =
                "5" + "\n" +
                "-9" + "\n" +
                "0" + "\n" +
                "3" + "\n" +
                "-4" + "\n";

            TestHelper.TestProgram(code, result);
        }

        [TestMethod]
        public void SinTest()
        {
            string code =
                "10 PRINT SIN(5)                                    \n" +
                "20 PRINT SIN(-9)                                   \n" +
                "30 PRINT SIN(0)                                    \n" +
                "40 PRINT SIN(3.8)                                  \n" +
                "50 PRINT SIN(-3.8)                                 \n";

            string result =
                "-0.9589243" + "\n" +
                "-0.4121185" + "\n" +
                "0" + "\n" +
                "-0.6118578" + "\n" +
                "0.6118578" + "\n";

            TestHelper.TestProgram(code, result);
        }

        [TestMethod]
        public void AtnTest()
        {
            string code =
                "10 PRINT ATN(5)                                    \n" +
                "20 PRINT ATN(-9)                                   \n" +
                "30 PRINT ATN(0)                                    \n" +
                "40 PRINT ATN(3.8)                                  \n" +
                "50 PRINT ATN(-3.8)                                 \n";

            string result =
                "1.373401" + "\n" +
                "-1.460139" + "\n" +
                "0" + "\n" +
                "1.313473" + "\n" +
                "-1.313473" + "\n";

            TestHelper.TestProgram(code, result);
        }

        [TestMethod]
        public void SqrTest()
        {
            string code =
                "10 PRINT SQR(5)                                    \n" +
                "30 PRINT SQR(0)                                    \n" +
                "40 PRINT SQR(3.8)                                  \n";

            string result =
                "2.236068" + "\n" +
                "0" + "\n" +
                "1.949359" + "\n";

            TestHelper.TestProgram(code, result);
        }

        [TestMethod]
        public void ExpTest()
        {
            string code =
                "10 PRINT EXP(5)                                    \n" +
                "20 PRINT EXP(-9)                                   \n" +
                "30 PRINT EXP(0)                                    \n" +
                "40 PRINT EXP(3.8)                                  \n" +
                "50 PRINT EXP(-3.8)                                 \n";

            string result =
                "148.4132" + "\n" +
                "0.0001234098" + "\n" +
                "1" + "\n" +
                "44.70118" + "\n" +
                "0.02237077" + "\n";

            TestHelper.TestProgram(code, result);
        }

        [TestMethod]
        public void LogTest()
        {
            string code =
                "10 PRINT LOG(5)                                    \n" +
                "20 PRINT LOG(10)                                   \n" +
                "30 PRINT LOG(0)                                    \n" +
                "40 PRINT LOG(3.8)                                  \n" +
                "50 PRINT LOG(2.71828)                                 \n";

            string result =
                "148.4132" + "\n" +
                "0.0001234098" + "\n" +
                "1" + "\n" +
                "44.70118" + "\n" +
                "0.02237077" + "\n";

            TestHelper.TestProgram(code, result);
        }
    }
}
