[内容]


----- 符号定义 -----
%		整形变量
$		字符串
+		加法运算符
-		减法运算符
*		乘法运算符
/		除法运算符
^		乘方运算符
>		大于
<		小于
=		等于
>=		大于或等于
<=		小于或等于
<>		不等于
AND		逻辑与
OR		逻辑或
NOT		逻辑非


----- 语句分类 -----
函数:
SIN,COS,TAN,LOG,EXP,SQR,ABS,INT,SGN,RND,LEFT$,RIGHT$,MID$,ASC,CHR$,LEN,SPC,TAB,STR$,MKI$,MKS$,CVI$,CVS$,VAL,POS,LOF,EOF
语句:
TEXT,GRAPH,NORMAL,INVERSE,LOCATE,READ,DATA,DEF,FN,DIM,END,FOR...TO...STEP,NEXT,ON...GOTO,GOTO,LET,IF...THEN...ELSE,CALL,INPUT,PRINT,REM,RESTORE,RESUME,RETURN,POP,STOP,CONT,WHILE...WEND,SWAP,BEEP,PLAY,DRAW,LINE,BOX,CIRCLE,ELLIPSE,OPEN,CLOSE,WRITE,FIELD,GET,PUT,LSET,RSET,INKEY$


----- 语法规则 -----
变量名最长:	16
字符串最长:	255
数组维数:	255
整型变量范围:
	-32768,32768
实型变量范围:
  -1E+38,1E+38
最大行号:	9999


------ 功能键 ------
F1		添加新文件
F2		删除文件
F4		修改文件
输入	执行文件


-----出错代码-----
01.	NEXT WITHOUT FOR:
NEXT语句没有对应的FOR语句,或是二者变量不对应。

02.	SYNTAX:
语法错误,如不相符的括号,拼写错了命令和语句,或不正确的标点符号。

03.	OUT OF DATA:
超过数据区的范围,READ语句已读完DATA语句中的所有数据,已无数据可供READ语句读。

04.	ILLEGAL QUANTITY:
非法的功能调用,出现一个不合法的参数被传送到系统函数中去。

05.	OVERFLOW:
溢出,数值太大,超过了GV BASIC允许的范围。

06.	OUT OF MEMORY:
记忆体溢出,程序太大,有过多的FOR循环或GOSUB,过多的变量。

07.	UNDEF'D STATEMENT
:未定义行号,在语句或命令中应用了程序中不存在的行号。

08.	BAD SUBSCRIPT:
下标月结,用户使用了错误的数组元素,或使用了错误的下标值。

09.	REDIM'D ARRAY:
重复定义数组。

10.	DIVISION BY ZERO:
被零除。

11.	ILLEGAL DIRECT:
非法的直接命令

12.	TYPE MISMATCH:
类型不相符

13.	STRING TOO LONG:
字符串太长

14.	FORMULA TOO COMPLEX:
字符串式子太复杂

15.	CAN'T CONTINUE:
不能继续运行

16.	UNDEF'D FUNCTION:
未定义用户自定义函数

17.	WEND WITHOUT WHILE:
有WEND语句无WHILE语句。

18.	RETURN WITHOUT GOSUB:
有RETURN语句无GOSUB语句。

19.	OUT OF SPACE:
空间不足

20.	FILE CREATE:
文件创建错误。

21.	FILE OPEN:
文件打开错误。

22.	FILE CLOSE:
文件关闭错误。

23.	FILE READ:
读文件错误。

24.	FILE WRITE:
写文件错误。

25.	FILE DELETE:
删除文件错误。

26.	FILE NOT EXIST:
文件不存在。

27.	RECORD NUMBER:
记录号错误。

28.	FILE NUMBER:
文件号错误。

29.	FILE MODE:
文件模式错误。

30.	SAME FILE EXIST:
有同名文件存在。

31.	FILE LENGTH READ:
读文件长度错误。

32.	ILLEGAL FILE NAME:
非法文件名。

33.	FILE TOO LONG:
数据文件超长。

34.	FILE REOPEN:
文件重打开错误。


-----ASCII码表-----

略。


[索引]


