using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GVBASIC_Compiler.Compiler;

public class EmuAPI : MonoBehaviour, IAPI
{
    public CodeRuntime m_codeRuntime;
    public TextDisplay m_textDisplay;
    public GraphDisplay m_graphDisplay;

    protected int m_state;

    /// <summary>
    /// show error code 
    /// </summary>
    /// <param name="error"></param>
    public void ErrorCode(string error) 
    {
        m_textDisplay.Clean();
        m_textDisplay.SetCursor(false);

        m_textDisplay.DrawText(0, 0, error);
        m_textDisplay.Refresh();
    }

    /// <summary>
    /// program start 
    /// </summary>
    public void ProgramStart() 
    {
        m_textDisplay.Clean();
        m_textDisplay.Refresh();
    }

    /// <summary>
    /// on program done 
    /// </summary>
    public void ProgramDone() 
    {
        //TODO 
    }

    public void Print(List<BaseData> expList) { }
    public void Beep() { }
    public void Cls() { }
    public void Inverse() { }
    public void Nromal() { }
    public void Graph() { }
    public void Text() { }
    public int Inkey() { return 0; }
}
