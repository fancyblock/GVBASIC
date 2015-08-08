using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class CodeList : State 
{
    protected int m_curItemIdx;
    protected List<string> m_basList;
    protected int m_lineOffset;

    public override void onSwitchIn()
    {
        GraphMode();
        m_lineOffset = 0;

        readFileList();
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
                if (m_curItemIdx < m_basList.Count - 1)
                    refreshList(m_curItemIdx + 1);
                break;
            case KCode.Return:
                executeItem();
                break;
            case KCode.F2:
                // 弹出确认删除的提示
                m_stateMgr.ShowMessageBox("Delete file",
                    () =>
                    {
                        deleteCurrentFile();
                    },
                    () =>
                    {
                        refreshList(m_curItemIdx);
                    });
                break;
            case KCode.F1:
                // 弹出确认创建的提示
                m_stateMgr.ShowMessageBox("Create file",
                    () =>
                    {
                        createNewFile();
                    },
                    () =>
                    {
                        refreshList(m_curItemIdx);
                    });
                break;
            case KCode.F4:
                editFile();
                break;
            default:
                break;
        }
    }


    protected void readFileList()
    {
        m_basList = new List<string>();

        foreach (string basFileName in CodeMgr.SharedInstance.BAS_LIST)
            m_basList.Add(basFileName);
    }

    protected void refreshList( int index )
    {
        m_curItemIdx = index;
        
        // update the offset 
        if( m_curItemIdx - m_lineOffset >= Defines.TEXT_AREA_HEIGHT )
            m_lineOffset = m_curItemIdx - Defines.TEXT_AREA_HEIGHT + 1;
        else if( m_curItemIdx - m_lineOffset < 0 )
            m_lineOffset = m_curItemIdx ;

        m_stateMgr.m_textDisplay.Clear();

        int y = 0;
        for (int i = m_lineOffset; i < m_basList.Count; i++)
        {
            m_stateMgr.m_textDisplay.DrawText(0, y, m_basList[i], i == m_curItemIdx);
            y++;
        }

        m_stateMgr.m_textDisplay.Refresh();
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

        readFileList();
        refreshList(m_curItemIdx);
    }

    protected void executeItem()
    {
        // run the code file 
        m_stateMgr.CUR_SOURCE_CODE = CodeMgr.SharedInstance.GetSourceCode(m_basList[m_curItemIdx]);
        m_stateMgr.GotoState(StateEnums.eStateRunner);
    }

    protected void editFile()
    {
        string fileName = m_basList[m_curItemIdx];

        m_stateMgr.CUR_CODE_FILE_NAME = fileName;
        m_stateMgr.CUR_SOURCE_CODE = CodeMgr.SharedInstance.GetSourceCode(fileName);
        m_stateMgr.GotoState(StateEnums.eStateEditor);
    }

}
