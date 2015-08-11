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

    public MessageBox m_msgBox;
    public HelpBoard m_helpBoard;
    public Keyboard m_keyboard;

    public string CUR_CODE_FILE_NAME { set; get; }
    public string CUR_SOURCE_CODE { set; get; }


    protected Dictionary<StateEnums, State> m_stateDic = new Dictionary<StateEnums, State>();

    protected State m_curState = null;

	// Use this for initialization
	void Start () 
	{
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
        if ( m_msgBox.IS_SHOW )
        {
            m_msgBox.onInput(key);
        }
        else if( m_helpBoard.IS_SHOW )
        {
            m_helpBoard.onInput(key);
        }
        else if( key == KCode.F3 )
        {
            ShowHelp();
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

        // 重置键盘
        m_keyboard.SetCaps(true);

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
        m_msgBox.Show( msg, yes, no );
    }

    /// <summary>
    /// 显示帮助信息
    /// </summary>
    public void ShowHelp()
    {
        m_helpBoard.Show();
    }

}
