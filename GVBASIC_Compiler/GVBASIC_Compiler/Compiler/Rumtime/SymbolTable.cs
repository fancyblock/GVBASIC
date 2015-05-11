using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVBASIC_Compiler.Compiler
{
    public class SymbolTable
    {
        protected Dictionary<string, Symbol> m_symbolDic;

        /// <summary>
        /// constructor 
        /// </summary>
        public SymbolTable()
        {
            m_symbolDic = new Dictionary<string, Symbol>();
        }

        /// <summary>
        /// define a symbol 
        /// </summary>
        /// <param name="sym"></param>
        public void Define( Symbol sym )
        {
            m_symbolDic.Add(sym.NAME, sym); 
        }

        /// <summary>
        /// return a symbol 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Symbol Resolve( string name )
        {
            return m_symbolDic[name];
        }

        /// <summary>
        /// has symbol type
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool HasSymbolType( int type, string name )
        {
            if( m_symbolDic.ContainsKey( name ) )
            {
                if (m_symbolDic[name].TYPE == type )
                    return true;
            }

            return false;
        }

    }
}
