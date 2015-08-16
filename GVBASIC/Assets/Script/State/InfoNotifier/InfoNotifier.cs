using UnityEngine;
using System.Collections;

public class InfoNotifier : MonoBehaviour 
{
    public UILabel m_txt;
    public float m_showTime;

    protected bool m_isShow = false;

    /// <summary>
    /// 显示信息
    /// </summary>
    /// <param name="info"></param>
    public void Show( string info )
    {
        m_txt.text = info;
        gameObject.SetActive(true);

        m_isShow = true;

        StartCoroutine( onShowing() );
    }

    /// <summary>
    /// 是否显示着
    /// </summary>
    public bool IS_SHOW { get { return m_isShow; } }

    public IEnumerator onShowing()
    {
        yield return new WaitForSeconds(m_showTime);

        gameObject.SetActive(false);
        m_isShow = false;
    }
}
