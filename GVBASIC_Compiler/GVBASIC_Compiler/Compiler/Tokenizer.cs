using System;
using System.Collections.Generic;
using System.Text;

namespace GVBASIC_Compiler.Compiler
{
    /// <summary>
    /// lexer 
    /// </summary>
    class Tokenizer
    {
        protected string m_sourceCode;

        /// <summary>
        /// constructor 
        /// </summary>
        public Tokenizer()
        {
            m_sourceCode = null;
        }

        /// <summary>
        /// set source code 
        /// </summary>
        /// <param name="source"></param>
        public void SetSource( string source )
        {
            m_sourceCode = source;
        }

        /// <summary>
        /// reset the tokenizer 
        /// </summary>
        public void Reset()
        {
            //TODO 
        }
    }
}
