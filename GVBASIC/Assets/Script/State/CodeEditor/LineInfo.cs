using UnityEngine;
using System.Collections;
using System.Text;

public class LineInfo
{
    protected StringBuilder m_buffer;
    protected int m_lineNum;
    protected bool m_newLine;

    protected StringBuilder m_tempBuffer = new StringBuilder();

    /// <summary>
    /// default constructor 
    /// </summary>
    public LineInfo()
    {
        m_buffer = new StringBuilder();
        m_newLine = true;

        m_lineNum = -1;
    }

    /// <summary>
    /// constructor 
    /// </summary>
    /// <param name="lineStr"></param>
    public LineInfo( string lineStr )
    {
        m_buffer = new StringBuilder(lineStr);
        m_newLine = true;

        refreshLineNum();
    }

    /// <summary>
    /// getter of the length 
    /// </summary>
    public int LENGTH { get { return m_buffer.Length; } }

    /// <summary>
    /// getter of the line status 
    /// </summary>
    public bool IS_NEW_LINE 
    { 
        get { return m_newLine; }
        set { m_newLine = value; }
    }

    /// <summary>
    /// getter of the text 
    /// </summary>
    public string TEXT 
    { 
        get 
        { 
            return m_buffer.ToString(); 
        }
        set
        {
            m_buffer.Remove(0, m_buffer.Length);
            m_buffer.Append(value);

            refreshLineNum();
        }
    }

    /// <summary>
    /// count of line 
    /// </summary>
    public int LINE_COUNT
    {
        get
        {
            int len = m_buffer.Length;

            if (len == 0)
                return 1;

            return Mathf.CeilToInt((float)len / (float)Defines.TEXT_AREA_WIDTH);
        }
    }

    /// <summary>
    /// key input 
    /// </summary>
    /// <param name="key"></param>
    /// <param name="curIndex"></param>
    /// <returns></returns>
    public int KeyInput( KCode key, int curIndex )
    {
        // move the cursor 
        int newIndex = curIndex;

        if (key == KCode.LeftArrow)
        {
            if (curIndex > 0)
                newIndex = curIndex - 1;
        }
        else if (key == KCode.RightArrow)
        {
            if (curIndex < LENGTH)
                newIndex = curIndex + 1;
        }
        else if( key == KCode.Delete )
        {
            if( LENGTH > 0 )
            {
                if (curIndex < LENGTH)
                {
                    Remove(curIndex);
                }
                else
                {
                    Remove(curIndex - 1);
                    newIndex = curIndex - 1;
                }
            }
            else
            {
                newIndex = -1;
            }
        }
        else
        {
            int chr = (int)key;

            // don't process this key input
            if (chr < 0 || chr >= 127 || key == KCode.Return || key == KCode.Escape)
            {
                newIndex = -1;
            }
            else
            {
                SetChar(curIndex, (char)chr);
                newIndex = curIndex + 1;
            }
        }

        refreshLineNum();

        return newIndex;
    }

    /// <summary>
    /// 获取行号
    /// </summary>
    /// <param name="lineNum"></param>
    /// <returns></returns>
    public int LINE_NUM { get { return m_lineNum; } }

    /// <summary>
    /// TODO 
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public int GetLastLineIndex( int index )
    {
        int idx = 0;

        idx = m_buffer.Length % Defines.TEXT_AREA_WIDTH;
        if (idx >= index)
            idx = index;

        idx += (m_buffer.Length / Defines.TEXT_AREA_WIDTH) * Defines.TEXT_AREA_WIDTH;

        return idx;
    }

    public int GetFirstLineIndex( int index )
    {
        int idx = 0;

        if (m_buffer.Length > index)
            idx = index;
        else if (m_buffer.Length == 0)
            idx = 0;
        else
            idx = m_buffer.Length - 1;

        return idx;
    }

    /// <summary>
    /// set char 
    /// </summary>
    /// <param name="index"></param>
    /// <param name="chr"></param>
    public void SetChar(int index, char chr)
    {
        if (index < m_buffer.Length)
            m_buffer[index] = chr;             // replace 
        else
            m_buffer.Append(chr);              // add to the end of line 

        m_newLine = false;

        refreshLineNum();
    }

    /// <summary>
    /// 插入一个字符
    /// </summary>
    /// <param name="index"></param>
    /// <param name="chr"></param>
    public void InsChar( int index, char chr )
    {
        if (index < m_buffer.Length)
            m_buffer.Insert( index, chr );     // 插入
        else
            m_buffer.Append(chr);              // 添加到末尾

        m_newLine = false;

        refreshLineNum();
    }

    /// <summary>
    /// remove index 
    /// </summary>
    /// <param name="index"></param>
    public void Remove(int index)
    {
        m_buffer.Remove(index, 1);
        refreshLineNum();
    }


    /// <summary>
    /// 更新行号
    /// </summary>
    protected void refreshLineNum()
    {
        if (m_buffer.Length == 0)
        {
            m_lineNum = -1;
            return;
        }

        if( m_tempBuffer.Length > 0 )
            m_tempBuffer.Remove(0, m_tempBuffer.Length);
        
        for( int i = 0; i < m_buffer.Length; i++ )
        {
            char c = m_buffer[i];

            // 加入数字
            if (char.IsNumber(c))
                m_tempBuffer.Append(c);
            else
                break;
        }

        string str = m_tempBuffer.ToString();

        if (!int.TryParse(str, out m_lineNum))
            m_lineNum = -1;
    }

}
