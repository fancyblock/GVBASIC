%{
	#include <stdio.h>
	#include "y.tab.h"

	int yywrap(void)
	{
		return 1;
	}

	char buffer[1024];
	int bufferIndex = 0;
%}

%s 	COMMENT
%s  STRING_STATE 

%%

<COMMENT>.						{}
<COMMENT>\n						BEGIN INITIAL;

<INITIAL>\n 					{}
<INITIAL>"REM"					BEGIN COMMENT;
<INITIAL>"AND"					return AND;
<INITIAL>"OR"					return OR;
<INITIAL>"NOT"					return NOT;
<INITIAL>"LET"					return LET;
<INITIAL>"READ"					return READ;
<INITIAL>"DATA"					return DATA;
<INITIAL>"RESTORE"				return RESTORE;
<INITIAL>"GOTO"					return GOTO;
<INITIAL>"IF"					return IF;
<INITIAL>"THEN"					return THEN;
<INITIAL>"ELSE"					return ELSE;
<INITIAL>"FOR"					return FOR;
<INITIAL>"NEXT"					return NEXT;
<INITIAL>"WHILE"				return WHILE;
<INITIAL>"WEND"					return WEND;
<INITIAL>"TO"					return TO;
<INITIAL>"STEP"					return STEP;
<INITIAL>"DEF"					return DEF;
<INITIAL>"FN"					return FN;
<INITIAL>"GOSUB"				return GOSUB;
<INITIAL>"RETURN"				return RETURN;
<INITIAL>"ON"					return ON;

<INITIAL>[A-Z][A-Z0-9]*[#\$]?	{
	strcpy( yylval.str_val, yytext );
	return SYMBOL;
}

<INITIAL>([1-9][0-9]*)|0		{
	sscanf( yytext, "%d", &yylval.int_val );
	return INT;
}

<INITIAL>[0-9]*\.[0-9]+			{
	sscanf( yytext, "%f", &yylval.float_val );
	return REAL;
}

<INITIAL>"+"					return PLUS;
<INITIAL>"-"					return MINUS;
<INITIAL>"*"					return MUL;
<INITIAL>"/"					return DIV;
<INITIAL>"^"					return POWER;
<INITIAL>"="					return EQUAL;
<INITIAL>">="					return GTE;
<INITIAL>"<="					return LTE;
<INITIAL>">"					return GTR;
<INITIAL>"<"					return LT;
<INITIAL>"<>"					return NEQ;
<INITIAL>";"					return SEMI;
<INITIAL>","					return COMMA;
<INITIAL>":"					return COLON;
<INITIAL>"("					return LEFTBRA;
<INITIAL>")"					return RIGHTBRA;

<INITIAL>\"						{
	BEGIN STRING_STATE;
	bufferIndex = 0;
}

<INITIAL>[\t\n\r ]				{}
<INITIAL>.						return ERROR;

<STRING_STATE>\\n 				{
	buffer[bufferIndex] = '\n';
	bufferIndex++;
}

<STRING_STATE>\\t 				{
	buffer[bufferIndex] = '\t';
	bufferIndex++;
}

<STRING_STATE>\" 				{
	buffer[bufferIndex] = '\0';
	strcpy( yylval.str_val, buffer );
	BEGIN INITIAL;

	return STRING;
}

<STRING_STATE>.					{
	buffer[bufferIndex] = yytext[0];
	bufferIndex++;
}

<STRING_STATE>\n 			return ERROR;

%%