ABS		取绝对值
用法:	ABS(exp)
如:
]10 A=ABS(35*-4)
]20 PRINT A
]30 END
运行
140
]

AND		逻辑与
用法:	AND
如:
]20	IF A>0 AND A<1 THEN PRINT A

ASC 	求ASCII码
用法:	ASC(s$)
*求取字符串s$中的第一个字符的ASCII码

ATN		反正切值
用法:	ANT(exp)

BEEP	喇叭发声
用法:	BEEP

BOX		画矩形
用法:	BOX X0,Y0,X1,Y1,FILL,TYPE
*FILL=1 填充
*FILL=0 不填充
*TYPE=1 画矩形
*TYPE=0 清矩形
如:
]10 GRAPH
]20 BOX 10,10,40,40
]40 END

CHR$ 	取字符
用法:	CHR$(n)
*求ASCII码为n的字符

CIRCLE	画圆
用法:	CIRCLE X0,Y0,R,FILL,TYPE
*FILL=1 填充
*FILL=0 不填充
*TYPE=1 画圆
*TYPE=0 清圆
如:
]10 GRAPH
]30 CIRCLE 80,40,30,1
]40 END

CLOSE	关闭数据文件
用法:	CLOSE #filenum%

CLS 	清屏
用法:	CLS

COS 	余弦值
用法:	COS(exp)

CVI$	二进制串(2byte)转为整数
用法:	CVI$(s$)
如:
]40 A%=CVI$("10")

CVS$	二进制串(5byte)转为实数
用法:	CVS$(s$)
如:
]30 LSET A$=MKS$(1.445)
]40 A=CVS$(A$)

DATA 	置数据区数据
用法:	DATA dat1,dat2,dat3...
*相关语句 READ

DEF FN 	自定义函数
用法:	DEF FN 函数名=表达式
如:
]10 DEF FN A(W)=2*W+W
]20 PRINT FN A(23)
]30 DEF FN B(X)=4+3
]40 G = FN B(23)
]50 PRINT G
]60 DEF FN A(Y)=FN B(Z)+Y
]70 PRINT FN A(G)
]80 END
运行
69
7
14
]

DIM		数组定义
用法:	DIM数组名(N1,N2,N3...)
*说明数组维数并分配空间,没有经过DIM说明的数组下标设定最大值是10。

DRAW	画点
用法:	DRAW X,Y,TYPE
*TYPE=1 画点
*TYPE=0 清点

ELLIPSE	画椭圆
用法:	ELLIPSE X0,Y0,A,B,FILL,TYPE
*FILL=1 填充
*FILL=0 不填充
*TYPE=1 画椭圆
*TYPE=0 清椭圆
如:
]10 GRAPH
]20 ELLIPSE 80,40,60,30,1
]40 END

IF...THEN/(GOTO)...ELSE 条件判断
用法:
IF...THEN n ,IF...GOTO n 条件成立时跳到n行处执行;条件不成立时接着下一行执行。
IF...THEN n ELSE... , IF...GOTO n ELSE... 条件成立时提到n行处执行;条件不成立时执行ELSE后的语句,接着执行下一行。
如:
]10 N=1
]20 PRINT N,SQR(N)
]30 N=N+1
]40 IF N<=4 THEN GOTO 20
]50 END
]

END		程序结束
用法:	END
如:
]10 LET N=2008-1999
]20 R=0.09
]30 LET AAL=100*(1+R)^N
]40 PRINT "AAL=";AAL
]50 END

EOF		顺序文件是否结束,未结束返回0
用法:	EOF(filenum%)
如:
]50 IF EOF(1) THEN PRINT "FILE END"

EXP 	e的n次方
用法:
EXP (x)
* e的x次方值

FIELD 	指定缓冲区大小分配缓冲区变量
用法:
FIELD #filenum%, width% AS var1$[,width% AS var2$]...
如:
]40 FIELD #1,2 AS A$, 3 AS B$

FOR...TO...STEP... NEXT 循环控制
用法:
FOR...TO...
...
NEXT
如:
]10 FOR I=5 TO 1 STEP -1
]20 PRINT I
]30 NEXT I
]40 END
]

GET		从指定文件读取指定记录放入缓冲区
用法:	GET #filenum%, recordnum%
如:
]40 GET #1, 1

