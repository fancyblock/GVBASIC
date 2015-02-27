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
        protected List<CodeLine> m_codeLines;
        protected int m_lineIndex;
        protected int m_tokenIndex;

        protected List<Statement> m_statements;

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

            // add the last line 
            if (tokenBuff.Count > 0)
            {
                m_codeLines.Add(new CodeLine(tokenBuff));
            }
        }

        /// <summary>
        /// do parse
        /// </summary>
        public void DoParse()
        {
            sortCodeLines();

            m_lineIndex = 0;
            m_tokenIndex = 0;

            m_statements = new List<Statement>();

            while (isProgramDone() == false)
            {
                m_statements.Add(eatStatement());
            }
        }


        //----------------------------- private functions ----------------------------


        /// <summary>
        /// sort code lines 
        /// </summary>
        protected void sortCodeLines()
        {
            if( m_codeLines == null )
            {
                throw new Exception("[Parse]: no codelines.");
            }

            // sort the codelines
            m_codeLines.Sort((CodeLine lineA, CodeLine lineB) => { return lineA.m_lineNum - lineB.m_lineNum; });
        }

        /// <summary>
        /// Looks the ahead.
        /// </summary>
        /// <returns>The ahead.</returns>
        protected Token lookAhead()
        {
            Token t = null;

            if (m_lineIndex < m_codeLines.Count)
            {
                CodeLine cl = m_codeLines[m_lineIndex];

                if (m_tokenIndex < cl.m_tokenCount)
                {
                    t = cl.m_tokens[m_tokenIndex];
                }
            }

            return null;
        }

        /// <summary>
        /// if program is done or not 
        /// </summary>
        /// <returns><c>true</c>, if program done was ised, <c>false</c> otherwise.</returns>
        protected bool isProgramDone()
        {
            if (m_lineIndex < m_codeLines.Count)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Nexts the line.
        /// </summary>
        protected void nextLine()
        {
            m_lineIndex++;
            m_tokenIndex = 0;
        }

        /// <summary>
        /// Eats the statement.
        /// </summary>
        /// <returns>The statement.</returns>
        protected Statement eatStatement()
        {
            Statement s = null;

            Token tok = lookAhead();

            switch (tok.m_type)
            {
                case TokenType.eSymbol:
                    //TODO 
                    break;
                default:
                    throw new Exception("[Parse]: eatStatement error.");
                    break;
            }

            return s;
        }

    }
}
