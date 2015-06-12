using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;


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
            string[] files = Directory.GetFiles( Application.persistentDataPath, "*.BAS");
            List<string> basList = new List<string>();

            foreach( string fileName in files )
            {
                int idx1 = fileName.LastIndexOf('/');
                int idx2 = fileName.LastIndexOf('\\');

                if (idx1 > idx2)
                    basList.Add(fileName.Substring(idx1 + 1));
                else
                    basList.Add(fileName.Substring(idx2 + 1));
            }

            return basList;
        }
    }

    /// <summary>
    /// get code 
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public string GetSourceCode( string fileName )
    {
        string code = null;

        using( StreamReader sr = new StreamReader(Application.persistentDataPath + "/" + fileName) )
        {
            code = sr.ReadToEnd();
        }

        return code;
    }

    /// <summary>
    /// save source code 
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="sourceCode"></param>
    public void SaveSourceCode( string fileName, string sourceCode )
    {
        using( StreamWriter sw = new StreamWriter(Application.persistentDataPath + "/" + fileName ))
        {
            sw.Write(sourceCode);
        }
    }

    public void DeleteSourceCode( string fileName )
    {
        string path = Application.persistentDataPath + "/" + fileName;

        if (File.Exists(path))
            File.Delete(path);
    }
}
