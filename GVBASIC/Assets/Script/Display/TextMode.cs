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
    /// active this mode    [TEXT]
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

    /// <summary>
    /// clean the screen    [CLS]
    /// </summary>
    public void CLS()
    {
        m_led.CleanScreen();
    }

    /// <summary>
    /// set as inverse mode [INVERSE]
    /// </summary>
    public void Inverse()
    {
        //TODO 
    }

    /// <summary>
    /// set normal mode     [NORMAL]
    /// </summary>
    public void Normal()
    {
        //TODO 
    }

    /// <summary>
    /// set locate          [LOCATE]
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void SetLocate( int x, int y )
    {
        //TODO 
    }

    /// <summary>
    /// print string        [PRINT]
    /// </summary>
    /// <param name="str"></param>
    public void Print( string str )
    {
        //TODO 
    }

    //TODO 

    /// <summary>
    /// print n spaces      [SPC]
    /// </summary>
    /// <param name="n"></param>
    public void PrintSpace( int n )
    {
        //TODO 
    }

    /// <summary>
    /// move the locate     [TAB]
    /// </summary>
    /// <param name="n"></param>
    public void Tab( int n )
    {
        //TODO 
    }

}
