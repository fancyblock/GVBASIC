using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// LED screen emulator 
/// </summary>
public class LED : MonoBehaviour 
{
    public Texture2D m_texture;
    public Color m_whiteColor;
    public Color m_blackColor;

    protected Dictionary<int,ASCII> m_asciis;
    protected Color[] m_cleanColorData;
    protected Color[] m_soildColorData;

    protected Color[] m_cursorData;
    protected Color[] m_cursorInvData;

	// Use this for initialization
	void Awake () 
    {
        // set clean color data 
        m_cleanColorData = new Color[12800];
        m_soildColorData = new Color[12800];

        for (int i = 0; i < 12800; i++)
        {
            m_cleanColorData[i] = m_whiteColor;
            m_soildColorData[i] = m_blackColor;
        }

        // 初始化字符表 
        initASCIITable();
        
        CleanScreen();
	}
	
	// Update is called once per frame
	void Update () 
    {
        m_texture.Apply();
	}

    /// <summary>
    /// set pixel 
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="set"></param>
    public void SetPixel( int x, int y, bool set )
    {
        m_texture.SetPixel(x, y, set ? m_blackColor : m_whiteColor);
    }

    /// <summary>
    /// draw box 
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="set"></param>
    public void DrawBox( int x, int y, int width, int height, bool set )
    {
        Color[] color = set ? m_soildColorData : m_cleanColorData;

        m_texture.SetPixels(x, y, width, height, color);
    }

    /// <summary>
    /// draw letter 
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="ascii"></param>
    public void DrawLetter( int x, int y, int ascii, bool inverse )
    {
        if( inverse )
            m_texture.SetPixels(x, y, ASCII.WIDTH, ASCII.HEIGHT, m_asciis[ascii].m_inverseColor);
        else
            m_texture.SetPixels(x, y, ASCII.WIDTH, ASCII.HEIGHT, m_asciis[ascii].m_color);
    }

    /// <summary>
    /// 绘制光标(下划线)
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="inverse"></param>
    public void DrawCursor( int x, int y, bool inverse )
    {
        if (inverse)
            m_texture.SetPixels(x, y, ASCII.WIDTH, 2, m_cursorData);
        else
            m_texture.SetPixels(x, y, ASCII.WIDTH, 2, m_cursorInvData);
    }

    /// <summary>
    /// clean screen 
    /// </summary>
    public void CleanScreen()
    {
        m_texture.SetPixels(m_cleanColorData);
    }


    /// <summary>
    /// initial the ASCII table 
    /// </summary>
    protected void initASCIITable()
    {
        m_asciis = new Dictionary<int, ASCII>();

        // initial the ASCII code 
        for( int i = 0; i < 128; i++ )
        {
            ASCII ascii = new ASCII(i, m_whiteColor, m_blackColor);
            m_asciis[i] = ascii;
        }

        m_cursorData = bytesToColors(new int[] { 0xff, 0xff });
        m_cursorInvData = bytesToColors(new int[] { 0x00, 0x00 });
    }

    /// <summary>
    /// 根据字节数据生成颜色数组
    /// </summary>
    /// <param name="bytes"></param>
    /// <returns></returns>
    protected Color[] bytesToColors( int[] bytes )
    {
        Color[] colors = new Color[bytes.Length * 8];
        int index = 0;

        foreach( int b in bytes )
        {
            for( int i = 0; i < 8; i++ )
            {
                bool isBlack = ( b >> ( 7 - i ) & 0x01 ) == 1 ? true : false;

                if (isBlack)
                    colors[index] = m_blackColor;
                else
                    colors[index] = m_whiteColor;

                index++;
            }
        }

        return colors;
    }

}
