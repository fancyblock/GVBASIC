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
        protected Token[] m_tokenBuff;
        protected int m_curTokenIndex;

        /// <summary>
        /// constructor 
        /// </summary>
        /// <param name="tokenizer"></param>
        public Parser( Tokenizer tokenizer, int k = 10 )
        {
            m_tokenizer = tokenizer;
            m_tokenBuff = new Token[k];
        }

        /// <summary>
        /// do parse
        /// </summary>
        public void Parsing()
        {
            m_tokenizer.Reset();
            // init the token buffer 
            for (int i = 0; i < m_tokenBuff.Length; i++)
                m_tokenBuff[i] = m_tokenizer.GetToken();
            m_curTokenIndex = 0;

            // do parsing 
            TokenType tk = lookAhead();
            while (tk != TokenType.eEOF)
            {
                //TODO 
            }
        }


        //----------------------------- private functions ----------------------------


        /// <summary>
        /// Looks the ahead. 
        /// </summary>
        /// <returns>The ahead.</returns>
        protected TokenType lookAhead( int step = 0 )
        {
            if (step >= m_tokenBuff.Length)
                throw new Exception("Look ahead too more...");

            int index = m_curTokenIndex + step;
            index %= m_tokenBuff.Length;

            return m_tokenBuff[index].m_type;
        }


        /// <summary>
        /// Eats the token.
        /// </summary>
        /// <returns><c>true</c>, if token was eaten, <c>false</c> otherwise.</returns>
        /// <param name="tok">Tok.</param>
        protected void eatToken( TokenType tok )
        {
            Token curTok = m_tokenBuff[m_curTokenIndex];

            // token type error exception 
            if( curTok.m_type != tok )
                throw new Exception( "Error token type " + curTok.m_type.ToString() + ",  "
                    + tok.ToString() + " expected." );

            // add new token to the buffer
            m_tokenBuff[m_curTokenIndex] = m_tokenizer.GetToken();

            // update the token index 
            m_curTokenIndex++;
            m_curTokenIndex %= m_tokenBuff.Length;
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
