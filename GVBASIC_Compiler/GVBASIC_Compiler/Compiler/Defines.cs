using System;
using System.Collections.Generic;

/// <summary>
/// all the token types 
/// </summary>
public enum TokenType : int
{
    eUndefine,

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

    eError,             // error        ( dev only )
    eEOL,               // end of line  ( dev only )
};



// define a token 
public class Token
{
	public TokenType m_type;
	public int m_intVal;
	public float m_realVal;
	public string m_strVal;
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
        {
            throw new Exception("No line.");
        }

        // no line number error. 
        if (tokens[0].m_type != TokenType.eIntNum)
        {
            // throw error, no line number at the beginning of the line. 
            throw new Exception("No line number.");
        }

        m_lineNum = tokens[0].m_intVal;
		m_tokenCount = tokens.Count - 1;

		m_tokens = new Token[m_tokenCount];
		for (int i = 0; i < m_tokenCount; i++)
        {
            m_tokens[i] = tokens[i+1];
        }
    }

    public int m_lineNum;
    public int m_tokenCount;
    public Token[] m_tokens;
}


// function call 
public class Function
{
    public string m_funcName;
	public Token[] m_params;
	//TODO 
}

