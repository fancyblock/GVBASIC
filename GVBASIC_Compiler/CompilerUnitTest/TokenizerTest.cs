using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GVBASIC_Compiler.Compiler;

namespace CompilerUnitTest
{
    [TestClass]
    public class TokenizerTest
    {
        [TestMethod]
        public void Tokenizer()
        {
            string sourceCode =
            "testSymbol #1 123 17.0 \"hjb\" \r\n" +
            "+-*/^" +
            "=>< >=<=<>" +
            "AND OR NOT" +
            ";,:()" +
            "LET DIM READ DATA RESTORE GOTO " +
            "IF THEN ELSE FOR NEXT " +
            "WHILE WEND TO STEP " +
            "DEF FN GOSUB RETURN ON " +
            "REM this is comment \r\n" +             // REM will be skip , replace with a EOL
            "PRINT OPEN CLOSE FIELD " +
            "GET LSET PUT RSET WRITE " +
            "INPUT INKEY$ " +
            "" +    //TODO 
            "@";

            TokenType[] typeList = new TokenType[] 
            {
            TokenType.eSymbol, TokenType.eFileNum, TokenType.eIntNum, TokenType.eRealNum, TokenType.eString, TokenType.eEOL,
            TokenType.ePlus, TokenType.eMinus, TokenType.eMul, TokenType.eDiv, TokenType.ePower,
            TokenType.eEqual, TokenType.eGtr, TokenType.eLt, TokenType.eGte, TokenType.eLte, TokenType.eNeq,
            TokenType.eAnd, TokenType.eOr, TokenType.eNot,
            TokenType.eSemi, TokenType.eComma, TokenType.eColon, TokenType.eLeftBra, TokenType.eRightBra,
            TokenType.eLet, TokenType.eDim, TokenType.eRead, TokenType.eData, TokenType.eRestore, TokenType.eGoto,
            TokenType.eIf, TokenType.eThen, TokenType.eElse, TokenType.eFor, TokenType.eNext,
            TokenType.eWhile, TokenType.eWend, TokenType.eTo, TokenType.eStep,
            TokenType.eDef, TokenType.eFn, TokenType.eGoSub, TokenType.eReturn, TokenType.eOn, 
            TokenType.eEOL,
            TokenType.ePrint, TokenType.eOpen, TokenType.eClose, TokenType.eField,
            TokenType.eGet, TokenType.eLset, TokenType.ePut, TokenType.eRset, TokenType.eWrite,
            TokenType.eInput, TokenType.eInkey,
            //TODO 
            TokenType.eError, TokenType.eEOF
            };

            Tokenizer tokenizer = new Tokenizer( sourceCode );

            for (int i = 0; i < typeList.Length; i++ )
            {
                Token tok = tokenizer.GetToken();

                if (tok.m_type != typeList[i])
                    throw new Exception("Token type incorrect. Must be " + typeList[i].ToString());
            }
        }
    }
}
