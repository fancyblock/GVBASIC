using UnityEngine;
using System.Collections;
using GVBASIC_Compiler.Compiler;


public class CodeRuntime : State 
{
    public EmuAPI m_emuAPI;

    protected Runtime m_runtime;
    protected bool m_waittingInputMode;

    public override void onSwitchIn()
    {
        // run the code 
        Tokenizer tokenizer = new Tokenizer( m_stateMgr.CUR_SOURCE_CODE );
        Parser parser = new Parser(tokenizer);
        m_runtime = new Runtime(parser);

        m_waittingInputMode = false;
        m_runtime.SetAPI(m_emuAPI);
        m_runtime.Run();

        m_emuAPI.Text();
        m_emuAPI.Locate(1, 1);
    }

    public override void onInput(KCode key)
    {
        switch(key)
        {
            case KCode.Escape:
                m_stateMgr.GotoState(StateEnums.eStateMenu);
                return;
            default:
                if (m_waittingInputMode)
                {
                    m_emuAPI.InjectKey(key);
                    m_waittingInputMode = false;
                }
                break;
        }
    }

    /// <summary>
    /// update 
    /// </summary>
    public override void onUpdate() 
    {
        if( !m_waittingInputMode )
            m_runtime.Step();
    }

    /// <summary>
    /// enter waitting input mode 
    /// </summary>
    public void WaittingInput()
    {
        m_waittingInputMode = true;
    }

}
