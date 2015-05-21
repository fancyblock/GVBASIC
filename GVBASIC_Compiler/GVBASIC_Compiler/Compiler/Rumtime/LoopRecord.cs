using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVBASIC_Compiler.Compiler
{
    /// <summary>
    /// loop statement info 
    /// </summary>
    public class ForRecord
    {
        protected VarSymbol m_loopSymbol;
        protected BaseData m_endValue;
        protected BaseData m_stepValue;
        protected int m_beginLine;

        /// <summary>
        /// getter of the loop var name 
        /// </summary>
        public string LOOP_VAR_NAME { get { return m_loopSymbol.NAME; } }

        /// <summary>
        /// getter of the loop begin line 
        /// </summary>
        public int LOOP_BEGIN_LINE { get { return m_beginLine; } }

        /// <summary>
        /// set loop record info 
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="startVal"></param>
        /// <param name="endVal"></param>
        /// <param name="stepVal"></param>
        public void SetLoopRecord( VarSymbol symbol, BaseData endVal, BaseData stepVal )
        {
            m_loopSymbol = symbol;

            m_endValue = endVal;
            m_stepValue = stepVal;
        }

        /// <summary>
        /// set for begin line 
        /// </summary>
        /// <param name="lineNum"></param>
        public void SetBeginLine( int lineNum )
        {
            m_beginLine = lineNum;
        }

        /// <summary>
        /// the return value is if loop is done or not ?
        /// </summary>
        /// <returns></returns>
        public bool UpdateLoop()
        {
            m_loopSymbol.VALUE = m_loopSymbol.VALUE + m_stepValue;

            if (m_stepValue > BaseData.ZERO)
            {
                if (m_loopSymbol.VALUE > m_endValue)
                    return true;
            }
            else if (m_stepValue < BaseData.ZERO)
            {
                if (m_loopSymbol.VALUE < m_endValue)
                    return true;
            }

            return false;
        }

    }


    /// <summary>
    /// while loop record 
    /// </summary>
    public class WhileRecord
    {
        //TODO
    }

}
