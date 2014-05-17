using System;
using System.Collections.Generic;
using System.Text;

// tokenizer 
namespace GVBASIC_Compiler.Compiler
{
    /// <summary>
    /// status of the tokenizer 
    /// </summary>
    enum LexStatus
    {
        eStart,
        eSymbol,
        eSpace,
        eIntNum,
        eRealNum,
        eString,
        eOpCode,
        eDelim,         // delimiter 
        //TODO 
    };

    /// <summary>
    /// lexer 
    /// </summary>
    class Tokenizer
    {
        protected string m_sourceCode;
        protected int m_curIndex;
        protected LexStatus m_status;

        /// <summary>
        /// constructor 
        /// </summary>
        public Tokenizer()
        {
            m_sourceCode = null;
            m_buffer = new StringBuilder();
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
            m_curIndex = 0;
            m_status = LexStatus.eStart;
        }

        /// <summary>
        /// return the next token 
        /// </summary>
        /// <returns></returns>
        public Token GetNextToken()
        {
            Token tok = new Token();
            StringBuilder buffer = new StringBuilder();
            bool addToBuffer;

            for (; m_curIndex < m_sourceCode.Length; m_curIndex++ )
            {
                char c = m_sourceCode[m_curIndex];
                addToBuffer = true;

                switch( m_status )
                {
                    case LexStatus.eStart:
                        //TODO 
                        break;
                    default:
                        break;
                }

                //TODO 

                if( addToBuffer )
                {
                    buffer.Append(c);
                }
            }

            return tok;
        }

        /// <summary>
        /// skip characters to next line 
        /// </summary>
        public void SkipToNextLine()
        {
            for( ; m_curIndex < m_sourceCode.Length; m_curIndex++ )
            {
                char c = m_sourceCode[m_curIndex];

                if( c == '\n' )
                {
                    break;
                }
            }
        }


        /// <summary>
        /// if is finished or not 
        /// </summary>
        /// <returns></returns>
        public bool IsFinish()
        {
            if( m_curIndex >= m_sourceCode.Length )
            {
                return true;
            }

            return false;
        }

    }
}
