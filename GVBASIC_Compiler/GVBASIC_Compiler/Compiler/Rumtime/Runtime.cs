using System;
using System.Collections.Generic;

namespace GVBASIC_Compiler.Compiler
{
    /// <summary>
    /// Basic runtime 
    /// </summary>
    public class Runtime
    {
        protected Parser m_parser;

        protected List<Statement> m_statements;
        protected Dictionary<int, Action<Statement>> m_executer;
        protected Dictionary<int, int> m_lineNumDic;

        protected DataArea m_dataRegion;
        protected SymbolTable m_symbolTable;
        protected Stack<ForRecord> m_forLoopStack;
        protected Stack<WhileRecord> m_whileLoopStack;
        protected Stack<int> m_goSubStack;

        protected bool m_isRunning;
        protected int m_index;

        protected bool m_inInkeyStatement;
        protected int m_restInkeyCount;
        protected Statement m_inkeyStatement;

        protected BuildinFunc m_innerFunc;
        protected IAPI m_apiCall;

        /// <summary>
        /// constructor 
        /// </summary>
        public Runtime(Parser parser)
        {
            m_parser = parser;

            m_executer = new Dictionary<int, Action<Statement>>()
            {
                { Statement.TYPE_PRINT, doPrint },
                { Statement.TYPE_ASSIGN, doAssign },
                { Statement.TYPE_IF, doIf },
                { Statement.TYPE_DATA, doData },
                { Statement.TYPE_READ, doRead },
                { Statement.TYPE_RESTORE, doRestore },
                { Statement.TYPE_GOTO, doGoto },
                { Statement.TYPE_END, doEnd },
                { Statement.TYPE_FOR_BEGIN, doForBegin },
                { Statement.TYPE_FOR_END, doForEnd },
                { Statement.TYPE_WHILE_BEGIN, onWhileBegin },
                { Statement.TYPE_WHILE_END, onWhileEnd },
                { Statement.TYPE_ON_GOTO, onOnGoto },
                { Statement.TYPE_GOSUB, onGoSub },
                { Statement.TYPE_RETURN, onReturn },
                { Statement.TYPE_POP, onPop },
                { Statement.TYPE_DEF_FN, onDefFn },
                { Statement.TYPE_DIM, onDim },
                { Statement.TYPE_SWAP, onSwap },
                { Statement.TYPE_SIMPLE_CMD, onSimpleCmd },
                { Statement.TYPE_PARAM_CMD, onParamCmd },
            };

            // initial the context 
            m_dataRegion = new DataArea();
            m_symbolTable = new SymbolTable();
            m_forLoopStack = new Stack<ForRecord>();
            m_whileLoopStack = new Stack<WhileRecord>();
            m_goSubStack = new Stack<int>();

            m_innerFunc = new BuildinFunc();
        }

        /// <summary>
        /// set api 
        /// </summary>
        /// <param name="api"></param>
        public void SetAPI( IAPI api )
        {
            m_apiCall = api;

            m_innerFunc.SetAPI(m_apiCall);
        }

        /// <summary>
        /// run the program
        /// </summary>
        public void Run()
        {
            try
            {
                // generate the IR
                m_parser.Parsing();

                m_statements = m_parser.STATEMENTS;

                // process the data statement 
                List<Statement> removeList = new List<Statement>();
                foreach (Statement s in m_statements)
                {
                    if (s.m_type == Statement.TYPE_DATA)
                    {
                        doData(s);
                        removeList.Add(s);
                    }
                }
                // remove the data statments
                foreach (Statement s in removeList)
                    m_statements.Remove(s);

                // index the line number
                m_lineNumDic = new Dictionary<int, int>();
                for (int i = 0; i < m_statements.Count; i++)
                {
                    Statement s = m_statements[i];

                    s.m_lineIndex = i;

                    if (!m_lineNumDic.ContainsKey(s.m_lineNum))
                        m_lineNumDic.Add(s.m_lineNum, i);
                }

                m_isRunning = true;
                m_index = 0;
                m_inInkeyStatement = false;

                m_apiCall.ProgramStart();
            }
            catch( ErrorCode ec )
            {
                m_apiCall.ErrorCode("?" + ec.Message + " ERROR IN " + m_parser.CUR_LINE_NUM);
            }
        }

