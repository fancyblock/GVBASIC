using System;
using System.Collections.Generic;
using System.Text;

// tokenizer 
namespace GVBASIC_Compiler.Compiler
{
    /// <summary>
    /// status of the tokenizer 
    /// </summary>
    enum LexStatus
    {
        eStart,
        eSymbol,
        eSpace,
        eIntNum,
        eRealNum,
        eString,
        eStringTrans,   // like '\n', '\t'
        eOpCode,
        eDelim,         // delimiter 
        eLineEnd,       // end of line ( for dev )
        eError,
    };

    /// <summary>
    /// lexer 
    /// </summary>
    class Tokenizer
    {
        protected string m_sourceCode;
        protected int m_curIndex;

        protected char[] m_opChr;
        protected char[] m_delimChr;

        /// <summary>
        /// constructor 
        /// </summary>
        public Tokenizer()
        {
            m_sourceCode = null;

            m_opChr = new char[] { '+', '-', '*', '/', '^', '=', '>', '<' };
            m_delimChr = new char[] { ':', ',', ';', '(', ')' };
        }

        /// <summary>
        /// set source code 
        /// </summary>
        /// <param name="source"></param>
        public void SetSource( string source )
        {
            m_sourceCode = source;
        }

        /// <summary>
        /// reset the tokenizer 
        /// </summary>
        public void Reset()
        {
            m_curIndex = 0;
        }

        /// <summary>
        /// return the next token 
        /// </summary>
        /// <returns></returns>
        public Token GetNextToken()
        {
            Token tok = new Token();
            StringBuilder buffer = new StringBuilder();
            LexStatus status = LexStatus.eStart;
            bool addToBuffer;
            bool isDone = false;

            for (; m_curIndex < m_sourceCode.Length && isDone == false; m_curIndex++ )
            {
                char c = m_sourceCode[m_curIndex];
                addToBuffer = true;

                switch( status )
                {
                    case LexStatus.eStart:
                        if( isWhiteChar( c ) )
                        {
                            addToBuffer = false;
                        }
                        else if( isNumber( c ) )
                        {
                            status = LexStatus.eIntNum;
                        }
                        else if( c == '\"' )
                        {
                            addToBuffer = false;
                            status = LexStatus.eString;
                        }
                        else if( isLetter( c ) )
                        {
                            status = LexStatus.eSymbol;
                        }
                        else if( isOpChar( c ))
                        {
                            status = LexStatus.eOpCode;
                        }
                        else if( c == '.' )
                        {
                            status = LexStatus.eRealNum;
                        }
                        else if( isLineEnd( c ) )
                        {
                            addToBuffer = false;
                            status = LexStatus.eLineEnd;
                        }
                        else
                        {
                            status = LexStatus.eError;
                            isDone = true;
                        }
                        break;
                    case LexStatus.eIntNum:
                        if( isNumber( c ) )
                        {
                            // do nothing 
                        }
                        else if( isWhiteChar( c ) )
                        {
                            addToBuffer = false;
                            isDone = true;
                        }
                        else if( c == '.' )
                        {
                            status = LexStatus.eRealNum;
                        }
                        else if( isOpChar( c ) )
                        {
                            addToBuffer = false;
                            isDone = true;
                            backAChar();
                        }
                        else if( isDelim( c ) )
                        {
                            addToBuffer = false;
                            isDone = true;
                            backAChar();
                        }
                        else if( isLineEnd( c ) )
                        {
                            addToBuffer = false;
                            isDone = true;
                            backAChar();
                        }
                        else
                        {
                            status = LexStatus.eError;
                            isDone = true;
                        }
                        break;
                    case LexStatus.eRealNum:
                        if( isNumber( c ) )
                        {
                            // do nothing 
                        }
                        else if( isWhiteChar( c ) )
                        {
                            addToBuffer = false;
                            isDone = true;
                        }
                        else if (isOpChar(c))
                        {
                            addToBuffer = false;
                            isDone = true;
                            backAChar();
                        }
                        else if (isDelim(c))
                        {
                            addToBuffer = false;
                            isDone = true;
                            backAChar();
                        }
                        else if (isLineEnd(c))
                        {
                            addToBuffer = false;
                            isDone = true;
                            backAChar();
                        }
                        else
                        {
                            status = LexStatus.eError;
                            isDone = true;
                        }
                        break;
                    case LexStatus.eString:
                        if( c == '\\' )
                        {
                            addToBuffer = false;
                            status = LexStatus.eStringTrans;
                        }
                        else if( c == '\"' )
                        {
                            addToBuffer = false;
                            status = LexStatus.eStart;
                            isDone = true;
                        }
                        else if( isLineEnd( c ) )
                        {
                            status = LexStatus.eError;
                            isDone = true;
                        }
                        break;
                    case LexStatus.eStringTrans:
                        if( c == 'n' )
                        {
                            c = '\n';
                            status = LexStatus.eString;
                        }
                        else if( c == 'r' )
                        {
                            c = '\r';
                            status = LexStatus.eString;
                        }
                        else if( c == '\\' )
                        {
                            c = '\\';
                        }
                        else if( isLineEnd( c ) )
                        {
                            status = LexStatus.eError;
                            isDone = true;
                        }
                        else
                        {
                            status = LexStatus.eError;
                            isDone = true;
                        }
                        break;
                    case LexStatus.eSymbol:
                        if( isLetter( c ) || isNumber( c ) )
                        {
                            // do nothing 
                        }
                        else if( c == '$' )
                        {
                            //TODO 
                        }
                        else if( c == '%' )
                        {
                            //TODO 
                        }
                        else
                        {
                            //TODO 
                        }
                        break;
                    case LexStatus.eOpCode:
                        //TODO 
                        break;
                    default:
                        break;
                }

                if( addToBuffer )
                {
                    buffer.Append(c);
                }
            }

            // confirm the token type 
            switch( status )
            {
                case LexStatus.eIntNum:
                    tok.m_type = TokenType.eIntNum;
                    tok.m_intVal = Int32.Parse(buffer.ToString());
                    break;
                case LexStatus.eString:
                    tok.m_type = TokenType.eString;
                    tok.m_strVal = buffer.ToString();
                    break;
                case LexStatus.eRealNum:
                    tok.m_type = TokenType.eRealNum;
                    tok.m_realVal = float.Parse(buffer.ToString());
                    break;
                case LexStatus.eLineEnd:
                    tok.m_type = TokenType.eEOL;
                    break;
                case LexStatus.eError:
                    tok.m_type = TokenType.eError;
                    tok.m_strVal = buffer.ToString();
                    break;
                default:
                    break;
            }

            return tok;
        }

