using System;
using System.Collections.Generic;


namespace GVBASIC_Compiler.Compiler
{
    public struct BaseData
    {
        public const int TYPE_INT       = 1;
        public const int TYPE_FLOAT     = 2;
        public const int TYPE_STRING    = 3;

        public const int TYPE_SPACE     = 4;
        public const int TYPE_TAB       = 5;
        public const int TYPE_NEXT_LINE = 6;
        public const int TYPE_CLOSE_TO  = 7;

        public static BaseData ZERO = new BaseData(0);

        private int m_intVal;
        private float m_floatVal;
        private string m_stringVal;
        private int m_type;

        /// <summary>
        /// getter of the type 
        /// </summary>
        public int TYPE { get { return m_type; } }

        public int INT_VAL { get { return m_intVal; } }
        public float FLOAT_VAL { get { return m_floatVal; } }
        public string STR_VAL { get { return m_stringVal; } }


        /// <summary>
        /// special type constructor
        /// </summary>
        /// <param name="type"></param>
        /// <param name="param"></param>
        public BaseData( int type, int param )
        {
            m_type = type;
            m_intVal = param;
            m_floatVal = 0.0f;
            m_stringVal = null;
        }

        /// <summary>
        /// float constructor
        /// </summary>
        /// <param name="val"></param>
        public BaseData(float val)
        {
            m_type = TYPE_FLOAT;
            m_intVal = (int)val;
            m_floatVal = val;
            m_stringVal = val.ToString();
        }

        /// <summary>
        /// int constructor
        /// </summary>
        /// <param name="val"></param>
        public BaseData(int val)
        {
            m_type = TYPE_INT;
            m_intVal = val;
            m_floatVal = val;
            m_stringVal = val.ToString();
        }

        /// <summary>
        /// string constructor
        /// </summary>
        /// <param name="val"></param>
        public BaseData(string val)
        {
            m_type = TYPE_STRING;
            m_intVal = 0;
            m_floatVal = 0.0f;
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
        /// neg the int && float value 
        /// </summary>
        public void NegValue()
        {
            if (m_type == BaseData.TYPE_INT)
                m_intVal = -m_intVal;
            else if (m_type == BaseData.TYPE_FLOAT)
                m_floatVal = -m_floatVal;
            else
                throw new ErrorCode(ErrorCode.ERROR_CODE_12);
        }

        /// <summary>
        /// plus 
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static BaseData operator +(BaseData lhs, BaseData rhs)
        {
            BaseData result = lhs;

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

        public static BaseData operator -(BaseData lhs, BaseData rhs)
        {
            BaseData result;

            if (lhs.m_type == BaseData.TYPE_FLOAT || rhs.m_type == BaseData.TYPE_FLOAT)
            {
                lhs.Convert(BaseData.TYPE_FLOAT);
                rhs.Convert(BaseData.TYPE_FLOAT);
                result = new BaseData(lhs.m_floatVal - rhs.m_floatVal);
            }
            else if (lhs.m_type == BaseData.TYPE_INT && rhs.m_type == BaseData.TYPE_INT)
            {
                result = new BaseData(lhs.m_intVal - rhs.m_intVal);
            }
            else
            {
                throw new ErrorCode(ErrorCode.ERROR_CODE_12);
            }

            return result;
        }

        public static BaseData operator *(BaseData lhs, BaseData rhs)
        {
            //TODO 

            return BaseData.ZERO;
        }

        public static BaseData operator /(BaseData lhs, BaseData rhs)
        {
            //TODO 

            return BaseData.ZERO;
        }

        /// <summary>
        /// >
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static bool operator >(BaseData lhs, BaseData rhs)
        {
            if (lhs.m_type == BaseData.TYPE_STRING || rhs.m_type == BaseData.TYPE_STRING)
            {
                lhs.Convert(BaseData.TYPE_STRING);
                rhs.Convert(BaseData.TYPE_STRING);

                return lhs.m_stringVal.CompareTo(rhs.m_stringVal) > 0;
            }
            else if (lhs.m_type == BaseData.TYPE_FLOAT || rhs.m_type == BaseData.TYPE_FLOAT)
            {
                lhs.Convert(BaseData.TYPE_FLOAT);
                rhs.Convert(BaseData.TYPE_FLOAT);

                return lhs.m_floatVal > rhs.m_floatVal;
            }
            else if (lhs.m_type == BaseData.TYPE_INT && rhs.m_type == BaseData.TYPE_INT)
            {
                return lhs.m_intVal > rhs.m_intVal;
            }
            else
            {
                throw new ErrorCode(ErrorCode.ERROR_CODE_12);
            }
        }

        /// <summary>
        /// <
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static bool operator <(BaseData lhs, BaseData rhs)
        {
            if (lhs.m_type == BaseData.TYPE_STRING || rhs.m_type == BaseData.TYPE_STRING)
            {
                lhs.Convert(BaseData.TYPE_STRING);
                rhs.Convert(BaseData.TYPE_STRING);

                return lhs.m_stringVal.CompareTo(rhs.m_stringVal) < 0;
            }
            else if (lhs.m_type == BaseData.TYPE_FLOAT || rhs.m_type == BaseData.TYPE_FLOAT)
            {
                lhs.Convert(BaseData.TYPE_FLOAT);
                rhs.Convert(BaseData.TYPE_FLOAT);

                return lhs.m_floatVal < rhs.m_floatVal;
            }
            else if (lhs.m_type == BaseData.TYPE_INT && rhs.m_type == BaseData.TYPE_INT)
            {
                return lhs.m_intVal < rhs.m_intVal;
            }
            else
            {
                throw new ErrorCode(ErrorCode.ERROR_CODE_12);
            }
        }

        public static bool operator ==(BaseData lhs, BaseData rhs)
        {
            if (lhs.m_type == BaseData.TYPE_STRING && rhs.m_type == BaseData.TYPE_STRING)
            {
                return lhs.m_stringVal == rhs.m_stringVal;
            }
            else if (lhs.m_type == BaseData.TYPE_FLOAT || rhs.m_type == BaseData.TYPE_FLOAT)
            {
                lhs.Convert(BaseData.TYPE_FLOAT);
                rhs.Convert(BaseData.TYPE_FLOAT);

                return Math.Abs(lhs.m_floatVal - rhs.m_floatVal) <= float.Epsilon;
            }
            else if (lhs.m_type == BaseData.TYPE_INT && rhs.m_type == BaseData.TYPE_INT)
            {
                return lhs.m_intVal == rhs.m_intVal;
            }
            else
            {
                throw new ErrorCode(ErrorCode.ERROR_CODE_12);
            }
        }

        /// <summary>
        /// compare 
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static bool operator !=(BaseData lhs, BaseData rhs)
        {
            if (lhs.m_type == BaseData.TYPE_STRING && rhs.m_type == BaseData.TYPE_STRING)
            {
                return lhs.m_stringVal != rhs.m_stringVal;
            }
            else if (lhs.m_type == BaseData.TYPE_FLOAT || rhs.m_type == BaseData.TYPE_FLOAT)
            {
                lhs.Convert(BaseData.TYPE_FLOAT);
                rhs.Convert(BaseData.TYPE_FLOAT);

                return Math.Abs( lhs.m_floatVal - rhs.m_floatVal ) > float.Epsilon;
            }
            else if (lhs.m_type == BaseData.TYPE_INT && rhs.m_type == BaseData.TYPE_INT)
            {
                return lhs.m_intVal != rhs.m_intVal;
            }
            else
            {
                throw new ErrorCode(ErrorCode.ERROR_CODE_12);
            }
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
    }
}
