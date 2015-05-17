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
            // convert the type 
            if( name.EndsWith("%") )
            {
                //TODO 
            }
            else if( name.EndsWith("$"))
            {
                //TODO 
            }
            else
            {
                //TODO 
            }

            m_type = type;
            m_name = name;

            m_value = bd;
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
