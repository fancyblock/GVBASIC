

/// <summary>
/// all the token types 
/// </summary>
public enum TokenType : int
{
    eSymbol,            // symbol 
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
    eNOt,				// NOT
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
    eError,             // error 
    eEOL,               // end of line ( dev only )
};



// define a token 
public class Token
{
	public TokenType m_type;
	public int m_intVal;
	public float m_realVal;
	public string m_strVal;
};


