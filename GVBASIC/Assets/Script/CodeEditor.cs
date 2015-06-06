using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;


/// <summary>
/// code editor 
/// </summary>
public class CodeEditor : MonoBehaviour 
{
    public TextDisplay m_textDisplay;

    protected List<StringBuilder> m_buffer;
    protected int m_curLine;
    protected int m_curIndex;

    protected bool m_isInsertMode;

	// Use this for initialization
	void Start () 
    {
        m_buffer = new List<StringBuilder>();
        onClearAll();

        m_textDisplay.Refresh();
	}

    /// <summary>
    /// key input 
    /// </summary>
    /// <param name="key"></param>
    public void Input( KeyCode key )
    {
        switch( key )
        {
            case KeyCode.Home:
                onClearAll();
                break;
            case KeyCode.Return:
                onEnter();
                break;
            case KeyCode.Delete:
                onDel();
                break;
            case KeyCode.Insert:
                m_isInsertMode = !m_isInsertMode;
                break;
            case KeyCode.UpArrow:
            case KeyCode.DownArrow:
            case KeyCode.LeftArrow:
            case KeyCode.RightArrow:
                onMoveCursor(key);
                break;
            default:
                onChar(key);
                break;
        }

        refreshLED();
    }

    
    protected void onClearAll()
    {
        m_buffer.Clear();
        m_buffer.Add(new StringBuilder());
        
        m_curLine = 0;
        m_curIndex = 0;

        m_textDisplay.Clean();
        m_textDisplay.SetCursor(true, 0, 0);
    }

    protected void onEnter()
    {
        //TODO 
    }

    protected void onDel()
    {
        StringBuilder sb = m_buffer[m_curLine];

        if (sb.Length == 0)
            return;

        if( m_curIndex < sb.Length )
        {
            sb.Remove(m_curIndex, 1);
        }
        else
        {
            sb.Remove(sb.Length - 1, 1);
            m_curIndex--;
        }
    }

    protected void onMoveCursor( KeyCode dir )
    {
        StringBuilder sb = null;

        if( dir == KeyCode.LeftArrow )
        {
            if (m_curIndex > 0)
                m_curIndex--;
        }
        else if( dir == KeyCode.RightArrow)
        {
            sb = m_buffer[m_curLine];
            if (m_curIndex < sb.Length)
                m_curIndex++;
        }
        else if( dir == KeyCode.UpArrow )
        {
            //TODO 
        }
        else if( dir == KeyCode.DownArrow )
        {
            //TODO 
        }
    }

    protected void onChar( KeyCode key )
    {
        int chr = (int)key;
        if (chr < 0 || chr >= 128)
            return;

        StringBuilder sb = m_buffer[m_curLine];
        if( m_curIndex < sb.Length )
            sb[m_curIndex] = (char)chr;             // replace 
        else
            sb.Insert(m_curIndex, (char)chr);       // add to the end of line 

        m_curIndex++;
    }

    protected void refreshLED()
    {
        m_textDisplay.Clean();

        int y = 0;
        foreach( StringBuilder sb in m_buffer )
        {
            m_textDisplay.DrawText(0, y, sb.ToString());
            y += Mathf.CeilToInt((float)sb.Length / (float)Defines.TEXT_AREA_WIDTH);
        }

        int x = m_curIndex % Defines.TEXT_AREA_WIDTH;
        y = 0;

        for (int i = 0; i <= m_curLine; i++)
        {
            if (i != m_curLine)
                y += Mathf.CeilToInt((float)m_buffer[i].Length / (float)Defines.TEXT_AREA_WIDTH);
            else
                y += m_curIndex / Defines.TEXT_AREA_WIDTH;
        }

        m_textDisplay.SetCursor(true, x, y);
        m_textDisplay.Refresh();
    }

}
