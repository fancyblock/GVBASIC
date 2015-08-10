using UnityEngine;
using System.Collections;
using System.Text;

public class LineInfo
{
    protected StringBuilder m_buffer;

    /// <summary>
    /// default constructor 
    /// </summary>
    public LineInfo()
    {
        m_buffer = new StringBuilder();
    }

    /// <summary>
    /// constructor 
    /// </summary>
    /// <param name="lineStr"></param>
    public LineInfo( string lineStr )
    {
        m_buffer = new StringBuilder(lineStr);
    }

    /// <summary>
    /// getter of the length 
    /// </summary>
    public int LENGTH { get { return m_buffer.Length; } }

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
        else if( key == KCode.UpArrow )
        {
            if (curIndex >= Defines.TEXT_AREA_WIDTH)
                newIndex = curIndex - Defines.TEXT_AREA_WIDTH;
            else
                newIndex = -1;
        }
        else if( key == KCode.DownArrow)
        {
            if (curIndex + Defines.TEXT_AREA_WIDTH < LENGTH)
                newIndex = curIndex + Defines.TEXT_AREA_WIDTH;
            else
                newIndex = -1;
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

        return newIndex;
    }

    /// <summary>
    /// remove index 
    /// </summary>
    /// <param name="index"></param>
    public void Remove(int index)
    {
        m_buffer.Remove(index, 1);
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
            m_buffer.Insert(index, chr);       // add to the end of line 
    }

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

}
