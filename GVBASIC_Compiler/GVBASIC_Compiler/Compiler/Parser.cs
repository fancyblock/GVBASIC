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
            List<Token> tokenBuff = new List<Token>();

            while( tokenizer.IsFinish == false )
            {
                Token t = tokenizer.GetNextToken();
                bool endLine = false;

                if( t.m_type == TokenType.eRem )
                {
                    tokenizer.SkipToNextLine();
                    endLine = true;
                }
                else if( t.m_type == TokenType.eEOL )
                {
                    endLine = true;
                }
                else if( t.m_type == TokenType.eError )
                {
                    // throw error , lex error.
                    //TODO 

                    //tokenizer.SkipToNextLine();
                    //endLine = true;
                }
                
                if( endLine )
                {
                    //TODO 
                }
                else
                {
                    tokenBuff.Add(t);
                }
            }
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
