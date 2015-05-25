using UnityEngine;
using System.Collections;

public class Processor : MonoBehaviour 
{
	public TextMode m_textMode;
    public GraphMode m_graphMode;

	// Use this for initialization
	void Start () 
    {
        m_textMode.CLS();
		m_textMode.ShowASCII (0);      //[TEST]
	}

}
