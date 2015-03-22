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
        protected BuildinFunc m_buildinFunc = null;
        protected Parser m_parser = null;
        protected Context m_context = null;

        /// <summary>
        /// constructor 
        /// </summary>
        public Runtime(Parser parser)
        {
            m_context = new Context();
            m_buildinFunc = new BuildinFunc();
            m_buildinFunc.SetContext(m_context);

            m_parser = parser;
        }

        /// <summary>
        /// run the program
        /// </summary>
        public void Run()
        {
			m_context.Reset();

			//TODO 
        }


		//-------------------- private function --------------------

        //TODO 

    }
}
