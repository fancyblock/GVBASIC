using UnityEngine;
using System.Collections;
using System.Text;
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

    protected StringBuilder m_inputBuff;

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
            m_stateMgr.GotoState(StateEnums.eStateList);
            return;
        }

        switch(m_status)
        {
            case STATUS_INKEY:
                m_emuAPI.AppendInkey(key);
                m_inkeyCount--;
                // 输入完毕退出
                if (m_inkeyCount <= 0)
                    m_status = STATUS_RUNNING;
                break;
            case STATUS_INPUT:
                if( key == KCode.Return )
                {
                    // 一个输入完毕
                    m_emuAPI.AppendInput(m_inputBuff.ToString());
                    m_inputCount--;

                    // 显示换行
                    m_stateMgr.m_textDisplay.DrawText(m_stateMgr.m_textDisplay.CURSOR_X, m_stateMgr.m_textDisplay.CURSOR_Y, "\n");
                    m_stateMgr.m_textDisplay.SetCursor(0, 0);

                    // 全部输入完毕
                    if (m_inputCount <= 0)
                        m_status = STATUS_RUNNING;
                    else
                        m_inputBuff = new StringBuilder();
                }
                else if( key == KCode.Backspace )
                {
                    // 删除一个字符 
                    if( m_inputCount > 0 )
                    {
                        //TODO 
                        m_inputCount--;
                    }
                }
                else
                {
                    if (key < KCode.Delete)
                    {
                        m_inputBuff.Append((char)key);
                        // 显示该字符 
                        m_stateMgr.m_textDisplay.DrawText(m_stateMgr.m_textDisplay.CURSOR_X, m_stateMgr.m_textDisplay.CURSOR_Y, ((char)key).ToString());
                        m_stateMgr.m_textDisplay.SetCursor(0, 0);
                    }
                }
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
        if (m_status == STATUS_RUNNING)
            m_runtime.Step();
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

        m_inputBuff = new StringBuilder();
        m_stateMgr.m_textDisplay.DrawText(m_stateMgr.m_textDisplay.CURSOR_X, m_stateMgr.m_textDisplay.CURSOR_Y, "?");
        m_stateMgr.m_textDisplay.SetCursor(0, 0);
    }

}
