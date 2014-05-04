using UnityEngine;
using System.Collections;
using Assets.Script.Display;


public class LED : MonoBehaviour 
{
    public SpriteRenderer m_spriteRender;
    public Color m_whiteColor;

    protected Texture2D m_texture;

	// Use this for initialization
	void Start () 
    {
        m_texture = m_spriteRender.sprite.texture;
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
        m_texture.SetPixel(x, y, set ? Color.black : m_whiteColor);
    }

    /// <summary>
    /// clean screen 
    /// </summary>
    public void CleanScreen()
    {
        m_texture.SetPixels(0, 0, 160, 80, new Color[] { m_whiteColor });
        
        //TODO 
        ASCII ascii = new ASCII(1, m_whiteColor, Color.black );
        m_texture.SetPixels(0, 0, ASCII.WIDTH, ASCII.HEIGHT, ascii.m_color);
    }

}
