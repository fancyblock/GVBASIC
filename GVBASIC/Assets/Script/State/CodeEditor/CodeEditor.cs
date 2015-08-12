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

    protected bool m_insertMode;
    protected int m_lineOffset;

	// Use this for initialization
    public override void onSwitchIn() 
    {
        onClearAll();
        m_stateMgr.TextMode();
        m_insertMode = false;

        if (!string.IsNullOrEmpty(m_stateMgr.CUR_SOURCE_CODE))
        {
            // 读取已有文件 
            readCode(m_stateMgr.CUR_SOURCE_CODE);

            m_curLine = 0;
            m_curIndex = 0;
        }

        refresh();
	}

    /// <summary>
    /// key input 
    /// </summary>
    /// <param name="key"></param>
    public override void onInput(KCode key)
    {
        switch (key)
        {
            case KCode.Home:
                onClearAll();
                break;
            case KCode.Return:
                onReturn();
                break;
            case KCode.Delete:
            case KCode.Backspace:
                onDel();
                break;
            case KCode.UpArrow:
            case KCode.DownArrow:
            case KCode.LeftArrow:
            case KCode.RightArrow:
                onMoveCursor(key);
                break;
            case KCode.Escape:
                exportCode();
                m_stateMgr.GotoState(StateEnums.eStateSaver);
                return;
            case KCode.F1:
                // 切换插入/覆盖模式
                m_insertMode = !m_insertMode;
                break;
            case KCode.CapsLock:
                // 切换字母大小写
                m_stateMgr.m_keyboard.SetCaps(!m_stateMgr.m_keyboard.CAPS);
                break;
            default:
                if( key >= KCode.Space && key < KCode.Delete )     // 可输入字符
                {
                    //TODO 
                }
                break;
        }

        refresh();
    }

    
    /// <summary>
    /// 清除
    /// </summary>
    protected void onClearAll()
    {
        m_buffer.Clear();
        m_buffer.Add(new LineInfo());

        m_buffer[0].TEXT = "10 ";
        
        m_curLine = 0;
        m_curIndex = 3;

        m_lineOffset = 0;
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

        if (m_buffer.Count > 1)
        {
            m_buffer.Remove(li);

            if (m_curLine >= m_buffer.Count)
            {
                // 光标移至删除行的上一行
                m_curLine--;
                m_curIndex = m_buffer[m_curLine].GetLastLineIndex(m_curIndex % Defines.TEXT_AREA_WIDTH);
            }
            else
            {
                // 光标不动（自动指向删除行的下一行）
                m_curIndex = m_buffer[m_curLine].GetFirstLineIndex(m_curIndex % Defines.TEXT_AREA_WIDTH);
            }
        }
    }

    protected void onMoveCursor( KCode dir )
    {
        if( dir == KCode.UpArrow )
        {
            if (m_curLine > 0)
            {
                m_curLine--;
                m_curIndex = m_buffer[m_curLine].GetLastLineIndex(m_curIndex);
            }
        }
        else if( dir == KCode.DownArrow )
        {
            if (m_curLine < m_buffer.Count - 1)
            {
                // 移至下一行
                m_curLine++;
                m_curIndex = m_buffer[m_curLine].GetFirstLineIndex(m_curIndex % Defines.TEXT_AREA_WIDTH);
            }
        }

        // 代码按行号排序
        sortCode();
    }

    /// <summary>
    /// 回车
    /// </summary>
    protected void onReturn()
    {
        if (m_curLine == m_buffer.Count - 1)
        {
            // 插入新行  
            if (m_buffer[m_curLine].LENGTH > 0)
            {
                // 自动设置行号
                //TODO 

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

        // 代码按行号排序
        sortCode();
    }

    protected void refresh()
    {
        m_stateMgr.m_textDisplay.Clear();

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
        m_stateMgr.m_textDisplay.SetCursor(x, y + m_lineOffset);

        // 绘制文本行
        y = 0;
        foreach (LineInfo li in m_buffer)
        {
            int yStartPos = y + m_lineOffset;
            int yEndPos = yStartPos + li.LENGTH - 1;

            if ( !((yStartPos < 0 && yEndPos < 0) || (yStartPos >= Defines.TEXT_AREA_HEIGHT && yEndPos >= Defines.TEXT_AREA_HEIGHT)) )
                m_stateMgr.m_textDisplay.DrawText(0, yStartPos, li.TEXT);

            y += li.LINE_COUNT;
        }

        // 刷新
        m_stateMgr.m_textDisplay.Refresh();
    }

    protected void exportCode()
    {
        StringBuilder code = new StringBuilder();

        foreach( LineInfo li in m_buffer )
            code.AppendLine(li.TEXT);

        m_stateMgr.CUR_SOURCE_CODE = code.ToString();
    }

    /// <summary>
    /// 刷新源代码，按行号排序
    /// </summary>
    protected void sortCode()
    {
        //TODO 
    }

}
