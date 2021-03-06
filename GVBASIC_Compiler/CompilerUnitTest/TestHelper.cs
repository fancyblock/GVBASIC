﻿using System;
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
        static public DebugAPI RunProgram( string code )
        {
            Tokenizer tokenizer = new Tokenizer(code);
            Parser parser = new Parser(tokenizer);
            Runtime rt = new Runtime(parser);

            DebugAPI api = new DebugAPI();
            rt.SetAPI(api);

            rt.Run();

            while (!rt.Step()) ;

            return api;
        }

        /// <summary>
        /// test program 
        /// </summary>
        /// <param name="code"></param>
        /// <param name="result"></param>
        static public void TestProgram( string code, string result )
        {
            DebugAPI api = RunProgram(code);

            if( api.OUT_PUT != result )
                throw new Exception("Result Error.");
        }

    }
}
