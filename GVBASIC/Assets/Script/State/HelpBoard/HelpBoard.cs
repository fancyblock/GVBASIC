using UnityEngine;
using System.Collections;

public class HelpBoard : MonoBehaviour 
{
    public UIProgressBar m_progressBar;

    protected bool m_isShow = false;

    /// <summary>
    /// 是否显示
    /// </summary>
    public bool IS_SHOW { get { return m_isShow; } }

    public void Show()
    {
        gameObject.SetActive(true);
        m_isShow = true;
    }

    /// <summary>
    /// 键盘输入
    /// </summary>
    /// <param name="key"></param>
    public void onInput(KCode key)
    {
        if( key == KCode.Escape )
        {
            gameObject.SetActive(false);
            m_isShow = false;
        }
        else if( key == KCode.LeftArrow )
        {
            if( m_progressBar.value > 0 )
            {
                //TODO 
            }
        }
        else if( key == KCode.RightArrow )
        {
            //TODO 
        }
        else if( key == KCode.UpArrow )
        {
            //TODO 
        }
        else if( key == KCode.DownArrow )
        {
            //TODO 
        }
    }

}
