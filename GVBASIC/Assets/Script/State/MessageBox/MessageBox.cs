using UnityEngine;
using System;
using System.Collections;

public class MessageBox : MonoBehaviour 
{
    public UILabel m_txtMsg;
    public GameObject m_focusYes;
    public GameObject m_focusNo;

    protected bool m_isShow;

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
        m_focusYes.SetActive(true);
        m_focusNo.SetActive(false);

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
                m_focusYes.SetActive(true);
                m_focusNo.SetActive(false);
                break;
            case KCode.RightArrow:
                m_focusYes.SetActive(false);
                m_focusNo.SetActive(true);
                break;
            case KCode.Y:
            case KCode.y_l:
                //TODO 
                break;
            case KCode.N:
            case KCode.n_l:
                //TODO 
                break;
            case KCode.Return:
                //TODO 
                break;
            default:
                break;
        }
    }

}
