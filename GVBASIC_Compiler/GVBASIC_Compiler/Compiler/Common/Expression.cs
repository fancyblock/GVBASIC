using System;
using System.Collections.Generic;


namespace GVBASIC_Compiler.Compiler
{
    /// <summary>
    /// expression type 
    /// </summary>
    public class Expression
    {
        public const int OP_ADD         = 0;
        public const int OP_MINUS       = 1;
        public const int OP_MUL         = 2;
        public const int OP_DIV         = 3;
        public const int OP_POWER       = 4;
        public const int OP_NEG         = 5;

        public const int OP_EQUAL       = 6;
        public const int OP_GREATER     = 7;
        public const int OP_GREATER_EQU = 8;
        public const int OP_LESS        = 9;
        public const int OP_LESS_EQ     = 10;

        public const int OP_AND         = 11;
        public const int OP_OR          = 12;
        public const int OP_NOT         = 13;

        public const int EXP_SYMBOL     = 14;
        public const int EXP_FUNC       = 15;
        public const int EXP_USER_FUNC  = 16;

        public const int VALUE          = 17;

        public const int TYPE_NEXT_LINE = 22;
        public const int TYPE_CLOSE_TO  = 23;

        public int m_type;
        public Expression m_leftExp;
        public Expression m_rightExp;
        public List<Expression> m_funcParams;
        public string m_symbolName;
        public BaseData m_value;

        /// <summary>
        /// constructor 
        /// </summary>
        /// <param name="type"></param>
        public Expression(int type)
        {
            m_type = type;
            m_leftExp = null;
            m_rightExp = null;
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="bd"></param>
        public Expression(BaseData bd)
        {
            m_type = Expression.VALUE;
            m_value = bd;
        }
    }
}
