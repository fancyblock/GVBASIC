using UnityEngine;
using System.Collections;

public class CodeRuntime : State 
{
    public GraphDisplay m_graphDisplay;

    public override void onSwitchIn()
    {
        //TODO 
    }

    public override void onInput(KCode key)
    {
        switch(key)
        {
            case KCode.Escape:
                m_stateMgr.GotoState(StateEnums.eStateMenu);
                return;
            default:
                break;
        }
    }

}
