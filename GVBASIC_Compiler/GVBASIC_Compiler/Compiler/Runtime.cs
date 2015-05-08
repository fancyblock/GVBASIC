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
    class Runtime
    {
        protected List<Statement> m_statements;
        protected Dictionary<StatementType, Action<Statement>> m_executer;
        protected Dictionary<int, int> m_lineNumDic;

        protected bool m_isRunning;
        protected int m_index;

        /// <summary>
        /// constructor 
        /// </summary>
        public Runtime(Parser parser)
        {
            m_statements = parser.STATEMENTS;

            m_executer = new Dictionary<StatementType, Action<Statement>>()
            {
                { StatementType.ePrint, doPrint },
                { StatementType.eAssign, doAssign },
                { StatementType.eIf, doIf },
                { StatementType.eData, doData },
                { StatementType.eRead, doRead },
                //TODO 
            };
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
                m_executer[s.m_type](s);
            }
        }

        /// <summary>
        /// print statement 
        /// </summary>
        /// <param name="s"></param>
        protected void doPrint( Statement s )
        {
            //TODO 
        }

        /// <summary>
        /// assignment 
        /// </summary>
        /// <param name="s"></param>
        protected void doAssign( Statement s )
        {
            //TODO 
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
            //TODO 
        }

        /// <summary>
        /// read statement 
        /// </summary>
        /// <param name="s"></param>
        protected void doRead( Statement s )
        {
            //TODO 
        }

    }
}
