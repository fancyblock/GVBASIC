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
                            Expression exp = reduceExpress(e);

                            if (exp.m_type == ExpressionType.eIntNum)
                                System.Console.Write(exp.m_intVal);
                            else if (exp.m_type == ExpressionType.eRealNum)
                                System.Console.Write(exp.m_realVal);
                            else if (exp.m_type == ExpressionType.eString)
                                System.Console.Write(exp.m_text);
                            else
                                throw new Exception("[Runtime]: Run , print statement error, wrong type of " + exp.m_type.ToString());
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
        protected Expression reduceExpress( Expression exp )
        {
            Expression e = exp;
            Expression lExp = null;
            Expression rExp = null;

            if( exp.m_type == ExpressionType.eOpPlus )
            {
                lExp = reduceExpress(exp.m_leftExp);
                rExp = reduceExpress(exp.m_rightExp);

                if( lExp.m_type == rExp.m_type )
                {
                    e = new Expression(lExp.m_type);
                    //TODO 
                }
            }
            else if (exp.m_type == ExpressionType.eOpMinus)
            {
                //
            }
            else if (exp.m_type == ExpressionType.eOpMul)
            {
                //
            }
            else if( exp.m_type == ExpressionType.eOpDiv)
            {
                //
            }
            else if( exp.m_type == ExpressionType.eOpPower)
            {
                //
            }
            else if( exp.m_type == ExpressionType.eOpNeg)
            {
                //
            }

            return e;
        }

    }
}
