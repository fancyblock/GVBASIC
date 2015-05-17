using System;
using System.Collections.Generic;
using System.Text;
using GVBASIC_Compiler.Compiler;
using GVBASIC_Compiler;

namespace CompilerUnitTest
{
    public class TestHelper
    {
        /// <summary>
        /// run the program 
        /// </summary>
        /// <param name="code"></param>
        static public void RunProgram( string code )
        {
            Tokenizer tokenizer = new Tokenizer(code);
            Parser parser = new Parser(tokenizer);

            parser.Parsing();

            Runtime rt = new Runtime(parser);
            rt.SetAPI(new DebugAPI());
            rt.Run();
        }

    }
}
