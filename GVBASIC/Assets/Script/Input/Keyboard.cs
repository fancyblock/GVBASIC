using UnityEngine;
using System.Collections;

public class Keyboard : MonoBehaviour 
{
    public CodeEditor m_editor;
	
#if UNITY_EDITOR
	// Update is called once per frame
	void Update () 
    {
        if (!Input.anyKeyDown)
            return;

        for (KCode k = KCode.None; k <= KCode.Home; k++)
        {
            if (Input.GetKeyDown( (KeyCode)k))
            {
                m_editor.Input(k);
                break;
            }
        }
	}
#endif

    /// <summary>
    /// send key down event
    /// </summary>
    /// <param name="key"></param>
    public void SendKey( KCode key )
    {
        m_editor.Input(key);
    }
}
