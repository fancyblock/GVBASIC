using UnityEngine;
using System.Collections;

public class State : MonoBehaviour 
{
    public StateEnums m_stateType;

    protected StateMgr m_stateMgr;

	// Update is called once per frame
	void Awake () 
	{
        m_stateMgr = GetComponentInParent<StateMgr>();
        m_stateMgr.AddState(this);
	}

    public virtual void onSwitchIn() { }
    public virtual void onSwitchOut() { }

    /// <summary>
    /// input inject 
    /// </summary>
    /// <param name="key"></param>
    public virtual void onInput( KCode key ) {}

}
