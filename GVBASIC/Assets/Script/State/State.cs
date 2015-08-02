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

        onInit();
	}

    public StateMgr MGR { get { return m_stateMgr; } }

    public virtual void onInit() { }
    public virtual void onSwitchIn() { }
    public virtual void onSwitchOut() { }
    public virtual void onUpdate() { }
    public virtual void onInput( KCode key ) {}


    public void GraphMode()
    {
        m_stateMgr.GraphMode();
    }

    public void TextMode()
    {
        m_stateMgr.TextMode();
    }

}
