using UnityEngine;
using System;
using System.Collections;

public class MessageBox : MonoBehaviour 
{
    public UILabel m_txtMsg;
    public GameObject m_focusYes;
    public GameObject m_focusNo;

    protected bool m_isShow;
    protected bool m_yes;

	// Use this for initialization
	void Awake () 
	{
        m_isShow = false;

        gameObject.SetActive(false);
	}

    /// <summary>
    /// 是否显示
    /// </summary>
    public bool IS_SHOW { get { return m_isShow; } }

    /// <summary>
    /// 显示选择框
    /// </summary>
    public void Show( string msg, Action yes, Action no )
    {
        m_txtMsg.text = msg;

        // 默认选择yes
        focosOn(true);

        gameObject.SetActive(true);
        m_isShow = true;
    }


    /// <summary>
    /// 键盘输入
    /// </summary>
    /// <param name="key"></param>
    public void onInput( KCode key )
    {
        switch(key)
        {
            case KCode.LeftArrow:
                focosOn(true);
                break;
            case KCode.RightArrow:
                focosOn(false);
                break;
            case KCode.Y:
            case KCode.y_l:
                focosOn(true);
                confirmSelect();
                break;
            case KCode.N:
            case KCode.n_l:
                focosOn(false);
                confirmSelect();
                break;
            case KCode.Return:
                confirmSelect();
                break;
            default:
                break;
        }
    }


    /// <summary>
    /// 确认选择 
    /// </summary>
    protected void confirmSelect()
    {
        //TODO 
    }

    /// <summary>
    /// 焦点在哪个按钮上
    /// </summary>
    /// <param name="yes"></param>
    protected void focosOn( bool yes )
    {
        m_focusYes.SetActive(yes);
        m_focusNo.SetActive(!yes);
        m_yes = yes;
    }

}
