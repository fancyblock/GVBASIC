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

    protected List<LineInfo> m_buffer;
    protected int m_curLine;
    protected int m_curIndex;

    protected bool m_isInsertMode;

	// Use this for initialization
	void Start () 
    {
        m_buffer = new List<LineInfo>();
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
        m_buffer.Add(new LineInfo());
        
        m_curLine = 0;
        m_curIndex = 0;

        m_textDisplay.Clean();
        m_textDisplay.SetCursor(true, 0, 0);
    }

    protected void onEnter()
    {
        onMoveCursor(KeyCode.DownArrow);
    }

    protected void onDel()
    {
        LineInfo li = m_buffer[m_curLine];

        if (li.LENGTH == 0)
        {
            if( m_buffer.Count > 1 )
            {
                m_buffer.Remove(li);

                if( m_curLine >= m_buffer.Count )
                {
                    // 光标移至删除行的上一行
                    m_curLine--;
                    m_curIndex = m_buffer[m_curLine].GetLastLineIndex(m_curIndex % Defines.TEXT_AREA_WIDTH);
                }
                else
                {
                    // 光标移至删除行的下一行
                    m_curIndex = m_buffer[m_curLine].GetFirstLineIndex(m_curIndex % Defines.TEXT_AREA_WIDTH);
                }
            }
            return;
        }

        if( m_curIndex < li.LENGTH )
        {
            li.Remove(m_curIndex);
        }
        else
        {
            li.Remove(m_curIndex - 1);
            m_curIndex--;
        }
    }

    protected void onMoveCursor( KeyCode dir )
    {
        LineInfo li = m_buffer[m_curLine];;

        if( dir == KeyCode.LeftArrow )
        {
            if (m_curIndex > 0)
                m_curIndex--;
        }
        else if( dir == KeyCode.RightArrow)
        {
            if (m_curIndex < li.LENGTH)
                m_curIndex++;
        }
        else if( dir == KeyCode.UpArrow )
        {
            if (m_curIndex >= Defines.TEXT_AREA_WIDTH)
            {
                m_curIndex -= Defines.TEXT_AREA_WIDTH;
            }
            else
            {
                if( m_curLine > 0 )
                {
                    m_curLine--;
                    m_curIndex = m_buffer[m_curLine].GetLastLineIndex(m_curIndex);
                }
            }
        }
        else if( dir == KeyCode.DownArrow )
        {
            if( m_curIndex + Defines.TEXT_AREA_WIDTH < li.LENGTH )
            {
                m_curIndex += Defines.TEXT_AREA_WIDTH;
            }
            else
            {
                if( m_curLine == m_buffer.Count - 1 )
                {
                    // 插入新行  
                    m_buffer.Add(new LineInfo());
                    m_curLine = m_buffer.Count - 1;
                    m_curIndex = 0;
                }
                else
                {
                    m_curLine++;
                    m_curIndex = m_buffer[m_curLine].GetFirstLineIndex(m_curIndex % Defines.TEXT_AREA_WIDTH);
                }
            }
        }
    }

    protected void onChar( KeyCode key )
    {
        int chr = (int)key;
        if (chr < 0 || chr >= 128)
            return;

        LineInfo li = m_buffer[m_curLine];
        li.SetChar(m_curIndex, (char)chr);

        m_curIndex++;
    }

    protected void refreshLED()
    {
        m_textDisplay.Clean();

        // 绘制文本行
        int y = 0;
        foreach( LineInfo li in m_buffer )
        {
            m_textDisplay.DrawText(0, y, li.TEXT);
            y += li.LINE_COUNT;
        }

        // 计算光标位置
        int x = m_curIndex % Defines.TEXT_AREA_WIDTH;
        y = 0;

        for (int i = 0; i < m_curLine; i++)
            y += m_buffer[i].LINE_COUNT;
        y += m_curIndex / Defines.TEXT_AREA_WIDTH;

        // 设置光标
        m_textDisplay.SetCursor(true, x, y);
        // 刷新
        m_textDisplay.Refresh();
    }

}
