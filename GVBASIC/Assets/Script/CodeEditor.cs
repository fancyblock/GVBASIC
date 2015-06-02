using UnityEngine;
using System.Collections;
using System.Text;


public class CodeEditor : MonoBehaviour 
{
    public TextDisplay m_textDisplay;

    protected StringBuilder m_buffer;
    protected int m_cursorX;
    protected int m_cursorY;

    void Awake()
    {
        m_buffer = new StringBuilder();

        m_cursorX = 0;
        m_cursorY = 0;
        //TODO 
    }

	// Use this for initialization
	void Start () 
    {
	}
	
	// Update is called once per frame
	void Update () 
    {
	}

}
