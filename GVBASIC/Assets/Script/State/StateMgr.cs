using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


public enum StateEnums
{
    eStateList,
    eStateEditor,
    eStateRunner,
    eStateSaver,
}


public class StateMgr : MonoBehaviour 
{
    public TextDisplay m_textDisplay;
    public GraphDisplay m_graphDisplay;

    public string CUR_CODE_FILE_NAME { set; get; }
    public string CUR_SOURCE_CODE { set; get; }


    protected Dictionary<StateEnums, State> m_stateDic = new Dictionary<StateEnums, State>();

    protected State m_curState = null;
    protected bool m_inMsgBox;
    protected Action m_yesAction;
    protected Action m_noAction;

	// Use this for initialization
	void Start () 
	{
        m_inMsgBox = false;

        GotoState(StateEnums.eStateList);
	}

    /// <summary>
    /// update 
    /// </summary>
    void Update()
    {
        m_curState.onUpdate();
    }

    /// <summary>
    /// input 
    /// </summary>
    /// <param name="key"></param>
    public void Input( KCode key )
    {
        if (m_inMsgBox)
        {
            //TODO 
        }
        else
        {
            m_curState.onInput(key);
        }
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
        // 切出前一个状态
        if (m_curState != null)
            m_curState.onSwitchOut();

        m_curState = m_stateDic[aimState];

        // 切入新状态
        m_curState.onSwitchIn();
    }

    /// <summary>
    /// 切换至图形模式（不带光标）
    /// </summary>
    public void GraphMode()
    {
        m_graphDisplay.enabled = true;
        m_textDisplay.enabled = false;

        m_graphDisplay.Clear();
    }

    /// <summary>
    /// 切换至文本模式（带光标）
    /// </summary>
    public void TextMode()
    {
        m_graphDisplay.enabled = false;
        m_textDisplay.enabled = true;

        m_textDisplay.Clear();
    }

    /// <summary>
    /// 显示选择框，只有是和否两种选择
    /// </summary>
    /// <param name="msg"></param>
    /// <param name="yes"></param>
    /// <param name="no"></param>
    public void ShowMessageBox( string msg, Action yes, Action no )
    {
        m_inMsgBox = true;

        m_yesAction = yes;
        m_noAction = no;

        //TODO 
    }

}
