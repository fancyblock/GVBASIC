using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Key : MonoBehaviour 
{
    public KeyCode m_key;

    protected Keyboard m_keyboard;

	// Use this for initialization
	void Awake () 
	{
        m_keyboard = GetComponentInParent<Keyboard>();

        // add event listener 
        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(onKeyDown);
	}

    /// <summary>
    /// on key down 
    /// </summary>
    public void onKeyDown()
    {
        m_keyboard.SendKey(m_key);
    }
	
}
