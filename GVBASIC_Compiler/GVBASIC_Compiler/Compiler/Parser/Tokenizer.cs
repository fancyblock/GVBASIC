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
        protected Dictionary<string, int> m_symbolTypes;

        /// <summary>
        /// constructor 
        /// </summary>
        public Tokenizer( string source )
        {
            // init the tokenizer 
            m_opChr = new List<char> { '+', '-', '*', '/', '^', '=', '>', '<' };
            m_delimChr = new List<char> { ':', ',', ';', '(', ')' };

            m_symbolTypes = new Dictionary<string, int>()
            {
                // keywords
                {"PRINT", Token.PRINT},
                {"AND", Token.AND},
                {"OR", Token.OR},
                {"NOT", Token.NOT},
                {"LET", Token.LET},
                {"DIM", Token.DIM},
                {"READ", Token.READ},
                {"DATA", Token.DATA},
                {"RESTORE", Token.RESTORE},
                {"GOTO", Token.GOTO},
                {"IF", Token.IF},
                {"THEN", Token.THEN},
                {"ELSE", Token.ELSE},
                {"WHILE", Token.WHILE},
                {"WEND", Token.WEND},
                {"TO", Token.TO},
                {"STEP", Token.STEP},
                {"DEF", Token.DEF},
                {"FN",Token.FN},
                {"GOSUB", Token.GOSUB},
                {"RETURN", Token.RETURN},
                {"ON", Token.ON},
                {"REM", Token.REM},
                {"NEXT", Token.NEXT},
                {"FOR", Token.FOR},
                {"OPEN", Token.OPEN},
                {"CLOSE", Token.CLOSE},
                {"FIELD", Token.FIELD},
                {"GET", Token.GET},
                {"LSET", Token.LSET},
                {"PUT", Token.PUT},
                {"RSET", Token.RSET},
                {"WRITE", Token.WRITE},
                {"INPUT", Token.INPUT},
                {"INKEY$", Token.INKEY},
                {"SWAP", Token.SWAP},
                {"END", Token.END},
                // simple command
                {"BEEP", Token.SIMPLE_CMD},
                {"CLS", Token.SIMPLE_CMD},
                {"INVERSE", Token.SIMPLE_CMD},
                {"NORMAL", Token.SIMPLE_CMD},
                {"GRAPH", Token.SIMPLE_CMD},
                {"TEXT", Token.SIMPLE_CMD},
                {"CLEAR", Token.SIMPLE_CMD},
                // param command
                {"PLAY", Token.PARAM_CMD},
                {"BOX", Token.PARAM_CMD},
                {"CIRCLE", Token.PARAM_CMD},
                {"DRAW", Token.PARAM_CMD},
                {"ELLIPSE", Token.PARAM_CMD},
                {"LINE", Token.PARAM_CMD},
                {"LOCATE", Token.PARAM_CMD},
                // buildin functions
                {"ABS", Token.FUNC},
                {"SGN", Token.FUNC},
                {"INT", Token.FUNC},
                {"SIN", Token.FUNC},
                {"COS", Token.FUNC},
                {"TAN", Token.FUNC},
                {"ATN", Token.FUNC},
                {"SQR", Token.FUNC},
                {"EXP", Token.FUNC},
                {"LOG", Token.FUNC},
                {"RND", Token.FUNC},
                {"ASC", Token.FUNC},
                {"LEN", Token.FUNC},
                {"CHR$", Token.FUNC},
                {"LEFT$", Token.FUNC},
                {"MID$", Token.FUNC},
                {"RIGHT$", Token.FUNC},
                {"STR$", Token.FUNC},
                {"VAL", Token.FUNC},
                {"CVI$", Token.FUNC},
                {"MKI$", Token.FUNC},
                {"CVS$", Token.FUNC},
                {"MKS$", Token.FUNC},
                {"POS", Token.FUNC},
                {"SPC", Token.FUNC},
                {"TAB", Token.FUNC},
                {"EOF", Token.FUNC},
                {"LOF", Token.FUNC},
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
                    tok.m_type = Token.INT;
                    tok.m_intVal = Int32.Parse(buffer.ToString());
                    break;
                case LexStatus.eString:
                    tok.m_type = Token.STRING;
                    tok.m_strVal = buffer.ToString();
                    break;
                case LexStatus.eRealNum:
                    tok.m_type = Token.FLOAT;
                    tok.m_floatVal = float.Parse(buffer.ToString());
                    break;
                case LexStatus.eOpCode:
                    tok.m_strVal = buffer.ToString();
                    if( tok.m_strVal == "+" )
                    {
                        tok.m_type = Token.PLUS;
                    }
                    else if( tok.m_strVal == "-" )
                    {
                        tok.m_type = Token.MINUS;
                    }
                    else if( tok.m_strVal == "*" )
                    {
                        tok.m_type = Token.MUL;
                    }
                    else if( tok.m_strVal == "/" )
                    {
                        tok.m_type = Token.DIV;
                    }
                    else if( tok.m_strVal == "^" )
                    {
                        tok.m_type = Token.POWER;
                    }
                    else if( tok.m_strVal == "=" )
                    {
                        tok.m_type = Token.EQUAL;
                    }
                    else if( tok.m_strVal == ">" )
                    {
                        tok.m_type = Token.GTR;
                    }
                    else if( tok.m_strVal == "<" )
                    {
                        tok.m_type = Token.LT;
                    }
                    else if( tok.m_strVal == ">=" )
                    {
                        tok.m_type = Token.GTE;
                    }
                    else if( tok.m_strVal == "<=" )
                    {
                        tok.m_type = Token.LTE;
                    }
                    else if( tok.m_strVal == "<>" )
                    {
                        tok.m_type = Token.NEG;
                    }
                    break;
                case LexStatus.eDelim:
                    if( buffer[0] == ';' )
                    {
                        tok.m_type = Token.SEMI;
                    }
                    else if( buffer[0] == ',' )
                    {
                        tok.m_type = Token.COMMA;
                    }
                    else if( buffer[0] == ':' )
                    {
                        tok.m_type = Token.COLON;
                    }
                    else if( buffer[0] == '(' )
                    {
                        tok.m_type = Token.LEFT_BRA;
                    }
                    else if( buffer[0] == ')' )
                    {
                        tok.m_type = Token.RIGHT_BRA;
                    }
                    break;
                case LexStatus.eSymbol:
                    tok.m_type = Token.SYMBOL;
                    tok.m_strVal = buffer.ToString();

                    // recognize the keyword 
                    if (m_symbolTypes.ContainsKey(tok.m_strVal))
                    {
                        tok.m_type = m_symbolTypes[tok.m_strVal];

                        // skip the code comment 
                        if (tok.m_type == Token.REM)
                        {
                            tok.m_type = Token.EOL;
                            nextLine();
                        }
                    }
                    break;
                case LexStatus.eFileNum:
                    tok.m_type = Token.FILE_NUM;
                    tok.m_strVal = buffer.ToString();
                    break;
                case LexStatus.eLineEnd:
                    tok.m_type = Token.EOL;
                    break;
                case LexStatus.eStart:
                    tok.m_type = Token.FILE_END;
                    break;
                case LexStatus.eError:
                    tok.m_type = Token.ERROR;
                    tok.m_strVal = buffer.ToString();
                    break;
                default:
                    tok.m_type = Token.ERROR;
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
