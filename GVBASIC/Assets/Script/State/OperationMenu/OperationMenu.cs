using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class OperationMenu : State 
{
    protected int m_curItemIdx;
    protected List<string> m_itemList;
    protected int m_lineOffset;

    public override void onInit()
    {
    }

    public override void onSwitchIn()
    {
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
                deleteCurrentFile();
                break;
            case KCode.F1:
                createNewFile();
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
        m_itemList = CodeMgr.SharedInstance.BAS_LIST;

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

    protected void createNewFile()
    {
        // create a new file 
        m_stateMgr.CUR_CODE_FILE_NAME = "";
        m_stateMgr.CUR_SOURCE_CODE = "";
        m_stateMgr.GotoState(StateEnums.eStateEditor);
    }

    protected void deleteCurrentFile()
    {
        List<string> baseList = CodeMgr.SharedInstance.BAS_LIST;

        if (baseList.Count == 0)
            return;

        string fileName = baseList[m_curItemIdx];
        CodeMgr.SharedInstance.DeleteSourceCode(fileName);

        if (m_curItemIdx >= (baseList.Count - 1))
            m_curItemIdx--;
        if (m_curItemIdx < 0)
            m_curItemIdx = 0;

        refreshList(m_curItemIdx);
    }

    protected void executeItem()
    {
        // run the code file 
        m_stateMgr.CUR_SOURCE_CODE = CodeMgr.SharedInstance.GetSourceCode(m_itemList[m_curItemIdx]);
        m_stateMgr.GotoState(StateEnums.eStateRunner);
    }

    protected void editFile()
    {
        string fileName = m_itemList[m_curItemIdx];

        m_stateMgr.CUR_CODE_FILE_NAME = fileName;
        m_stateMgr.CUR_SOURCE_CODE = CodeMgr.SharedInstance.GetSourceCode(fileName);
        m_stateMgr.GotoState(StateEnums.eStateEditor);
    }

}
