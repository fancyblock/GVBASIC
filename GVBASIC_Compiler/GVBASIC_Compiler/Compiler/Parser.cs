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

            // read all the tokens into codeline struct
            while( tokenizer.IsFinish == false )
            {
                Token t = tokenizer.GetNextToken();
                bool endLine = false;

                if( t.m_type == TokenType.eRem )        // filter the comment 
                {
                    tokenizer.SkipToNextLine();
                    endLine = true;
                }
                else if( t.m_type == TokenType.eEOL )   // omit end of line 
                {
                    endLine = true;
                }
                else if( t.m_type == TokenType.eError )
                {
                    // throw error , lex error.
                    throw new Exception("Error token in line " + tokenBuff[0].ToString() );
                }
                
                if( endLine )
                {
                    // save the line 
                    if (tokenBuff.Count > 0)
                    {
                        m_codeLines.Add(new CodeLine(tokenBuff));
                        tokenBuff.Clear();
                    }
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
            if( m_codeLines == null )
            {
                return false;
            }

            m_codeLines.Sort((CodeLine lineA, CodeLine lineB) => { return lineA.m_lineNum - lineB.m_lineNum; });

            return true;
        }

        /// <summary>
        /// scan line labels 
        /// </summary>
        public void ScanLabels()
        {
            //TODO 
        }


        //----------------------------- private functions ----------------------------

        //TODO 

    }
}
