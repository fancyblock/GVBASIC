%{
	#include <stdio.h>
	#include "y.tab.h"

	int yywrap(void)
	{
		return 1;
	}

	int line = 1;
	char buffer[1024];
	int bufferIndex = 0;
%}

%s 	COMMENT
%s  STRING_STATE 

%%

<INITIAL>\n 					{
	printf("\n%d\t", ++line);
	BEGIN INITIAL;
}

<INITIAL>[1-9][0-9]*			{
	printf( "INT " );
	//sscanf( yytext, "%d", &yylval.int_val );
	return INT;
}

<INITIAL>[A-Z][A-Z0-9]*[#\$]?	{
	printf( "SYMBOL " );
	//strcpy( yylval.str_val, yytext );
	return SYMBOL;
}

<INITIAL>[0-9]|([1-9][0-9]+).[0-9]+	{
	printf( "REAL " );
	//sscanf( yytext, "%f", &yylval.float_val );
	return REAL;
}

<INITIAL>"+"					{
	printf( "PLUS " );
	return PLUS;
}

<INITIAL>"-"					{
	printf( "SUB " );
	return MINUS;
}

<INITIAL>"*"					{
	printf( "MUL " );
	return MUL;
}

<INITIAL>"/"					{
	printf( "DIV " );
	return DIV;
}

<INITIAL>"^"					{
	printf( "POWER " );
	return POWER;
}

<INITIAL>"="					{
	printf( "EQUAL " );
	return EQUAL;
}

<INITIAL>\"						{
	BEGIN STRING_STATE;
	bufferIndex = 0;
}

<INITIAL>.						{}

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
	//strcpy( yylval.str_val, buffer );
	printf( "STRING " );
	BEGIN INITIAL;

	return STRING;
}

<STRING_STATE>.					{
	buffer[bufferIndex] = yytext[0];
	bufferIndex++;
}

<STRING_STATE>\n 			return ERROR;


%%



/*
 * 	test program 
 */
int main( void )
{
	FILE* pBasSrc = fopen( "../Bas/tank.bas", "r" );

	if( pBasSrc != NULL )
	{
		printf( "compiling...\n\n" );

		yyin = pBasSrc;
		
		printf( "1\t" );			// first line number 
		int result = yylex();
		while( result )
		{
			result = yylex();
		}

		fclose( pBasSrc );
	}
	else
	{
		printf( "Can not find file \"tank.bas\"." );
	}

	printf( "\n\n[Over]\n" );

	return 0;
}


