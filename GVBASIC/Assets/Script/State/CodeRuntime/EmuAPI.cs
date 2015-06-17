using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GVBASIC_Compiler.Compiler;

public class EmuAPI : MonoBehaviour, IAPI
{
    public CodeRuntime m_codeRuntime;
    public TextDisplay m_textDisplay;
    public GraphDisplay m_graphDisplay;
    public LED m_led;

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

        int cursorX = m_textDisplay.CURSOR_X;
        int cursorY = m_textDisplay.CURSOR_Y;

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

            m_textDisplay.DrawText(cursorX, cursorY, output);

            // set the new cursor position 
            cursorX = m_textDisplay.LAST_TEXT_X;
            cursorY = m_textDisplay.LAST_TEXT_Y;

            if( cursorY >= Defines.TEXT_AREA_HEIGHT )
            {
                m_textDisplay.RollUp( cursorY - Defines.TEXT_AREA_HEIGHT + 1 );
                cursorY = Defines.TEXT_AREA_HEIGHT - 1;
            }

            m_textDisplay.SetCursor(true, cursorX, cursorY);

            lastType = dat.TYPE;
        }

        if (!(lastType == BaseData.TYPE_CLOSE_TO && closeTo))
        {
            cursorX = 0;
            cursorY++;

            if (cursorY >= Defines.TEXT_AREA_HEIGHT)
            {
                m_textDisplay.RollUp(cursorY - Defines.TEXT_AREA_HEIGHT + 1);
                cursorY = Defines.TEXT_AREA_HEIGHT - 1;
            }

            m_textDisplay.SetCursor(true, cursorX, cursorY);
        }

        m_textDisplay.Refresh();
    }

    /// <summary>
    /// play a beep sound 
    /// </summary>
    public void Beep() 
    {
        //TODO 
    }

    /// <summary>
    /// clean the screen 
    /// </summary>
    public void Cls() 
    {
        m_textDisplay.Clean();
        m_textDisplay.Refresh();
    }

    /// <summary>
    /// inverse mode 
    /// </summary>
    public void Inverse() 
    {
        //TODO 
    }

    /// <summary>
    /// normal mode 
    /// </summary>
    public void Normal() 
    {
        //TODO 
    }

    /// <summary>
    /// switch to graph mode 
    /// </summary>
    public void Graph() 
    {
        m_textDisplay.enabled = false;
        m_graphDisplay.enabled = true;

        m_led.CleanScreen();
    }

    /// <summary>
    /// text mode 
    /// </summary>
    public void Text() 
    {
        m_textDisplay.enabled = true;
        m_graphDisplay.enabled = false;

        m_led.CleanScreen();

        m_textDisplay.SetCursor(true);
    }

    /// <summary>
    /// get a inkey 
    /// </summary>
    /// <returns></returns>
    public int Inkey() 
    {
        //TODO 

        return 0;
    }

}
