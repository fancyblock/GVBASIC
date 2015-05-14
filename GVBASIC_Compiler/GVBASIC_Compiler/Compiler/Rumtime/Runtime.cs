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
        protected Dictionary<StatementType, Action<Statement>> m_executer;
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

            m_executer = new Dictionary<StatementType, Action<Statement>>()
            {
                { StatementType.eStatementSet, doStatements },
                { StatementType.ePrint, doPrint },
                { StatementType.eAssign, doAssign },
                { StatementType.eIf, doIf },
                { StatementType.eData, doData },
                { StatementType.eRead, doRead },
                { StatementType.eGoto, doGoto },
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
                if( s.m_type == StatementType.eData )
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

                if (subS.m_type == StatementType.eGoto)
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
            m_apiCall.Print(s.m_expressList);
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
            BaseData bdLeft = null;
            BaseData bdRight = null;

            if( exp.m_type == ExpressionType.eIntNum )
            {
                result = new BaseData(exp.m_intVal);
            }
            else if( exp.m_type == ExpressionType.eRealNum)
            {
                result = new BaseData(exp.m_realVal);
            }
            else if( exp.m_type == ExpressionType.eString )
            {
                result = new BaseData(exp.m_text);
            }
            //TODO 

            return result;
        }

    }
}
