using System;
using System.Collections.Generic;
using System.Text;

namespace GVBASIC_Compiler.Compiler
{
    /// <summary>
    /// parser 
    /// </summary>
    public class Parser
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
            // initial the token buffer 
            for (int i = 0; i < m_tokenBuff.Length; i++)
                m_tokenBuff[i] = m_tokenizer.GetToken();
            m_curTokenIndex = 0;

            m_statements = new List<Statement>();

            // do parsing 
            while (lookAhead() != TokenType.eEOF)
            {
                // line number
                Token tok = eatToken(TokenType.eIntNum);

                Statement s = statement();
                s.m_num = tok.m_intVal;
                m_statements.Add(s);

                // filter the end of line 
                while (lookAhead() == TokenType.eEOL)
                    eatToken(TokenType.eEOL);
            }

            // parse done 
        }

        /// <summary>
        /// getter of all the statements 
        /// </summary>
        public List<Statement> STATEMENTS { get { return m_statements; } }


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
        /// statement 
        /// </summary>
        /// <returns></returns>
        protected Statement statement()
        {
            Statement s = new Statement(StatementType.eStatementSet);
            s.m_statements = new List<Statement>();
            TokenType tt;

            while(true)
            {
                tt = lookAhead();
                Statement ss = null;

                switch (tt)
                {
                    case TokenType.ePrint:
                        ss = print();
                        break;
                    case TokenType.eLet:
                        ss = assignLet();
                        break;
                    case TokenType.eSymbol:
                        ss = assign();
                        break;
                    case TokenType.eData:
                        ss = data();
                        break;
                    case TokenType.eRead:
                        ss = read();
                        break;
                    case TokenType.eIf:
                        ss = ifStatement();
                        break;
                    case TokenType.eFor:
                        ss = forBegin();
                        break;
                    case TokenType.eNext:
                        ss = forEnd();
                        break;
                    case TokenType.eWhile:
                        ss = whileStatement();
                        break;
                    case TokenType.eWend:
                        ss = wend();
                        break;
                    case TokenType.eGoto:
                        ss = gotoStatement();
                        break;
                    default:
                        throw new Exception("unexpected token: " + tt.ToString());
                }

                s.m_statements.Add(ss);

                if( lookAhead() == TokenType.eColon )
                    eatToken(TokenType.eColon);
                else
                    break;
            }

            if (s.m_statements.Count == 1)
                s = s.m_statements[0];

            return s;
        }

        /// <summary>
        /// goto statement 
        /// </summary>
        /// <returns></returns>
        protected Statement gotoStatement()
        {
            Statement s = new Statement(StatementType.eGoto);

            eatToken(TokenType.eGoto);

            Token tok = eatToken(TokenType.eIntNum);
            s.m_intVal = tok.m_intVal;

            return s;
        }

        /// <summary>
        /// read statement 
        /// </summary>
        /// <returns></returns>
        protected Statement read()
        {
            Statement s = new Statement(StatementType.eRead);

            eatToken(TokenType.eRead);
            s.m_symbolList = new List<string>();

            Token tok = eatToken(TokenType.eSymbol);
            s.m_symbolList.Add(tok.m_strVal);

            TokenType t = lookAhead();
            while( t == TokenType.eComma)
            {
                eatToken(TokenType.eComma);
                tok = eatToken(TokenType.eSymbol);

                s.m_symbolList.Add(tok.m_strVal);
            }

            return s;
        }

        /// <summary>
        /// data statement 
        /// </summary>
        /// <returns></returns>
        protected Statement data()
        {
            Statement s = new Statement(StatementType.eData);

            eatToken(TokenType.eData);
            s.m_dataList = new List<BaseData>();

            TokenType t = lookAhead();
            Token tok = null;

            while(true)
            {
                tok = eatToken(t);

                if( t == TokenType.eIntNum )
                    s.m_dataList.Add(new BaseData(tok.m_intVal));
                else if( t == TokenType.eRealNum)
                    s.m_dataList.Add(new BaseData(tok.m_realVal));
                else if( t == TokenType.eString)
                    s.m_dataList.Add(new BaseData(tok.m_strVal));

                t = lookAhead();

                if (t != TokenType.eComma)
                    break;
                else
                    eatToken(TokenType.eComma);
            }

            return s;
        }

        /// <summary>
        /// if statement ( including if...then && if...goto )
        /// </summary>
        /// <returns></returns>
        protected Statement ifStatement()
        {
            Statement ifS = new Statement(StatementType.eIf);

            eatToken(TokenType.eIf);
            ifS.m_expressList = new List<Expression>();
            ifS.m_statements = new List<Statement>();

            ifS.m_expressList.Add(expression());

            TokenType tt = lookAhead();
            if( tt != TokenType.eThen && tt != TokenType.eGoto )
                throw new Exception("[Parser]: ifStatement, error token, then/goto expected.");

            eatToken(tt);

            Token tok = null;
            Statement s = null;

            if( lookAhead() == TokenType.eIntNum )
            {
                tok = eatToken(TokenType.eIntNum);

                s = new Statement(StatementType.eGoto);
                s.m_intVal = tok.m_intVal;
            }
            else
            {
                if (tt == TokenType.eGoto)
                    throw new Exception("[Parser]: ifStatement, error token, number expected.");

                s = statement();
            }

            ifS.m_statements.Add(s);

            // else case 
            if (lookAhead() == TokenType.eElse)
            {
                eatToken( TokenType.eElse );

                if (lookAhead() == TokenType.eIntNum)
                {
                    tok = eatToken(TokenType.eIntNum);

                    s = new Statement(StatementType.eGoto);
                    s.m_intVal = tok.m_intVal;
                }
                else
                {
                    s = statement();
                }

                ifS.m_statements.Add(s);
            }

            return ifS;
        }

        /// <summary>
        /// for statement 
        /// </summary>
        /// <returns></returns>
        protected Statement forBegin()
        {
            Statement forS = new Statement(StatementType.eForBegin);

            eatToken(TokenType.eFor);

            Token tok = eatToken(TokenType.eIntNum);
            //TODO 

            return forS;
        }

        /// <summary>
        /// next statement 
        /// </summary>
        /// <returns></returns>
        protected Statement forEnd()
        {
            Statement next = new Statement(StatementType.eForEnd);

            eatToken(TokenType.eNext);
            //TODO

            return next;
        }

        /// <summary>
        /// while statement 
        /// </summary>
        /// <returns></returns>
        protected Statement whileStatement()
        {
            Statement whileStatement = new Statement(StatementType.eWhileBegin);

            eatToken(TokenType.eWhile);
            //TODO 

            return whileStatement;
        }

        /// <summary>
        /// wend statement 
        /// </summary>
        /// <returns></returns>
        protected Statement wend()
        {
            Statement wend = new Statement(StatementType.eWhileEnd);

            eatToken(TokenType.eWend);
            //TODO 

            return wend;
        }

        /// <summary>
        /// normal assignment 
        /// </summary>
        /// <returns></returns>
        protected Statement assign()
        {
            Statement s = new Statement(StatementType.eAssign);

            Token t = eatToken(TokenType.eSymbol);
            eatToken(TokenType.eEqual);

            s.m_symbol = t.m_strVal;
            s.m_expressList = new List<Expression>() { expression() };

            return s;
        }

        /// <summary>
        /// assignment by let keyword
        /// </summary>
        /// <returns></returns>
        protected Statement assignLet()
        {
            Statement s = new Statement(StatementType.eAssign);
            s.m_type = StatementType.eAssign;

            eatToken(TokenType.eLet);

            Token t = eatToken(TokenType.eSymbol);
            eatToken(TokenType.eEqual);

            s.m_symbol = t.m_strVal;
            s.m_expressList = new List<Expression>() { expression() };

            return s;
        }

        /// <summary>
        /// parse print statement 
        /// </summary>
        protected Statement print()
        {
            Statement s = new Statement(StatementType.ePrint);
            s.m_type = StatementType.ePrint;
            s.m_expressList = new List<Expression>();

            eatToken(TokenType.ePrint);

            while (true)
            {
                TokenType tok = lookAhead();

                if (tok == TokenType.eSemi)
                {
                    // print close to prior exp  
                    eatToken(TokenType.eSemi);
                    continue;
                }
                else if (tok == TokenType.eComma)
                {
                    // print next line 
                    eatToken(TokenType.eComma);
                    s.m_expressList.Add(new Expression(ExpressionType.eString).SetText("\n"));
                }
                else if( tok == TokenType.eColon || tok == TokenType.eEOL || tok == TokenType.eEOF || tok == TokenType.eElse )
                {
                    // this statement end 
                    break;
                }
                else
                {
                    // expression 
                    s.m_expressList.Add(expression());
                }
            }

            return s;
        }

        /// <summary>
        /// parse expression
        /// </summary>
        /// <returns></returns>
        protected Expression expression()
        {
            Expression exp = expression2();

            TokenType tt = lookAhead();
            while( tt == TokenType.eAnd || tt == TokenType.eOr )
            {
                Expression subExp = null;

                eatToken(tt);

                if (tt == TokenType.eAnd)
                    subExp = new Expression(ExpressionType.eOpLogicAnd);
                else if (tt == TokenType.eOr)
                    subExp = new Expression(ExpressionType.eOpLogicOr);

                subExp.m_leftExp = exp;
                subExp.m_rightExp = expression2();

                exp = subExp;

                tt = lookAhead();
            }

            return exp;
        }
        
        protected Expression expression2()
        {
            Expression exp = null;
            Expression subExp = null;

            TokenType tt = lookAhead();

            // NOT at the first 
            while( tt == TokenType.eNot )
            {
                eatToken(tt);
                subExp = new Expression(ExpressionType.eOpLogicNot);

                if (exp != null)
                    subExp.m_leftExp = exp;

                exp = subExp;

                tt = lookAhead();
            }

            if (exp != null)
            {
                subExp = exp;
                while( subExp.m_leftExp != null )
                {
                    subExp = subExp.m_leftExp;
                }
                subExp.m_leftExp = expression3();
            }
            else
            {
                exp = expression3();
            }

            return exp;
        }

        protected Expression expression3()
        {
            Expression exp = expression4();

            TokenType tt = lookAhead();
            while( tt == TokenType.eEqual || tt == TokenType.eGtr || tt == TokenType.eLt || tt == TokenType.eGte || tt == TokenType.eLte )
            {
                Expression subExp = null;

                eatToken( tt );

                if( tt == TokenType.eEqual )
                    subExp = new Expression(ExpressionType.eOpCmpEqual);
                else if( tt == TokenType.eGtr )
                    subExp = new Expression(ExpressionType.eOpCmpGtr);
                else if( tt == TokenType.eGte)
                    subExp = new Expression(ExpressionType.eOpCmpGte);
                else if( tt == TokenType.eLt)
                    subExp = new Expression(ExpressionType.eOpCmpLtr);
                else if( tt == TokenType.eLte)
                    subExp = new Expression(ExpressionType.eOpCmpLte);

                subExp.m_leftExp = exp;
                subExp.m_rightExp = expression4();

                exp = subExp;

                tt = lookAhead();
            }

            return exp;
        }

        protected Expression expression4()
        {
            Expression exp = expression5();

            TokenType tt = lookAhead();

            while (tt == TokenType.ePlus || tt == TokenType.eMinus)
            {
                Expression subExp = null;

                eatToken(tt);
                if (tt == TokenType.ePlus)
                    subExp = new Expression(ExpressionType.eOpPlus);
                else if (tt == TokenType.eMinus)
                    subExp = new Expression(ExpressionType.eOpMinus);

                subExp.m_leftExp = exp;
                subExp.m_rightExp = expression5();

                exp = subExp;

                tt = lookAhead();
            }

            return exp;
        }

        protected Expression expression5()
        {
            Expression exp = expression6();

            TokenType tt = lookAhead();

            while (tt == TokenType.eMul || tt == TokenType.eDiv)
            {
                Expression subExp = null;

                eatToken(tt);
                if (tt == TokenType.eMul)
                    subExp = new Expression(ExpressionType.eOpMul);
                else if (tt == TokenType.eDiv)
                    subExp = new Expression(ExpressionType.eOpDiv);

                subExp.m_leftExp = exp;
                subExp.m_rightExp = expression6();

                exp = subExp;

                tt = lookAhead();
            }

            return exp;
        }

        protected Expression expression6()
        {
            Expression exp = expression7();

            TokenType tt = lookAhead();

            while( tt == TokenType.ePower )
            {
                eatToken(TokenType.ePower);
                Expression subExp = new Expression(ExpressionType.eOpPower);

                subExp.m_leftExp = exp;
                subExp.m_rightExp = expression7();

                exp = subExp;

                tt = lookAhead();
            }

            return exp;
        }

        protected Expression expression7()
        {
            Expression exp = null;

            TokenType tt = lookAhead();
            Token tok = null;

            if (tt == TokenType.eSymbol)
            {
                exp = new Expression(ExpressionType.eSymbol);
                tok = eatToken(TokenType.eSymbol);
                exp.m_symbol = tok.m_strVal;
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
                exp.m_leftExp = expression7();
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
            else if(tt == TokenType.eString)
            {
                exp = new Expression(ExpressionType.eString);
                tok = eatToken(TokenType.eString);
                exp.m_text = tok.m_strVal;
            }
            else if( tt == TokenType.eFn)
            {
                exp = new Expression(ExpressionType.eUserFn);
                //TODO 
            }

            return exp;
        }

    }
}
