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
            case KCode.UpArrow:
            case KCode.DownArrow:
            case KCode.LeftArrow:
            case KCode.RightArrow:
                onMoveCursor(key);
                break;
            case KCode.Delete:
            case KCode.Backspace:
                onDel();
                break;
            case KCode.Escape:
                exportCode();
                m_stateMgr.GotoState(StateEnums.eStateSaver);
                return;
            case KCode.F1:
                m_insertMode = !m_insertMode;                       // 切换插入/覆盖模式
                //TODO 
                break;
            case KCode.CapsLock:
                // 切换字母大小写
                m_stateMgr.m_keyboard.SetCaps(!m_stateMgr.m_keyboard.CAPS);
                break;
            default:
                if (key >= KCode.Space && key < KCode.Delete)       // 可输入字符
                {
                    if( m_insertMode )
                        m_buffer[m_curLine].InsChar(m_curIndex, (char)key);
                    else
                        m_buffer[m_curLine].SetChar(m_curIndex, (char)key);

                    m_curIndex++;
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
        m_buffer[0].IS_NEW_LINE = true;
        
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
                LineInfo line = new LineInfo(codeLine);
                line.IS_NEW_LINE = false;
                m_buffer.Add(line);

                codeLine = sr.ReadLine();
            }
        }
    }

    /// <summary>
    /// 回车，创建新行（自动补行号）或者移至下一行，新行上回车会将新行移至第一行
    /// </summary>
    protected void onReturn()
    {
        LineInfo line = m_buffer[m_curLine];

        if (line.IS_NEW_LINE)   // 卷屏至最上?
            return;

        // 负数代表这行行号错误或者没行号
        int lineNum = line.LINE_NUM;
        if ( lineNum < 0 )
        {
            m_stateMgr.NotifierInfo("Line number error !");
            return;
        }

        if( m_curLine == m_buffer.Count - 1 )
        {
            // 创建新行  
            lineNum += 10;
            LineInfo newLine = new LineInfo( lineNum + " " );

            m_buffer.Add( newLine );
            m_curLine = m_buffer.Count - 1;
            m_curIndex = newLine.LENGTH;
        }
        else
        {
            // 移至下行 
            m_curLine++;
            m_curIndex = 0;
        }

        // 代码按行号排序
        sortCode();
    }

    /// <summary>
    /// 字符输入
    /// </summary>
    /// <param name="code"></param>
    public void onInputKey( KCode code )
    {
        //TODO 
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
        LineInfo li = m_buffer[m_curLine];

        if( dir == KCode.UpArrow )
        {
            if (m_curIndex >= Defines.TEXT_AREA_WIDTH)
            {
                m_curIndex -= Defines.TEXT_AREA_WIDTH;
            }
            else if(m_curLine > 0)
            {
                m_curLine--;
                m_curIndex = m_buffer[m_curLine].GetLastLineIndex(m_curIndex);
            }
        }
        else if( dir == KCode.DownArrow )
        {
            if (m_curIndex + Defines.TEXT_AREA_WIDTH < li.LENGTH)
            {
                m_curIndex += Defines.TEXT_AREA_WIDTH;
            }
            else if (m_curLine < m_buffer.Count - 1)
            {
                // 移至下一行
                m_curLine++;
                m_curIndex = m_buffer[m_curLine].GetFirstLineIndex(m_curIndex % Defines.TEXT_AREA_WIDTH);
            }
        }
        else if( dir == KCode.LeftArrow )
        {
            if (m_curIndex > 0)
                m_curIndex--;
        }
        else if( dir == KCode.RightArrow )
        {
            if (m_curIndex < li.LENGTH)
                m_curIndex++;
        }
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

        foreach (LineInfo li in m_buffer)
        {
            if( !li.IS_NEW_LINE )
                code.AppendLine(li.TEXT);
        }

        m_stateMgr.CUR_SOURCE_CODE = code.ToString();
    }

    /// <summary>
    /// 刷新源代码，按行号排序
    /// </summary>
    protected void sortCode()
    {
        m_buffer.Sort((LineInfo line1, LineInfo line2) =>
        {
            return line1.LINE_NUM - line2.LINE_NUM;
        });
    }

}
