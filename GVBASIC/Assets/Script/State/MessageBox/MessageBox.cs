using UnityEngine;
using System;
using System.Collections;

public class MessageBox : MonoBehaviour 
{
    public UILabel m_txtMsg;
    public GameObject m_focusYes;
    public GameObject m_focusNo;
    public UISprite m_btnYes;
    public UISprite m_btnNo;

    protected bool m_isShow;
    protected bool m_yes;
    protected Action m_yesCallback;
    protected Action m_noCallback;

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
        m_yesCallback = yes;
        m_noCallback = no;

        // 默认选择yes
        focosOn(true);

        m_btnYes.color = Color.white;
        m_btnNo.color = Color.white;

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
        if (m_yes)
            m_btnYes.color = Color.gray;
        else
            m_btnNo.color = Color.gray;

        Invoke("_confirmSelect", 0.12f);
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


    protected void _confirmSelect()
    {
        gameObject.SetActive(false);
        m_isShow = false;

        if (m_yes)
            m_yesCallback();
        else
            m_noCallback();
    }

}
