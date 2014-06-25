%{
	#include <stdio.h>
	#include <stdlib.h>
	#define YYDEBUG		1
%}
%union
{
	int		int_val;
	float	float_val;
	double	double_val;
	char	str_val[18];			// the max length of symbol in GVBASIC is 16 
}
%token
//TODO 
%%
//TODO
%%

//TODO 
