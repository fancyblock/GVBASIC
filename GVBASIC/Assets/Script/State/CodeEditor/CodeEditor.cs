using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;


/// <summary>
/// code editor 
/// </summary>
public class CodeEditor : State 
{
    protected List<LineInfo> m_buffer = new List<LineInfo>();
    protected int m_curLine;
    protected int m_curIndex;

    protected bool m_isInsertMode;
    protected int m_lineOffset;

	// Use this for initialization
    public override void onSwitchIn() 
    {
        onClearAll();
        readCode(m_stateMgr.CUR_SOURCE_CODE);

        refreshLED();
	}

    /// <summary>
    /// key input 
    /// </summary>
    /// <param name="key"></param>
    public override void onInput(KCode key)
    {
        switch( key )
        {
            case KCode.Home:
                onClearAll();
                break;
            case KCode.Return:
                onMoveCursor(KCode.DownArrow);
                break;
            case KCode.Delete:
                onDel();
                break;
            case KCode.Insert:
                m_isInsertMode = !m_isInsertMode;
                break;
            case KCode.UpArrow:
            case KCode.DownArrow:
            case KCode.LeftArrow:
            case KCode.RightArrow:
                onMoveCursor(key);
                break;
            case KCode.Escape:
                m_stateMgr.GotoState(StateEnums.eStateMenu);
                return;
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

        m_lineOffset = 0;

        m_textDisplay.Clean();
        m_textDisplay.SetCursor(true, 0, 0);
    }

    protected void readCode( string code )
    {
        m_buffer.Clear();

        using( StringReader sr = new StringReader(code) )
        {
            string codeLine = sr.ReadLine();

            while( codeLine != null )
            {
                m_buffer.Add(new LineInfo( codeLine ));
                codeLine = sr.ReadLine();
            }
        }
        
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

    protected void onMoveCursor( KCode dir )
    {
        LineInfo li = m_buffer[m_curLine];;

        if( dir == KCode.LeftArrow )
        {
            if (m_curIndex > 0)
                m_curIndex--;
        }
        else if( dir == KCode.RightArrow)
        {
            if (m_curIndex < li.LENGTH)
                m_curIndex++;
        }
        else if( dir == KCode.UpArrow )
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
        else if( dir == KCode.DownArrow )
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
                    if (m_buffer[m_curLine].LENGTH > 0)
                    {
                        m_buffer.Add(new LineInfo());
                        m_curLine = m_buffer.Count - 1;
                        m_curIndex = 0;
                    }
                }
                else
                {
                    // 移至下一行
                    m_curLine++;
                    m_curIndex = m_buffer[m_curLine].GetFirstLineIndex(m_curIndex % Defines.TEXT_AREA_WIDTH);
                }
            }
        }
    }

    protected void onChar( KCode key )
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

        // 计算光标新位置
        int x = m_curIndex % Defines.TEXT_AREA_WIDTH;
        int y = 0;

        for (int i = 0; i < m_curLine; i++)
            y += m_buffer[i].LINE_COUNT;
        y += m_curIndex / Defines.TEXT_AREA_WIDTH;
        
        // 光标超出范围，滚动屏幕
        if( ( y + m_lineOffset ) < 0 )
            m_lineOffset -= (y + m_lineOffset);
        else if ( ( y + m_lineOffset ) >= Defines.TEXT_AREA_HEIGHT)
            m_lineOffset -= ( y + m_lineOffset - Defines.TEXT_AREA_HEIGHT + 1 );

        // 设置光标
        m_textDisplay.SetCursor(true, x, y + m_lineOffset);

        // 绘制文本行
        y = 0;
        foreach (LineInfo li in m_buffer)
        {
            int yStartPos = y + m_lineOffset;
            int yEndPos = yStartPos + li.LENGTH - 1;

            if ( !((yStartPos < 0 && yEndPos < 0) || (yStartPos >= Defines.TEXT_AREA_HEIGHT && yEndPos >= Defines.TEXT_AREA_HEIGHT)) )
                m_textDisplay.DrawText(0, yStartPos, li.TEXT);

            y += li.LINE_COUNT;
        }

        // 刷新
        m_textDisplay.Refresh();
    }

}