        /// <summary>
        /// run program step by step 
        /// </summary>
        /// <returns></returns>
        public bool Step()
        {
            bool done = false;

            try
            {
                // 处理语句中带有需要等待输入的情况
                if (m_inInkeyStatement)
                {
                    if (m_restInkeyCount > 0)
                    {
                        m_apiCall.WaittingInkey();
                        m_restInkeyCount--;
                    }
                    else
                    {
                        m_executer[m_inkeyStatement.m_type](m_inkeyStatement);
                        m_inInkeyStatement = false;
                        m_apiCall.CleanInkeyBuff();
                    }
                }
                else
                {
                    // execute statements 
                    if (m_isRunning)
                    {
                        if (m_index >= m_statements.Count)
                        {
                            m_isRunning = false;
                        }
                        else
                        {
                            Statement s = m_statements[m_index];
                            m_index++;      // 这一句必须在执行语句之前，因为语句中可能有改变该值的GOTO之类的语句

                            if (s.m_inkeyCnt > 0)
                                executeInkeyStatement(s);
                            else
                                m_executer[s.m_type](s);
                        }
                    }
                    else
                    {
                        m_apiCall.ProgramDone();
                        done = true;
                    }
                }
            }
            catch (ErrorCode ec)
            {
                m_apiCall.ErrorCode("?" + ec.Message + " ERROR IN " + m_statements[m_index - 1].m_lineNum);
            }

            return done;
        }

        
        protected void executeInkeyStatement( Statement s )
        {
            m_inInkeyStatement = true;
            m_restInkeyCount = s.m_inkeyCnt;
            m_inkeyStatement = s;
        }

        #region statement

        /// <summary>
        /// do goto statement 
        /// </summary>
        /// <param name="s"></param>
        protected void doGoto( Statement s )
        {
            m_index = m_lineNumDic[s.m_intVal];
        }

        /// <summary>
        /// do end 
        /// </summary>
        /// <param name="s"></param>
        protected void doEnd( Statement s )
        {
            m_isRunning = false;
        }

        /// <summary>
        /// print statement 
        /// </summary>
        /// <param name="s"></param>
        protected void doPrint( Statement s )
        {
            List<BaseData> dataList = new List<BaseData>();

            foreach( Expression exp in s.m_expressList )
                dataList.Add(calculateExp(exp).m_value);

            m_apiCall.Print( dataList );
        }

        /// <summary>
        /// assignment 
        /// </summary>
        /// <param name="s"></param>
        protected void doAssign( Statement s )
        {
            // calculate the expression value 
            BaseData dat = calculateExp(s.m_exp).m_value;

            if( s.m_expressList == null )
            {
                VarSymbol symbol = m_symbolTable.ResolveVar(s.m_symbol);
                symbol.VALUE = dat;
            }
            else
            {
                List<int> indexs = expressListToIndexs(s.m_expressList);
                ArraySymbol arrSymbol = m_symbolTable.ResolveArray(s.m_symbol, indexs);
                arrSymbol.SetValue(indexs, dat);
            }
        }

        /// <summary>
        /// if statement 
        /// </summary>
        /// <param name="s"></param>
        protected void doIf( Statement s )
        {
            BaseData condition = calculateExp(s.m_expressList[0]).m_value;

            bool first = false;

            if (condition.TYPE == BaseData.TYPE_INT)
            {
                first = condition.INT_VAL != 0;
            }
            else if (condition.TYPE == BaseData.TYPE_FLOAT)
            {
                if (condition.FLOAT_VAL < float.Epsilon && condition.FLOAT_VAL > -float.Epsilon)
                    first = false;
                else
                    first = true;
            }
            else if (condition.TYPE == BaseData.TYPE_STRING)
            {
                first = !string.IsNullOrEmpty( condition.STR_VAL );
            }

            if( first )
            {
                foreach (Statement ss in s.m_statements)
                    m_executer[ss.m_type](ss);
            }
            else if (s.m_elseStatements.Count > 0)
            {
                foreach (Statement ss in s.m_elseStatements)
                    m_executer[ss.m_type](ss);
            }
        }

