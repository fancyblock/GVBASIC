using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class CodeMgr 
{
    static protected CodeMgr m_instance = null;

    /// <summary>
    /// getter of the singleton 
    /// </summary>
    static public CodeMgr SharedInstance
    {
        get
        {
            if (m_instance == null)
                m_instance = new CodeMgr();

            return m_instance;
        }
    }

    /// <summary>
    /// getter of the bas file list 
    /// </summary>
    public List<string> BAS_LIST
    {
        get
        {
            return new List<string>() { "test.bas" };
        }
    }

    /// <summary>
    /// get code 
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public string GetCode( string fileName )
    {
        //TODO 

        return "";
    }

}
