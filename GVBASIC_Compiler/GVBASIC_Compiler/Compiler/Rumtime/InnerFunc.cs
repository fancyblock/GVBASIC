using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVBASIC_Compiler.Compiler
{
    public class InnerFunc
    {
        protected IAPI m_iapi;
        protected Dictionary<string, Func<List<BaseData>, BaseData>> m_funcDic;

        /// <summary>
        /// constructor
        /// </summary>
        public InnerFunc()
        {
            // initial the functions 
            m_funcDic = new Dictionary<string, Func<List<BaseData>, BaseData>>();

            //TODO 
        }

        /// <summary>
        /// set api 
        /// </summary>
        /// <param name="apiCall"></param>
        public void SetAPI( IAPI apiCall )
        {
            m_iapi = apiCall;
        }
        
        /// <summary>
        /// has function or not 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool HasFunc( string name )
        {
            return m_funcDic.ContainsKey(name);
        }

        /// <summary>
        /// call function 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public BaseData CallFunc( string name, List<BaseData> parameters )
        {
            BaseData result = null;

            //TODO 

            return result;
        }

    }
}
