using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVBASIC_Compiler.Compiler
{
    /// <summary>
    /// error codes 
    /// </summary>
    public class ErrorCode : Exception
    {
        public const string ERROR_CODE_01 = "NEXT WITHOUT FOR";
        public const string ERROR_CODE_02 = "SYNTAX";
        public const string ERROR_CODE_03 = "OUT OF DATA";
        public const string ERROR_CODE_04 = "ILLEGAL QUANTITY";
        public const string ERROR_CODE_05 = "OVERFLOW";
        public const string ERROR_CODE_06 = "OUT OF MEMORY";
        public const string ERROR_CODE_07 = "UNDEF'D STATEMENT";
        public const string ERROR_CODE_08 = "BAD SUBSCRIPT";
        public const string ERROR_CODE_09 = "REDIM'D ARRAY";
        public const string ERROR_CODE_10 = "DIVISION BY ZERO";
        public const string ERROR_CODE_11 = "ILLEGAL DIRECT";
        public const string ERROR_CODE_12 = "TYPE MISMATCH";
        public const string ERROR_CODE_13 = "STRING TOO LONG";
        public const string ERROR_CODE_14 = "FORMULA TOO COMPLEX";
        public const string ERROR_CODE_15 = "CAN'T CONTINUE";
        public const string ERROR_CODE_16 = "UNDEF'D FUNCTION";
        public const string ERROR_CODE_17 = "WEND WITHOUT WHILE";
        public const string ERROR_CODE_18 = "RETURN WITHOUT GOSUB";
        public const string ERROR_CODE_19 = "OUT OF SPACE";
        public const string ERROR_CODE_20 = "FILE CREATE";
        public const string ERROR_CODE_21 = "FILE OPEN";
        public const string ERROR_CODE_22 = "FILE CLOSE";
        public const string ERROR_CODE_23 = "FILE READ";
        public const string ERROR_CODE_24 = "FILE WRITE";
        public const string ERROR_CODE_25 = "FILE DELETE";
        public const string ERROR_CODE_26 = "FILE NOT EXIST";
        public const string ERROR_CODE_27 = "RECORD NUMBER";
        public const string ERROR_CODE_28 = "FILE NUMBER";
        public const string ERROR_CODE_29 = "FILE MODE";
        public const string ERROR_CODE_30 = "SAME FILE EXIST";
        public const string ERROR_CODE_31 = "FILE LENGTH READ";
        public const string ERROR_CODE_32 = "ILLEGAL FILE NAME";
        public const string ERROR_CODE_33 = "FILE TOO LONG";
        public const string ERROR_CODE_34 = "FILE REOPEN";

        /// <summary>
        /// constructor 
        /// </summary>
        /// <param name="code"></param>
        public ErrorCode( string code ):base(code)
        {
        }

    }
}
