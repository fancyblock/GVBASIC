using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;


public class ASCII
{
    static public int WIDTH = 8;
    static public int HEIGHT = 16;

    public int m_asciiCode;
    public Color[] m_color;
    public Color[] m_reverseColor;

    /// <summary>
    /// constructor 
    /// </summary>
    /// <param name="asciiCode"></param>
    public ASCII( int asciiCode, Color white, Color black )
    {
        m_asciiCode = asciiCode;
        m_color = new Color[128];
        m_reverseColor = new Color[128];
        int[] dotMatrix = getDotMatrix(asciiCode);

        int k = 0;
        for( int i = 0; i < 16; i++ )
        {
            int line = dotMatrix[i];

            for( int j = 0; j < 8; j++ )
            {
                bool isBlack = ( line >> (7-j) & 0x01 ) == 1 ? true : false;

                if( isBlack )
                {
                    m_color[k] = black;
                    m_reverseColor[k] = white;
                }
                else
                {
                    m_color[k] = white;
                    m_reverseColor[k] = black;
                }

                k++;
            }
        }
    }

    /// <summary>
    /// return the ASCII info 
    /// </summary>
    /// <param name="asciiCode"></param>
    /// <returns></returns>
    protected int[] getDotMatrix( int asciiCode )
    {
        return ASCIITable.ASCII_DOT_MATRIX[asciiCode];
    }

}
