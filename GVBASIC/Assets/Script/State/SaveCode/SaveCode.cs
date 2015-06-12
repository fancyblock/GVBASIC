using UnityEngine;
using System.Collections;
using System.Text;

public class SaveCode : State 
{
    protected StringBuilder m_fileName;
    protected int m_curInputIndex;

    public override void onInit() 
    {
        m_fileName = new StringBuilder();
    }

    public override void onSwitchIn() 
    {
        string pureBasName = m_stateMgr.CUR_CODE_FILE_NAME;
        pureBasName = pureBasName.Substring(0, pureBasName.Length - 4);

        m_fileName = new StringBuilder(pureBasName);
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
            case KCode.Return:
                CodeMgr.SharedInstance.SaveSourceCode(m_fileName.ToString() + ".BAS", m_stateMgr.CUR_SOURCE_CODE);
                m_stateMgr.GotoState(StateEnums.eStateMenu);
                break;
            case KCode.LeftArrow:
            case KCode.RightArrow:
                onMoveCursor(key);
                break;
            default:
                onChar(key);
                break;
        }
    }


    protected void onChar( KCode key )
    {
        // limit the file name length 
        if (m_fileName.Length >= Defines.TEXT_AREA_WIDTH - 2)
            return;

        int chr = (int)key;
        if (chr < 0 || chr >= 128)
            return;

        //TODO 
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
            //TODO 
        }
    }

}
