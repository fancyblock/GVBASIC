using UnityEngine;
using System;
using System.Collections;

public class TextDisplay : MonoBehaviour 
{
    public LED m_led;
    public float m_cursorInterval;

	protected int[,] m_buffer;
    protected bool[,] m_inverseBuffer;

    protected bool m_hasCursor;
    protected int m_cursorX;
    protected int m_cursorY;
    protected int m_lastTextX;
    protected int m_lastTextY;
    protected float m_timer;

    /// <summary>
    /// initial 
    /// </summary>
    void Awake()
    {
        // init buffer 
        m_buffer = new int[Defines.TEXT_AREA_WIDTH, Defines.TEXT_AREA_HEIGHT];
        m_inverseBuffer = new bool[Defines.TEXT_AREA_WIDTH, Defines.TEXT_AREA_HEIGHT];
        Clean();

        // no cursor 
        m_cursorX = 0;
        m_cursorY = 0;
        m_lastTextX = 0;
        m_lastTextY = 0;
        m_hasCursor = false;
        m_timer = 0.0f;
    }

    /// <summary>
    /// update 
    /// </summary>
    void Update ()
    {
        if( m_hasCursor )
        {
            // draw the flash char 
            if (m_timer > m_cursorInterval * 0.5f)
                m_led.DrawLetter(m_cursorX * ASCII.WIDTH, m_cursorY * ASCII.HEIGHT, m_buffer[m_cursorX, m_cursorY], true);
            else
                m_led.DrawLetter(m_cursorX * ASCII.WIDTH, m_cursorY * ASCII.HEIGHT, m_buffer[m_cursorX, m_cursorY], false);

            // update timer 
            m_timer += Time.deltaTime;

            if (m_timer > m_cursorInterval)
                m_timer = 0.0f;
        }
    }
    
    /// <summary>
    /// set cursor 
    /// </summary>
    /// <param name="show"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void SetCursor( bool show, int x = 0, int y = 0 )
    {
        m_hasCursor = show;

        if (m_hasCursor == false)
            m_inverseBuffer[m_cursorX, m_cursorY] = false;

        m_cursorX = x % Defines.TEXT_AREA_WIDTH;
        m_cursorY = y + x / Defines.TEXT_AREA_WIDTH;
    }

    /// <summary>
    /// getter && setter of the cursor 
    /// </summary>
    public int CURSOR_X { get { return m_cursorX; } }
    public int CURSOR_Y { get { return m_cursorY; } }

    /// <summary>
    /// getter of the last text position 
    /// </summary>
    public int LAST_TEXT_X { get { return m_lastTextX; } }
    public int LAST_TEXT_Y { get { return m_lastTextY; } }

    /// <summary>
    /// refresh the display 
    /// </summary>
    public void Refresh()
    {
        m_led.CleanScreen();

        for (int i = 0; i < Defines.TEXT_AREA_WIDTH; i++)
        {
            for (int j = 0; j < Defines.TEXT_AREA_HEIGHT; j++)
                m_led.DrawLetter(i * ASCII.WIDTH, j * ASCII.HEIGHT, m_buffer[i,j], m_inverseBuffer[i,j]);
        }
    }

    /// <summary>
    /// draw a string 
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="str"></param>
    public void DrawText( int x, int y, string str, bool inverse = false )
    {
        foreach( char c in str )
        {
            bool nextLine = false;

            if (c == '\n')
            {
                nextLine = true;
            }
            else
            {
                if (!(x < 0 || y < 0 || x >= Defines.TEXT_AREA_WIDTH || y >= Defines.TEXT_AREA_HEIGHT))
                {
                    m_buffer[x, y] = c;
                    m_inverseBuffer[x, y] = inverse;
                }

                x++;
                if (x >= Defines.TEXT_AREA_WIDTH)
                    nextLine = true;
            }

            if (nextLine)
            {
                x = 0;
                y ++;
            }
        }

        m_lastTextX = x;
        m_lastTextY = y;
    }

    /// <summary>
    /// roll up 
    /// </summary>
    /// <param name="lineCnt"></param>
    public void RollUp( int lineCnt )
    {
        if (lineCnt <= 0)
            return;

        if( lineCnt > Defines.TEXT_AREA_HEIGHT )
        {
            Clean();
            return;
        }

        // move the lines
        for( int i = Defines.TEXT_AREA_HEIGHT - 1; i > 0; i-- )
        {
            int destLine = i - lineCnt;

            if (destLine < 0)
                break;

            for( int j = 0; j < Defines.TEXT_AREA_WIDTH; j++ )
            {
                m_buffer[j, destLine] = m_buffer[j, i];
                m_inverseBuffer[j, destLine] = m_inverseBuffer[j, i];
            }
        }

        // clean the new line 
        for( int i = 0; i < Defines.TEXT_AREA_WIDTH; i++ )
        {
            for (int j = Defines.TEXT_AREA_HEIGHT - lineCnt; j < Defines.TEXT_AREA_HEIGHT; j++ )
            {
                m_buffer[i, j] = 0;
                m_inverseBuffer[i, j] = false;
            }
        }
    }

    /// <summary>
    /// clean the screen
    /// </summary>
    public void Clean()
    {
        for (int i = 0; i < Defines.TEXT_AREA_WIDTH; i++)
        {
            for (int j = 0; j < Defines.TEXT_AREA_HEIGHT; j++)
            {
                m_buffer[i, j] = 0;
                m_inverseBuffer[i, j] = false;
            }
        }
    }

}