        /// <summary>
        /// for statement 
        /// </summary>
        /// <param name="s"></param>
        protected void doForBegin( Statement s )
        {
            string varName = s.m_symbol;
            VarSymbol symbol = m_symbolTable.ResolveVar(varName);

            ForRecord lr = null;

            // use the top of LoopRecord or push a new one ? 
            if( m_forLoopStack.Count > 0 )
                lr = m_forLoopStack.Peek();

            if( lr == null || lr.LOOP_VAR_NAME != varName )
            {
                lr = new ForRecord();
                m_forLoopStack.Push(lr);
            }

            BaseData startValue = calculateExp(s.m_expressList[0]).m_value;
            BaseData endValue = calculateExp(s.m_expressList[1]).m_value;
            BaseData stepValue = calculateExp(s.m_expressList[2]).m_value;

            // check the value type 
            if (startValue.TYPE != BaseData.TYPE_INT && startValue.TYPE != BaseData.TYPE_FLOAT)
                throw new ErrorCode(ErrorCode.ERROR_CODE_02);
            if (endValue.TYPE != BaseData.TYPE_INT && endValue.TYPE != BaseData.TYPE_FLOAT)
                throw new ErrorCode(ErrorCode.ERROR_CODE_02);
            if (stepValue.TYPE != BaseData.TYPE_INT && stepValue.TYPE != BaseData.TYPE_FLOAT)
                throw new ErrorCode(ErrorCode.ERROR_CODE_02);

            // initital the loop var 
            lr.SetLoopRecord(symbol, endValue, stepValue);
            lr.SetBeginIndex(s.m_lineIndex);

            // init the symbol value 
            symbol.VALUE = startValue;
        }

        /// <summary>
        /// next statement 
        /// </summary>
        /// <param name="s"></param>
        protected void doForEnd( Statement s )
        {
            if( m_forLoopStack.Count <= 0 )
                throw new ErrorCode( ErrorCode.ERROR_CODE_01);

            ForRecord lr = m_forLoopStack.Peek();

            if( s.m_symbol != null && s.m_symbol != lr.LOOP_VAR_NAME )
                throw new ErrorCode( ErrorCode.ERROR_CODE_01);

            if (lr.UpdateLoop())
            {
                m_forLoopStack.Pop();
            }
            else
            {
                // set the next index to the for begin line 
                m_index = lr.LOOP_BEGIN_INDEX;
                while (m_statements[m_index].m_type != Statement.TYPE_FOR_BEGIN)
                    m_index++;

                m_index++;
            }
        }

        /// <summary>
        /// while 
        /// </summary>
        /// <param name="s"></param>
        protected void onWhileBegin( Statement s )
        {
            WhileRecord wr = new WhileRecord( s.m_expressList[0], s.m_lineIndex );

            if (wr.IsLoopDone( calculateExp ))
            {
                int whileCnt = 0;
                // skip to the wend next line 
                for (; ; )
                {
                    Statement statement = m_statements[m_index];

                    if (statement.m_type == Statement.TYPE_WHILE_BEGIN)
                    {
                        whileCnt++;
                    }
                    else if (statement.m_type == Statement.TYPE_WHILE_END)
                    {
                        if (whileCnt == 0)
                            break;
                        else
                            whileCnt--;
                    }

                    m_index++;
                }

                m_index++;
            }
            else
            {
                m_whileLoopStack.Push(wr);
            }
        }

        /// <summary>
        /// wend 
        /// </summary>
        /// <param name="s"></param>
        protected void onWhileEnd( Statement s )
        {
            if (m_whileLoopStack.Count <= 0)
                throw new ErrorCode(ErrorCode.ERROR_CODE_17);

            WhileRecord wr = m_whileLoopStack.Peek();

            if( wr.IsLoopDone( calculateExp ) )
            {
                m_whileLoopStack.Pop();
            }
            else
            {
                m_index = wr.LOOP_BEGIN_INDEX;
                m_index++;
            }
        }

        protected void onOnGoto( Statement s )
        {
            BaseData val = calculateExp(s.m_expressList[0]).m_value;

            val.Convert(BaseData.TYPE_INT);
            int index = val.INT_VAL;

            if (index <= 0 || index > s.m_dataList.Count)
                return;

            val = s.m_dataList[index-1];
            
            if( m_lineNumDic.ContainsKey(val.INT_VAL) )
                m_index = m_lineNumDic[val.INT_VAL];
            else
                throw new ErrorCode(ErrorCode.ERROR_CODE_07);
        }

        protected void onGoSub( Statement s )
        {
            m_index = m_lineNumDic[s.m_intVal];
            m_goSubStack.Push(s.m_lineIndex);
        }

        protected void onReturn( Statement s )
        {
            if (m_goSubStack.Count > 0)
            {
                m_index = m_goSubStack.Pop();
                m_index++;
            }
            else
            {
                throw new ErrorCode(ErrorCode.ERROR_CODE_18);
            }
        }

