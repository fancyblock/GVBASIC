using System;
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

        protected Dictionary<int, Func<Expression,Expression,Expression>> m_binaryOperators;
        protected DataArea m_dataRegion;
        protected SymbolTable m_symbolTable;

        protected bool m_isRunning;
        protected int m_index;

        protected InnerFunc m_innerFunc;
        protected IAPI m_apiCall;

        /// <summary>
        /// constructor 
        /// </summary>
        public Runtime(Parser parser)
        {
            m_statements = parser.STATEMENTS;

            m_executer = new Dictionary<int, Action<Statement>>()
            {
                { Statement.TYPE_STATEMENT_SET, doStatements },
                { Statement.TYPE_PRINT, doPrint },
                { Statement.TYPE_ASSIGN, doAssign },
                { Statement.TYPE_IF, doIf },
                { Statement.TYPE_DATA, doData },
                { Statement.TYPE_READ, doRead },
                { Statement.TYPE_GOTO, doGoto },
                { Statement.TYPE_END, doEnd },
                //TODO 
            };

            m_binaryOperators = new Dictionary<int, Func<Expression, Expression, Expression>>()
            {
                { Expression.OP_ADD, opAdd },
                { Expression.OP_MINUS, opMinus },
                { Expression.OP_MUL, opMul },
                { Expression.OP_DIV, opDiv },
                { Expression.OP_POWER, opPower },
                { Expression.OP_AND, opAnd },
                { Expression.OP_OR, opOr },
                { Expression.OP_EQUAL, opEqual },
                { Expression.OP_GREATER, opGtr },
                { Expression.OP_GREATER_EQU, opGte },
                { Expression.OP_LESS, opLtr },
                { Expression.OP_LESS_EQ, opLte },
            };

            // initial the context 
            m_dataRegion = new DataArea();
            m_symbolTable = new SymbolTable();

            m_innerFunc = new InnerFunc();
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
                m_lineNumDic.Add(m_statements[i].m_num, i);

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
                    m_index++;
                    m_executer[s.m_type](s);
                }
            }
            catch( ErrorCode ec )
            {
                m_apiCall.ErrorCode( "?" + ec.Message + " ERROR IN LINE " + m_statements[m_index-1].m_num);
            }

            m_apiCall.ProgramDone();
        }

        #region statement

        /// <summary>
        /// do statement set 
        /// </summary>
        /// <param name="s"></param>
        protected void doStatements( Statement s )
        {
            List<Statement> statements = s.m_statements;

            for( int i = 0; i < statements.Count; i++ )
            {
                Statement subS = statements[i];
                m_executer[subS.m_type](subS);

                if (subS.m_type == Statement.TYPE_GOTO)
                    break;
            }
        }

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
                dataList.Add(calculateExpression(exp));

            m_apiCall.Print( dataList );
        }

        /// <summary>
        /// assignment 
        /// </summary>
        /// <param name="s"></param>
        protected void doAssign( Statement s )
        {
            // calculate the expression value 
            BaseData dat = calculateExpression(s.m_expressList[0]);

            Symbol symbol = m_symbolTable.Resolve(s.m_symbol);

            if( symbol == null )
            {
                symbol = new VarSymbol(Symbol.VAR, s.m_symbol, dat);
                // add to symbol table 
                m_symbolTable.Define(symbol);
            }
            else
            {
                (symbol as VarSymbol).SetValue(dat);
            }
        }

        /// <summary>
        /// if statement 
        /// </summary>
        /// <param name="s"></param>
        protected void doIf( Statement s )
        {
            BaseData condition = calculateExpression(s.m_expressList[0]);

            bool first = false;

            if( condition.m_type == BaseData.TYPE_INT )
            {
                first = condition.m_intVal != 0;
            }
            else if( condition.m_type == BaseData.TYPE_FLOAT )
            {
                if (condition.m_floatVal < float.Epsilon && condition.m_floatVal > -float.Epsilon)
                    first = false;
                else
                    first = true;
            }
            else if( condition.m_type == BaseData.TYPE_STRING )
            {
                first = !string.IsNullOrEmpty( condition.m_stringVal );
            }

            Statement exeS = null;

            if( first )
            {
                exeS = s.m_statements[0];
                m_executer[exeS.m_type](exeS);
            }
            else if (s.m_statements.Count > 1)
            {
                exeS = s.m_statements[1];
                m_executer[exeS.m_type](exeS);
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
                VarSymbol symbol = m_symbolTable.Resolve(symbolName) as VarSymbol;

                if (symbol == null)
                {
                    symbol = new VarSymbol(VarSymbol.VAR, symbolName, m_dataRegion.GetData());
                    m_symbolTable.Define(symbol);
                }
                else
                {
                    symbol.SetValue(m_dataRegion.GetData());
                }
            }
        }

        #endregion

        /// <summary>
        /// calculate the expression 
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        protected BaseData calculateExpression( Expression exp )
        {
            BaseData result = null;

            exp = reduceExpression(exp);

            switch( exp.m_type )
            {
                case Expression.VAL_INT:
                    result = new BaseData(exp.m_intVal);
                    break;
                case Expression.VAL_FLOAT:
                    result = new BaseData(exp.m_floatVal);
                    break;
                case Expression.VAL_STRING:
                    result = new BaseData(exp.m_strVal);
                    break;
                case Expression.TYPE_CLOSE_TO:
                    result = new BaseData();
                    result.m_type = BaseData.TYPE_CLOSE_TO;
                    break;
                case Expression.TYPE_NEXT_LINE:
                    result = new BaseData();
                    result.m_type = BaseData.TYPE_NEXT_LINE;
                    break;
                default:
                    throw new Exception("[Runtime]: calculateExpression, error expression type " + exp.m_type);
            }

            return result;
        }

        /// <summary>
        /// BaseData to Expression
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected Expression baseDataToExp( BaseData data )
        {
            Expression exp = null;

            switch( data.m_type )
            {
                case BaseData.TYPE_INT:
                    exp = new Expression(Expression.VAL_INT);
                    exp.m_intVal = data.m_intVal;
                    break;
                case BaseData.TYPE_FLOAT:
                    exp = new Expression(Expression.VAL_FLOAT);
                    exp.m_floatVal = data.m_floatVal;
                    break;
                case BaseData.TYPE_STRING:
                    exp = new Expression(Expression.VAL_STRING);
                    exp.m_strVal = data.m_stringVal;
                    break;
                case BaseData.TYPE_CLOSE_TO:
                    exp = new Expression(Expression.TYPE_CLOSE_TO);
                    break;
                case BaseData.TYPE_NEXT_LINE:
                    exp = new Expression(Expression.TYPE_NEXT_LINE);
                    break;
                default:
                    break;
            }

            return exp;
        }

        /// <summary>
        /// reduce the expession 
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        protected Expression reduceExpression( Expression exp )
        {
            Expression result = null;

            switch( exp.m_type )
            {
                case Expression.VAL_FLOAT:
                case Expression.VAL_INT:
                case Expression.VAL_STRING:
                case Expression.TYPE_CLOSE_TO:
                case Expression.TYPE_NEXT_LINE:
                    result = exp;
                    break;
                case Expression.EXP_SYMBOL:
                    Symbol s = m_symbolTable.Resolve(exp.m_strVal);
                    result = baseDataToExp((s as VarSymbol).m_value);
                    break;
                case Expression.EXP_FUNC:
                    if( m_innerFunc.HasFunc( exp.m_strVal ) )
                    {
                        List<BaseData> param = new List<BaseData>();
                        // convert the parameters 
                        foreach (Expression e in exp.m_funcParams)
                            param.Add(calculateExpression(e));
                        // call the function 
                        BaseData returnVal = m_innerFunc.CallFunc(exp.m_strVal, param);
                        result = baseDataToExp(returnVal);
                    }
                    else
                    {
                        throw new Exception("[Runtime]: reduceExpression, can not found inner function " + exp.m_strVal );
                    }
                    break;
                case Expression.EXP_USER_FUNC:
                    //TODO 
                    break;
                case Expression.OP_NOT:
                    result = new Expression(Expression.VAL_INT);
                    if( exp.m_leftExp.m_type == Expression.VAL_FLOAT )
                        result.m_intVal = (exp.m_leftExp.m_floatVal < float.Epsilon && exp.m_leftExp.m_floatVal > -float.Epsilon) ? 0 : 1;
                    else if( exp.m_leftExp.m_type == Expression.VAL_INT)
                        result.m_intVal = exp.m_leftExp.m_intVal == 0 ? 0 : 1;
                    else
                        throw new ErrorCode(ErrorCode.ERROR_CODE_12);
                    break;
                case Expression.OP_NEG:
                    if (exp.m_leftExp.m_type == Expression.VAL_FLOAT)
                    {
                        result = new Expression(Expression.VAL_FLOAT);
                        result.m_floatVal = -exp.m_leftExp.m_floatVal;
                    }
                    else if (exp.m_leftExp.m_type == Expression.VAL_INT)
                    {
                        result = new Expression(Expression.VAL_INT);
                        result.m_intVal = -exp.m_leftExp.m_intVal;
                    }
                    else
                    {
                        throw new ErrorCode(ErrorCode.ERROR_CODE_12);
                    }
                    break;
                default:
                    // binary operator 
                    if( m_binaryOperators.ContainsKey(exp.m_type) )
                    {
                        Expression leftExp = reduceExpression(exp.m_leftExp);
                        Expression rightExp = reduceExpression(exp.m_rightExp);

                        result = m_binaryOperators[exp.m_type](leftExp, rightExp);
                    }
                    else
                    {
                        throw new Exception("[Runtime]: reduceExpression, error expression type " + exp.m_type);
                    }
                    break;
            }

            return result;
        }

        #region operators

        /// <summary>
        /// add operation 
        /// </summary>
        /// <param name="expLeft"></param>
        /// <param name="expRight"></param>
        /// <returns></returns>
        protected Expression opAdd( Expression expLeft, Expression expRight )
        {
            Expression result = new Expression( binaryOperateType(expLeft.m_type, expRight.m_type) );

            if( expLeft.m_type == Expression.VAL_STRING )
            {
                if (expRight.m_type == Expression.VAL_STRING)
                    result.m_strVal = expLeft.m_strVal + expRight.m_strVal;
                else if (expRight.m_type == Expression.VAL_INT)
                    result.m_strVal = expLeft.m_strVal + expRight.m_intVal.ToString();
                else if (expRight.m_type == Expression.VAL_FLOAT)
                    result.m_strVal = expLeft.m_strVal + expRight.m_floatVal.ToString();
                else
                    throw new ErrorCode( ErrorCode.ERROR_CODE_12 );
            }
            else if( expLeft.m_type == Expression.VAL_INT )
            {
                if (expRight.m_type == Expression.VAL_STRING)
                    result.m_strVal = expLeft.m_intVal.ToString() + expRight.m_strVal;
                else if (expRight.m_type == Expression.VAL_INT)
                    result.m_intVal = expLeft.m_intVal + expRight.m_intVal;
                else if (expRight.m_type == Expression.VAL_FLOAT)
                    result.m_floatVal = expLeft.m_intVal + expRight.m_floatVal;
                else
                    throw new ErrorCode(ErrorCode.ERROR_CODE_12);
            }
            else if( expLeft.m_type == Expression.VAL_FLOAT )
            {
                if (expRight.m_type == Expression.VAL_STRING)
                    result.m_strVal = expLeft.m_floatVal.ToString() + expRight.m_strVal;
                else if (expRight.m_type == Expression.VAL_INT)
                    result.m_floatVal = expLeft.m_floatVal + expRight.m_intVal;
                else if (expRight.m_type == Expression.VAL_FLOAT)
                    result.m_floatVal = expLeft.m_floatVal + expRight.m_floatVal;
                else
                    throw new ErrorCode(ErrorCode.ERROR_CODE_12);
            }
            else
            {
                throw new ErrorCode(ErrorCode.ERROR_CODE_12);
            }

            return result;
        }

        /// <summary>
        /// minus operation 
        /// </summary>
        /// <param name="expLeft"></param>
        /// <param name="expRight"></param>
        /// <returns></returns>
        protected Expression opMinus(Expression expLeft, Expression expRight)
        {
            Expression result = new Expression( binaryOperateType( expLeft.m_type, expRight.m_type ) );

            if( expLeft.m_type == Expression.VAL_INT )
            {
                if( expRight.m_type == Expression.VAL_INT )
                    result.m_intVal = expLeft.m_intVal - expRight.m_intVal;
                else if( expRight.m_type == Expression.VAL_FLOAT )
                    result.m_floatVal = expLeft.m_intVal - expRight.m_floatVal;
                else
                    throw new ErrorCode(ErrorCode.ERROR_CODE_12);
            }
            else if( expLeft.m_type == Expression.VAL_FLOAT )
            {
                if (expRight.m_type == Expression.VAL_INT)
                    result.m_floatVal = expLeft.m_floatVal - expRight.m_intVal;
                else if (expRight.m_type == Expression.VAL_FLOAT)
                    result.m_floatVal = expLeft.m_floatVal - expRight.m_floatVal;
            }
            else
            {
                throw new ErrorCode(ErrorCode.ERROR_CODE_12);
            }

            return result;
        }

        /// <summary>
        /// mul 
        /// </summary>
        /// <param name="expLeft"></param>
        /// <param name="expRight"></param>
        /// <returns></returns>
        protected Expression opMul(Expression expLeft, Expression expRight)
        {
            Expression result = null;

            if (expLeft.m_type == Expression.VAL_INT)
            {
                if (expRight.m_type == Expression.VAL_INT)
                    result.m_intVal = expLeft.m_intVal * expRight.m_intVal;
                else if (expRight.m_type == Expression.VAL_FLOAT)
                    result.m_floatVal = expLeft.m_intVal * expRight.m_floatVal;
                else
                    throw new ErrorCode(ErrorCode.ERROR_CODE_12);
            }
            else if (expLeft.m_type == Expression.VAL_FLOAT)
            {
                if (expRight.m_type == Expression.VAL_INT)
                    result.m_floatVal = expLeft.m_floatVal * expRight.m_intVal;
                else if (expRight.m_type == Expression.VAL_FLOAT)
                    result.m_floatVal = expLeft.m_floatVal * expRight.m_floatVal;
            }
            else
            {
                throw new ErrorCode(ErrorCode.ERROR_CODE_12);
            }

            return result;
        }

        /// <summary>
        /// div
        /// </summary>
        /// <param name="expLeft"></param>
        /// <param name="expRight"></param>
        /// <returns></returns>
        protected Expression opDiv(Expression expLeft, Expression expRight)
        {
            Expression result = null;

            if (expLeft.m_type == Expression.VAL_INT)
            {
                if (expRight.m_type == Expression.VAL_INT)
                    result.m_intVal = expLeft.m_intVal / expRight.m_intVal;
                else if (expRight.m_type == Expression.VAL_FLOAT)
                    result.m_floatVal = expLeft.m_intVal / expRight.m_floatVal;
                else
                    throw new ErrorCode(ErrorCode.ERROR_CODE_12);
            }
            else if (expLeft.m_type == Expression.VAL_FLOAT)
            {
                if (expRight.m_type == Expression.VAL_INT)
                    result.m_floatVal = expLeft.m_floatVal / expRight.m_intVal;
                else if (expRight.m_type == Expression.VAL_FLOAT)
                    result.m_floatVal = expLeft.m_floatVal / expRight.m_floatVal;
            }
            else
            {
                throw new ErrorCode(ErrorCode.ERROR_CODE_12);
            }

            return result;
        }

        /// <summary>
        /// power 
        /// </summary>
        /// <param name="expLeft"></param>
        /// <param name="expRight"></param>
        /// <returns></returns>
        protected Expression opPower(Expression expLeft, Expression expRight)
        {
            Expression result = null;

            //TODO 

            return result;
        }

        protected Expression opAnd(Expression expLeft, Expression expRight)
        {
            Expression result = null;

            //TODO 

            return result;
        }

        protected Expression opOr(Expression expLeft, Expression expRight)
        {
            Expression result = null;

            //TODO 

            return result;
        }

        protected Expression opEqual(Expression expLeft, Expression expRight)
        {
            Expression result = null;

            //TODO 

            return result;
        }

        protected Expression opGtr(Expression expLeft, Expression expRight)
        {
            Expression result = new Expression( Expression.VAL_INT );

            if( expLeft.m_type == Expression.VAL_INT )
            {
                if (expRight.m_type == Expression.VAL_INT)
                    result.m_intVal = expLeft.m_intVal > expRight.m_intVal ? 1 : 0;
                else if (expRight.m_type == Expression.VAL_FLOAT)
                    result.m_intVal = expLeft.m_intVal > expRight.m_floatVal ? 1 : 0;
                else
                    throw new ErrorCode(ErrorCode.ERROR_CODE_12);
            }
            else if( expLeft.m_type == Expression.VAL_FLOAT )
            {
                if (expRight.m_type == Expression.VAL_INT)
                    result.m_intVal = expLeft.m_floatVal > expRight.m_intVal ? 1 : 0;
                else if (expRight.m_type == Expression.VAL_FLOAT)
                    result.m_intVal = expLeft.m_floatVal > expRight.m_floatVal ? 1 : 0;
                else
                    throw new ErrorCode(ErrorCode.ERROR_CODE_12);
            }
            else if( expLeft.m_type == Expression.VAL_STRING )
            {
                //TODO 
            }

            return result;
        }

        protected Expression opGte(Expression expLeft, Expression expRight)
        {
            Expression result = null;

            //TODO 

            return result;
        }

        protected Expression opLtr(Expression expLeft, Expression expRight)
        {
            Expression result = new Expression( Expression.VAL_INT );;

            if (expLeft.m_type == Expression.VAL_INT)
            {
                if (expRight.m_type == Expression.VAL_INT)
                    result.m_intVal = expLeft.m_intVal < expRight.m_intVal ? 1 : 0;
                else if (expRight.m_type == Expression.VAL_FLOAT)
                    result.m_intVal = expLeft.m_intVal < expRight.m_floatVal ? 1 : 0;
                else
                    throw new ErrorCode(ErrorCode.ERROR_CODE_12);
            }
            else if (expLeft.m_type == Expression.VAL_FLOAT)
            {
                if (expRight.m_type == Expression.VAL_INT)
                    result.m_intVal = expLeft.m_floatVal < expRight.m_intVal ? 1 : 0;
                else if (expRight.m_type == Expression.VAL_FLOAT)
                    result.m_intVal = expLeft.m_floatVal < expRight.m_floatVal ? 1 : 0;
                else
                    throw new ErrorCode(ErrorCode.ERROR_CODE_12);
            }
            else if (expLeft.m_type == Expression.VAL_STRING)
            {
                //TODO 
            }

            return result;
        }

        protected Expression opLte(Expression expLeft, Expression expRight)
        {
            Expression result = null;

            //TODO 

            return result;
        }

        #endregion

        /// <summary>
        /// operation type match 
        /// </summary>
        /// <param name="leftType"></param>
        /// <param name="rightType"></param>
        /// <returns></returns>
        protected int binaryOperateType( int leftType, int rightType )
        {
            int type = -1;

            if (leftType == Expression.VAL_STRING || rightType == Expression.VAL_STRING)
                type = Expression.VAL_STRING;
            else if (leftType == Expression.VAL_FLOAT || rightType == Expression.VAL_FLOAT)
                type = Expression.VAL_FLOAT;
            else if (leftType == Expression.VAL_INT && rightType == Expression.VAL_INT)
                type = Expression.VAL_INT;
            else
                throw new ErrorCode(ErrorCode.ERROR_CODE_12);

            return type;
        }

    }
}