GOSUB 	跳转子程序
用法:	GOSUB 行号
*程序跳转到以指定行号开始的子程序运行,遇到RETURN语句时就返回到GOSUB的下条语句接着运行。
如:
]10 GOSUB 100
]20 IF X>0 THEN PRINT X ELSE PRINT "ERR"
]30 END
]100 INPUT X
]110 RETURN
运行
?45
45
]

GOTO	无条件跳转
用法:	GOTO 行号
*程序跳转到指定行号处运行.

GRAPH	图形模式
用法:	GRAPH
如:
]10 GRAPH
]20 BOX 10,10,40,40
]30 CIRCLE 50,50,30
]40 END

INKEY$	读键值
用法:	INKEY$

INPUT 	从键盘或文件输入
用法:	INPUT A1,A2,A3...
如:
]10 INPUT A
]20 INPUT B
]30 END
运行
?23
23
]10 INPUT A$
]20 PRINT A$
]30 END
运行
?I LOVE GVBASIC
I LOVE GVBASIC
]
用法:
INPUT #filenum%,varlist[,varlist,...]
如:
]10 INPUT #1,A$,A1$

INT 	取整
用法:
INT (x)
*取值为x的整数部分
如:
]10 A=234.58
]20 PRINT INT(A)
]30 END
运行
234
]

INVERSE	反显字符
用法:	INVERSE
如:
]30 INVERSE

LEFT$	取字符
用法:
LEFT$ (S$,n)
*取字符串s$左端的n个字符。

LEN 	求串长
用法:	LEN(S$)
*求字符串s$的长度

LET 	给变量赋值
用法:	LET 变量=表达式
*计算表达式的值,将其赋给指定变量。LET可省略。
如:
]10 LET N=2008-1999
]20 R=0.09
]30 LET AAL=100*(1+R)^N
]40 PRINT "AAL=";AAL
]50 END

LINE	画直线
用法:	LINE X0,Y0,X1,Y1,TYPE
*TYPE=1 画线
*TYPE=0 清线
如:
]10 GRAPH
]30 LINE 10,10,50,50,1
]40 END

LOCATE	置显示位置
用法:	LOCATE 行号,列号

LOF		返回随机文件长度
用法:	LOF(filenum%)
如:
]40 PRINT LOF(1)

LOG		取对数
用法:	LOG(x)
*以e为底的x对数。

LSET	向缓冲区分配的变量赋值,并做左对齐调整
用法:
LSET var$=strexpr$
如:
]40 LSET A$="HELLO"

MID$	取字符
用法:	MID$(s$,n,m)
*从字符串n位置起取m个字符。

MKI$	整数转为二进制串(2byte表示)
用法:	MKI$(intexpr)
如:
]40 RSET A$=MKI$(10)

MKS$	实数转为二进制串(5byte表示)
用法:	MKS$(numexpr)
如:
]40 LSET A$=MKS$(1.445)

NORMAL	正常显示
用法:	NORMAL
如:
]20 NORMAL

NOT		逻辑非
用法:	NOT
如:
]20 IF NOT(A>2) THEN PRINT A

NOTRACE	退出单步状态
用法:	NOTRACE
* 进入单步跟踪状态后,执行NOTRACE语句后,退出单步状态返回正常状态。

ON 		控制转移
用法:	ON...GOTO n1,n2,n3...
*计算ON后的表达式值,为1时对应n1子程序,为2时对应n2子程序.....,当无对应时继续执行下一条语句。

OPEN	打开文件
用法:	OPEN file$ [FOR mode] AS #filenum% [LEN=reclen%]
file$:	文件名串
mode:	打开方式
		INPUT 只读
		OUTPUT 只写
		APPEND 追加
		RANDOM 随机
filenum%: 文件号
LEN:	缓冲区长度,默认为32(只在RANDOM方式下有效)
如:
]10 OPEN "DAT" FOR OUTPUT AS #1
]20 OPEN "DAT1" FOR RANDOM AS #2 LEN=50

OR 		逻辑或
用法:	OR
如:
]20 IF A>1 OR B>1 THEN PRINT A*B

