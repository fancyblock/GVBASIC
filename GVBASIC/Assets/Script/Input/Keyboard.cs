using UnityEngine;
using System.Collections;

public class Keyboard : MonoBehaviour 
{
    public CodeEditor m_editor;
	
	// Update is called once per frame
	void Update () 
    {
        if (!Input.anyKeyDown)
            return;

        for (KeyCode k = KeyCode.None; k < KeyCode.Home; k++)
        {
            if (Input.GetKeyDown(k))
            {
                m_editor.Input(k);
                break;
            }
        }
	}

    /// <summary>
    /// send key down event
    /// </summary>
    /// <param name="key"></param>
    public void SendKey( KeyCode key )
    {
        m_editor.Input(key);
    }
}
