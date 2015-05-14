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
        eFileNum,
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
    public class Tokenizer
    {
        protected string m_sourceCode;
        protected int m_curIndex;

        protected List<char> m_opChr;                   // operator
        protected List<char> m_delimChr;                // separator
        protected Dictionary<string, TokenType> m_symbolTypes;

        /// <summary>
        /// constructor 
        /// </summary>
        public Tokenizer( string source )
        {
            // init the tokenizer 
            m_opChr = new List<char> { '+', '-', '*', '/', '^', '=', '>', '<' };
            m_delimChr = new List<char> { ':', ',', ';', '(', ')' };

            m_symbolTypes = new Dictionary<string, TokenType>()
            {
                // keywords
                {"PRINT", TokenType.ePrint},
                {"AND", TokenType.eAnd},
                {"OR", TokenType.eOr},
                {"NOT", TokenType.eNot},
                {"LET", TokenType.eLet},
                {"DIM", TokenType.eDim},
                {"READ", TokenType.eRead},
                {"DATA", TokenType.eData},
                {"RESTORE", TokenType.eRestore},
                {"GOTO", TokenType.eGoto},
                {"IF", TokenType.eIf},
                {"THEN", TokenType.eThen},
                {"ELSE", TokenType.eElse},
                {"WHILE", TokenType.eWhile},
                {"WEND", TokenType.eWend},
                {"TO", TokenType.eTo},
                {"STEP", TokenType.eStep},
                {"DEF", TokenType.eDef},
                {"FN",TokenType.eFn},
                {"GOSUB", TokenType.eGoSub},
                {"RETURN", TokenType.eReturn},
                {"ON", TokenType.eOn},
                {"REM", TokenType.eRem},
                {"NEXT", TokenType.eNext},
                {"FOR", TokenType.eFor},
                {"OPEN", TokenType.eOpen},
                {"CLOSE", TokenType.eClose},
                {"FIELD", TokenType.eField},
                {"GET", TokenType.eGet},
                {"LSET", TokenType.eLset},
                {"PUT", TokenType.ePut},
                {"RSET", TokenType.eRset},
                {"WRITE", TokenType.eWrite},
                {"INPUT", TokenType.eInput},
                {"INKEY$", TokenType.eInkey},
                // simple command
                {"BEEP", TokenType.eSimpleCmd},
                {"CLS", TokenType.eSimpleCmd},
                {"INVERSE", TokenType.eSimpleCmd},
                {"NORMAL", TokenType.eSimpleCmd},
                {"GRAPH", TokenType.eSimpleCmd},
                {"TEXT", TokenType.eSimpleCmd},
                // param command
                {"PLAY", TokenType.eParamCmd},
                {"BOX", TokenType.eParamCmd},
                {"CIRCLE", TokenType.eParamCmd},
                {"DRAW", TokenType.eParamCmd},
                {"LINE", TokenType.eParamCmd},
                {"LOCATE", TokenType.eParamCmd},
                // inner function
                {"ABS", TokenType.eFunc},
                {"SGN", TokenType.eFunc},
                {"INT", TokenType.eFunc},
                {"SIN", TokenType.eFunc},
                {"COS", TokenType.eFunc},
                {"TAN", TokenType.eFunc},
                {"ATN", TokenType.eFunc},
                {"SQR", TokenType.eFunc},
                {"EXP", TokenType.eFunc},
                {"LOG", TokenType.eFunc},
                {"RND", TokenType.eFunc},
                {"ASC", TokenType.eFunc},
                {"LEN", TokenType.eFunc},
                {"CHR$", TokenType.eFunc},
                {"LEFT$", TokenType.eFunc},
                {"MID$", TokenType.eFunc},
                {"RIGHT$", TokenType.eFunc},
                {"STR$", TokenType.eFunc},
                {"VAL", TokenType.eFunc},
                {"CVI$", TokenType.eFunc},
                {"MKI$", TokenType.eFunc},
                {"CVS$", TokenType.eFunc},
                {"POS", TokenType.eFunc},
                {"SPC", TokenType.eFunc},
                {"TAB", TokenType.eFunc},
                {"EOF", TokenType.eFunc},
                {"LOF", TokenType.eFunc},
            };

            m_sourceCode = source;
            m_curIndex = 0;
        }

        /// <summary>
        /// return the next token 
        /// </summary>
        /// <returns></returns>
        public Token GetToken()
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
                        if (isWhiteChar(c))
                        {
                            addToBuffer = false;
                        }
                        else if (isNumber(c))
                        {
                            status = LexStatus.eIntNum;
                        }
                        else if (c == '\"')
                        {
                            addToBuffer = false;
                            status = LexStatus.eString;
                        }
                        else if (isLetter(c))
                        {
                            status = LexStatus.eSymbol;
                        }
                        else if (isOpChar(c))
                        {
                            status = LexStatus.eOpCode;
                        }
                        else if (isDelim(c))
                        {
                            status = LexStatus.eDelim;
                            isDone = true;
                        }
                        else if (c == '.')
                        {
                            status = LexStatus.eRealNum;
                        }
                        else if (isLineEnd(c))
                        {
                            addToBuffer = false;
                            status = LexStatus.eLineEnd;
                            isDone = true;
                        }
                        else if (c == '#')
                        {
                            status = LexStatus.eFileNum;
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
                        else if (isOpChar(c) || isDelim(c) || isLineEnd(c))
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
                        else if (isOpChar(c) || isDelim(c) || isLineEnd(c))
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
                        else if( c == '$' || c == '%' )
                        {
                            char ac = lookAheadChar();

                            if( isWhiteChar( ac ) )
                            {
                                isDone = true;
                            }
                            else if( isOpChar( ac ) || isDelim( ac ) || isLineEnd( ac ) )
                            {
                                isDone = true;
                            }
                            else
                            {
                                status = LexStatus.eError;
                                isDone = true;
                            }
                        }
                        else if (isWhiteChar(c))
                        {
                            addToBuffer = false;
                            isDone = true;
                        }
                        else if (isOpChar(c) || isDelim(c) || isLineEnd(c) )
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
                    case LexStatus.eFileNum:
                        if (isNumber(c))
                        {
                            // do nothing 
                        }
                        else if (isWhiteChar(c))
                        {
                            addToBuffer = false;
                            isDone = true;
                        }
                        else if (isOpChar(c) || isDelim(c) || isLineEnd(c))
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
                    case LexStatus.eOpCode:
                        if( buffer[0] == '>' && c == '=' )
                        {
                            isDone = true;
                        }
                        else if( buffer[0] == '<' && ( c == '=' || c == '>' ) )
                        {
                            isDone = true;
                        }
                        else
                        {
                            addToBuffer = false;
                            backAChar();
                            isDone = true;
                        }
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
                case LexStatus.eOpCode:
                    tok.m_strVal = buffer.ToString();
                    if( tok.m_strVal == "+" )
                    {
                        tok.m_type = TokenType.ePlus;
                    }
                    else if( tok.m_strVal == "-" )
                    {
                        tok.m_type = TokenType.eMinus;
                    }
                    else if( tok.m_strVal == "*" )
                    {
                        tok.m_type = TokenType.eMul;
                    }
                    else if( tok.m_strVal == "/" )
                    {
                        tok.m_type = TokenType.eDiv;
                    }
                    else if( tok.m_strVal == "^" )
                    {
                        tok.m_type = TokenType.ePower;
                    }
                    else if( tok.m_strVal == "=" )
                    {
                        tok.m_type = TokenType.eEqual;
                    }
                    else if( tok.m_strVal == ">" )
                    {
                        tok.m_type = TokenType.eGtr;
                    }
                    else if( tok.m_strVal == "<" )
                    {
                        tok.m_type = TokenType.eLt;
                    }
                    else if( tok.m_strVal == ">=" )
                    {
                        tok.m_type = TokenType.eGte;
                    }
                    else if( tok.m_strVal == "<=" )
                    {
                        tok.m_type = TokenType.eLte;
                    }
                    else if( tok.m_strVal == "<>" )
                    {
                        tok.m_type = TokenType.eNeq;
                    }
                    break;
                case LexStatus.eDelim:
                    if( buffer[0] == ';' )
                    {
                        tok.m_type = TokenType.eSemi;
                    }
                    else if( buffer[0] == ',' )
                    {
                        tok.m_type = TokenType.eComma;
                    }
                    else if( buffer[0] == ':' )
                    {
                        tok.m_type = TokenType.eColon;
                    }
                    else if( buffer[0] == '(' )
                    {
                        tok.m_type = TokenType.eLeftBra;
                    }
                    else if( buffer[0] == ')' )
                    {
                        tok.m_type = TokenType.eRightBra;
                    }
                    break;
                case LexStatus.eSymbol:
                    tok.m_type = TokenType.eSymbol;
                    tok.m_strVal = buffer.ToString();

                    // recognize the keyword 
                    if (m_symbolTypes.ContainsKey(tok.m_strVal))
                    {
                        tok.m_type = m_symbolTypes[tok.m_strVal];

                        // skip the code comment 
                        if (tok.m_type == TokenType.eRem)
                        {
                            tok.m_type = TokenType.eEOL;
                            nextLine();
                        }
                    }
                    break;
                case LexStatus.eFileNum:
                    tok.m_type = TokenType.eFileNum;
                    tok.m_strVal = buffer.ToString();
                    break;
                case LexStatus.eLineEnd:
                    tok.m_type = TokenType.eEOL;
                    break;
                case LexStatus.eStart:
                    tok.m_type = TokenType.eEOF;
                    break;
                case LexStatus.eError:
                    tok.m_type = TokenType.eError;
                    tok.m_strVal = buffer.ToString();
                    break;
                default:
                    tok.m_type = TokenType.eError;
                    break;
            }

            return tok;
        }

        /// <summary>
        /// reset the tokenizer 
        /// </summary>
        public void Reset()
        {
            m_curIndex = 0;
        }


        //------------------------ private functions ------------------------ 

        /// <summary>
        /// skip characters to next line 
        /// </summary>
        protected void nextLine()
        {
            for (; m_curIndex < m_sourceCode.Length; m_curIndex++)
            {
                if (m_sourceCode[m_curIndex] == '\n')
                {
                    m_curIndex++;
                    break;
                }
            }
        }

        /// <summary>
        /// step back a char 
        /// </summary>
        protected void backAChar()
        {
            if( m_curIndex > 0 )
                m_curIndex--;
            else
                throw new Exception("[Tokenizer]: can not step back. ");
        }

        /// <summary>
        /// look ahead a char 
        /// </summary>
        /// <returns></returns>
        protected char lookAheadChar()
        {
            char c = char.MaxValue;

            if( ( m_curIndex + 1 ) < m_sourceCode.Length )
                c = m_sourceCode[m_curIndex + 1];

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
            if( c == ' ' || c == '\t' || c == '\r' )
                return true;

            return false;
        }

        /// <summary>
        /// if this char belong to operator
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        protected bool isOpChar( char c )
        {
            return m_opChr.Contains(c);
        }

        /// <summary>
        /// judge if is delimter or not 
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        protected bool isDelim( char c )
        {
            return m_delimChr.Contains(c);
        }

        /// <summary>
        /// if is end of line or not 
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        protected bool isLineEnd( char c )
        {
            if( c == '\n' || c == '\r' )
                return true;

            return false;
        }
    }
}
