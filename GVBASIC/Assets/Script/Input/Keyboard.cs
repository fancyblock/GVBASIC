using UnityEngine;
using System.Collections;

public class Keyboard : MonoBehaviour 
{
    public StateMgr m_stateMgr;
	
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
                m_stateMgr.Input(k);
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
        m_stateMgr.Input(key);
    }
}
