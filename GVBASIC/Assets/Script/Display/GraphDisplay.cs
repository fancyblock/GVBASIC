using UnityEngine;
using System.Collections;

public class GraphDisplay : MonoBehaviour 
{
    public LED m_led;

	// Use this for initialization
	void Start () 
	{
	}
	
	// Update is called once per frame
	void Update () 
	{
	}

    /// <summary>
    /// set pixel 
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="set"></param>
    public void DrawPixel( int x, int y, bool set = true )
    {
        m_led.SetPixel(x, y, set);
    }

}
