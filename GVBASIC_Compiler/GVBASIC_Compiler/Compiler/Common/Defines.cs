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

	ePound,				// #
    eQm,				// ? 

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
    eInnerFunc,
    eSimpleCmd,
    eParamCmd,
    ePrint,
    //TODO 
    eAssign,
    eData,
    eRead,
    eIfThen,
    eIfGoto,
    eIfThenElse,
    eIfGotoElse,
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

    public List<Expression> m_expressList;
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

    /// <summary>
    /// set text 
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public Expression SetText( string text )
    {
        m_text = text;
        return this;
    }
}
