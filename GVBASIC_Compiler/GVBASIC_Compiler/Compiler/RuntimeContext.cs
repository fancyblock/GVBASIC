using System;

namespace GVBASIC_Compiler.Compiler
{
    /// <summary>
    /// Runtime context.
    /// </summary>
    public class RuntimeContext
    {
        public int CURRENT_CODE_LINE { get; set; }
        public int CURRENT_TOKEN_INDEX { get; set; }

        /// <summary>
        /// constructor 
        /// </summary>
        public RuntimeContext()
        {
            Reset();
        }

        /// <summary>
        /// Reset this instance.
        /// </summary>
        public void Reset()
        {
            CURRENT_CODE_LINE = 0;
            CURRENT_TOKEN_INDEX = 0;
        }

    }
}

