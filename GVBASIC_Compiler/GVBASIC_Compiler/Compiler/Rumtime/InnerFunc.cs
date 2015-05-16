using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVBASIC_Compiler.Compiler
{
    public class InnerFunc
    {
        protected IAPI m_iapi;
        protected Dictionary<string, Func<List<BaseData>, BaseData>> m_funcDic;

        /// <summary>
        /// constructor
        /// </summary>
        public InnerFunc()
        {
            // initial the functions 
            m_funcDic = new Dictionary<string, Func<List<BaseData>, BaseData>>()
            {
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

        /// <summary>
        /// abs 
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        protected BaseData ABS( List<BaseData> param )
        {
            checkMathParam(param);

            BaseData ret = null;
            BaseData p = param[0];

            if (p.m_type == BaseData.TYPE_INT)
                ret = new BaseData(p.m_intVal >= 0 ? p.m_intVal : -p.m_intVal);
            else if (p.m_type == BaseData.TYPE_FLOAT)
                ret = new BaseData(p.m_floatVal >= 0.0f ? p.m_floatVal : -p.m_floatVal);

            return ret;
        }

        protected BaseData SGN(List<BaseData> param)
        {
            checkMathParam(param);

            BaseData ret = null;
            BaseData p = param[0];

            //TODO 

            return ret;
        }

        protected BaseData INT( List<BaseData> param )
        {
            checkMathParam(param);

            BaseData ret = null;
            BaseData p = param[0];

            //TODO 

            return ret;
        }

        protected BaseData SIN(List<BaseData> param)
        {
            checkMathParam(param);

            BaseData ret = null;
            BaseData p = param[0];

            //TODO 

            return ret;
        }

        protected BaseData COS(List<BaseData> param)
        {
            checkMathParam(param);

            BaseData ret = null;
            BaseData p = param[0];

            //TODO 

            return ret;
        }

        protected BaseData TAN(List<BaseData> param)
        {
            checkMathParam(param);

            BaseData ret = null;
            BaseData p = param[0];

            //TODO 

            return ret;
        }

        protected BaseData ATN(List<BaseData> param)
        {
            checkMathParam(param);

            BaseData ret = null;
            BaseData p = param[0];

            //TODO 

            return ret;
        }

        protected BaseData SQR(List<BaseData> param)
        {
            checkMathParam(param);

            BaseData ret = null;
            BaseData p = param[0];

            //TODO 

            return ret;
        }

        protected BaseData EXP(List<BaseData> param)
        {
            checkMathParam(param);

            BaseData ret = null;
            BaseData p = param[0];

            //TODO 

            return ret;
        }

        protected BaseData LOG(List<BaseData> param)
        {
            checkMathParam(param);

            BaseData ret = null;
            BaseData p = param[0];

            //TODO 

            return ret;
        }

        protected BaseData RND(List<BaseData> param)
        {
            checkMathParam(param);

            BaseData ret = null;
            BaseData p = param[0];

            //TODO 

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

            if (p.m_type != BaseData.TYPE_INT && p.m_type != BaseData.TYPE_FLOAT)
                throw new ErrorCode(ErrorCode.ERROR_CODE_12);

            return true;
        }

    }
}
