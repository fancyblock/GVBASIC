using UnityEngine;
using System.Collections;
using System.Text;

public class SaveCode : State 
{
    protected LineInfo m_fileName;
    protected int m_curInputIndex;

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
        m_curInputIndex = 0;

        m_textDisplay.Clean();
        m_textDisplay.DrawText(0, 0, "Input file name:");
        m_textDisplay.DrawText(0, 1, pureBasName);
        m_textDisplay.Refresh();

        m_textDisplay.SetCursor(true, 0, 1);
    }

    public override void onInput(KCode key) 
    {
        switch(key)
        {
            case KCode.Escape:
                m_stateMgr.GotoState(StateEnums.eStateMenu);
                return;
            case KCode.Return:
                CodeMgr.SharedInstance.SaveSourceCode(m_fileName.TEXT + ".BAS", m_stateMgr.CUR_SOURCE_CODE);
                m_stateMgr.GotoState(StateEnums.eStateMenu);
                return;
            case KCode.LeftArrow:
            case KCode.RightArrow:
                onMoveCursor(key);
                break;
            default:
                onChar(key);
                break;
        }

        refresh();
    }


    protected void onChar( KCode key )
    {
        // limit the file name length 
        if (m_fileName.LENGTH >= Defines.TEXT_AREA_WIDTH - 4)
            return;

        int chr = (int)key;
        if (chr < 0 || chr >= 128)
            return;

        m_fileName.SetChar(m_curInputIndex, (char)chr);
        m_curInputIndex++;
    }

    protected void onMoveCursor( KCode dir )
    {
        if( dir == KCode.LeftArrow )
        {
            if (m_curInputIndex > 0)
                m_curInputIndex--;
        }
        else if( dir == KCode.RightArrow )
        {
            if (m_curInputIndex < m_fileName.LENGTH)
                m_curInputIndex++;
        }
    }

    protected void refresh()
    {
        m_textDisplay.Clean();
        m_textDisplay.DrawText(0, 0, "Input file name:");
        m_textDisplay.DrawText(0, 1, m_fileName.TEXT);
        m_textDisplay.SetCursor(true, m_curInputIndex, 1);
        m_textDisplay.Refresh();
    }

}
