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
        public const int VAR    = 1;
        public const int FUNC   = 3;

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
        public BaseData m_value;

        /// <summary>
        /// constructor 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="bd"></param>
        public VarSymbol( int type, string name, BaseData bd )
        {
            m_type = type;
            m_name = name;

            SetValue(bd);
        }

        /// <summary>
        /// set value 
        /// </summary>
        /// <param name="bd"></param>
        public void SetValue( BaseData bd )
        {
            m_value = bd;

            // convert the type 
            if (m_name.EndsWith("%"))
            {
                m_value.Convert(BaseData.TYPE_INT);
            }
            else if (m_name.EndsWith("$"))
            {
                if (bd.m_type != BaseData.TYPE_STRING)
                    throw new ErrorCode(ErrorCode.ERROR_CODE_12);
            }
            else
            {
                m_value.Convert(BaseData.TYPE_FLOAT);
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
