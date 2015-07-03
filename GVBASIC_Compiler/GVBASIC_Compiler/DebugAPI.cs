using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GVBASIC_Compiler.Compiler;

namespace GVBASIC_Compiler
{
    public class DebugAPI : IAPI
    {
        protected StringBuilder m_outputBuff;
        protected string m_output;

        protected int m_inkeyBufferCount;
        protected int m_inputBufferCount;

        public DebugAPI()
        {
            m_outputBuff = new StringBuilder();
        }

        /// <summary>
        /// getter of the program output 
        /// </summary>
        public string OUT_PUT { get { return m_output; } }

        /// <summary>
        /// error code 
        /// </summary>
        /// <param name="error"></param>
        public void ErrorCode( string error )
        {
            throw new Exception(error);
        }

        public void ProgramStart()
        {
            m_inkeyBufferCount = 0;
            m_inputBufferCount = 0;
        }

        /// <summary>
        /// program runing done 
        /// </summary>
        public void ProgramDone()
        {
            System.Console.Write("\n\n[END OF PROGRAM]");

            // set the output 
            m_output = m_outputBuff.ToString();
        }

        /// <summary>
        /// print 
        /// </summary>
        /// <param name="expList"></param>
        public void Print(List<BaseData> dataList)
        {
            int lastType = -1;
            bool closeTo = false;
            string output = null;

            foreach (BaseData dat in dataList)
            {
                output = "";

                switch( dat.TYPE )
                {
                    case BaseData.TYPE_FLOAT:
                        output = dat.FLOAT_VAL.ToString();
                        break;
                    case BaseData.TYPE_INT:
                        output = dat.INT_VAL.ToString();
                        break;
                    case BaseData.TYPE_STRING:
                        output = dat.STR_VAL;
                        break;
                    case BaseData.TYPE_NEXT_LINE:
                        if (lastType == BaseData.TYPE_FLOAT || lastType == BaseData.TYPE_INT || lastType == BaseData.TYPE_STRING)
                            output = "\n";
                        break;
                    case BaseData.TYPE_SPACE:
                        for (int i = 0; i < dat.INT_VAL; i++)
                            output = " ";
                        break;
                    case BaseData.TYPE_TAB:
                        //TODO 
                        break;
                    case BaseData.TYPE_CLOSE_TO:
                        if (lastType == BaseData.TYPE_FLOAT || lastType == BaseData.TYPE_INT || lastType == BaseData.TYPE_STRING)
                            closeTo = true;
                        break;
                    default:
                        break;
                }

                System.Console.Write(output);
                m_outputBuff.Append(output);

                lastType = dat.TYPE;
            }

            if (!(lastType == BaseData.TYPE_CLOSE_TO && closeTo))
            {
                System.Console.Write("\n");
                m_outputBuff.Append("\n");
            }
        }

        public void AskInkey(int count) { m_inkeyBufferCount = count; }
        public bool HasInkeyBuffer() { return m_inkeyBufferCount > 0; }

        public string Inkey()
        {
            m_inkeyBufferCount--;

            return "0";
        }

        public bool HasInputBuffer() { return m_inputBufferCount > 0; }
        public void AskInput(int count) { m_inputBufferCount = count; }
        public string Input()
        {
            m_inputBufferCount--;

            return "0";
        }

        public void Beep() { }
        public void Cls() { }
        public void Inverse() { }
        public void Normal() { }
        public void Graph() { }
        public void Text() { }
        public void Locate(int x, int y) { }
        public int CursorX() { return 0; }
        public void Play(string sound) { }
        public void Box(int x0, int y0, int x1, int y1, int fill, int type) { }
        public void Circle(int x, int y, int r, int fill, int type) { }
        public void Draw(int x, int y, int type) { }          // draw point 
        public void Ellipse(int x, int y, int a, int b, int fill, int type) { }
        public void Line(int x0, int y0, int x1, int y1, int type) { }

    }
}
