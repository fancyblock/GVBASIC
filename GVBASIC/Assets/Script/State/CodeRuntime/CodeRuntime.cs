using UnityEngine;
using System.Collections;
using GVBASIC_Compiler.Compiler;


public class CodeRuntime : State 
{
    public EmuAPI m_emuAPI;

    protected Runtime m_runtime;

    public override void onSwitchIn()
    {
        // run the code 
        Tokenizer tokenizer = new Tokenizer( m_stateMgr.CUR_SOURCE_CODE );
        Parser parser = new Parser(tokenizer);
        m_runtime = new Runtime(parser);

        m_runtime.SetAPI(m_emuAPI);
        m_runtime.Run();
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

    /// <summary>
    /// update 
    /// </summary>
    public override void onUpdate() 
    {
        m_runtime.Step();
    }

}
