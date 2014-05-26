using UnityEngine;
using System.Collections;

public class Keyboard : MonoBehaviour 
{
    public Processor m_processor;

	// Use this for initialization
	void Start () 
    {	
	}
	
	// Update is called once per frame
	void Update () 
    {
        if( Input.GetKeyUp( KeyCode.A ) )
        {
            m_processor.onKeyUp(KeyEnums.eKeyA);
        }
        else if( Input.GetKeyUp( KeyCode.B ))
        {
            m_processor.onKeyUp(KeyEnums.eKeyB);
        }
        //TODO 
	}

}
