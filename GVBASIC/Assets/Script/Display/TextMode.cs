using UnityEngine;
using System.Collections;

public class TextMode : MonoBehaviour 
{
    public LED m_led;
    public GraphMode m_otherMode;

	// Use this for initialization
	void Start () 
    {
	}
	
	// Update is called once per frame
	void Update () 
    {
	}

    /// <summary>
    /// active this mode 
    /// </summary>
    public void SetActive()
    {
        m_otherMode.enabled = false;
        this.enabled = true;

        //TODO 
    }
}
