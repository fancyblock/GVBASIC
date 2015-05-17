using System;
using System.Collections.Generic;


/// <summary>
/// define a token 
/// </summary>
public class Token
{
    public const int UNDEFINE       = 0;

    public const int SYMBOL         = 10;                // symbol 
    public const int FILE_NUM       = 11;                // file handler

    public const int INT            = 20;                // int number
    public const int FLOAT          = 21;                // real number 
    public const int STRING         = 22;                // string 

    public const int PLUS           = 30;				// +
    public const int MINUS          = 31;				// -
    public const int MUL            = 32;				// *
    public const int DIV            = 33;				// /
    public const int POWER          = 34;				// ^

    public const int EQUAL          = 40;				// =
    public const int GTR            = 41;				// > 
    public const int LT             = 42;				// <
    public const int GTE            = 43;				// >=
    public const int LTE            = 44;				// <=
    public const int NEG            = 45;				// <>

    public const int AND            = 50;				// AND
    public const int OR             = 51;				// OR
    public const int NOT            = 52;				// NOT

    public const int SEMI           = 60;				// ;
    public const int COMMA          = 61;				// ,
    public const int COLON          = 62;				// :
    public const int LEFT_BRA       = 63;			    // (
    public const int RIGHT_BRA      = 64;			    // )

    public const int LET            = 70;				// LET 
    public const int DIM            = 71;				// DIM
    public const int READ           = 72;				// READ
    public const int DATA           = 73;				// DATA
    public const int RESTORE        = 74;			    // RESTORE
    public const int GOTO           = 75;				// GOTO
    public const int IF             = 76;				// IF
    public const int THEN           = 77;				// THEN
    public const int ELSE           = 78;				// ELSE 
    public const int FOR            = 79;               // FOR 
    public const int NEXT           = 80;               // NEXT
    public const int WHILE          = 81;				// WHILE
    public const int WEND           = 82;				// WEND
    public const int TO             = 83;				// TO
    public const int STEP           = 84;				// STEP
    public const int DEF            = 85;				// DEF
    public const int FN             = 86;				// FN
    public const int GOSUB          = 87;				// GOSUB
    public const int RETURN         = 88;			    // RETURN
    public const int ON             = 89;				// ON
    public const int POP            = 90;               // POP 
    public const int REM            = 91;               // REM 
    public const int END            = 92;               // END 

    public const int PRINT          = 100;
    public const int OPEN           = 101;
    public const int CLOSE          = 102;
    public const int FIELD          = 103;
    public const int GET            = 104;
    public const int LSET           = 105;
    public const int PUT            = 106;
    public const int RSET           = 107;
    public const int WRITE          = 108;
    public const int INPUT          = 109;
    public const int INKEY          = 110;

    public const int FUNC           = 130;               // inner function 
    public const int SIMPLE_CMD     = 131;
    public const int PARAM_CMD      = 132;

    public const int ERROR          = 140;              // error 
    public const int EOL            = 141;              // end of line 
    public const int FILE_END       = 142;              // end of file

    /// <summary>
    /// Initializes a new instance of the <see cref="Token"/> class.
    /// </summary>
    public Token()
    {
        m_type = Token.UNDEFINE;
        m_intVal = 0;
        m_floatVal = 0.0f;
        m_strVal = "";
    }

	public int m_type;
	public int m_intVal;
	public float m_floatVal;
	public string m_strVal;
}


/// <summary>
/// statement type 
/// </summary>
public class Statement
{
    public const int TYPE_STATEMENT_SET     = 0;
    public const int TYPE_INNER_FUNC        = 1;
    public const int TYPE_SIMPLE_CMD        = 2;
    public const int TYPE_PARAM_CMD         = 3;
    public const int TYPE_PRINT             = 4;
    public const int TYPE_ASSIGN            = 5;
    public const int TYPE_DATA              = 6;
    public const int TYPE_READ              = 7;
    public const int TYPE_IF                = 8;
    public const int TYPE_FOR_BEGIN         = 9;
    public const int TYPE_FOR_END           = 10;
    public const int TYPE_WHILE_BEGIN       = 11;
    public const int TYPE_WHILE_END         = 12;
    public const int TYPE_ON                = 13;
    public const int TYPE_GOSUB             = 14;
    public const int TYPE_RETURN            = 15;
    public const int TYPE_POP               = 16;
    public const int TYPE_GOTO              = 17;
    public const int TYPE_DEF_FN            = 18;
    public const int TYPE_DIM               = 19;
    public const int TYPE_SWAP              = 20;
    public const int TYPE_END               = 21;

