using UnityEngine;
using System.Collections;

public class TextMode : MonoBehaviour 
{
	public int TEXT_CNT_WIDTH = 20;
	public int TEXT_CNT_HEIGHT = 5;

    public LED m_led;

	protected int[,] m_displayBuffer;
	protected int m_curPosX;
	protected int m_curPosY;
	protected bool m_inverseMode;

    /// <summary>
    /// initial 
    /// </summary>
    void Awake()
    {
        // init buffer 
        m_displayBuffer = new int[TEXT_CNT_WIDTH, TEXT_CNT_HEIGHT];
        for (int i = 0; i < TEXT_CNT_WIDTH; i++)
        {
            for (int j = 0; j < TEXT_CNT_HEIGHT; j++)
                m_displayBuffer[i, j] = 0;
        }

        // reset the cursor 
        m_curPosX = 0;
        m_curPosY = 0;

        // reset the inverse mode 
        m_inverseMode = false;
    }

    void Start()
    {
        DrawChar(0, 0, 65);
        DrawChar(0, 1, 97);
        DrawChar(0, 2, 77);
        DrawChar(0, 3, 111);
        DrawChar(0, 4, 109);
    }

    /// <summary>
    /// draw a char 
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="chr"></param>
    public void DrawChar( int x, int y, int chr )
    {
        m_led.DrawLetter( x*ASCII.WIDTH, y*ASCII.HEIGHT, chr, m_inverseMode);
    }

    /// <summary>
    /// clean the screen    [CLS]
    /// </summary>
    public void CLS()
    {
        m_led.CleanScreen();

		//TODO 
    }

    /// <summary>
    /// set as inverse mode [INVERSE]
    /// </summary>
    public void Inverse()
    {
		m_inverseMode = true;
    }

    /// <summary>
    /// set normal mode     [NORMAL]
    /// </summary>
    public void Normal()
    {
		m_inverseMode = false; 
    }

    /// <summary>
    /// set locate          [LOCATE]
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void SetLocate( int x, int y )
    {
		m_curPosX = x;
		m_curPosY = y;
    }

}
