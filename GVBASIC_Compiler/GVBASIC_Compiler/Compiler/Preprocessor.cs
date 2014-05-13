using System;
using System.Collections.Generic;
using System.Text;

namespace GVBASIC_Compiler.Compiler
{
    class Preprocessor
    {
        /// <summary>
        /// preprocess the code , remove comment && cast all the letters 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        static public string Preprocess( string source )
        {
            StringBuilder sb = new StringBuilder();
            StringBuilder line = new StringBuilder();

            for (int i = 0; i < source.Length; i++)
            {
                char c = source[i];
                
                if( c == '\n' )
                {
                    // remove the commments
                    //TODO 

                    line.AppendLine();
                    sb.Append(line);
                    line.Remove(0, line.Length);
                }
                else
                {
                    line.Append( Char.ToUpper( c ) );
                }
            }

            if (line.Length > 0)
            {
                line.AppendLine();
                sb.Append(line);
            }

            return sb.ToString();
        }
    }
}
