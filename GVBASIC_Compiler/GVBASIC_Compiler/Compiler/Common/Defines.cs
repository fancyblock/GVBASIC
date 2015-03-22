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

    eFunc,              // inner function 
    eSimpleCmd,
    eParamCmd,

    eError,             // error 
    eEOL,               // end of line 
    eNull,
};



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
        m_lineNumber = -1;
    }

	public TokenType m_type;
	public int m_intVal;
	public float m_realVal;
	public string m_strVal;
    public int m_lineNumber;
};


// store a code line 
public class CodeLine
{
    /// <summary>
    /// constructor 
    /// </summary>
    /// <param name="tokens"></param>
    public CodeLine(List<Token> tokens)
    {
        // line number check 
        if (tokens.Count <= 0)
            throw new Exception("No line.");

        // throw error, no line number at the beginning of the line. 
        if (tokens[0].m_type != TokenType.eIntNum)
            throw new Exception("No line number.");

        m_lineNum = tokens[0].m_intVal;
		m_tokenCount = tokens.Count - 1;

        m_tokens = new List<Token>();
		for (int i = 0; i < m_tokenCount; i++)
        {
            m_tokens.Add(tokens[i + 1]);
        }
    }

    public int m_lineNum;
    public int m_tokenCount;
    public List<Token> m_tokens;
}
