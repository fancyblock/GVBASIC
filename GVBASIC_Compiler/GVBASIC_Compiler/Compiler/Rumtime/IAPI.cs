using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVBASIC_Compiler.Compiler
{
    public interface IAPI
    {
        void ErrorCode(string error);
        void ProgramDone();

        void Print(List<BaseData> expList);
        void Beep();
        void Cls();
        void Inverse();
        void Nromal();
        void Graph();
        void Text();
        int Inkey();
    }
}
