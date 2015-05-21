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

        public void Beep() { }
        public void Cls() { }
        public void Inverse() { }
        public void Nromal() { }
        public void Graph() { }
        public void Text() { }

    }
}
