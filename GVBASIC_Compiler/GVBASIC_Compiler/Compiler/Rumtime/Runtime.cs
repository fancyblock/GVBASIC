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

        protected bool m_isRunning;
        protected int m_index;
        protected DataArea m_dataRegion;
        protected SymbolTable m_symbolTable;

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
                //TODO 
            };

            // initial the context 
            m_dataRegion = new DataArea();
            m_symbolTable = new SymbolTable();
        }

        /// <summary>
        /// set api 
        /// </summary>
        /// <param name="api"></param>
        public void SetAPI( IAPI api )
        {
            m_apiCall = api;
        }

        /// <summary>
        /// run the program
        /// </summary>
        public void Run()
        {
            // process the data statement 
            foreach( Statement s in m_statements )
            {
                if( s.m_type == Statement.TYPE_DATA )
                {
                    doData(s);
                    m_statements.Remove(s);
                }
            }

            // index the line number
            m_lineNumDic = new Dictionary<int, int>();
            for (int i = 0; i < m_statements.Count; i++)
                m_lineNumDic.Add(m_statements[i].m_num, i);

            m_isRunning = true;
            m_index = 0;

            // execute statements 
			while( m_isRunning )
            {
                if (m_index >= m_statements.Count)
                    break;

                Statement s = m_statements[m_index];
                m_index++;
                m_executer[s.m_type](s);
            }
        }

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

            string symbolName = s.m_symbol;
            Symbol symbol = null;

            if (symbolName.EndsWith("%"))           // int value 
                symbol = new VarSymbol( Symbol.INT, symbolName, dat );
            else if (symbolName.EndsWith("$"))      // string value 
                symbol = new VarSymbol( Symbol.STRING, symbolName, dat );
            else                                    // float value 
                symbol = new VarSymbol( Symbol.FLOAT, symbolName, dat );

            // add to symbol table 
            m_symbolTable.Define(symbol);
        }

        /// <summary>
        /// if statement 
        /// </summary>
        /// <param name="s"></param>
        protected void doIf( Statement s )
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
            //TODO 
        }


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
                    result = symbolToExp(s);
                    break;
                case Expression.EXP_FUNC:
                    //TODO
                    break;
                case Expression.EXP_USER_FUNC:
                    //TODO
                    break;
                case Expression.OP_PLUS:
                    //TODO
                    break;
                case Expression.OP_MINUS:
                    //TODO
                    break;
                case Expression.OP_MUL:
                    //TODO
                    break;
                case Expression.OP_DIV:
                    //TODO
                    break;
                case Expression.OP_POWER:
                    //TODO
                    break;
                case Expression.OP_NEG:
                    //TODO
                    break;
                case Expression.OP_EQUAL:
                    //TODO
                    break;
                case Expression.OP_GREATER:
                    //TODO
                    break;
                case Expression.OP_GREATER_EQU:
                    //TODO
                    break;
                case Expression.OP_LESS:
                    //TODO
                    break;
                case Expression.OP_LESS_EQ:
                    //TODO
                    break;
                case Expression.OP_AND:
                    //TODO
                    break;
                case Expression.OP_OR:
                    //TODO
                    break;
                case Expression.OP_NOT:
                    //TODO
                    break;
                default:
                    throw new Exception("[Runtime]: reduceExpression, error expression type " + exp.m_type );
            }

            return result;
        }

        /// <summary>
        /// convert symbol to exp 
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        protected Expression symbolToExp( Symbol symbol )
        {
            Expression res = null;

            switch( symbol.TYPE )
            {
                case Symbol.INT:
                    res = new Expression(Expression.VAL_INT);
                    res.m_intVal = (symbol as VarSymbol).m_intVal;
                    break;
                case Symbol.FLOAT:
                    res = new Expression(Expression.VAL_FLOAT);
                    res.m_floatVal = (symbol as VarSymbol).m_floatVal;
                    break;
                case Symbol.STRING:
                    res = new Expression(Expression.VAL_STRING);
                    res.m_strVal = (symbol as VarSymbol).m_strVal;
                    break;
                default:
                    throw new Exception("[Runtime]: symbolToExp, error symbol type " + symbol.TYPE );
            }

            return res;
        }
    }
}