PLAY	演奏音乐
用法:	PLAY "CDEFGAB"

POP		将堆栈中的返回地址弹栈
用法:	POP
*和RETURN功能差不多,但程序不是返回到GOSUB的下条语句,而是接着运行。

POS		取光标位置
用法:	POS(n)
*得到光标的水平位置

PRINT	屏幕显示
用法:	1.PRINT exp
		显示完表达式以后换行。
		2.PRINT exp1,exp2,exp3...
		显示完每个表达式以后就换行。
		3.PRINT exp1;exp2;exp3...
		显示每个表达式以后不换行,接着显示下一个表达式。
		*当显示超过一行时,自动换行接着显示。
如:
]A=23:LET B=15
]PRINT A,B
23
15
]PRINT "THIS";"IS";"GVBASIC"
THISISGVBASIC
]

PUT		向指定文件的指定记录写入缓冲区内容
用法:
PUT #filenum%,recordnum%
如:
]40 PUT #1,1

READ 	从数据区读数
用法:	READ dat1,dat2,dat3...
如:
]10 REM AVERAGE
]20 S0=0
]30 GOSUB 100
]40 PRINT S0
]50 GOTO 990
]100 READ A
]110 WHILE A<>-1
]120 S0=S0+A
]130 READ A
]140 WEND
]150 RETURN
]900 DATA 34,45,50,65,23,88,-1
]990 END
运行

REM 	注释
用法:	REM 说明
*REM是非执行语句,用来对程序或程序中的某些语句做注释,便于阅读程序。

RESTORE	恢复指针
用法:	RESTORE
*恢复数据区指针到数据区头。

RETURN 	子程序返回
用法:	RETURN
*相关语句 GOSUB

RIGHT$	取字符
用法:	RIGHT$(s$,n)
*取字符串s$右端的n个字符。

RND		产生随机数
用法:	RND(x)
*产生一个(0,1)间的随机小数。如果x>0,每次产生不同的随机数;如果x<0,产生有一定序列的随机数;如果x=0,输出上次产生的随机数。

RSET	向缓冲区分配的变量赋值,并做右对齐调整
用法:	RSET var$=strexpr$
如:
]40 RSET A$="WORLD"

SGN		取符号
用法:	SGN(x)

SIN 	正弦值
用法:	SIN(exp)

SPC		打印空格
用法:	SPC(n)
*打印n个空格。
如:
]PRINT 45;SPC(4);56
45    56
]

SQR		平方根
用法:	SQR(exp)

STR$	数字转为字串
用法:	STR$(n)
*将数字n转化为字符串

SWAP	变量互换
用法:	SWAP A1,B1
如:
]10 A=28:B=50
]20 PRINT A,B
]30 SWAP A,B
]40 PRINT A,B
]50 END
]

TAB 	输出控制
用法:	TAB(n)
*从第n列开始输出。
如:
]PRINT "QWER";TAB(8);"ASDF"
QWER    ASDF
]PRINT "QWERTYUI";TAB(5);"ASDFGH"
QWERTYUI
    ASDFGH
]

TAN		正切值
用法:	TAN(exp)

TEXT 	文本模式
用法:	TEXT

TRACE	单步跟踪
用法:	TRACE
* TRACE指令执行后进入单步跟踪状态,这是要执行的每一条语句和行号都显示在屏幕上,直到执行了NOTRACE才退出单步跟踪状态。

VAL		字串转为数字
用法:	VAL(s$)
*将字符串s$转化为数字。

WHILE/WEND 循环控制
用法:
WHILE 表达式
...
WEND
*当表达式为真时执行循环,当表达式为假时继续执行WEND后的下一条语句。
如:
]10 REM AVERAGE
]20 S0=0
]30 GOSUB 100
]40 PRINT S0
]50 GOTO 990
]100 READ A
]110 WHILE A<>-1
]120 S0=S0+A
]130 READ A
]140 WEND
]150 RETURN
]900 DATA 34,45,50,65,23,88,-1
]990 END
运行
305
]

WRITE	写数据到文件(如无文件号写到屏幕)
用法:
WRITE [#filenum%,] expr1, expr2, ...
如:
]40 WRITE #1, A$, B%

