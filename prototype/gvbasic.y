%{
	#include <stdio.h>
	#include <stdlib.h>
	#define YYDEBUG		1
%}
%union
{
	int		int_val;
	float	float_val;
	char	str_val[128];			// the max length of string 
}
%token <str_val> SYMBOL STRING 
%token <int_val> INT
%token <float_val> REAL
%token PLUS MINUS MUL DIV POWER EQUAL GTR LT GTE LTE NEQ AND OR NOT SEMI COMMA COLON LEFTBRA RIGHTBRA POUND QM LET READ DATA RESTORE GOTO IF THEN ELSE FOR NEXT WHILE WEND TO STEP DEF FN GOSUB RETURN ON REM ERROR 
//%type --------- TODO ---------
%%
line : INT;
%%


/*
 * 	entry 
 */
int main(void)
{
	//TODO 

	return 0;
}
