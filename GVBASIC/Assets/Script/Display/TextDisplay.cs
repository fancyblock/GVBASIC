using UnityEngine;
using System;
using System.Collections;

public class TextDisplay : MonoBehaviour 
{
    public LED m_led;

	protected int[,] m_displayBuffer;
	protected bool m_inverseMode;

    /// <summary>
    /// initial 
    /// </summary>
    void Awake()
    {
        // init buffer 
        m_displayBuffer = new int[Defines.TEXT_AREA_WIDTH, Defines.TEXT_AREA_HEIGHT];
        for (int i = 0; i < Defines.TEXT_AREA_WIDTH; i++)
        {
            for (int j = 0; j < Defines.TEXT_AREA_HEIGHT; j++)
                m_displayBuffer[i, j] = 0;
        }

        // reset the inverse mode 
        m_inverseMode = false;
    }

    /// <summary>
    /// getter && setter of the inverse mode 
    /// </summary>
    public bool INVERSE_MODE
    {
        get
        {
            return m_inverseMode;
        }
        set
        {
            m_inverseMode = value;
        }
    }

    /// <summary>
    /// draw a char 
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="chr"></param>
    public void DrawChar( int x, int y, int chr )
    {
        DrawChar(x, y, chr, m_inverseMode);
    }

    /// <summary>
    /// draw char 
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="chr"></param>
    /// <param name="inverse"></param>
    public void DrawChar( int x, int y, int chr, bool inverse )
    {
        if (x < 0 || y < 0 || x >= Defines.TEXT_AREA_WIDTH || y >= Defines.TEXT_AREA_HEIGHT)
            throw new Exception("[TextMode]: DrawChar, out of space.");

        m_led.DrawLetter(x * ASCII.WIDTH, y * ASCII.HEIGHT, chr, inverse);
    }

    /// <summary>
    /// draw a string 
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="str"></param>
    public void DrawText( int x, int y, string str )
    {
        if (x < 0 || y < 0 || x >= Defines.TEXT_AREA_WIDTH || y >= Defines.TEXT_AREA_HEIGHT)
            throw new Exception("[TextMode]: DrawChar, out of space.");

        int xPos = x * ASCII.WIDTH;
        int yPos = y * ASCII.HEIGHT;

        foreach( char c in str )
        {
            bool nextLine = false;

            if (c == '\n')
            {
                nextLine = true;
            }
            else
            {
                m_led.DrawLetter(xPos, yPos, c, m_inverseMode);

                xPos += ASCII.WIDTH;
                if (xPos >= 160)
                    nextLine = true;
            }

            if (nextLine)
            {
                xPos = 0;
                yPos += ASCII.HEIGHT;

                if (yPos >= 80)
                    break;
            }
        }
    }

    /// <summary>
    /// clean the screen    [CLS]
    /// </summary>
    public void Clean()
    {
        m_led.CleanScreen();
    }

}
