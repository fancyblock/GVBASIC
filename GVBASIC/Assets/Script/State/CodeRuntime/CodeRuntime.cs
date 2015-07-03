using UnityEngine;
using System.Collections;
using GVBASIC_Compiler.Compiler;


public class CodeRuntime : State 
{
    protected const int STATUS_RUNNING  = 1;
    protected const int STATUS_INKEY    = 2;
    protected const int STATUS_INPUT    = 3;

    public EmuAPI m_emuAPI;

    protected Runtime m_runtime;
    protected int m_status;

    protected int m_inkeyCount;
    protected int m_inputCount;

    public override void onSwitchIn()
    {
        // run the code 
        Tokenizer tokenizer = new Tokenizer( m_stateMgr.CUR_SOURCE_CODE );
        Parser parser = new Parser(tokenizer);
        m_runtime = new Runtime(parser);

        m_status = STATUS_RUNNING;

        m_runtime.SetAPI(m_emuAPI);
        m_runtime.Run();

        m_emuAPI.Text();
        m_emuAPI.Locate(1, 1);
    }

    public override void onInput(KCode key)
    {
        if( key == KCode.Escape )
        {
            m_stateMgr.GotoState(StateEnums.eStateMenu);
            return;
        }

        switch(m_status)
        {
            case STATUS_INKEY:
                m_emuAPI.AppendInkey(key);
                m_inkeyCount--;

                if (m_inkeyCount <= 0)
                    m_status = STATUS_RUNNING;
                break;
            case STATUS_INPUT:
                //TODO 
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// update 
    /// </summary>
    public override void onUpdate() 
    {
        switch( m_status )
        {
            case STATUS_RUNNING:
                m_runtime.Step();
                break;
            case STATUS_INKEY:
                break;
            case STATUS_INPUT:
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// enter waitting input mode 
    /// </summary>
    public void GetInkey( int count )
    {
        m_inkeyCount = count;
        m_status = STATUS_INKEY;
    }

    public void GetInput( int count )
    {
        m_inputCount = count;
        m_status = STATUS_INPUT;
    }

}
