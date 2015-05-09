using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVBASIC_Compiler.Compiler
{
    public class DataRegion
    {
        protected List<BaseData> m_datas;
        protected int m_curIndex;

        /// <summary>
        /// constructor
        /// </summary>
        public DataRegion()
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

    }
}
