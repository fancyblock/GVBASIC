%{
	#include <stdio.h>
	#include "y.tab.h"

	int yywrap(void)
	{
		return 1;
	}
%}
%%
""
%%

//TODO 
