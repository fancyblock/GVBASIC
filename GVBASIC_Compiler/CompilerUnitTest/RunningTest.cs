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
        public void AssignVar()
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

        [TestMethod]
        public void ReadData()
        {
            string sourceCode =
                "10 DATA 1,23,2.6,19.5                     \n" +
                "20 READ A,T,F% , R%                       \n" +
                "30 PRINT T,A,F%, R%                       \n";

            // 23
            // 1
            // 2
            // 19

            TestHelper.RunProgram(sourceCode);
        }

        [TestMethod]
        public void IfElse()
        {
            string sourceCode =
                "10 A = 1: B = 7                            \n" +
                "20 IF A > 0 THEN PRINT \"A>0\"             \n" +
                "30 IF B < 5 THEN PRINT \"B<5\"             \n" +
                "40 C = 110                                  \n" +
                "50 IF C < 20 GOTO 70 ELSE PRINT \"CCC\"    \n" +
                "60 PRINT \"THIS IS 60\"                    \n" +
                "70 PRINT \"THIS IS 70\"                    \n" +
                "80 END                                     \n" +
                "90 PRINT 117                               ";

            TestHelper.RunProgram(sourceCode);
        }

    }
}
