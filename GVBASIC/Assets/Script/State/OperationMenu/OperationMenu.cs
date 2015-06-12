using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class OperationMenu : State 
{
    protected int m_curItemIdx;
    protected List<string> m_itemList = new List<string>();
    protected int m_lineOffset;

    public override void onInit()
    {
    }

    public override void onSwitchIn()
    {
        m_itemList.Clear();
        m_itemList.Add("New File..");

        // load file list 
        foreach (string code in CodeMgr.SharedInstance.BAS_LIST)
            m_itemList.Add(code);

        m_textDisplay.SetCursor(false);
        m_lineOffset = 0;

        refreshList(0);
    }

    public override void onInput(KCode key)
    {
        switch( key )
        {
            case KCode.UpArrow:
                if (m_curItemIdx > 0)
                    refreshList(m_curItemIdx - 1);
                break;
            case KCode.DownArrow:
                if (m_curItemIdx < m_itemList.Count - 1)
                    refreshList(m_curItemIdx + 1);
                break;
            case KCode.Return:
                executeItem();
                break;
            case KCode.Delete:
            case KCode.F2:
                if( m_curItemIdx > 0 )
                {
                    //TODO 
                }
                break;
            case KCode.F4:
                editFile();
                break;
            default:
                break;
        }
    }


    protected void refreshList( int index )
    {
        m_curItemIdx = index;

        // update the offset 
        if( m_curItemIdx - m_lineOffset >= Defines.TEXT_AREA_HEIGHT )
            m_lineOffset = m_curItemIdx - Defines.TEXT_AREA_HEIGHT + 1;
        else if( m_curItemIdx - m_lineOffset < 0 )
            m_lineOffset = m_curItemIdx ;

        m_textDisplay.Clean();

        int y = 0;
        for (int i = m_lineOffset; i < m_itemList.Count; i++ )
            m_textDisplay.DrawText(0, y++, m_itemList[i], i == m_curItemIdx);

        m_textDisplay.Refresh();
    }

    protected void executeItem()
    {
        if( m_curItemIdx == 0 )
        {
            // create a new file 
            m_stateMgr.CUR_CODE_FILE_NAME = "NEW FILE.BAS";
            m_stateMgr.CUR_SOURCE_CODE = "";
            m_stateMgr.GotoState(StateEnums.eStateEditor);
        }
        else
        {
            // run the code file 
            m_stateMgr.CUR_SOURCE_CODE = CodeMgr.SharedInstance.GetSourceCode(m_itemList[m_curItemIdx]);
            m_stateMgr.GotoState(StateEnums.eStateRunner);
        }
    }

    protected void editFile()
    {
        if (m_curItemIdx == 0)
            return;

        string fileName = m_itemList[m_curItemIdx];

        m_stateMgr.CUR_CODE_FILE_NAME = fileName;
        m_stateMgr.CUR_SOURCE_CODE = CodeMgr.SharedInstance.GetSourceCode(fileName);
        m_stateMgr.GotoState(StateEnums.eStateEditor);
    }

}
