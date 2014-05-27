using UnityEngine;
using System.Collections;

public class Processor : MonoBehaviour 
{

	// Use this for initialization
	void Start () 
    {
	}
	
	// Update is called once per frame
	void Update () 
    {
	}

    /// <summary>
    /// on key up 
    /// </summary>
    /// <param name="key"></param>
    public void onKeyUp( KeyEnums key )
    {
        //TODO 

        Debug.Log("on key up " + key.ToString());
    }
}
