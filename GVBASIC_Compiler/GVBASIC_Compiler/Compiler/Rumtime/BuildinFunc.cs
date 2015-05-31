using System;
using System.Collections.Generic;

namespace GVBASIC_Compiler.Compiler
{
    public class BuildinFunc
    {
        protected IAPI m_iapi;
        protected Dictionary<string, Func<List<BaseData>, BaseData>> m_funcDic;
        protected float m_lastRandomNum = 0.0f;
        protected Random m_random = new Random();

        /// <summary>
        /// constructor
        /// </summary>
        public BuildinFunc()
        {
            // initial the functions 
            m_funcDic = new Dictionary<string, Func<List<BaseData>, BaseData>>()
            {
                {"INKEY$", INKEY},
                // math function 
                {"ABS", ABS},
                {"SGN", SGN},
                {"INT", INT},
                {"SIN", SIN},
                {"COS", COS},
                {"TAN", TAN},
                {"ATN", ATN},
                {"SQR", SQR},
                {"EXP", EXP},
                {"LOG", LOG},
                {"RND", RND},
                // string function 
                {"", ABS},
                // display function 
                {"1", ABS},
                // file function
                {"2", ABS},
            };
        }
        
        /// <summary>
        /// set api 
        /// </summary>
        /// <param name="apiCall"></param>
        public void SetAPI( IAPI apiCall )
        {
            m_iapi = apiCall;
        }
        
        /// <summary>
        /// has function or not 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool HasFunc( string name )
        {
            return m_funcDic.ContainsKey(name);
        }

        /// <summary>
        /// call function 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public BaseData CallFunc( string name, List<BaseData> parameters )
        {
            return m_funcDic[name](parameters);
        }


        #region inner functions

        protected BaseData INKEY( List<BaseData> param )
        {
            BaseData ret = new BaseData( m_iapi.Inkey() );

            return ret;
        }

        protected BaseData ABS( List<BaseData> param )
        {
            checkMathParam(param);

            BaseData ret = BaseData.ZERO;
            BaseData p = param[0];

            if (p.TYPE == BaseData.TYPE_INT)
                ret = new BaseData(p.INT_VAL >= 0 ? p.INT_VAL : -p.INT_VAL);
            else if (p.TYPE == BaseData.TYPE_FLOAT)
                ret = new BaseData(p.FLOAT_VAL >= 0.0f ? p.FLOAT_VAL : -p.FLOAT_VAL);

            return ret;
        }

        protected BaseData SGN(List<BaseData> param)
        {
            checkMathParam(param);

            BaseData ret = BaseData.ZERO;
            BaseData p = param[0];

            if (p.TYPE == BaseData.TYPE_INT)
            {
                ret = new BaseData(p.INT_VAL == 0 ? 0 : p.INT_VAL / Math.Abs(p.INT_VAL));
            }
            else if (p.TYPE == BaseData.TYPE_FLOAT)
            {
                if (p.FLOAT_VAL < float.Epsilon && p.FLOAT_VAL > -float.Epsilon)
                    ret = new BaseData(0.0f);
                else
                    ret = new BaseData(p.FLOAT_VAL >= 0.0f ? 1 : -1);
            }

            return ret;
        }

        protected BaseData INT( List<BaseData> param )
        {
            checkMathParam(param);

            BaseData ret = BaseData.ZERO;
            BaseData p = param[0];

            if (p.TYPE == BaseData.TYPE_INT)
                ret = new BaseData(p.INT_VAL);
            else if (p.TYPE == BaseData.TYPE_FLOAT)
                ret = new BaseData(((int)p.FLOAT_VAL));

            return ret;
        }

        protected BaseData SIN(List<BaseData> param)
        {
            checkMathParam(param);

            BaseData ret = BaseData.ZERO;
            BaseData p = param[0];

            //TODO 

            return ret;
        }

        protected BaseData COS(List<BaseData> param)
        {
            checkMathParam(param);

            BaseData ret = BaseData.ZERO;
            BaseData p = param[0];

            //TODO 

            return ret;
        }

        protected BaseData TAN(List<BaseData> param)
        {
            checkMathParam(param);

            BaseData ret = BaseData.ZERO;
            BaseData p = param[0];

            //TODO 

            return ret;
        }

        protected BaseData ATN(List<BaseData> param)
        {
            checkMathParam(param);

            BaseData ret = BaseData.ZERO;
            BaseData p = param[0];

            //TODO 

            return ret;
        }

        protected BaseData SQR(List<BaseData> param)
        {
            checkMathParam(param);

            BaseData ret = BaseData.ZERO;
            BaseData p = param[0];

            //TODO 

            return ret;
        }

        protected BaseData EXP(List<BaseData> param)
        {
            checkMathParam(param);

            BaseData ret = BaseData.ZERO;
            BaseData p = param[0];

            //TODO 

            return ret;
        }

        protected BaseData LOG(List<BaseData> param)
        {
            checkMathParam(param);

            BaseData ret = BaseData.ZERO;
            BaseData p = param[0];

            //TODO 

            return ret;
        }

        /// <summary>
        /// *产生一个(0,1)间的随机小数。如果x>0,每次产生不同的随机数;如果x<0,产生有一定序列的随机数;如果x=0,输出上次产生的随机数。
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        protected BaseData RND(List<BaseData> param)
        {
            checkMathParam(param);

            BaseData ret = BaseData.ZERO;
            BaseData p = param[0];

            int randParam = 0;

            if (p.TYPE == BaseData.TYPE_INT)
                randParam = p.INT_VAL;
            else if (p.TYPE == BaseData.TYPE_FLOAT)
                randParam = (int)p.FLOAT_VAL;

            if( randParam > 0 )
            {
                m_lastRandomNum = (float)m_random.NextDouble();
            }
            else if( randParam < 0 )
            {
                m_lastRandomNum = (float)m_random.NextDouble(); //[TEMP]
            }

            ret = new BaseData(m_lastRandomNum);

            return ret;
        }


        #endregion


        /// <summary>
        /// check if math function parameter is correct or not 
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        protected bool checkMathParam( List<BaseData> param )
        {
            if (param.Count != 1)
                throw new ErrorCode(ErrorCode.ERROR_CODE_02);

            BaseData p = param[0];

            if (p.TYPE != BaseData.TYPE_INT && p.TYPE != BaseData.TYPE_FLOAT)
                throw new ErrorCode(ErrorCode.ERROR_CODE_12);

            return true;
        }

    }
}
