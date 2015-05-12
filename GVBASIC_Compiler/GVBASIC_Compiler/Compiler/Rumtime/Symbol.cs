using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVBASIC_Compiler.Compiler
{

    /// <summary>
    /// symbol class
    /// </summary>
    public class Symbol
    {
        public const int INT = 0;
        public const int FLOAT = 1;
        public const int STRING = 2;
        public const int FUNC = 3;

        protected string m_name;
        protected int m_type;

        /// <summary>
        /// getter of the name 
        /// </summary>
        public string NAME { get { return m_name; } }

        /// <summary>
        /// getter of the type
        /// </summary>
        public int TYPE { get { return m_type; } }
    }

    /// <summary>
    /// variable symbol 
    /// </summary>
    public class VarSymbol : Symbol
    {
        public int m_intVal;
        public float m_floatVal;
        public string m_strVal;

        /// <summary>
        /// constructor 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="bd"></param>
        public VarSymbol( int type, string name, BaseData bd )
        {
            m_type = type;
            m_name = name;

            if( type == Symbol.FLOAT )
            {
                if (bd.m_type == BaseData.TYPE_FLOAT)
                    m_floatVal = bd.m_floatVal;
                else if (bd.m_type == BaseData.TYPE_INT)
                    m_floatVal = bd.m_intVal;
                else
                    throw new ErrorCode(ErrorCode.ERROR_CODE_12);
            }
            else if( type == Symbol.INT )
            {
                if (bd.m_type == BaseData.TYPE_INT)
                    m_intVal = bd.m_intVal;
                else if (bd.m_type == BaseData.TYPE_FLOAT)
                    m_intVal = (int)bd.m_floatVal;
                else
                    throw new ErrorCode(ErrorCode.ERROR_CODE_12);
            }
            else if( type == Symbol.STRING )
            {
                if (bd.m_type == BaseData.TYPE_STRING)
                    m_strVal = bd.m_stringVal;
                else
                    throw new ErrorCode(ErrorCode.ERROR_CODE_12);
            }
        }
    }

    /// <summary>
    /// function symbol 
    /// </summary>
    public class FunSymbol : Symbol
    {
        //TODO 
    }

}
