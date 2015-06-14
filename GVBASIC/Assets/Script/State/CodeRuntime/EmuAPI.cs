using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GVBASIC_Compiler.Compiler;

public class EmuAPI : MonoBehaviour, IAPI
{
    public CodeRuntime m_codeRuntime;
    public TextDisplay m_textDisplay;
    public GraphDisplay m_graphDisplay;

    protected int m_status;

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

        m_status = Defines.PROGRAM_STATUS_ERROR;
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
        m_status = Defines.PROGRAM_STATUS_END;
    }

    public void Print(List<BaseData> expList) 
    {
        int lastType = -1;
        bool closeTo = false;
        string output = null;

        foreach (BaseData dat in expList)
        {
            output = "";

            switch (dat.TYPE)
            {
                case BaseData.TYPE_FLOAT:
                    output = dat.FLOAT_VAL.ToString();
                    break;
                case BaseData.TYPE_INT:
                    output = dat.INT_VAL.ToString();
                    break;
                case BaseData.TYPE_STRING:
                    output = dat.STR_VAL;
                    break;
                case BaseData.TYPE_NEXT_LINE:
                    if (lastType == BaseData.TYPE_FLOAT || lastType == BaseData.TYPE_INT || lastType == BaseData.TYPE_STRING)
                        output = "\n";
                    break;
                case BaseData.TYPE_SPACE:
                    for (int i = 0; i < dat.INT_VAL; i++)
                        output = " ";
                    break;
                case BaseData.TYPE_TAB:
                    //TODO 
                    break;
                case BaseData.TYPE_CLOSE_TO:
                    if (lastType == BaseData.TYPE_FLOAT || lastType == BaseData.TYPE_INT || lastType == BaseData.TYPE_STRING)
                        closeTo = true;
                    break;
                default:
                    break;
            }

            //m_textDisplay.DrawText()
            //System.Console.Write(output);

            lastType = dat.TYPE;
        }

        if (!(lastType == BaseData.TYPE_CLOSE_TO && closeTo))
        {
            //System.Console.Write("\n");
        }
    }

    public void Beep() { }
    public void Cls() { }
    public void Inverse() { }
    public void Nromal() { }
    public void Graph() { }
    public void Text() { }
    public int Inkey() { return 0; }
}