        protected void onPop( Statement s )
        {
            if (m_goSubStack.Count > 0)
                m_goSubStack.Pop();
            else
                throw new ErrorCode(ErrorCode.ERROR_CODE_18);
        }

        protected void onDefFn( Statement s )
        {
            m_symbolTable.Define(new FunSymbol(s.m_symbol, s.m_expressList[0]));
        }

        protected void onDim( Statement s )
        {
            m_symbolTable.Define(new ArraySymbol(s.m_symbol, s.m_arrayDimension));
        }
                
        protected void onSwap( Statement s )
        {
            Expression exp1 = s.m_expressList[0];
            Expression exp2 = s.m_expressList[1];

            BaseData dat1 = calculateExp(exp1).m_value;
            BaseData dat2 = calculateExp(exp2).m_value;

            List<int> indexs;

            if( exp1.m_type == Expression.EXP_SYMBOL )
            {
                m_symbolTable.ResolveVar(exp1.m_symbolName).VALUE = dat2;
            }
            else if (exp1.m_type == Expression.EXP_ARRAY_SYMBOL)
            {
                indexs = expressListToIndexs(exp1.m_funcParams);
                m_symbolTable.ResolveArray(exp1.m_symbolName, indexs ).SetValue(indexs,dat2);
            }
            else
            {
                throw new ErrorCode(ErrorCode.ERROR_CODE_02);
            }

            if (exp2.m_type == Expression.EXP_SYMBOL)
            {
                m_symbolTable.ResolveVar(exp2.m_symbolName).VALUE = dat1;
            }
            else if (exp2.m_type == Expression.EXP_ARRAY_SYMBOL)
            {
                indexs = expressListToIndexs(exp2.m_funcParams);
                m_symbolTable.ResolveArray(exp2.m_symbolName, indexs).SetValue(indexs, dat1);
            }
            else
            {
                throw new ErrorCode(ErrorCode.ERROR_CODE_02);
            }
        }

        protected void onSimpleCmd( Statement s )
        {
            if (s.m_symbol == "BEEP")
                m_apiCall.Beep();
            else if (s.m_symbol == "CLS")
                m_apiCall.Cls();
            else if (s.m_symbol == "INVERSE")
                m_apiCall.Inverse();
            else if (s.m_symbol == "NORMAL")
                m_apiCall.Normal();
            else if (s.m_symbol == "GRAPH")
                m_apiCall.Graph();
            else if (s.m_symbol == "TEXT")
                m_apiCall.Text();
            else
                throw new ErrorCode(ErrorCode.ERROR_CODE_02);
        }

        protected void onParamCmd( Statement s )
        {
            if( s.m_symbol == "PLAY" )
            {
                if( s.m_expressList.Count != 1 )
                    throw new ErrorCode(ErrorCode.ERROR_CODE_02);

                BaseData bd = calculateExp(s.m_expressList[0]).m_value;

                if (bd.TYPE != BaseData.TYPE_STRING)
                    throw new ErrorCode(ErrorCode.ERROR_CODE_02);

                m_apiCall.Play(bd.STR_VAL);
            }
            else
            {
                List<int> indexs = expressListToIndexs(s.m_expressList);

                if( s.m_symbol == "LOCATE" )
                {
                    if (indexs.Count != 2)
                        throw new ErrorCode(ErrorCode.ERROR_CODE_02);

                    m_apiCall.Locate(indexs[0], indexs[1]);
                }
                else if( s.m_symbol == "BOX" )
                {
                    if (indexs.Count == 4)
                        m_apiCall.Box(indexs[0], indexs[1], indexs[2], indexs[3], 0, 1);
                    else if( indexs.Count == 5 )
                        m_apiCall.Box(indexs[0], indexs[1], indexs[2], indexs[3], indexs[4], 1);
                    else if( indexs.Count == 6 )
                        m_apiCall.Box(indexs[0], indexs[1], indexs[2], indexs[3], indexs[4], indexs[5]);
                    else
                        throw new ErrorCode(ErrorCode.ERROR_CODE_02);
                }
                else if (s.m_symbol == "DRAW")
                {
                    if (indexs.Count == 2)
                        m_apiCall.Draw(indexs[0], indexs[1], 1);
                    else if (indexs.Count == 3)
                        m_apiCall.Draw(indexs[0], indexs[1], indexs[2]);
                    else
                        throw new ErrorCode(ErrorCode.ERROR_CODE_02);
                }
                else if( s.m_symbol == "CIRCLE")
                {
                    if (indexs.Count == 3)
                        m_apiCall.Circle(indexs[0], indexs[1], indexs[2], 0, 1);
                    else if (indexs.Count == 4)
                        m_apiCall.Circle(indexs[0], indexs[1], indexs[2], indexs[3], 0);
                    else if (indexs.Count == 5)
                        m_apiCall.Circle(indexs[0], indexs[1], indexs[2], indexs[3], indexs[4]);
                    else
                        throw new ErrorCode(ErrorCode.ERROR_CODE_02);
                }
                else if( s.m_symbol == "ELLIPSE")
                {
                    if (indexs.Count == 4)
                        m_apiCall.Ellipse(indexs[0], indexs[1], indexs[2], indexs[3], 0, 1);
                    else if (indexs.Count == 5)
                        m_apiCall.Ellipse(indexs[0], indexs[1], indexs[2], indexs[3], indexs[4], 1);
                    else if (indexs.Count == 6)
                        m_apiCall.Ellipse(indexs[0], indexs[1], indexs[2], indexs[3], indexs[4], indexs[5]);
                    else
                        throw new ErrorCode(ErrorCode.ERROR_CODE_02);
                }
                else if (s.m_symbol == "LINE")
                {
                    if (indexs.Count == 4)
                        m_apiCall.Line(indexs[0], indexs[1], indexs[2], indexs[3], 1);
                    else if (indexs.Count == 5)
                        m_apiCall.Line(indexs[0], indexs[1], indexs[2], indexs[3], indexs[4]);
                    else
                        throw new ErrorCode(ErrorCode.ERROR_CODE_02);
                }
            }
        }

