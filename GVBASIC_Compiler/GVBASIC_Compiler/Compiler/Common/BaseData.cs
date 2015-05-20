using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVBASIC_Compiler.Compiler
{
    public class BaseData
    {
        public const int TYPE_INT = 1;
        public const int TYPE_FLOAT = 2;
        public const int TYPE_STRING = 3;
        public const int TYPE_SPACE = 4;
        public const int TYPE_TAB = 5;
        public const int TYPE_NEXT_LINE = 6;
        public const int TYPE_CLOSE_TO = 7;

        public int m_intVal;
        public float m_floatVal;
        public string m_stringVal;

        public int m_type;

        /// <summary>
        /// default constructor
        /// </summary>
        public BaseData() { }

        /// <summary>
        /// constructor 
        /// </summary>
        /// <param name="bd"></param>
        public BaseData(BaseData bd)
        {
            m_type = bd.m_type;
            m_intVal = bd.m_intVal;
            m_floatVal = bd.m_floatVal;
            m_stringVal = bd.m_stringVal;
        }

        /// <summary>
        /// float constructor
        /// </summary>
        /// <param name="val"></param>
        public BaseData(float val)
        {
            m_type = TYPE_FLOAT;
            m_floatVal = val;
        }

        /// <summary>
        /// int constructor
        /// </summary>
        /// <param name="val"></param>
        public BaseData(int val)
        {
            m_type = TYPE_INT;
            m_intVal = val;
        }

        /// <summary>
        /// string constructor
        /// </summary>
        /// <param name="val"></param>
        public BaseData(string val)
        {
            m_type = TYPE_STRING;
            m_stringVal = val;
        }

        /// <summary>
        /// convert the data type 
        /// </summary>
        /// <param name="type"></param>
        public void Convert(int type)
        {
            switch (type)
            {
                case BaseData.TYPE_INT:
                    if (m_type == BaseData.TYPE_FLOAT)
                        m_intVal = (int)m_floatVal;
                    else if (m_type != BaseData.TYPE_INT)
                        throw new ErrorCode(ErrorCode.ERROR_CODE_12);
                    break;
                case BaseData.TYPE_FLOAT:
                    if (m_type == BaseData.TYPE_INT)
                        m_floatVal = m_intVal;
                    else if (m_type != BaseData.TYPE_FLOAT)
                        throw new ErrorCode(ErrorCode.ERROR_CODE_12);
                    break;
                case BaseData.TYPE_STRING:
                    if (m_type == BaseData.TYPE_FLOAT)
                        m_stringVal = m_floatVal.ToString();
                    else if (m_type == BaseData.TYPE_INT)
                        m_stringVal = m_intVal.ToString();
                    else if (m_type != BaseData.TYPE_STRING)
                        throw new ErrorCode(ErrorCode.ERROR_CODE_12);
                    break;
                default:
                    throw new ErrorCode(ErrorCode.ERROR_CODE_12);
            }

            m_type = type;
        }

        /// <summary>
        /// plus 
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static BaseData operator +(BaseData lhs, BaseData rhs)
        {
            BaseData result = new BaseData(lhs);

            if (lhs.m_type == BaseData.TYPE_STRING || rhs.m_type == BaseData.TYPE_STRING)
            {
                result.Convert(BaseData.TYPE_STRING);
                lhs.Convert(BaseData.TYPE_STRING);
                rhs.Convert(BaseData.TYPE_STRING);

                result.m_stringVal = lhs.m_stringVal + rhs.m_stringVal;
            }
            else if (lhs.m_type == BaseData.TYPE_FLOAT || rhs.m_type == BaseData.TYPE_FLOAT)
            {
                result.Convert(BaseData.TYPE_FLOAT);
                lhs.Convert(BaseData.TYPE_FLOAT);
                rhs.Convert(BaseData.TYPE_FLOAT);

                result.m_floatVal = lhs.m_floatVal + rhs.m_floatVal;
            }
            else if (lhs.m_type == BaseData.TYPE_INT && rhs.m_type == BaseData.TYPE_INT)
            {
                result.m_intVal = lhs.m_intVal + rhs.m_intVal;
            }
            else
            {
                throw new ErrorCode(ErrorCode.ERROR_CODE_12);
            }

            return result;
        }
    }
}
