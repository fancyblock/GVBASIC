﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GVBASIC_Compiler.Compiler;

namespace GVBASIC_Compiler
{
    public class DebugAPI : IAPI
    {
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
        }

        /// <summary>
        /// print 
        /// </summary>
        /// <param name="expList"></param>
        public void Print(List<BaseData> dataList)
        {
            int lastType = -1;
            bool closeTo = false;

            foreach (BaseData dat in dataList)
            {
                switch( dat.TYPE )
                {
                    case BaseData.TYPE_FLOAT:
                        System.Console.Write(dat.FLOAT_VAL.ToString());
                        break;
                    case BaseData.TYPE_INT:
                        System.Console.Write(dat.INT_VAL.ToString());
                        break;
                    case BaseData.TYPE_STRING:
                        System.Console.Write(dat.STR_VAL);
                        break;
                    case BaseData.TYPE_NEXT_LINE:
                        if (lastType == BaseData.TYPE_FLOAT || lastType == BaseData.TYPE_INT || lastType == BaseData.TYPE_STRING)
                            System.Console.Write("\n");
                        break;
                    case BaseData.TYPE_SPACE:
                        for (int i = 0; i < dat.INT_VAL; i++)
                            System.Console.Write(" ");
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

                lastType = dat.TYPE;
            }

            if (!(lastType == BaseData.TYPE_CLOSE_TO && closeTo))
                System.Console.Write("\n");
        }
    }
}
