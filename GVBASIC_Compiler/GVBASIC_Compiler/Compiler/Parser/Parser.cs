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
            while( tokenizer.IsFinish() == false )
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
            // sort the codelines
            m_codeLines.Sort((CodeLine lineA, CodeLine lineB) => { return lineA.m_lineNum - lineB.m_lineNum; });
            // init the info 
            m_lineIndex = 0;
            m_tokenIndex = 0;

            // do parsing 
            while (m_lineIndex < m_codeLines.Count)
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

                nextLine();
            }
        }


        //----------------------------- private functions ----------------------------


        /// <summary>
        /// Looks the ahead. 
        /// </summary>
        /// <returns>The ahead.</returns>
        protected TokenType lookAhead( int step = 0 )
        {
            if (m_lineIndex >= m_codeLines.Count)
                return TokenType.eNull;

            CodeLine cl = m_codeLines[m_lineIndex];
            int index = m_tokenIndex + step;

            if (index < cl.m_tokenCount)
            {
                return cl.m_tokens[index].m_type;
            }

            return TokenType.eNull;
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
        /// Eats the token.
        /// </summary>
        /// <returns><c>true</c>, if token was eaten, <c>false</c> otherwise.</returns>
        /// <param name="tok">Tok.</param>
        protected void eatToken( TokenType tok )
        {
            Token t = m_codeLines[m_lineIndex].m_tokens[m_tokenIndex];

            if (t.m_type != tok)
            {
                throw new Exception("[Parse]: eatToken, " + tok.ToString() + " is missiong.");
            }

            m_tokenIndex++;
        }

        /// <summary>
        /// get next token 
        /// </summary>
        /// <returns>The next token.</returns>
        protected Token getNextToken()
        {
            Token t = null;

            t = m_codeLines[m_lineIndex].m_tokens[m_tokenIndex];
            m_tokenIndex++;

            return t;
        }

        /// <summary>
        /// Eats the statement.
        /// </summary>
        /// <returns>The statement.</returns>
        protected void statement()
        {
            Statement s = null;

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
            Statement s = new Statement( eStatementType.eAssigment );

            Token tok = getNextToken();

            if (tok.m_type == TokenType.eSymbol)
            {
                s.m_symbol = tok;
            }
            else
            {
                throw new Exception("[Parse]: eatAssign, missing symbol.");
            }

            eatToken(TokenType.eEqual);

            //s.m_express = 
            express();
        }

        /// <summary>
        /// parse express 
        /// </summary>
        /// <returns></returns>
        protected void express()
        {
            Express e = null;

            Token tok = getNextToken();

            switch( tok.m_type )
            {
                default:
                    break;
            }
        }

        /// <summary>
        /// eat a func 
        /// </summary>
        /// <returns></returns>
        protected void func()
        {
            //TODO 
        }

    }
}
