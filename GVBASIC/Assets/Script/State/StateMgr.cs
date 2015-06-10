using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public enum StateEnums
{
    eStateMenu,
    eStateEditor,
    eStateRunner
}


public class StateMgr : MonoBehaviour 
{
    public StateEnums m_startState;

    protected Dictionary<StateEnums, State> m_stateDic = new Dictionary<StateEnums, State>();
    public State m_curState = null;

	// Use this for initialization
	void Start () 
	{
        GotoState(m_startState);
	}

    /// <summary>
    /// input 
    /// </summary>
    /// <param name="key"></param>
    public void Input( KCode key )
    {
        m_curState.onInput(key);
    }

    /// <summary>
    /// add state 
    /// </summary>
    /// <param name="state"></param>
    public void AddState( State state )
    {
        m_stateDic.Add(state.m_stateType, state);
    }

    /// <summary>
    /// goto state 
    /// </summary>
    /// <param name="aimState"></param>
    public void GotoState( StateEnums aimState )
    {
        if (m_curState != null)
            m_curState.onSwitchOut();

        m_curState = m_stateDic[aimState];
        m_curState.onSwitchIn();
    }

}
