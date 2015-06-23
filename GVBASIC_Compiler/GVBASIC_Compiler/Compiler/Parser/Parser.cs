using System;
using System.Collections.Generic;

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
        protected int m_curLineNumber = 10;

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
            while (lookAhead() != Token.FILE_END)
            {
                // line number
                Token tok = eatToken(Token.INT);
                m_curLineNumber = tok.m_intVal;

                List<Statement> statementList = statements();
                foreach( Statement s in statementList )
                {
                    s.m_lineNum = m_curLineNumber;
                    m_statements.Add(s);
                }

                // filter the end of line 
                while (lookAhead() == Token.EOL)
                    eatToken(Token.EOL);
            }
        }

        /// <summary>
        /// getter of all the statements 
        /// </summary>
        public List<Statement> STATEMENTS { get { return m_statements; } }

        /// <summary>
        /// getter of the current line number
        /// </summary>
        public int CUR_LINE_NUM { get { return m_curLineNumber; } }



        /// <summary>
        /// Looks the ahead. 
        /// </summary>
        /// <returns>The ahead.</returns>
        protected int lookAhead( int step = 0 )
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
        protected Token eatToken( int tok )
        {
            Token curTok = m_tokenBuff[m_curTokenIndex];

            // token type error exception 
            if (curTok.m_type != tok)
                throw new ErrorCode(ErrorCode.ERROR_CODE_02);

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
        protected List<Statement> statements()
        {
            List<Statement> statements = new List<Statement>();

            while(true)
            {
                int tt = lookAhead();
                Statement ss = null;

                switch (tt)
                {
                    case Token.PRINT:
                        ss = print();
                        break;
                    case Token.LET:
                        ss = assignLet();
                        break;
                    case Token.SYMBOL:
                        ss = assign();
                        break;
                    case Token.DATA:
                        ss = data();
                        break;
                    case Token.READ:
                        ss = read();
                        break;
                    case Token.RESTORE:
                        ss = restore();
                        break;
                    case Token.IF:
                        ss = ifStatement();
                        break;
                    case Token.FOR:
                        ss = forBegin();
                        break;
                    case Token.NEXT:
                        ss = forEnd();
                        break;
                    case Token.WHILE:
                        ss = whileStatement();
                        break;
                    case Token.WEND:
                        ss = wend();
                        break;
                    case Token.GOTO:
                        ss = gotoStatement();
                        break;
                    case Token.ON:
                        ss = onStatement();
                        break;
                    case Token.GOSUB:
                        ss = gosubStatement();
                        break;
                    case Token.RETURN:
                        ss = returnStatement();
                        break;
                    case Token.POP:
                        ss = popStatement();
                        break;
                    case Token.DIM:
                        ss = dimStatement();
                        break;
                    case Token.DEF:
                        ss = defFnStatement();
                        break;
                    case Token.SWAP:
                        ss = swapStatement();
                        break;
                    case Token.SIMPLE_CMD:
                        ss = simpleCommand();
                        break;
                    case Token.PARAM_CMD:
                        ss = paramCommand();
                        break;
                    case Token.END:
                        ss = endStatement();
                        break;
                    default:
                        throw new ErrorCode(ErrorCode.ERROR_CODE_02);
                }

                // process the ss and count the INKEY$
                countInkey(ss);

                statements.Add(ss);

                if( lookAhead() == Token.COLON )
                    eatToken(Token.COLON);
                else
                    break;
            }

            return statements;
        }

        /// <summary>
        /// 计算该语句中INKEY$的调用次数
        /// </summary>
        /// <param name="statement"></param>
        protected void countInkey( Statement statement )
        {
            int cnt = 0;

            if (statement.m_exp != null)
                cnt += expInkeyCount(statement.m_exp);

            if (statement.m_expressList != null)
            {
                foreach (Expression exp in statement.m_expressList)
                    cnt += expInkeyCount(exp);
            }

            // 统计if语句中确定部分的INKEY$
            if( statement.m_statements != null )
            {
                foreach (Statement s in statement.m_statements)
                {
                    countInkey(s);
                    cnt += s.m_inkeyCnt;
                }
            }

            // 统计if语句中else部分的INKEY$
            if( statement.m_elseStatements != null )
            {
                foreach( Statement s in statement.m_elseStatements )
                {
                    countInkey(s);
                    cnt += s.m_inkeyCnt;
                }
            }

            statement.m_inkeyCnt = cnt;
        }

        protected int expInkeyCount( Expression exp )
        {
            if (exp.m_type == Expression.EXP_INKEY)
                return 1;

            int cnt = 0;

            if (exp.m_leftExp != null)
                cnt += expInkeyCount(exp.m_leftExp);

            if (exp.m_rightExp != null)
                cnt += expInkeyCount(exp.m_rightExp);

            if( exp.m_funcParams != null )
            {
                foreach (Expression e in exp.m_funcParams)
                    cnt += expInkeyCount(e);
            }

            return cnt;
        }

        /// <summary>
        /// goto statement 
        /// </summary>
        /// <returns></returns>
        protected Statement gotoStatement()
        {
            Statement s = new Statement(Statement.TYPE_GOTO);

            eatToken(Token.GOTO);

            Token tok = eatToken(Token.INT);
            s.m_intVal = tok.m_intVal;

            return s;
        }

        /// <summary>
        /// read statement 
        /// </summary>
        /// <returns></returns>
        protected Statement read()
        {
            Statement s = new Statement(Statement.TYPE_READ);

            eatToken(Token.READ);
            s.m_symbolList = new List<string>();

            // read the first 
            Token tok = eatToken(Token.SYMBOL);
            s.m_symbolList.Add(tok.m_strVal);

            int t = lookAhead();
            while( t == Token.COMMA )
            {
                eatToken(Token.COMMA);
                tok = eatToken(Token.SYMBOL);

                s.m_symbolList.Add(tok.m_strVal);

                t = lookAhead();
            }

            return s;
        }

        /// <summary>
        /// restore the data area pointer
        /// </summary>
        /// <returns></returns>
        protected Statement restore()
        {
            Statement s = new Statement(Statement.TYPE_RESTORE);
            eatToken(Token.RESTORE);

            return s;
        }

        /// <summary>
        /// data statement 
        /// </summary>
        /// <returns></returns>
        protected Statement data()
        {
            Statement s = new Statement(Statement.TYPE_DATA);

            eatToken(Token.DATA);
            s.m_dataList = new List<BaseData>();

            while(true)
            {
                int t = lookAhead();
                Token tok = eatToken(t);

                if (t == Token.INT)
                    s.m_dataList.Add(new BaseData(tok.m_intVal));
                else if (t == Token.FLOAT)
                    s.m_dataList.Add(new BaseData(tok.m_floatVal));
                else if (t == Token.STRING)
                    s.m_dataList.Add(new BaseData(tok.m_strVal));
                else
                    throw new ErrorCode(ErrorCode.ERROR_CODE_02);

                t = lookAhead();

                if (t != Token.COMMA)
                    break;
                else
                    eatToken(Token.COMMA);
            }

            return s;
        }

        /// <summary>
        /// if statement ( including if...then && if...goto )
        /// </summary>
        /// <returns></returns>
        protected Statement ifStatement()
        {
            Statement ifS = new Statement(Statement.TYPE_IF);

            eatToken(Token.IF);
            ifS.m_expressList = new List<Expression>();
            ifS.m_statements = new List<Statement>();

            ifS.m_expressList.Add(expression());

            int tt = lookAhead();
            if (tt != Token.THEN && tt != Token.GOTO)
                throw new ErrorCode(ErrorCode.ERROR_CODE_02);

            eatToken(tt);

            Token tok = null;
            List<Statement> s = new List<Statement>();
            Statement ss = null;

            if( lookAhead() == Token.INT )
            {
                tok = eatToken(Token.INT);

                ss = new Statement(Statement.TYPE_GOTO);
                ss.m_intVal = tok.m_intVal;
                s.Add(ss);
            }
            else
            {
                if (tt == Token.GOTO)
                    throw new ErrorCode(ErrorCode.ERROR_CODE_02);

                s = statements();
            }

            ifS.m_statements = s;

            // else case 
            if (lookAhead() == Token.ELSE)
            {
                eatToken( Token.ELSE );

                s = new List<Statement>();

                if (lookAhead() == Token.INT)
                {
                    tok = eatToken(Token.INT);

                    ss = new Statement(Statement.TYPE_GOTO);
                    ss.m_intVal = tok.m_intVal;
                    s.Add(ss);
                }
                else
                {
                    s = statements();
                }

                ifS.m_elseStatements = s;
            }
            else
            {
                ifS.m_elseStatements = new List<Statement>();
            }

            return ifS;
        }

        /// <summary>
        /// for statement 
        /// </summary>
        /// <returns></returns>
        protected Statement forBegin()
        {
            Statement forS = new Statement(Statement.TYPE_FOR_BEGIN);
            forS.m_expressList = new List<Expression>();

            // keyword
            eatToken(Token.FOR);
            // loop var 
            Token tok = eatToken(Token.SYMBOL);
            string symbolName = tok.m_strVal;
            forS.m_symbol = symbolName;

            if (symbolName.EndsWith("%"))
                throw new ErrorCode(ErrorCode.ERROR_CODE_02);
            else if (symbolName.EndsWith("$"))
                throw new ErrorCode(ErrorCode.ERROR_CODE_12);

            // equal 
            eatToken(Token.EQUAL);

            // start number 
            forS.m_expressList.Add(expression());

            // to 
            eatToken(Token.TO);

            // end number 
            forS.m_expressList.Add(expression());

            // if has step or not ? 
            int t = lookAhead();
            if( t == Token.STEP )
            {
                eatToken(Token.STEP);

                forS.m_expressList.Add(expression());
            }
            else
            {
                // default step 
                Expression step = new Expression(new BaseData(1));
                forS.m_expressList.Add(step);
            }

            return forS;
        }

        /// <summary>
        /// next statement 
        /// </summary>
        /// <returns></returns>
        protected Statement forEnd()
        {
            Statement next = new Statement(Statement.TYPE_FOR_END);

            eatToken(Token.NEXT);
            int t = lookAhead();

            if( t == Token.SYMBOL )
            {
                Token tok = eatToken( Token.SYMBOL );
                string symbolName = tok.m_strVal;

                if (symbolName.EndsWith("%"))
                    throw new ErrorCode(ErrorCode.ERROR_CODE_02);
                else if (symbolName.EndsWith("$"))
                    throw new ErrorCode(ErrorCode.ERROR_CODE_12);

                next.m_symbol = symbolName;
            }

            return next;
        }

        /// <summary>
        /// while statement 
        /// </summary>
        /// <returns></returns>
        protected Statement whileStatement()
        {
            Statement whileStatement = new Statement(Statement.TYPE_WHILE_BEGIN);

            eatToken(Token.WHILE);

            Expression exp = expression();
            whileStatement.m_expressList = new List<Expression>();
            whileStatement.m_expressList.Add(exp);

            return whileStatement;
        }

        /// <summary>
        /// wend statement 
        /// </summary>
        /// <returns></returns>
        protected Statement wend()
        {
            Statement wend = new Statement(Statement.TYPE_WHILE_END);
            eatToken(Token.WEND);

            return wend;
        }

        /// <summary>
        /// on statement 
        /// </summary>
        /// <returns></returns>
        protected Statement onStatement()
        {
            Statement on = new Statement( Statement.TYPE_ON_GOTO );

            eatToken( Token.ON );
            on.m_expressList = new List<Expression>();
            on.m_expressList.Add(expression());

            eatToken(Token.GOTO);

            on.m_dataList = new List<BaseData>();

            while (true)
            {
                int tt = lookAhead();
                Token tok = eatToken(tt);

                if( tok.m_type == Token.INT )
                    on.m_dataList.Add(new BaseData(tok.m_intVal));
                else if( tok.m_type == Token.FLOAT )
                    on.m_dataList.Add(new BaseData((int)tok.m_floatVal));
                else
                    throw new ErrorCode(ErrorCode.ERROR_CODE_07);

                tt = lookAhead();
                if (tt == Token.COMMA)
                    eatToken(Token.COMMA);
                else
                    break;
            }

            return on;
        }
        
        /// <summary>
        /// GOSUB statement 
        /// </summary>
        /// <returns></returns>
        protected Statement gosubStatement()
        {
            Statement gosub = new Statement( Statement.TYPE_GOSUB );

            eatToken( Token.GOSUB );

            int tt = lookAhead();
            Token tok = eatToken(tt);

            if( tt == Token.INT )
                gosub.m_intVal = tok.m_intVal;
            else if( tt == Token.FLOAT)
                gosub.m_intVal = (int)tok.m_floatVal;
            else
                throw new ErrorCode(ErrorCode.ERROR_CODE_07);

            return gosub;
        }

        /// <summary>
        /// return statment 
        /// </summary>
        /// <returns></returns>
        protected Statement returnStatement()
        {
            Statement returnStatement = new Statement( Statement.TYPE_RETURN );
            eatToken( Token.RETURN );

            return returnStatement;
        }

        /// <summary>
        /// pop statement 
        /// </summary>
        /// <returns></returns>
        protected Statement popStatement()
        {
            Statement popStatement = new Statement( Statement.TYPE_POP );
            eatToken( Token.POP );

            return popStatement;
        }

        protected Statement dimStatement()
        {
            Statement dimStatement = new Statement(Statement.TYPE_DIM);
            eatToken(Token.DIM);

            Token tok = eatToken(Token.SYMBOL);
            dimStatement.m_symbol = tok.m_strVal;

            eatToken(Token.LEFT_PAREN);
            dimStatement.m_arrayDimension = numberList();
            eatToken(Token.RIGHT_PAREN);

            return dimStatement;
        }

        protected Statement defFnStatement()
        {
            Statement fnStatement = new Statement(Statement.TYPE_DEF_FN);

            eatToken(Token.DEF);
            eatToken(Token.FN);

            Token tok = eatToken(Token.SYMBOL);

            if (tok.m_strVal.EndsWith("%"))
                throw new ErrorCode(ErrorCode.ERROR_CODE_02);
            else if (tok.m_strVal.EndsWith("$"))
                throw new ErrorCode(ErrorCode.ERROR_CODE_12);

            fnStatement.m_symbol = tok.m_strVal;

            eatToken(Token.LEFT_PAREN);
            tok = eatToken(Token.SYMBOL);

            if (tok.m_strVal.EndsWith("%"))
                throw new ErrorCode(ErrorCode.ERROR_CODE_02);
            else if (tok.m_strVal.EndsWith("$"))
                throw new ErrorCode(ErrorCode.ERROR_CODE_12);

            string paramSymbol = tok.m_strVal;

            eatToken(Token.RIGHT_PAREN);
            eatToken(Token.EQUAL);

            fnStatement.m_expressList = new List<Expression>();
            Expression fnExp = expression();
            // 用一个通用参数名代替公式中出现的参数符号名
            replaceSymbolName(fnExp, paramSymbol, CommonDef.FN_PARAM_SYMBOL);

            fnStatement.m_expressList.Add(fnExp);

            return fnStatement;
        }

        /// <summary>
        /// swap two vars
        /// </summary>
        /// <returns></returns>
        protected Statement swapStatement()
        {
            Statement s = new Statement(Statement.TYPE_SWAP);

            eatToken(Token.SWAP);
            s.m_expressList = new List<Expression>();

            s.m_expressList.Add(expression());
            eatToken(Token.COMMA);
            s.m_expressList.Add(expression());

            return s;
        }

        /// <summary>
        /// normal assignment 
        /// </summary>
        /// <returns></returns>
        protected Statement assign()
        {
            Statement s = new Statement(Statement.TYPE_ASSIGN);

            Token t = eatToken(Token.SYMBOL);

            if (lookAhead() == Token.LEFT_PAREN)
            {
                eatToken(Token.LEFT_PAREN);
                s.m_expressList = expressList();
                eatToken(Token.RIGHT_PAREN);
            }

            eatToken(Token.EQUAL);

            s.m_symbol = t.m_strVal;
            s.m_exp = expression();

            return s;
        }

        /// <summary>
        /// assignment by let keyword
        /// </summary>
        /// <returns></returns>
        protected Statement assignLet()
        {
            Statement s = new Statement(Statement.TYPE_ASSIGN);

            eatToken(Token.LET);

            Token t = eatToken(Token.SYMBOL);

            if (lookAhead() == Token.LEFT_PAREN)
            {
                eatToken(Token.LEFT_PAREN);
                s.m_expressList = expressList();
                eatToken(Token.RIGHT_PAREN);
            }

            eatToken(Token.EQUAL);

            s.m_symbol = t.m_strVal;
            s.m_exp = expression();

            return s;
        }

        /// <summary>
        /// parse print statement 
        /// </summary>
        protected Statement print()
        {
            Statement s = new Statement(Statement.TYPE_PRINT);
            s.m_expressList = new List<Expression>();

            eatToken(Token.PRINT);

            while (true)
            {
                int tok = lookAhead();

                if (tok == Token.SEMI)
                {
                    // print close to prior exp  
                    eatToken(Token.SEMI);
                    s.m_expressList.Add(new Expression(Expression.TYPE_CLOSE_TO));
                }
                else if (tok == Token.COMMA)
                {
                    // print next line 
                    eatToken(Token.COMMA);
                    s.m_expressList.Add(new Expression(Expression.TYPE_NEXT_LINE));
                }
                else if( tok == Token.COLON || tok == Token.EOL || tok == Token.FILE_END || tok == Token.ELSE )
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

            int tt = lookAhead();
            while( tt == Token.AND || tt == Token.OR )
            {
                Expression subExp = null;

                eatToken(tt);

                if (tt == Token.AND)
                    subExp = new Expression(Expression.OP_AND);
                else if (tt == Token.OR)
                    subExp = new Expression(Expression.OP_OR);

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

            int tt = lookAhead();

            // NOT at the first 
            while( tt == Token.NOT )
            {
                eatToken(tt);
                subExp = new Expression(Expression.OP_NOT);

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

            int tt = lookAhead();
            while( tt == Token.EQUAL || tt == Token.GTR || tt == Token.LT || tt == Token.GTE || tt == Token.LTE )
            {
                Expression subExp = null;

                eatToken( tt );

                if( tt == Token.EQUAL )
                    subExp = new Expression(Expression.OP_EQUAL);
                else if( tt == Token.GTR )
                    subExp = new Expression(Expression.OP_GREATER);
                else if( tt == Token.GTE)
                    subExp = new Expression(Expression.OP_GREATER_EQU);
                else if( tt == Token.LT)
                    subExp = new Expression(Expression.OP_LESS);
                else if( tt == Token.LTE)
                    subExp = new Expression(Expression.OP_LESS_EQ);

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

            int tt = lookAhead();

            while (tt == Token.PLUS || tt == Token.MINUS)
            {
                Expression subExp = null;

                eatToken(tt);
                if (tt == Token.PLUS)
                    subExp = new Expression(Expression.OP_ADD);
                else if (tt == Token.MINUS)
                    subExp = new Expression(Expression.OP_MINUS);

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

            int tt = lookAhead();

            while (tt == Token.MUL || tt == Token.DIV)
            {
                Expression subExp = null;

                eatToken(tt);
                if (tt == Token.MUL)
                    subExp = new Expression(Expression.OP_MUL);
                else if (tt == Token.DIV)
                    subExp = new Expression(Expression.OP_DIV);

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

            int tt = lookAhead();

            while( tt == Token.POWER )
            {
                eatToken(Token.POWER);
                Expression subExp = new Expression(Expression.OP_POWER);

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

            int tt = lookAhead();
            Token tok = null;

            if (tt == Token.SYMBOL)
            {
                if (lookAhead(1) == Token.LEFT_PAREN)
                {
                    // array 
                    exp = new Expression(Expression.EXP_ARRAY_SYMBOL);
                    tok = eatToken(Token.SYMBOL);
                    exp.m_symbolName = tok.m_strVal;
                    eatToken(Token.LEFT_PAREN);
                    exp.m_funcParams = expressList();
                    eatToken(Token.RIGHT_PAREN);
                }
                else
                {
                    exp = new Expression(Expression.EXP_SYMBOL);
                    tok = eatToken(Token.SYMBOL);
                    exp.m_symbolName = tok.m_strVal;
                }
            }
            else if (tt == Token.INT)
            {
                tok = eatToken(Token.INT);
                exp = new Expression(new BaseData(tok.m_intVal));
            }
            else if (tt == Token.FLOAT)
            {
                tok = eatToken(Token.FLOAT);
                exp = new Expression(new BaseData(tok.m_floatVal));
            }
            else if (tt == Token.LEFT_PAREN)
            {
                eatToken(Token.LEFT_PAREN);
                exp = expression();
                eatToken(Token.RIGHT_PAREN);
            }
            else if( tt == Token.MINUS)
            {
                exp = new Expression(Expression.OP_NEG);
                eatToken(Token.MINUS);
                exp.m_leftExp = expression7();
            }
            else if( tt == Token.FUNC )
            {
                exp = new Expression(Expression.EXP_FUNC);
                exp.m_funcParams = new List<Expression>();
                // function name
                tok = eatToken(Token.FUNC);
                exp.m_symbolName = tok.m_strVal;
                // function parameters 
                eatToken(Token.LEFT_PAREN);
                exp.m_funcParams.Add( expression() );
                while( lookAhead() == Token.COMMA )
                {
                    eatToken(Token.COMMA);
                    exp.m_funcParams.Add(expression());
                }
                eatToken(Token.RIGHT_PAREN);
            }
            else if(tt == Token.STRING)
            {
                tok = eatToken(Token.STRING);
                exp = new Expression(new BaseData(tok.m_strVal));
            }
            else if( tt == Token.FN)
            {
                exp = new Expression(Expression.EXP_USER_FUNC);
                eatToken(Token.FN);
                tok = eatToken(Token.SYMBOL);
                exp.m_symbolName = tok.m_strVal;

                eatToken(Token.LEFT_PAREN);
                exp.m_funcParams = new List<Expression>();
                exp.m_funcParams.Add(expression());
                eatToken(Token.RIGHT_PAREN);
            }
            else if( tt == Token.INKEY )
            {
                exp = new Expression(Expression.EXP_INKEY);
                eatToken(Token.INKEY);
            }

            return exp;
        }

        /// <summary>
        /// simple command 
        /// </summary>
        /// <returns></returns>
        protected Statement simpleCommand()
        {
            Statement s = new Statement(Statement.TYPE_SIMPLE_CMD);

            Token tok = eatToken(Token.SIMPLE_CMD);
            s.m_symbol = tok.m_strVal;

            return s;
        }

        /// <summary>
        /// param command 
        /// </summary>
        /// <returns></returns>
        protected Statement paramCommand()
        {
            Statement s = new Statement(Statement.TYPE_PARAM_CMD);

            Token tok = eatToken(Token.PARAM_CMD);
            s.m_symbol = tok.m_strVal;

            s.m_expressList = expressList();

            return s;
        }

        /// <summary>
        /// end statement 
        /// </summary>
        /// <returns></returns>
        protected Statement endStatement()
        {
            Statement s = new Statement(Statement.TYPE_END);
            eatToken(Token.END);

            return s;
        }

        /// <summary>
        /// number list like 1,2,3,4,5
        /// </summary>
        /// <returns></returns>
        protected List<int> numberList()
        {
            List<int> result = new List<int>();

            while (true)
            {
                BaseData bd;
                int tt = lookAhead();
                Token tok = eatToken(tt);

                if (tt == Token.INT)
                    bd = new BaseData(tok.m_intVal);
                else if (tt == Token.FLOAT)
                    bd = new BaseData(tok.m_floatVal);
                else
                    throw new ErrorCode(ErrorCode.ERROR_CODE_12);
                bd.Convert(BaseData.TYPE_INT);

                result.Add(bd.INT_VAL);

                if (lookAhead() == Token.COMMA)
                    eatToken(Token.COMMA);
                else
                    break;
            }

            return result;
        }

        /// <summary>
        /// expression list 
        /// </summary>
        /// <returns></returns>
        protected List<Expression> expressList()
        {
            List<Expression> expList = new List<Expression>();

            while (true)
            {
                expList.Add(expression());

                if (lookAhead() == Token.COMMA)
                    eatToken(Token.COMMA);
                else
                    break;
            }

            return expList;
        }

        /// <summary>
        /// replace symbol name in the expression 
        /// </summary>
        /// <param name="exp"></param>
        protected void replaceSymbolName( Expression exp, string oldName, string newName )
        {
            if ( exp.m_type == Expression.EXP_SYMBOL && exp.m_symbolName == oldName)
                exp.m_symbolName = newName;

            if( exp.m_funcParams != null)
            {
                foreach (Expression e in exp.m_funcParams)
                    replaceSymbolName(e, oldName, newName);
            }

            if (exp.m_leftExp != null)
                replaceSymbolName(exp.m_leftExp, oldName, newName);

            if (exp.m_rightExp != null)
                replaceSymbolName(exp.m_rightExp, oldName, newName);
        }

    }
}
