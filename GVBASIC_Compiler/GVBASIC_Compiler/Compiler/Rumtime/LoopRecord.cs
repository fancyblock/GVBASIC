using System;
using System.Collections.Generic;

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
        protected int m_beginIndex;

        /// <summary>
        /// getter of the loop var name 
        /// </summary>
        public string LOOP_VAR_NAME { get { return m_loopSymbol.NAME; } }

        /// <summary>
        /// getter of the loop begin line 
        /// </summary>
        public int LOOP_BEGIN_INDEX { get { return m_beginIndex; } }

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
        public void SetBeginIndex( int lineNum )
        {
            m_beginIndex = lineNum;
        }

        /// <summary>
        /// the return value is if loop is done or not ?
        /// </summary>
        /// <returns></returns>
        public bool UpdateLoop()
        {
            m_loopSymbol.VALUE = m_loopSymbol.VALUE + m_stepValue;

            if (m_stepValue > BaseData.ZERO)
                if (m_loopSymbol.VALUE > m_endValue)        return true;
            else if (m_stepValue < BaseData.ZERO)
                if (m_loopSymbol.VALUE < m_endValue)        return true;

            return false;
        }

    }


    /// <summary>
    /// while loop record 
    /// </summary>
    public class WhileRecord
    {
        protected Expression m_judgeExp;
        protected int m_beginIndex;

        /// <summary>
        /// getter of the begin index 
        /// </summary>
        public int LOOP_BEGIN_INDEX { get { return m_beginIndex; } }

        /// <summary>
        /// constrcutor
        /// </summary>
        /// <param name="exp"></param>
        public WhileRecord( Expression exp, int beginIndex )
        {
            m_judgeExp = exp;
            m_beginIndex = beginIndex;
        }

        /// <summary>
        /// judge if the loop is done or not 
        /// </summary>
        /// <returns></returns>
        public bool IsLoopDone( Func<Expression,Expression> calcExp )
        {
            BaseData dat = calcExp(m_judgeExp).m_value;

            if( dat.TYPE == BaseData.TYPE_STRING )
                return string.IsNullOrEmpty(dat.STR_VAL);
            else if( dat.TYPE == BaseData.TYPE_INT )
                return dat.INT_VAL == 0;
            else if( dat.TYPE == BaseData.TYPE_FLOAT )
                return dat.FLOAT_VAL == 0.0f;
            else
                throw new ErrorCode(ErrorCode.ERROR_CODE_02);
        }
    }

}
