using UnityEngine;
using System.Collections;

public class TextMode : MonoBehaviour 
{
    public LED m_led;
    public GraphMode m_otherMode;

	// Use this for initialization
	void Start () 
    {
	}
	
	// Update is called once per frame
	void Update () 
    {
	}

    /// <summary>
    /// active this mode 
    /// </summary>
    public void SetActive()
    {
        m_otherMode.enabled = false;
        this.enabled = true;

        //TODO 
    }

    /// <summary>
    /// show ASCII 
    /// </summary>
    /// <param name="offset"></param>
    public void ShowASCII( int offset )
    {
        for( int i = 0; i < 100; i++ )
        {
            int letter = i + offset;

            int x = (i % 20) * 8;
            int y = i/20 * 16;

            m_led.DrawLetter(x, y, letter, false);
        }
    }
}
