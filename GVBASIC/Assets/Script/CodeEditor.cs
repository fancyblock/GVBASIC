using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;


public class CodeEditor : MonoBehaviour 
{
    public TextDisplay m_textDisplay;
    public float m_flashInterval;

    protected List<StringBuilder> m_buffer;
    protected int m_curLine;
    protected int m_curIndex;

    protected bool m_isInsertMode;

    protected float m_timer;

	// Use this for initialization
	void Start () 
    {
        m_buffer = new List<StringBuilder>();
        m_buffer.Add(new StringBuilder());

        m_curLine = 0;
        m_curIndex = 0;

        m_isInsertMode = false;

        m_timer = 0.0f;
	}
	
	// Update is called once per frame
	void Update () 
    {
        int chr = 0;

        StringBuilder sb = m_buffer[m_curLine];
        if( m_curIndex < sb.Length )
            chr = sb[m_curIndex];

        int x = m_curIndex % Defines.TEXT_AREA_WIDTH;
        int y = 0;

        for (int i = 0; i <= m_curLine; i++ )
        {
            if( i != m_curLine)
            {
                //TODO 
            }
            else
            {
                y += m_curIndex / Defines.TEXT_AREA_WIDTH;
            }
        }

        // draw the flash char 
        if (m_timer > m_flashInterval * 0.5f)
            m_textDisplay.DrawChar(x, y, chr, true);
        else
            m_textDisplay.DrawChar(x, y, chr, false);

        // update timer 
        m_timer += Time.deltaTime;

        if (m_timer > m_flashInterval)
            m_timer = 0.0f;
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
    }

    protected void onEnter()
    {
        //TODO 
    }

    protected void onDel()
    {
        //TODO 
    }

    protected void onMoveCursor( KeyCode dir )
    {
        //TODO 
    }

    protected void onChar( KeyCode key )
    {
        int chr = (int)key;
        if (chr < 0 || chr >= 128)
            return;

        StringBuilder sb = m_buffer[m_curLine];
        if( m_curIndex < sb.Length )
            sb[m_curIndex] = (char)chr;
        else
            sb.Insert(m_curIndex, (char)chr);

        m_curIndex++;
    }

    protected void refreshLED()
    {
        m_textDisplay.Clean();

        foreach( StringBuilder sb in m_buffer )
        {
            m_textDisplay.DrawText(0, 0, sb.ToString());
            //TODO 
        }
    }

}