        /// <summary>
        /// skip characters to next line 
        /// </summary>
        public void SkipToNextLine()
        {
            for( ; m_curIndex < m_sourceCode.Length; m_curIndex++ )
            {
                char c = m_sourceCode[m_curIndex];

                if( c == '\n' )
                {
                    break;
                }
            }
        }


        /// <summary>
        /// if is finished or not 
        /// </summary>
        /// <returns></returns>
        public bool IsFinish()
        {
            if( m_curIndex >= m_sourceCode.Length )
            {
                return true;
            }

            return false;
        }


        //------------------------ private functions ------------------------ 

        /// <summary>
        /// step back a char 
        /// </summary>
        protected void backAChar()
        {
            if( m_curIndex > 0 )
            {
                m_curIndex--;
            }
            else
            {
                throw new Exception("[Tokenizer]: can not step back.");
            }
        }

        /// <summary>
        /// look ahead a char 
        /// </summary>
        /// <returns></returns>
        protected char lookAheadChar()
        {
            char c = char.MaxValue;

            if( ( m_curIndex + 1 ) < m_sourceCode.Length )
            {
                c = m_sourceCode[m_curIndex + 1];
            }

            return c;
        }

        /// <summary>
        /// judge if the character is number or not 
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        protected bool isNumber( char c )
        {
            return Char.IsNumber(c);
        }

        /// <summary>
        /// judge if the character is letter or not 
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        protected bool isLetter( char c )
        {
            return Char.IsLetter(c);
        }

        /// <summary>
        /// is white char 
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        protected bool isWhiteChar( char c )
        {
            if( c == ' ' || c == '\t' )
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// if this char belong to operator
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        protected bool isOpChar( char c )
        {
            for (int i = 0; i < m_opChr.Length; i++ )
            {
                if( c == m_opChr[i] )
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// judge if is delimter or not 
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        protected bool isDelim( char c )
        {
            for (int i = 0; i < m_delimChr.Length; i++ )
            {
                if( c == m_delimChr[i] )
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// if is end of line or not 
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        protected bool isLineEnd( char c )
        {
            if( c == '\n' )
            {
                return true;
            }

            return false;
        }
    }
}
