10 CLEAR <->
20 CLS
30 LOCATE 1,5:PRINT "**Tank**"
40 LOCATE 3,5:PRINT "Code:HXL"
50 LOCATE 4,2:PRINT "Press Any Key"
60 S$=""
70 S$=INKEY$
80 IF S$="" THEN GOTO 70
90 IF S$="q" THEN CLS:PRINT "Code:haoxinli1234@163.com":END
100 CLS
110 GRAPH
120 DIM TKX(3)
130 DIM TKY(3)
140 DIM TL(3)
150 DIM MAP(19,7)
160 PC=3
170 ROUND=1
180 V=3:G=.12:N=6:L=8:A=45
190 TKID=0
200 JDX=15
210 JDY=70
220 POWERMAX=7
230 POW=.1
240 LINE 0,60,160,60,1
250 CIRCLE 15,70,9,0,1
260 MOVEID=MOVEMAX
270 LX=INT(RND(1)*2)
280 XS=1
290 XL=INT(RND(1)*5)
300 YS=INT(RND(1)*5)+4
310 FOR XUNX=XS TO XS+XL
320 IF XUNX >18  THEN GOTO 430
330 IF MAP(XUNX,YS-2)<>0 THEN GOTO 400
340 MAP(XUNX,YS-2)=2
350 DX=XUNX*8
360 DY=YS*6
370 BOX DX,DY,DX+7,DY+5,0,1
380 LINE DX+2,DY+2,DX+5,DY+2,1
390 LINE DX+2,DY+3,DX+2,DY+3,1
400 NEXT XUNX
410 XS=XS+XL
420 GOTO 290
430 FOR XUN=0 TO PC
440 SX=INT(RND(1)*18)+1
450 FOR XUNY=INT(RND(1)*6)+2 TO 7
460 IF MAP(SX,XUNY-2)=0 AND MAP(SX,XUNY-1)>0 THEN GOTO 490
470 NEXT XUNY
480 GOTO 440
490 MAP(SX,XUNY-2)=-1
500 TKX(XUN)=SX*8
510 TKY(XUN)=XUNY*6
520 TL(XUN)=100
530 XX=TKX(XUN)
540 YY=TKY(XUN)
550 FX=2
560 IF XUN>0 THEN GOTO 600
570 BOX XX,YY,XX+7,YY+5,1,1
580 W$=INKEY$
590 BOX XX,YY,XX+7,YY+5,1,0
600 GOSUB 1890
610 NEXT XUN
620 BLFH=0
630 GOTO 2020
640 BOX 50,62,158,65,0,1
650 SAVEFX=2
660 IF TL(TKID)<1 THEN GOTO 1300
670 IF CPURE=1 THEN GOTO 2750
680 SMOVE=0
690 IF PEEK(200)+PEEK(201)+PEEK(202)+PEEK(203)+PEEK(204)+PEEK(205)=1275 THEN GOTO 660
700 IF PEEK(202)=254 THEN GOTO 10
710 IF PEEK (201)=251 AND FX=2  THEN A=A+5:GOSUB 1770
720 IF PEEK (201)=247 AND FX=2  THEN A=A-5:GOSUB 1770
730 IF PEEK (201)=251 AND FX=1  THEN A=A-5:GOSUB 1770
740 IF PEEK (201)=247 AND FX=1  THEN A=A+5:GOSUB 1770
750 IF PEEK(205)=127 THEN FX=2:GOTO 1370
760 IF PEEK(201)=127 THEN FX=1:GOTO 1370
770 IF PEEK(203)=247 THEN GOTO 1670
780 GOTO 660
790 IF FX=1 THEN X=TKX(TKID)+9
800 IF FX=2 THEN X=TKX(TKID)-2
810 Y=TKY(TKID)-2
820 IF TKID=0 THEN  VX=V*COS(AA):VY=V*SIN(AA)
830 WHILE X>0 AND  Y<57  AND  X<158
840 IF PEEK(202)=254 THEN GOTO 10
850 IF TKID>0 AND Y<2 THEN GOTO 1300
860 X=X-VX:Y=Y-VY
870 IF TKID=0 THEN VY=VY-G
880 IF X<1 OR Y<1  THEN GOTO 900
890 BOX X,Y,X+1,Y+1,1,1
900 MX=INT(X/8+.2)
910 MY=INT(Y/6+.2)-2
920 IF MX>19 OR MX<0 OR MY>7 OR MY<0 THEN  GOTO  1060
930 IF MAP(MX,MY)<1 THEN GOTO 1060
940 BX=MX*8:BY=MY*6+12
950 GOSUB 2590
960 ZHI=MAP(MX,MY)
970 IF ZHI<1 THEN GOTO 1060
980 ZHI=ZHI-1
990 MAP(MX,MY)=ZHI
1000 IF X<1 OR Y<1 THEN GOTO  1020
1010 BOX X,Y,X+1,Y+1,1,0
1020 IF ZHI=0  THEN  GOTO 1040
1030 BOX MX*8,MY*6+12,MX*8+7,MY*6+17,0,1
1040 SMOVE=1
1050 GOTO 2260
1060 IF SMOVE=1 THEN GOTO 1300
1070 FOR XUN=0 TO PC
1080 PDX=TKX(XUN)
1090 PDY=TKY(XUN)
1100 IF X>PDX-1 AND X<PDX+6 AND Y>PDY-1 AND Y<PDY+5 AND TL(XUN)>0 THEN ID=XUN: GOTO 1130
1110 NEXT XUN
1120 GOTO 1270
1130 BX=PDX
1140 BY=PDY
1150 GOSUB 2590
1160 XX=PDX
1170 YY=PDY
1180 FX=2
1190 IF ID=0 THEN FX=PFX
1200 IF X<1 OR Y<1 THEN GOTO  1220
1210 BOX X,Y,X+1,Y+1,1,0
1220 TL(XUN)=TL(XUN)-(INT(RND(1)*5)+14)
1230 IF TL(XUN)>0 THEN GOSUB 1890
1240 BLFH=1
1250 GOTO 2020
1260 GOTO 1300
1270 IF X<1 OR Y<1 THEN GOTO  1290
1280 BOX X,Y,X+1,Y+1,1,0
1290 WEND
1300 IF TKID=0 THEN  PA=A:PFX=SAVEFX
1310 CANMOVE=0
1320 TKID=TKID+1
1330 IF TKID>PC THEN TKID=0
1340 IF TKID=0 THEN A=PA:SAVEFX=PFX:FX=PFX:CPURE=0:GOSUB 1770
1350 IF TKID>0 AND TL(TKID)>0  THEN ID=TKID: GOTO  2660
1360 GOTO 660
1370 XXX=TKX(TKID)
1380 YYY=TKY(TKID)
1390 IF FX=1 THEN JSZA=TKX(TKID)+8
1400 IF FX=2 THEN JSZA=TKX(TKID)-8
1410 IF SAVEFX=FX THEN GOTO 1450
1420 IF FX=1 THEN A=90+(90-A):GOSUB 1770
1430 IF FX=2 THEN A=180-A:GOSUB 1770
1440 GOTO 1550
1450 IF JSZA<0 THEN JSZA=0
1460 IF JSZA>152 THEN JSZA=152
1470 FOR XUN=TKY(TKID) TO TKY(TKID)+5
1480 RAM=20*XUN+JSZA/8+2496
1490 IF PEEK(RAM)>0 THEN  GOTO 1550
1500 NEXT XUN
1510 IF FX=1 THEN TKX(TKID)=TKX(TKID)+8
1520 IF FX=2 THEN TKX(TKID)=TKX(TKID)-8
1530 IF TKX(TKID)<0 THEN TKX(TKID)=0
1540 IF TKX(TKID)>152 THEN TKX(TKID)=152
1550 SAVEFX=FX
1560 XX=TKX(TKID)
1570 YY=TKY(TKID)
1580 BOX XXX,YYY,XXX+7,YYY+5,1,0
1590 GOSUB 1890
1600 CTX=TKX(TKID)/8
1610 CTY=TKY(TKID)/6
1620 ID=TKID
1630 POSFH=1
1640 GOTO 2340
1650 IF CPURE=1 THEN GOTO 2750
1660 GOTO 660
1670 BOX 50,62,158,65,1,0
1680 FOR V=POW TO POWERMAX STEP .1
1690 POWER= (V/POWERMAX )*100
1700 BOX 50,62,158,65,0,1
1710 IF POWER>100  THEN POWER=100
1720 IF POWER<0  THEN POWER=0
1730 BOX 50,62,50+POWER*1.08,65,1,1
1740 IF PEEK(203)<>247 THEN GOTO  790
1750 NEXT V
1760 GOTO 790
1770 IF  A>270  THEN A=270
1780 IF  A<-90 THEN A=-90
1790 IF FX=1  AND A<90 AND TKID=0  THEN A=90
1800 IF FX=2 AND A>90 AND TKID=0  THEN A=90
1810 DX=JDX-7*COS(AA)
1820 DY=JDY-7*SIN(AA)
1830 LINE DX,DY,JDX,JDY,0
1840 AA=A/180*3.1415926
1850 DX=JDX-7*COS(AA)
1860 DY=JDY-7*SIN(AA)
1870 LINE DX,DY,JDX,JDY,1
1880 RETURN
1890 LINE XX+3,YY+1,XX+4,YY+1,1
1900 LINE XX+2,YY+2,XX+2,YY+4,1
1910 LINE XX+5,YY+2,XX+5,YY+3,1
1920 LINE XX+1,YY+3,XX+6,YY+3,1
1930 LINE XX,YY+4,XX,YY+4,1
1940 LINE XX+4,YY+4,XX+4,YY+4,1
1950 LINE XX+6,YY+4,XX+7,YY+4,1
1960 LINE XX+1,YY+5,XX+6,YY+5,1
1970 IF FX=1 THEN GOTO 2000
1980 LINE XX,YY,XX+1,YY+1,1
1990 GOTO 2010
2000 LINE XX+7,YY,XX+5,YY+2,1
2010 RETURN
2020 LC=0
2030 LID=0
2040 BLY=68
2050 BLI=0
2060 FOR BYY=0 TO 1
2070 FOR BLX=50 TO 140 STEP 58
2080 BLOOD=TL(BLI)
2090 PROSCALE=.5
2100 BOX BLX,BLY,BLX+50,BLY+3,1,0
2110 IF BLOOD>100  THEN BLOOD=100
2120 IF BLOOD<0  THEN BLOOD=0
2130 BOX BLX,BLY,BLX+BLOOD*PROSCALE,BLY+3,1,1
2140 IF TL(BLI)>0 THEN LC=LC+1:LID=XUN
2150 BLI=BLI+1
2160 NEXT BLX
2170 BLY=BLY+6
2180 NEXT BYY
2190 IF LC>1 THEN GOTO 2230
2200 LOCATE 2,7:PRINT  LID ; " Win"
2210 FOR YS=0 TO 5000:NEXT
2220 GOTO 10
2230 IF BLFH=0 THEN GOTO 640
2240 IF BLFH=1 THEN GOTO 1260
2250 IF BLFH=2 THEN GOTO 2560
2260 FOR XUN=0 TO PC
2270 CTX=TKX(XUN)/8
2280 CTY=TKY(XUN)/6
2290 ID=XUN
2300 POSFH=2
2310 GOTO 2340
2320 NEXT XUN
2330 GOTO 1060
2340 IF TL(ID)<1 THEN GOTO 2570
2350 RAM=20*(CTY*6+6)+CTX+2496
2360 IF PEEK(RAM)>0 THEN GOTO 2570
2370 XXXX=CTX*8
2380 YYYY=CTY*6
2390 FOR DOWNY=CTY*6 TO 54 STEP 3
2400 BOX XXXX,YYYY,XXXX+7,YYYY+5,1,0
2410 XX=CTX*8
2420 YY=DOWNY
2430 GOSUB 1890
2440 XXXX=XX
2450 YYYY=YY
2460 TKX(ID)=XX
2470 TKY(ID)=YY
2480 IF DOWNY>50 THEN GOTO 2510
2490 RAM=20*(DOWNY+6)+CTX+2496
2500 IF PEEK(RAM)>0 THEN GOTO 2570
2510 NEXT DOWNY
2520 BX=CTX*8:BY=54:GOSUB 2590
2530 TL(ID)=0
2540 BLFH=2
2550 GOTO 2020
2560 GOTO 1300
2570 IF POSFH=1 THEN GOTO 1650
2580 IF POSFH=2 THEN GOTO 2320
2590 FOR BXUN=0 TO 7
2600 BOX BX,BY,BX+BXUN,BY+5,1,1
2610 NEXT BXUN
2620 FOR BXUN=0 TO 7
2630 BOX BX,BY,BX+BXUN,BY+5,1,0
2640 NEXT BXUN
2650 RETURN
2660 IF TL(TKID)<1 THEN  GOTO 1300
2670 FMOVE=INT(RND(1)*5)+1
2680 FX=INT(RND(1)*2)+1
2690 IF FX=1 THEN ADDM=8 ELSE ADDM=-8
2700 FOR FMOVE=0 TO FMOVE
2710 IF  TKY(TKID)>53 THEN GOTO 2730
2720 IF MAP((TKX(TKID)+ADDM)/8,TKY(TKID)/6-1)<1  THEN GOTO 2760
2730 CPURE=1
2740 GOTO 1370
2750 NEXT
2760 OBJ=INT(RND(1)*(PC+1))
2770 IF TL(OBJ)<=0 OR OBJ=ID  THEN GOTO 2760
2780 OX=TKX(OBJ)+INT(RND(1)*10)-(INT(RND(1)*10))
2790 OY=TKY(OBJ)+INT(RND(1)*10)-(INT(RND(1)*10))
2800 IF DISX>DISY THEN DBL=DISX ELSE DBL=DISY
2810 SMOVE=0
2820 CPUX=TKX(ID)
2830 CPUY=TKY(ID)
2840 X=CPUX-2
2850 Y=CPUY-2
2860 CYX=1:CYY=1
2870 IF CPUX<=OX THEN CYX=-1:FX=1:X=CPUX+9
2880 IF CPUX>OX THEN FX=2
2890 IF CPUY<=OY THEN CYY=-1
2900 VX=ABS(X-OX)/30*CYX
2910 VY=ABS(Y-OY)/30*CYY
2920 IF VY<>0 THEN  XYBL=ABS(VX/VY)
2930 IF VX<4 AND VY<4 THEN VX=4*CYX:IF VY<>0 AND XYBL<>0 THEN  VY=(ABS(VX)/XYBL)*CYY
2940 XX=CPUX:YY=CPUY
2950 BOX XX,YY,XX+7,YY+5,1,0
2960 GOSUB 1890
2970 GOTO 830
2980 FOR DOWNX=0 TO PC
2990 CTX=TKX(DOWNX)/8
3000 CTY=TKY(DOWNX)/6
3010 ID=DOWNX
3020 GOSUB 2340
3030 NEXT
3040 RETURN
