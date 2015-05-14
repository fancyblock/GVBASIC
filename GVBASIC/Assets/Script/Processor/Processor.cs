using UnityEngine;
using System.Collections;

public class Processor : MonoBehaviour 
{
	public TextMode m_textMode;

	// Use this for initialization
	void Start () 
    {
		m_textMode.ShowASCII (27);      //[TEST]
	}
	
	// Update is called once per frame
	void Update () 
    {
	}

    /// <summary>
    /// on key up 
    /// </summary>
    /// <param name="key"></param>
    public void onKeyUp( KeyEnums key )
    {
        //TODO 

        Debug.Log("on key up " + key.ToString());
    }
}
