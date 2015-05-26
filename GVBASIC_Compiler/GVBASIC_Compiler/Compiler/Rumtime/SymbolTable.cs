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
        protected Dictionary<string, ArraySymbol> m_arraySymbolDic;

        /// <summary>
        /// constructor 
        /// </summary>
        public SymbolTable()
        {
            m_varSymbolDic = new Dictionary<string, VarSymbol>();
            m_funSymbolDic = new Dictionary<string, FunSymbol>();
            m_arraySymbolDic = new Dictionary<string, ArraySymbol>();
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
        /// define a array 
        /// </summary>
        /// <param name="sym"></param>
        public void Define( ArraySymbol sym )
        {
            if (m_arraySymbolDic.ContainsKey(sym.NAME))
                m_arraySymbolDic[sym.NAME] = sym;
            else
                m_arraySymbolDic.Add(sym.NAME, sym);
        }

        /// <summary>
        /// define a function 
        /// </summary>
        /// <param name="sym"></param>
        public void Define( FunSymbol sym )
        {
            if (m_funSymbolDic.ContainsKey(sym.NAME))
                m_funSymbolDic[sym.NAME] = sym;
            else
                m_funSymbolDic.Add(sym.NAME, sym);
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
                symbol = new VarSymbol( name, new BaseData(0));
            else if (name.EndsWith("$"))
                symbol = new VarSymbol( name, new BaseData(""));
            else
                symbol = new VarSymbol( name, new BaseData(0.0f));

            m_varSymbolDic.Add(name, symbol);

            return symbol;
        }

        /// <summary>
        /// resolve array 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ArraySymbol ResolveArray( string name )
        {
            if (m_arraySymbolDic.ContainsKey(name))
                return m_arraySymbolDic[name];

            // create a new 
            ArraySymbol symbol = null;

            //TODO 

            return symbol;
        }

        /// <summary>
        /// resolve func
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public FunSymbol ResolveFunc( string name )
        {
            if (m_funSymbolDic.ContainsKey(name))
                return m_funSymbolDic[name];
            else
                throw new ErrorCode(ErrorCode.ERROR_CODE_16);
        }

    }
}
