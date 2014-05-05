using UnityEngine;
using System.Collections;

public class GraphMode : MonoBehaviour 
{
    public LED m_led;
    public TextMode m_otherMode;

	// Use this for initialization
	void Start () 
    {	
	}
	
	// Update is called once per frame
	void Update () 
    {
	}

    /// <summary>
    /// active this mode    [GRAPH]
    /// </summary>
    public void SetActive()
    {
        m_otherMode.enabled = false;
        this.enabled = true;

        //TODO 
    }

    /// <summary>
    /// draw rectangle      [BOX]
    /// </summary>
    /// <param name="x0"></param>
    /// <param name="y0"></param>
    /// <param name="x1"></param>
    /// <param name="y1"></param>
    /// <param name="fill"></param> 1 = fill, 0 = not fill 
    /// <param name="type"></param> 1 = draw, 0 = clean 
    public void DrawRect( int x0, int y0, int x1, int y1, int fill, int type )
    {
        //TODO 
    }

    /// <summary>
    /// draw circle         [CIRCLE]
    /// </summary>
    /// <param name="x0"></param>
    /// <param name="y0"></param>
    /// <param name="r"></param>
    /// <param name="fill"></param>
    /// <param name="type"></param>
    public void DrawCircle( int x0, int y0, int r, int fill, int type )
    {
        //TODO 
    }

    /// <summary>
    /// draw point          [DRAW]
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="type"></param>
    public void DrawPoint( int x, int y, int type )
    {
        m_led.SetPixel(x, y, type == 1);
    }

    /// <summary>
    /// draw ellipse        [ELLIPSE]
    /// </summary>
    /// <param name="x0"></param>
    /// <param name="y0"></param>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <param name="fill"></param>
    /// <param name="type"></param>
    public void DrawEllipse( int x0, int y0, int a, int b, int fill, int type )
    {
        //TODO 
    }

    /// <summary>
    /// draw line           [LINE]
    /// </summary>
    /// <param name="x0"></param>
    /// <param name="y0"></param>
    /// <param name="x1"></param>
    /// <param name="y1"></param>
    /// <param name="type"></param>
    public void DrawLine( int x0, int y0, int x1, int y1, int type )
    {
        //TODO 
    }

}
