using UnityEngine;
using System.Collections;
using System.Text;

public class CodeSaver : State 
{
    protected LineInfo m_fileName;
    protected int m_curIndex;

    public override void onInit() 
    {
        m_fileName = new LineInfo();
    }

    public override void onSwitchIn() 
    {
        string pureBasName = m_stateMgr.CUR_CODE_FILE_NAME;

        // remove the extesion name
        if( !string.IsNullOrEmpty(pureBasName) )
            pureBasName = pureBasName.Substring(0, pureBasName.Length - 4);

        m_fileName = new LineInfo(pureBasName);
        m_curIndex = 0;

        m_stateMgr.m_textDisplay.Clear();
        m_stateMgr.m_textDisplay.DrawText(0, 0, "Input file name:");
        m_stateMgr.m_textDisplay.DrawText(0, 1, pureBasName);
        m_stateMgr.m_textDisplay.Refresh();

        m_stateMgr.m_textDisplay.SetCursor(0,1);
    }

    public override void onInput(KCode key) 
    {
        int newIndex = m_fileName.KeyInput(key, m_curIndex);
        if (newIndex >= 0)
        {
            m_curIndex = newIndex;
        }
        else
        {
            switch (key)
            {
                case KCode.Escape:
                    m_stateMgr.GotoState(StateEnums.eStateList);
                    return;
                case KCode.Return:
                    CodeMgr.SharedInstance.SaveSourceCode(m_fileName.TEXT + ".BAS", m_stateMgr.CUR_SOURCE_CODE);
                    m_stateMgr.GotoState(StateEnums.eStateList);
                    return;
                default:
                    break;
            }
        }

        refresh();
    }

    protected void refresh()
    {
        m_stateMgr.m_textDisplay.Clear();
        m_stateMgr.m_textDisplay.DrawText(0, 0, "Input file name:");
        m_stateMgr.m_textDisplay.DrawText(0, 1, m_fileName.TEXT);
        m_stateMgr.m_textDisplay.SetCursor(m_curIndex,1);
        m_stateMgr.m_textDisplay.Refresh();
    }

}
