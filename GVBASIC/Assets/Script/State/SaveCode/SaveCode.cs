using UnityEngine;
using System.Collections;
using System.Text;

public class SaveCode : State 
{
    protected StringBuilder m_fileName;

    public override void onInit() 
    {
        m_fileName = new StringBuilder();
    }

    public override void onSwitchIn() 
    {
        //TODO 

        m_textDisplay.Clean();
        m_textDisplay.DrawText(0, 0, "Input file name:");
        m_textDisplay.Refresh();

        m_textDisplay.SetCursor(true, 0, 1);
    }

    public override void onInput(KCode key) 
    {
        //TODO 
    }

}
