﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;


public class CodeEditor : MonoBehaviour 
{
    public TextDisplay m_textDisplay;

    protected List<StringBuilder> m_buffer;
    protected int m_curLine;
    protected int m_curIndex;

    protected bool m_isInsertMode;

	// Use this for initialization
	void Start () 
    {
        m_buffer = new List<StringBuilder>();
        m_buffer.Add(new StringBuilder());

        m_curLine = 0;
        m_curIndex = 0;

        m_isInsertMode = false;
	}
	
	// Update is called once per frame
	void Update () 
    {
        //TODO 
	}

    /// <summary>
    /// key input 
    /// </summary>
    /// <param name="key"></param>
    public void Input( KeyCode key )
    {
        switch( key )
        {
            case KeyCode.Return:
                onEnter();
                break;
            case KeyCode.Delete:
                onDel();
                break;
            case KeyCode.Insert:
                m_isInsertMode = !m_isInsertMode;
                break;
            case KeyCode.UpArrow:
            case KeyCode.DownArrow:
            case KeyCode.LeftArrow:
            case KeyCode.RightArrow:
                onMoveCursor(key);
                break;
            default:
                onChar();
                break;
        }
    }

    
    protected void onEnter()
    {
        //TODO 
    }

    protected void onDel()
    {
        //TODO 
    }

    protected void onMoveCursor( KeyCode dir )
    {
        //TODO 
    }

    protected void onChar()
    {
        //TODO 
    }

}
