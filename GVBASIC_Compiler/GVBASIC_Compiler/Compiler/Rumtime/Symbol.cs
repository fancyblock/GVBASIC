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
        public const int ARRAY  = 2;
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
        protected BaseData m_value;

        /// <summary>
        /// constructor 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="bd"></param>
        public VarSymbol( string name, BaseData bd )
        {
            m_type = Symbol.VAR;
            m_name = name;

            VALUE = bd;
        }

        /// <summary>
        /// getter && setter of the value 
        /// </summary>
        public BaseData VALUE
        {
            get
            {
                return m_value;
            }
            set
            {
                m_value = value;

                // convert the type 
                if (m_name.EndsWith("%"))
                {
                    m_value.Convert(BaseData.TYPE_INT);
                }
                else if (m_name.EndsWith("$"))
                {
                    if (value.TYPE != BaseData.TYPE_STRING)
                        throw new ErrorCode(ErrorCode.ERROR_CODE_12);
                }
                else
                {
                    m_value.Convert(BaseData.TYPE_FLOAT);
                }
            }
        }

    }

    /// <summary>
    /// array symbol 
    /// </summary>
    public class ArraySymbol:Symbol
    {
        protected List<int> m_dimension;
        //TODO 

        /// <summary>
        /// constructor 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="dimension"></param>
        public ArraySymbol( string name, List<int> dimension )
        {
            m_type = Symbol.ARRAY;
            m_name = name;
            m_dimension = dimension;

            if (dimension.Count > 2)
                throw new ErrorCode(ErrorCode.ERROR_CODE_06);

            //TODO 
        }

        /// <summary>
        /// set value 
        /// </summary>
        /// <param name="indexs"></param>
        /// <param name="val"></param>
        public void SetValue( List<int> indexs, BaseData val )
        {
            //TODO 
        }

        /// <summary>
        /// get value 
        /// </summary>
        /// <param name="indexs"></param>
        /// <returns></returns>
        public BaseData GetValue( List<int> indexs )
        {
            //TODO 

            return BaseData.ZERO;
        }
    }

    /// <summary>
    /// function symbol 
    /// </summary>
    public class FunSymbol : Symbol
    {
        protected Expression m_exp;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <param name="exp"></param>
        public FunSymbol( string name, Expression exp )
        {
            m_type = Symbol.FUNC;
            m_name = name;

            m_exp = exp;
        }

        /// <summary>
        /// getter of the expression 
        /// </summary>
        public Expression EXP
        {
            get
            {
                return m_exp;
            }
        }
    }

}
