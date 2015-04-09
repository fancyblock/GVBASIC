using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GVBASIC_Compiler.Compiler;

namespace CompilerUnitTest
{
    [TestClass]
    public class TokenizerTest
    {
        [TestMethod]
        public void Tokenizer()
        {
            string sourceCode = "";     //[TEMP]

            Tokenizer tokenizer = new Tokenizer( sourceCode );

            //TODO 
        }
    }
}
