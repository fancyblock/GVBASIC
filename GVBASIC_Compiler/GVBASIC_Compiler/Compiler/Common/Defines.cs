using System;
using System.Collections.Generic;

/// <summary>
/// all the token types 
/// </summary>
public enum TokenType : int
{
    eUndefine,

    eSymbol,            // symbol 
    eFileNum,           // file handler

    eIntNum,            // int number
    eRealNum,           // real number 
    eString,            // string 

    ePlus,				// +
    eMinus,				// -
    eMul,				// *
    eDiv,				// /
    ePower,				// ^

    eEqual,				// =
    eGtr,				// > 
    eLt,				// <
    eGte,				// >=
    eLte,				// <=
    eNeq,				// <>

    eAnd,				// AND
    eOr,				// OR
    eNot,				// NOT

    eSemi,				// ;
    eComma,				// ,
    eColon,				// :
    eLeftBra,			// (
    eRightBra,			// )

    eLet,				// LET 
    eDim,				// DIM
    eRead,				// READ
    eData,				// DATA
    eRestore,			// RESTORE
    eGoto,				// GOTO
    eIf,				// IF
    eThen,				// THEN
    eElse,				// ELSE 
    eFor,               // FOR 
    eNext,              // NEXT
    eWhile,				// WHILE
    eWend,				// WEND
    eTo,				// TO
    eStep,				// STEP
    eDef,				// DEF
    eFn,				// FN
    eGoSub,				// GOSUB
    eReturn,			// RETURN
    eOn,				// ON
    eRem,               // REM 

    ePrint,
    eOpen,
    eClose,
    eField,
    eGet,
    eLset,
    ePut,
    eRset,
    eWrite,
    eInput,
    eInkey,

    eFunc,              // inner function 
    eSimpleCmd,
    eParamCmd,

    eError,             // error 
    eEOL,               // end of line 
    eEOF,               // end of file
}


// define a token 
public class Token
{
    //TODO 

    /// <summary>
    /// Initializes a new instance of the <see cref="Token"/> class.
    /// </summary>
    public Token()
    {
        m_type = TokenType.eUndefine;
        m_intVal = 0;
        m_realVal = 0.0f;
        m_strVal = "";
    }

	public TokenType m_type;
	public int m_intVal;
	public float m_realVal;
	public string m_strVal;
}


/// <summary>
/// statement type 
/// </summary>
public class Statement
{
    public const int TYPE_STATEMENT_SET = 0;
    public const int TYPE_INNER_FUNC = 1;
    public const int TYPE_SIMPLE_CMD = 2;
    public const int TYPE_PARAM_CMD = 3;
    public const int TYPE_PRINT = 4;
    public const int TYPE_ASSIGN = 5;
    public const int TYPE_DATA = 6;
    public const int TYPE_READ = 7;
    public const int TYPE_IF = 8;
    public const int TYPE_FOR_BEGIN = 9;
    public const int TYPE_FOR_END = 10;
    public const int TYPE_WHILE_BEGIN = 11;
    public const int TYPE_WHILE_END = 12;
    public const int TYPE_ON = 13;
    public const int TYPE_GOSUB = 14;
    public const int TYPE_RETURN = 15;
    public const int TYPE_POP = 16;
    public const int TYPE_GOTO = 17;
    public const int TYPE_DEF_FN = 18;
    public const int TYPE_DIM = 19;
    public const int TYPE_SWAP = 20;
    public const int TYPE_END = 21;

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
    public const int TYPE_INT = 1;
    public const int TYPE_FLOAT = 2;
    public const int TYPE_STRING = 3;

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

}


/// <summary>
/// expression type 
/// </summary>
public class Expression
{
    public const int OP_PLUS = 0;
    public const int OP_MINUS = 1;
    public const int OP_MUL = 2;
    public const int OP_DIV = 3;
    public const int OP_POWER = 4;
    public const int OP_NEG = 5;

    public const int OP_EQUAL = 6;
    public const int OP_GREATER = 7;
    public const int OP_GREATER_EQU = 8;
    public const int OP_LESS = 9;
    public const int OP_LESS_EQ = 10;

    public const int OP_AND = 11;
    public const int OP_OR = 12;
    public const int OP_NOT = 13;

    public const int eSymbol = 14;
    public const int eFunc = 15;
    public const int eUserFn = 16;

    public const int VAL_INT = 17;
    public const int VAL_FLOAT = 18;
    public const int VAL_STRING = 19;

    public const int eSpecSpace = 20;
    public const int eSpecTab = 21;
    public const int eSpecNextLine = 22;
    public const int eSpecCloseTo = 23;

    public int m_type;
    public Expression m_leftExp;
    public Expression m_rightExp;
    public int m_intVal;
    public float m_realVal;
    public string m_symbol;
    public string m_text;

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
