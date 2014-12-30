using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVBASIC_Compiler.Compiler
{
    /// <summary>
    /// API call interface 
    /// </summary>
    interface IAPICall
    {
        void SetContext(RuntimeContext context);
        void CallFunction(Func func);
    }
}
