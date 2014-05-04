using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


namespace Assets.Script.Display
{
    class ASCII
    {
        static public int WIDTH = 8;
        static public int HEIGHT = 16;

        public int m_asciiCode;
        public Color[] m_color;

        /// <summary>
        /// constructor 
        /// </summary>
        /// <param name="asciiCode"></param>
        public ASCII( int asciiCode, Color white, Color black )
        {
            m_color = new Color[128];

            //TODO 
        }
    }
}
