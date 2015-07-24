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

    protected List<char> m_inkeyBuff;
    protected List<string> m_inputBuff;

    /// <summary>
    /// show error code 
    /// </summary>
    /// <param name="error"></param>
    public void ErrorCode(string error) 
    {
        m_textDisplay.Clear();

        m_textDisplay.DrawText(0, 0, error);
        m_textDisplay.Refresh();

        m_status = Defines.PROGRAM_STATUS_ERROR;
    }

    /// <summary>
    /// program start 
    /// </summary>
    public void ProgramStart() 
    {
        m_textDisplay.Clear();
        m_textDisplay.Refresh();

        m_inkeyBuff = new List<char>();
        m_inputBuff = new List<string>();
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
            cursorX = 0;
            cursorY = 0;

            if( cursorY >= Defines.TEXT_AREA_HEIGHT )
            {
                m_textDisplay.RollUp( cursorY - Defines.TEXT_AREA_HEIGHT + 1 );
                cursorY = Defines.TEXT_AREA_HEIGHT - 1;
            }

            m_textDisplay.SetCursor(cursorX, cursorY);

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

            m_textDisplay.SetCursor(cursorX, cursorY);
        }

        m_textDisplay.Refresh();
    }

    /// <summary>
    /// set the cursor 
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void Locate(int x, int y)
    {
        m_textDisplay.SetCursor( x-1, y-1);
    }

    public int CursorX()
    {
        return m_textDisplay.CURSOR_X;
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
        m_textDisplay.Clear();
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
        m_codeRuntime.MGR.GraphMode();
    }

    /// <summary>
    /// text mode 
    /// </summary>
    public void Text() 
    {
        m_codeRuntime.MGR.TextMode();
    }

    public void AskInkey(int count)
    {
        m_codeRuntime.GetInkey(count);
    }

    public bool HasInkeyBuffer()
    {
        return m_inkeyBuff.Count > 0;
    }

    /// <summary>
    /// get a inkey 
    /// </summary>
    /// <returns></returns>
    public string Inkey() 
    {
        char c = m_inkeyBuff[0];
        // remove the return key 
        m_inkeyBuff.RemoveAt(0);

        return c.ToString(); ;
    }

    public void AppendInkey( KCode kcode )
    {
        if (kcode < KCode.Delete)
            m_inkeyBuff.Add((char)kcode);
        else
            m_inkeyBuff.Add(' ');
    }

    public bool HasInputBuffer()
    {
        return m_inputBuff.Count > 0;
    }

    public void AskInput(int count)
    {
        m_codeRuntime.GetInput(count);
    }

    public string Input()
    {
        string str = m_inputBuff[0];
        m_inputBuff.RemoveAt(0);

        return str;
    }

    public void AppendInput(string str)
    {
        m_inputBuff.Add(str);
    }

    /// <summary>
    /// play music 
    /// </summary>
    /// <param name="sound"></param>
    public void Play(string sound)
    {
        //TODO 
    }

    /// <summary>
    /// draw a box 
    /// </summary>
    /// <param name="x0"></param>
    /// <param name="y0"></param>
    /// <param name="x1"></param>
    /// <param name="y1"></param>
    /// <param name="fill"></param>
    /// <param name="type"></param>
    public void Box(int x0, int y0, int x1, int y1, int fill, int type)
    {
        bool set = type == 1;

        if( fill == 1 )
        {
            m_led.DrawBox(x0, y0, x1 - x0 + 1, y1 - y0 + 1, set);
        }
        else
        // 画一个矩形框 (中空)
        {
            m_led.DrawBox(x0, y0, x1 - x0 + 1, 1, set);
            m_led.DrawBox(x0, y1, x1 - x0 + 1, 1, set);
            m_led.DrawBox(x0, y0, 1, y1 - y0 + 1, set);
            m_led.DrawBox(x1, y0, 1, y1 - y0 + 1, set);
        }
    }

    /// <summary>
    /// draw a circle 
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="fill"></param>
    /// <param name="type"></param>
    public void Circle(int x, int y, int r, int fill, int type)
    {
        //TODO 
    }

    /// <summary>
    /// draw a point 
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="type"></param>
    public void Draw(int x, int y, int type)
    {
        m_graphDisplay.DrawPixel(x, y, type == 1);
    }

    /// <summary>
    /// draw a ellipse 
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <param name="fill"></param>
    /// <param name="type"></param>
    public void Ellipse(int x, int y, int a, int b, int fill, int type)
    {
        //TODO 
    }

    /// <summary>
    /// draw a line 
    /// </summary>
    /// <param name="x0"></param>
    /// <param name="y0"></param>
    /// <param name="x1"></param>
    /// <param name="y1"></param>
    /// <param name="type"></param>
    public void Line(int x0, int y0, int x1, int y1, int type)
    {
        //TODO 
    }

}
