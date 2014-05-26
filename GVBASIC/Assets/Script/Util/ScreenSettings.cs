using UnityEngine;
using System.Collections;

public class ScreenSettings : MonoBehaviour 
{
    public int m_screenWidth;
    public int m_screenHeight;
    public int m_fps;

	// Use this for initialization
	void Start () 
    {
        Screen.SetResolution(m_screenWidth, m_screenHeight, true, m_fps);
	}
	
}
