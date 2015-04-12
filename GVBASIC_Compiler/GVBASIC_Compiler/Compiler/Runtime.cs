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
        protected BuildinFunc m_buildinFunc = null;
        protected List<Statement> m_statements;
        protected Context m_context = null;

        /// <summary>
        /// constructor 
        /// </summary>
        public Runtime(Parser parser)
        {
            m_context = new Context();
            m_buildinFunc = new BuildinFunc();
            m_buildinFunc.SetContext(m_context);

            m_statements = parser.STATEMENTS;
        }

        /// <summary>
        /// run the program
        /// </summary>
        public void Run()
        {
			m_context.Reset();

			foreach( Statement s in m_statements )
            {
                switch( s.m_type )
                {
                    case StatementType.ePrint:
                        foreach( Expression e in s.m_expressList )
                        {
                            System.Console.Write(calculateExpress(e));
                        }
                        break;
                    default:
                        break;
                }
            }
        }


		//-------------------- private function --------------------

        /// <summary>
        /// calculate the express 
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        protected string calculateExpress( Expression exp )
        {
            string result = "";

            //TODO 

            return result;
        }

    }
}
