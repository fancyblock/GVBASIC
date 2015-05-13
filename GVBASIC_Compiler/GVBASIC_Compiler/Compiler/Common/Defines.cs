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
/// statement type define 
/// </summary>
public enum StatementType : int
{
    eStatementSet,
    eInnerFunc,
    eSimpleCmd,
    eParamCmd,
    ePrint,
    //TODO 
    eAssign,
    eData,
    eRead,
    eIf,
    eForBegin,
    eForEnd,
    eWhileBegin,
    eWhileEnd,
    eOn,
    eGoSub,
    eReturn,
    ePop,
    eGoto,
    eDefFn,
    eDim,
    eSwap,
    eEnd,
}


/// <summary>
/// statement type 
/// </summary>
public class Statement
{
    public StatementType m_type;
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
    public Statement( StatementType type )
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

public enum ExpressionType : int
{
    eOpPlus,
    eOpMinus,
    eOpMul,
    eOpDiv,
    eOpPower,
    eOpNeg,

    eOpCmpEqual,
    eOpCmpGtr,
    eOpCmpGte,
    eOpCmpLtr,
    eOpCmpLte,

    eOpLogicAnd,
    eOpLogicOr,
    eOpLogicNot,

    eSymbol,
    eFunc,
    eUserFn,

    eIntNum,
    eRealNum,
    eString,

    eSpecSpace,
    eSpecTab,
    eSpecNextLine,
    eSpecCloseTo,
}


/// <summary>
/// expression type 
/// </summary>
public class Expression
{
    public ExpressionType m_type;
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
    public Expression( ExpressionType type )
    {
        m_type = type;
        m_leftExp = null;
        m_rightExp = null;
    }
}
