using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVBASIC_Compiler.Compiler
{
    public class DataArea
    {
        protected List<BaseData> m_datas;
        protected int m_curIndex;

        /// <summary>
        /// constructor
        /// </summary>
        public DataArea()
        {
            m_datas = new List<BaseData>();
            m_curIndex = 0;
        }

        /// <summary>
        /// add data
        /// </summary>
        /// <param name="data"></param>
        public void AddDatas( List<BaseData> data )
        {
            for (int i = 0; i < data.Count; i++)
                m_datas.Add(data[i]);
        }

        /// <summary>
        /// get a data from data region 
        /// </summary>
        /// <returns></returns>
        public BaseData GetData()
        {
            if (m_curIndex >= m_datas.Count)
                throw new ErrorCode(ErrorCode.ERROR_CODE_03);

            BaseData bd = m_datas[m_curIndex];
            m_curIndex++;

            return bd;
        }

        /// <summary>
        /// restore data 
        /// </summary>
        public void Restore()
        {
            m_curIndex = 0;
        }

    }
}
