using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVBASIC_Compiler.Compiler
{
    public enum SymbolType : int
    {
        sInt = 1,
        sFloat,
        sString,
        sFunc,
    }

    /// <summary>
    /// symbol class
    /// </summary>
    public class Symbol
    {
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
        //TODO 
    }

    /// <summary>
    /// function symbol 
    /// </summary>
    public class FunSymbol : Symbol
    {
        //TODO 
    }

}
