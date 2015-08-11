using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Keyboard : MonoBehaviour 
{
    public StateMgr m_stateMgr;
    public List<Key> m_letterKeys;

    protected bool m_caps = true;

    #region ONLY_IN_EDITOR

#if UNITY_EDITOR
    // Update is called once per frame
	void Update () 
    {
        if (!Input.anyKeyDown)
            return;

        for (KCode k = KCode.None; k <= KCode.CapsLock; k++)
        {
            if (Input.GetKeyDown( (KeyCode)k))
            {
                // 小写字母转换成大写字母
                if ( (int)k >= 97 && (int)k <= 122)
                    k -= 32;

                SendKey(k);
                break;
            }
        }
	}
#endif

    #endregion

    /// <summary>
    /// getter of the caps 
    /// </summary>
    public bool CAPS
    {
        get
        {
            return m_caps;
        }
    }

    /// <summary>
    /// 设置大写小写
    /// </summary>
    /// <param name="capsOn"></param>
    public void SetCaps( bool caps )
    {
        if(m_caps != caps)
        {
            foreach (Key k in m_letterKeys)
            {
                if (k.m_key <= KCode.Z)
                    k.SetKeyCode(k.m_key + 32);
                else
                    k.SetKeyCode(k.m_key - 32);
            }
        }

        m_caps = caps;
    }

    /// <summary>
    /// send key down event
    /// </summary>
    /// <param name="key"></param>
    public void SendKey( KCode key )
    {
        m_stateMgr.Input(key);
    }

}