        /// <summary>
        /// data statement 
        /// </summary>
        /// <param name="s"></param>
        protected void doData( Statement s )
        {
            m_dataRegion.AddDatas(s.m_dataList);
        }

        /// <summary>
        /// read statement 
        /// </summary>
        /// <param name="s"></param>
        protected void doRead( Statement s )
        {
            foreach( string symbolName in s.m_symbolList )
            {
                VarSymbol symbol = m_symbolTable.ResolveVar(symbolName);
                symbol.VALUE = m_dataRegion.GetData();
            }
        }

        /// <summary>
        /// restore 
        /// </summary>
        /// <param name="s"></param>
        protected void doRestore( Statement s )
        {
            m_dataRegion.Restore();
        }

        #endregion

        /// <summary>
        /// reduce the expession 
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        protected Expression calculateExp( Expression exp )
        {
            Expression result = null;
            Expression midExp = null;
            Expression expLeft = null;
            Expression expRight = null;

            switch( exp.m_type )
            {
                case Expression.VALUE:
                    result = exp;
                    break;
                case Expression.TYPE_CLOSE_TO:
                    result = exp;
                    result.m_value = new BaseData(BaseData.TYPE_CLOSE_TO, 0);
                    break;
                case Expression.TYPE_NEXT_LINE:
                    result = exp;
                    result.m_value = new BaseData(BaseData.TYPE_NEXT_LINE, 0);
                    break;
                case Expression.EXP_SYMBOL:
                    VarSymbol s = m_symbolTable.ResolveVar(exp.m_symbolName);
                    result = new Expression( s.VALUE );
                    break;
                case Expression.EXP_ARRAY_SYMBOL:
                    List<int> indexs = expressListToIndexs(exp.m_funcParams);
                    ArraySymbol arr = m_symbolTable.ResolveArray(exp.m_symbolName, indexs);
                    result = new Expression(arr.GetValue(indexs));
                    break;
                case Expression.EXP_FUNC:
                    List<BaseData> param = new List<BaseData>();
                    // convert the parameters 
                    foreach (Expression e in exp.m_funcParams)
                        param.Add( calculateExp(e).m_value );
                    // call the function 
                    BaseData returnVal = m_innerFunc.CallFunc(exp.m_symbolName, param);
                    result = new Expression(returnVal);
                    break;
                case Expression.EXP_USER_FUNC:
                    string funcName = exp.m_symbolName;
                    FunSymbol func = m_symbolTable.ResolveFunc(funcName);
                    Expression funcParam = exp.m_funcParams[0];
                    BaseData paramVal = calculateExp(funcParam).m_value;
                    m_symbolTable.Define(new VarSymbol(CommonDef.FN_PARAM_SYMBOL, paramVal));
                    result = calculateExp(func.EXP);
                    break;
                case Expression.OP_NOT:
                    midExp = calculateExp( exp.m_leftExp );
                    if( midExp.m_type == Expression.VALUE )
                    {
                        if (midExp.m_value != BaseData.ZERO)
                            result = new Expression( new BaseData(0) );
                        else
                            result = new Expression( new BaseData(1) );
                    }
                    else
                    {
                        throw new ErrorCode(ErrorCode.ERROR_CODE_12);
                    }
                    break;
                case Expression.OP_NEG:
                    midExp = calculateExp(exp.m_leftExp);
                    if (midExp.m_type == Expression.VALUE)
                    {
                        result = midExp;
                        result.m_value.NegValue();
                    }
                    else
                    {
                        throw new ErrorCode(ErrorCode.ERROR_CODE_12);
                    }
                    break;
                case Expression.OP_ADD:
                    expLeft = calculateExp(exp.m_leftExp);
                    expRight = calculateExp(exp.m_rightExp);
                    if (expLeft.m_type != Expression.VALUE || expRight.m_type != Expression.VALUE)
                        throw new ErrorCode(ErrorCode.ERROR_CODE_12);
                    result = new Expression(expLeft.m_value + expRight.m_value);
                    break;
                case Expression.OP_MINUS:
                    expLeft = calculateExp(exp.m_leftExp);
                    expRight = calculateExp(exp.m_rightExp);
                    if (expLeft.m_type != Expression.VALUE || expRight.m_type != Expression.VALUE)
                        throw new ErrorCode(ErrorCode.ERROR_CODE_12);
                    result = new Expression(expLeft.m_value - expRight.m_value);
                    break;
                case Expression.OP_MUL:
                    expLeft = calculateExp(exp.m_leftExp);
                    expRight = calculateExp(exp.m_rightExp);
                    if (expLeft.m_type != Expression.VALUE || expRight.m_type != Expression.VALUE)
                        throw new ErrorCode(ErrorCode.ERROR_CODE_12);
                    result = new Expression(expLeft.m_value * expRight.m_value);
                    break;
                case Expression.OP_DIV:
                    expLeft = calculateExp(exp.m_leftExp);
                    expRight = calculateExp(exp.m_rightExp);
                    if (expLeft.m_type != Expression.VALUE || expRight.m_type != Expression.VALUE)
                        throw new ErrorCode(ErrorCode.ERROR_CODE_12);
                    result = new Expression(expLeft.m_value / expRight.m_value);
                    break;
                case Expression.OP_POWER:
                    //TODO 
                    break;
                case Expression.OP_AND:
                    //TODO 
                    break;
                case Expression.OP_OR:
                    //TODO 
                    break;
                case Expression.OP_EQUAL:
                    //TODO 
                    break;
                case Expression.OP_GREATER:
                    expLeft = calculateExp(exp.m_leftExp);
                    expRight = calculateExp(exp.m_rightExp);
                    if (expLeft.m_type != Expression.VALUE || expRight.m_type != Expression.VALUE)
                        throw new ErrorCode(ErrorCode.ERROR_CODE_12);
                    result = new Expression( new BaseData( expLeft.m_value > expRight.m_value ? 1 : 0 ) );
                    break;
                case Expression.OP_GREATER_EQU:
                    //TODO 
                    break;
                case Expression.OP_LESS:
                    expLeft = calculateExp(exp.m_leftExp);
                    expRight = calculateExp(exp.m_rightExp);
                    if (expLeft.m_type != Expression.VALUE || expRight.m_type != Expression.VALUE)
                        throw new ErrorCode(ErrorCode.ERROR_CODE_12);
                    result = new Expression( new BaseData( expLeft.m_value < expRight.m_value ? 1 : 0 ) );
                    break;
                case Expression.OP_LESS_EQ:
                    //TODO 
                    break;
                case Expression.EXP_INKEY:
                    result = new Expression( new BaseData( m_apiCall.Inkey() ));
                    break;
                default:
                    throw new ErrorCode(ErrorCode.ERROR_CODE_02);
            }

            return result;
        }

        /// <summary>
        /// expression list to indexs 
        /// </summary>
        /// <param name="expList"></param>
        /// <returns></returns>
        protected List<int> expressListToIndexs( List<Expression> expList )
        {
            List<int> indexs = new List<int>();
            
            foreach( Expression exp in expList)
            {
                BaseData bd = calculateExp(exp).m_value;
                bd.Convert(BaseData.TYPE_INT);

                indexs.Add(bd.INT_VAL);
            }

            return indexs;
        }

    }
}
