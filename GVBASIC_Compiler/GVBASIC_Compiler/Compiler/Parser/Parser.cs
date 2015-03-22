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
        protected Tokenizer m_tokenizer;

        /// <summary>
        /// constructor 
        /// </summary>
        /// <param name="tokenizer"></param>
        public Parser( Tokenizer tokenizer )
        {
            m_tokenizer = tokenizer;
        }

        /// <summary>
        /// do parse
        /// </summary>
        public void Parsing()
        {
            m_tokenizer.Reset();

            // do parsing 
            Token tk = m_tokenizer.GetNextToken();
            while (tk.m_type != TokenType.eEOF)
            {
                while ( lookAhead() != TokenType.eNull )
                {
                    //TODO 
                    TokenType token = lookAhead();

                    if (token != TokenType.eNull)
                    {
                        eatToken(TokenType.eColon);
                    }
                }
            }
        }


        //----------------------------- private functions ----------------------------


        /// <summary>
        /// Looks the ahead. 
        /// </summary>
        /// <returns>The ahead.</returns>
        protected TokenType lookAhead( int step = 0 )
        {
            /*
            if (m_lineIndex >= m_codeLines.Count)
                return TokenType.eNull;

            CodeLine cl = m_codeLines[m_lineIndex];
            int index = m_tokenIndex + step;

            if (index < cl.m_tokenCount)
            {
                return cl.m_tokens[index].m_type;
            }
             */

            return TokenType.eNull;
        }


        /// <summary>
        /// Eats the token.
        /// </summary>
        /// <returns><c>true</c>, if token was eaten, <c>false</c> otherwise.</returns>
        /// <param name="tok">Tok.</param>
        protected void eatToken( TokenType tok )
        {
            /*
            Token t = m_codeLines[m_lineIndex].m_tokens[m_tokenIndex];

            if (t.m_type != tok)
            {
                throw new Exception("[Parse]: eatToken, " + tok.ToString() + " is missiong.");
            }

            m_tokenIndex++;
             */
        }

        /// <summary>
        /// Eats the statement.
        /// </summary>
        /// <returns>The statement.</returns>
        protected void statement()
        {
            switch (lookAhead())
            {
                case TokenType.eSymbol:
                    assign();
                    break;
                case TokenType.eLet:
                    eatToken(TokenType.eLet);
                    assign();
                    break;
                default:
                    throw new Exception("[Parse]: eatStatement error.");
            }
        }

        /// <summary>
        /// eat assignment 
        /// </summary>
        /// <returns>The assign.</returns>
        protected void assign()
        {
            Token tok = null;// getNextToken();

            if (tok.m_type == TokenType.eSymbol)
            {
                //s.m_symbol = tok;
            }
            else
            {
                throw new Exception("[Parse]: eatAssign, missing symbol.");
            }

            eatToken(TokenType.eEqual);

            express();
        }

        /// <summary>
        /// parse express 
        /// </summary>
        /// <returns></returns>
        protected void express()
        {
            Token tok = null;// getNextToken();

            switch( tok.m_type )
            {
                default:
                    break;
            }
        }

    }
}
