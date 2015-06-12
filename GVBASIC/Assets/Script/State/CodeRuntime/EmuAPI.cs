using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GVBASIC_Compiler.Compiler;

public class EmuAPI : MonoBehaviour, IAPI
{
    public void ErrorCode(string error) { }
    public void ProgramDone() { }

    public void Print(List<BaseData> expList) { }
    public void Beep() { }
    public void Cls() { }
    public void Inverse() { }
    public void Nromal() { }
    public void Graph() { }
    public void Text() { }
    public int Inkey() { return 0; }
}