    public int m_type;
    public int m_num;

    public string m_symbol;
    public int m_intVal;

    public List<Statement> m_statements;
    public List<Expression> m_expressList;
    public List<string> m_symbolList;
    public List<BaseData> m_dataList;

    /// <summary>
    /// constructor
    /// </summary>
    public Statement() { }

    /// <summary>
    /// constructor
    /// </summary>
    /// <param name="type"></param>
    public Statement( int type )
    {
        m_type = type;
    }
}

/// <summary>
/// base data 
/// </summary>
public class BaseData
{
    public const int TYPE_INT       = 1;
    public const int TYPE_FLOAT     = 2;
    public const int TYPE_STRING    = 3;
    public const int TYPE_SPACE     = 4;
    public const int TYPE_TAB       = 5;
    public const int TYPE_NEXT_LINE = 6;
    public const int TYPE_CLOSE_TO  = 7;

    public int m_intVal;
    public float m_floatVal;
    public string m_stringVal;
    public bool m_boolVal;
    public int m_type;

    /// <summary>
    /// default constructor
    /// </summary>
    public BaseData() { }
    
    /// <summary>
    /// float constructor
    /// </summary>
    /// <param name="val"></param>
    public BaseData(float val) 
    {
        m_type = TYPE_FLOAT;
        m_floatVal = val;
    }

    /// <summary>
    /// int constructor
    /// </summary>
    /// <param name="val"></param>
    public BaseData(int val)
    {
        m_type = TYPE_INT;
        m_intVal = val;
    }

    /// <summary>
    /// string constructor
    /// </summary>
    /// <param name="val"></param>
    public BaseData(string val)
    {
        m_type = TYPE_STRING;
        m_stringVal = val;
    }

    /// <summary>
    /// convert the data type 
    /// </summary>
    /// <param name="type"></param>
    public void Convert( int type )
    {
        switch( type )
        {
            case BaseData.TYPE_INT:
                if (m_type == BaseData.TYPE_FLOAT)
                    m_intVal = (int)m_floatVal;
                else if( m_type != BaseData.TYPE_INT )
                    throw new ErrorCode(ErrorCode.ERROR_CODE_12);
                break;
            case BaseData.TYPE_FLOAT:
                if (m_type == BaseData.TYPE_INT)
                    m_floatVal = m_intVal;
                else if( m_type != BaseData.TYPE_FLOAT )
                    throw new ErrorCode(ErrorCode.ERROR_CODE_12);
                break;
            case BaseData.TYPE_STRING:
                if (m_type == BaseData.TYPE_FLOAT)
                    m_stringVal = m_floatVal.ToString();
                else if (m_type == BaseData.TYPE_INT)
                    m_stringVal = m_intVal.ToString();
                else if( m_type != BaseData.TYPE_STRING )
                    throw new ErrorCode(ErrorCode.ERROR_CODE_12);
                break;
            default:
                throw new ErrorCode(ErrorCode.ERROR_CODE_12);
        }

        m_type = type;
    }

}


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

    public const int VAL_INT        = 17;
    public const int VAL_FLOAT      = 18;
    public const int VAL_STRING     = 19;

    public const int TYPE_NEXT_LINE = 22;
    public const int TYPE_CLOSE_TO  = 23;

    public int m_type;
    public Expression m_leftExp;
    public Expression m_rightExp;
    public List<Expression> m_funcParams;
    public int m_intVal;
    public float m_floatVal;
    public string m_strVal;

    /// <summary>
    /// constructor 
    /// </summary>
    /// <param name="type"></param>
    public Expression( int type )
    {
        m_type = type;
        m_leftExp = null;
        m_rightExp = null;
    }
}


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
    public ErrorCode(string code)
        : base(code)
    {
    }

}