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

        protected List<Statement> m_statements;

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

            m_statements = new List<Statement>();

            // do parsing 
            while (lookAhead() != TokenType.eEOF)
            {
                // parse statement 
                Token t = eatToken(TokenType.eIntNum);

                TokenType tok = TokenType.eUndefine;

                do
                {
                    if (tok != TokenType.eUndefine)
                        eatToken(TokenType.eColon);

                    Statement s = null;
                    tok = lookAhead();

                    switch( tok )
                    {
                        case TokenType.ePrint:
                            s = print();
                            break;
                        //TODO 
                        default:
                            throw new Exception("unexpected token: " + tok.ToString());
                    }

                    // add the statement 
                    s.m_num = t.m_intVal;
                    m_statements.Add(s);

                } while (lookAhead() != TokenType.eEOL && lookAhead() != TokenType.eEOF);
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
        protected Token eatToken( TokenType tok )
        {
            Token curTok = m_tokenBuff[m_curTokenIndex];

            // token type error exception 
            if( curTok.m_type != tok )
                throw new Exception( "Error token type " + curTok.m_type.ToString() + ",  " + tok.ToString() + " expected." );

            // add new token to the buffer
            m_tokenBuff[m_curTokenIndex] = m_tokenizer.GetToken();

            // update the token index 
            m_curTokenIndex++;
            m_curTokenIndex %= m_tokenBuff.Length;

            return curTok;
        }


        /// <summary>
        /// parse print statement 
        /// </summary>
        protected Statement print()
        {
            Statement s = new Statement();
            s.m_type = StatementType.ePrint;
            s.m_expressList = new List<Expression>();

            eatToken(TokenType.ePrint);

            while (true)
            {
                s.m_expressList.Add(expression());

                TokenType tok = lookAhead();

                if (tok == TokenType.eSemi)
                {
                    // print close to prior exp  
                    //TODO 
                }
                else if (tok == TokenType.eComma)
                {
                    // print next line 
                    //TODO 
                }
                else
                {
                    break;
                }
            }

            return s;
        }
        

        /// <summary>
        /// parse express
        /// </summary>
        protected Expression expression()
        {
            Expression exp = expression2();

            TokenType tt = lookAhead();

            while (tt == TokenType.ePlus || tt == TokenType.eMinus)
            {
                Expression subExp = null;

                if (tt == TokenType.ePlus)
                {
                    eatToken(TokenType.ePlus);
                    subExp = new Expression(ExpressionType.eOpPlus);
                }
                else if (tt == TokenType.eMinus)
                { 
                    eatToken(TokenType.eMinus);
                    subExp = new Expression(ExpressionType.eOpMinus);
                }

                subExp.m_leftExp = exp;
                subExp.m_rightExp = expression2();

                exp = subExp;

                tt = lookAhead();
            }

            return exp;
        }

        protected Expression expression2()
        {
            Expression exp = expression3();

            TokenType tt = lookAhead();

            while (tt == TokenType.eMul || tt == TokenType.eDiv)
            {
                Expression subExp = null;

                if (tt == TokenType.eMul)
                {
                    eatToken(TokenType.eMul);
                    subExp = new Expression(ExpressionType.eOpMul);
                }
                else if (tt == TokenType.eDiv)
                {
                    eatToken(TokenType.eDiv);
                    subExp = new Expression(ExpressionType.eOpDiv);
                }

                subExp.m_leftExp = exp;
                subExp.m_rightExp = expression3();

                exp = subExp;

                tt = lookAhead();
            }

            return exp;
        }

        protected Expression expression3()
        {
            Expression exp = expression4();

            TokenType tt = lookAhead();

            if( tt == TokenType.ePower )
            {
                eatToken(TokenType.ePower);
                Expression subExp = new Expression(ExpressionType.eOpPower);

                subExp.m_leftExp = exp;
                subExp.m_rightExp = expression4();

                exp = subExp;
            }

            return exp;
        }

        protected Expression expression4()
        {
            Expression exp = null;

            TokenType tt = lookAhead();
            Token tok = null;

            if (tt == TokenType.eSymbol)
            {
                exp = new Expression(ExpressionType.eSymbol);
                tok = eatToken(TokenType.eSymbol);
                //TODO 
            }
            else if (tt == TokenType.eIntNum)
            {
                exp = new Expression(ExpressionType.eIntNum);
                tok = eatToken(TokenType.eIntNum);
                exp.m_intVal = tok.m_intVal;
            }
            else if (tt == TokenType.eRealNum)
            {
                exp = new Expression(ExpressionType.eRealNum);
                tok = eatToken(TokenType.eRealNum);
                exp.m_realVal = tok.m_realVal;
            }
            else if (tt == TokenType.eLeftBra)
            {
                eatToken(TokenType.eLeftBra);
                exp = expression();
                eatToken(TokenType.eRightBra);
            }
            else if( tt == TokenType.eMinus)
            {
                exp = new Expression(ExpressionType.eOpNeg);
                eatToken(TokenType.eMinus);
                exp.m_leftExp = expression4();
            }
            else if( tt == TokenType.eFunc )
            {
                exp = new Expression(ExpressionType.eFunc);
                //TODO 
                eatToken(TokenType.eFunc);
                eatToken(TokenType.eLeftBra);
                expression();
                eatToken(TokenType.eRightBra);
            }

            return exp;
        }

    }
}
