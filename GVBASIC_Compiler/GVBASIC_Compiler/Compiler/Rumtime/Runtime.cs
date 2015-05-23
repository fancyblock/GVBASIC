﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVBASIC_Compiler.Compiler
{
    /// <summary>
    /// Basic runtime 
    /// </summary>
    public class Runtime
    {
        protected List<Statement> m_statements;
        protected Dictionary<int, Action<Statement>> m_executer;
        protected Dictionary<int, int> m_lineNumDic;

        protected DataArea m_dataRegion;
        protected SymbolTable m_symbolTable;
        protected Stack<ForRecord> m_forLoopStack;
        protected Stack<WhileRecord> m_whileLoopStack;

        protected bool m_isRunning;
        protected int m_index;

        protected BuildinFunc m_innerFunc;
        protected IAPI m_apiCall;

        /// <summary>
        /// constructor 
        /// </summary>
        public Runtime(Parser parser)
        {
            m_statements = parser.STATEMENTS;

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
                //TODO 
            };

            // initial the context 
            m_dataRegion = new DataArea();
            m_symbolTable = new SymbolTable();
            m_forLoopStack = new Stack<ForRecord>();
            m_whileLoopStack = new Stack<WhileRecord>();

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
            // process the data statement 
            List<Statement> removeList = new List<Statement>();
            foreach( Statement s in m_statements )
            {
                if( s.m_type == Statement.TYPE_DATA )
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

            try
            {
                m_isRunning = true;
                m_index = 0;

                // execute statements 
                while (m_isRunning)
                {
                    if (m_index >= m_statements.Count)
                        break;

                    Statement s = m_statements[m_index];
                    m_index++;      // 这一句必须在执行语句之前，因为语句中可能有改变该值的GOTO之类的语句
                    m_executer[s.m_type](s);
                }
            }
            catch( ErrorCode ec )
            {
                m_apiCall.ErrorCode( "?" + ec.Message + " ERROR IN LINE " + m_statements[m_index-1].m_lineNum);
            }

            m_apiCall.ProgramDone();
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
            BaseData dat = calculateExp(s.m_expressList[0]).m_value;

            VarSymbol symbol = m_symbolTable.ResolveVar(s.m_symbol);
            symbol.VALUE = dat;
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
            WhileRecord wr = new WhileRecord();

            //TODO 

            if (wr.IsLoopDone())
            {
                //TODO 
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
            //TODO 
        }

        protected void onOnGoto( Statement s )
        {
            //TODO 
        }

        protected void onGoSub( Statement s )
        {
            //TODO 
        }

        protected void onReturn( Statement s )
        {
            //TODO 
        }

        protected void onPop( Statement s )
        {
            //TODO 
        }

        protected void onDefFn( Statement s )
        {
            //TODO 
        }

        protected void onDim( Statement s )
        {
            //TODO 
        }
                
        protected void onSwap( Statement s )
        {
            //TODO 
        }

        protected void onSimpleCmd( Statement s )
        {
            //TODO 
        }

        protected void onParamCmd( Statement s )
        {
            //TODO 
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
                    //TODO 
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
                default:
                    throw new Exception("[Runtime]: calculate, no this expression type: " + exp.m_type);
            }

            return result;
        }

    }
}
