using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Key : MonoBehaviour 
{
    public KCode m_key;

    protected Keyboard m_keyboard;

	// Use this for initialization
	void Awake () 
	{
        m_keyboard = GetComponentInParent<Keyboard>();

        // add event listener 
        UIButton btn = GetComponent<UIButton>();
        btn.onClick.Add(new EventDelegate(onKeyDown));
	}

    /// <summary>
    /// on key down 
    /// </summary>
    public void onKeyDown()
    {
        m_keyboard.SendKey(m_key);
    }
	
}
