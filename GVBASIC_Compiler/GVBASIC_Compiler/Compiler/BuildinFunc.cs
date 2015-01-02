using System;

namespace GVBASIC_Compiler.Compiler
{
    public class BuildinFunc : IAPICall
    {
        protected RuntimeContext m_context = null;

        /// <summary>
        /// Constructor 
        /// </summary>
        public BuildinFunc()
        {
        }

        /// <summary>
        /// set context 
        /// </summary>
        /// <param name="context"></param>
        public void SetContext( RuntimeContext context )
        {
            m_context = context;
        }

        /// <summary>
        /// Determines whether this instance has function the specified func.
        /// </summary>
        /// <returns><c>true</c> if this instance has function the specified func; otherwise, <c>false</c>.</returns>
        /// <param name="func">Func.</param>
        public bool HasFunction( Function func )
        {
            //TODO 

            return true;
        }

        /// <summary>
        /// call function 
        /// </summary>
        /// <param name="func"></param>
        public void CallFunction( Function func )
        {
            //TODO 
        }
    }
}

