using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVBASIC_Compiler.Compiler
{
    /// <summary>
    /// Basic runtime 
    /// </summary>
    class Runtime
    {
        protected IAPICall m_apiCall = null;
        protected Parser m_parser = null;
        protected RuntimeContext m_context = null;

        /// <summary>
        /// constructor 
        /// </summary>
        public Runtime()
        {
            m_context = new RuntimeContext();
        }

        /// <summary>
        /// set API call 
        /// </summary>
        /// <param name="apiCall"></param>
        public void SetAPICall( IAPICall apiCall )
        {
            m_apiCall = apiCall;
            m_apiCall.SetContext(m_context);
        }

        /// <summary>
        /// set parser 
        /// </summary>
        /// <param name="parser"></param>
        public void SetParser( Parser parser )
        {
            m_parser = parser;
        }

        /// <summary>
        /// run the program
        /// </summary>
        public void Run()
        {
            //TODO 
        }

    }
}
