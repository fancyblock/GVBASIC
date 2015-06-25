using System;
using System.Collections.Generic;

namespace GVBASIC_Compiler.Compiler
{
    public interface IAPI
    {
        void ErrorCode(string error);
        void ProgramStart();
        void ProgramDone();

        void Print(List<BaseData> expList);
        void Locate(int x, int y);
        int CursorX();  // 得到光标的水平位置
        void Beep();
        void Cls();
        void Inverse();
        void Normal();
        void Graph();
        void Text();
        void Play(string sound);
        void Box(int x0, int y0, int x1, int y1, int fill, int type);
        void Circle(int x, int y, int r, int fill, int type);
        void Draw(int x, int y, int type);          // draw point 
        void Ellipse(int x, int y, int a, int b, int fill, int type);
        void Line(int x0, int y0, int x1, int y1, int type);
        string Inkey();
        void WaittingInkey();
        void CleanInkeyBuff();

    }
}
