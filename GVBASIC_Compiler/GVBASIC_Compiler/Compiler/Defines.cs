

/// <summary>
/// all the token types 
/// </summary>
public enum TokenType : int
{
    eLineNum = 0,
    eSpacing,
    eSymbol,
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
    eQm,				// ?
    eLeftBra,			// (
    eRightBra,			// )
	ePound,				// #
	eQuota,				// "
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
};



// define a token 
public class Token
{
	public TokenType m_type;
	public int m_intVal;
	public float m_realVal;
	public string m_strVal;
};


