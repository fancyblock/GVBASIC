using UnityEngine;
using System.Collections;

public class Processor : MonoBehaviour 
{
	public TextMode m_textMode;
    public GraphMode m_graphMode;

	// Use this for initialization
	void Start () 
    {
		m_textMode.ShowASCII (0);      //[TEST]
	}
	
	// Update is called once per frame
	void Update () 
    {
	}

    void OnApplicationPause(bool pauseStatus)
    {
        m_textMode.CLS();
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
