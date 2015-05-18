using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVBASIC_Compiler.Compiler
{
    public class SymbolTable
    {
        protected Dictionary<string, VarSymbol> m_varSymbolDic;
        protected Dictionary<string, FunSymbol> m_funSymbolDic;

        /// <summary>
        /// constructor 
        /// </summary>
        public SymbolTable()
        {
            m_varSymbolDic = new Dictionary<string, VarSymbol>();
            m_funSymbolDic = new Dictionary<string, FunSymbol>();
        }

        /// <summary>
        /// define a symbol 
        /// </summary>
        /// <param name="sym"></param>
        public void Define( VarSymbol sym )
        {
            if (m_varSymbolDic.ContainsKey(sym.NAME))
                m_varSymbolDic[sym.NAME] = sym;
            else
                m_varSymbolDic.Add(sym.NAME, sym); 
        }

        /// <summary>
        /// return a symbol 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public VarSymbol ResolveVar( string name )
        {
            if (m_varSymbolDic.ContainsKey(name))
                return m_varSymbolDic[name];

            // create a new 
            VarSymbol symbol = null;

            if (name.EndsWith("%"))
                symbol = new VarSymbol(Symbol.VAR, name, new BaseData(0));
            else if (name.EndsWith("$"))
                symbol = new VarSymbol(Symbol.VAR, name, new BaseData(""));
            else
                symbol = new VarSymbol(Symbol.VAR, name, new BaseData(0.0f));

            m_varSymbolDic.Add(name, symbol);

            return symbol;
        }

    }
}
