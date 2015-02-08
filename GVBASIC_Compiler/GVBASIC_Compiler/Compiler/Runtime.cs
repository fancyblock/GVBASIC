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
        protected RuntimeContext m_context = null;

        /// <summary>
        /// constructor 
        /// </summary>
        public Runtime()
        {
            m_context = new RuntimeContext();
            m_buildinFunc = new BuildinFunc();
            m_buildinFunc.SetContext(m_context);
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
			m_context.Reset();

			while( isProgramEnd() == false )
			{
				Token tok = getNextToken();

				//TODO 
			}
        }


		//-------------------- private function --------------------

		/// <summary>
		/// get next token 
		/// </summary>
		/// <returns>The next token.</returns>
		protected Token getNextToken()
		{
			Token tok = null;

			List<CodeLine> clList = m_parser.CODE_LINES;
			int curCodeLine = m_context.CURRENT_CODE_LINE;
			int curTokenIdx = m_context.CURRENT_TOKEN_INDEX;

			if ( curCodeLine < clList.Count && 
				 curTokenIdx < clList[curCodeLine].m_tokenCount)
			{
				tok = clList[curCodeLine].m_tokens[curTokenIdx];

				// move pointer to the next token 
				m_context.CURRENT_TOKEN_INDEX++;

				if (m_context.CURRENT_TOKEN_INDEX >= clList[curCodeLine].m_tokenCount)
				{
					m_context.CURRENT_TOKEN_INDEX = 0;
					m_context.CURRENT_CODE_LINE++;
				}
			}

			return tok;
		}

		/// <summary>
		/// if the program end or not 
		/// </summary>
		/// <returns><c>true</c>, if program end was ised, <c>false</c> otherwise.</returns>
		protected bool isProgramEnd()
		{
			List<CodeLine> clList = m_parser.CODE_LINES;
			int curCodeLine = m_context.CURRENT_CODE_LINE;
			int curTokenIdx = m_context.CURRENT_TOKEN_INDEX;

			if (curCodeLine < clList.Count &&
			    curTokenIdx < clList[curCodeLine].m_tokenCount)
			{
                return false;
			}

            return true;
		}

    }
}
