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
    /// getter of the length 
    /// </summary>
    public int LENGTH
    {
        get
        {
            return m_buffer.Length;
        }
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
    }

    /// <summary>
    /// count of line 
    /// </summary>
    public int LINE_COUNT
    {
        get
        {
            return Mathf.CeilToInt((float)m_buffer.Length / (float)Defines.TEXT_AREA_WIDTH);
        }
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
        else
            idx = m_buffer.Length - 1;

        return idx;
    }

}
