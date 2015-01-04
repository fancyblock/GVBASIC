using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVBASIC_Compiler.Compiler
{
    public class BaseFunc : IAPICall
    {
        protected RuntimeContext m_context = null;

        /// <summary>
        /// constructor 
        /// </summary>
        public BaseFunc()
        {
        }

        /// <summary>
        /// set context 
        /// </summary>
        /// <param name="context"></param>
        public void SetContext(RuntimeContext context)
        {
            m_context = context;
        }

        /// <summary>
        /// has function 
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        public bool HasFunction(Function func)
        {
            //TODO 

            return false;
        }

        /// <summary>
        /// call function 
        /// </summary>
        /// <param name="func"></param>
        public void CallFunction(Function func)
        {
            //TODO 
        }

    }
}
