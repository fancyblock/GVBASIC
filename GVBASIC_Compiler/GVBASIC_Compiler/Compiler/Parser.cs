using System;
using System.Collections.Generic;
using System.Text;

namespace GVBASIC_Compiler.Compiler
{
    /// <summary>
    /// parser 
    /// </summary>
    class Parser
    {
        protected List<CodeLine> m_codeLines = null;

        /// <summary>
        /// constructor 
        /// </summary>
        /// <param name="tokenizer"></param>
        public Parser( Tokenizer tokenizer )
        {
            tokenizer.Reset();
            m_codeLines = new List<CodeLine>();

            //TODO 
        }

        /// <summary>
        /// sort code lines 
        /// </summary>
        public bool SortCodeLines()
        {
            if( m_codeLines != null )
            {
                return false;
            }

            //TODO 

            return true;
        }

        /// <summary>
        /// scan line labels 
        /// </summary>
        public void ScanLabels()
        {
            //TODO 
        }

    }
}
