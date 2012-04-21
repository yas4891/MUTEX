#define _CRT_SECURE_NO_DEPRECATE
#include <stdio.h>
#include <math.h>


int DegToRad(int deg);
int Sin100(int rad);


void main(void)
{
	/* ab hier folgt Ihre Lösung */


	for(;;){
	int deg;
	char vorz;
	fflush(stdin);
	printf("Bitte einen Winkel in Grad eingeben: \n");
	scanf ("%d", &deg);
	if (deg<=0) 
	{
		vorz= '-';
	}
	else (deg>0);
	{	
		vorz= '+';
	}
	if (deg >=0)
	printf ("Das Bogenmass von %d%c ist %d,%02d rad \n", deg,0xF8, DegToRad(deg)/100, DegToRad(deg)%100);
	else (printf("Das Bogenmass von %d%c ist %d,%02d rad \n", deg, 0xF8, DegToRad(deg)/100, abs (DegToRad(deg)%100)));

	if ((deg<-180) || (deg >180)){
		printf("Winkel muss zwischen -100 \xF8 und 100 \xF8 liegen! ");
		printf ("Das Bogenmass von %d%c ist %c%d,%02d rad \n", deg,0xF8,vorz, DegToRad(deg)/100, DegToRad(deg)%100);
	}else {
		(printf ("Der Sinus von %d%c ist %c%d, %02d \n", deg,0xF8, vorz, Sin100(DegToRad(deg))/100, abs (Sin100(DegToRad(deg))%100)));
			(printf("Das Bogenmass von %d%c ist %c%d,%02d rad \n", deg, 0xF8,vorz, DegToRad(deg)/100, abs (DegToRad(deg)%100)));
	}
}
}
int DegToRad (int deg){
return deg*100*3.14/180;
}
int Sin100(int rad)
{
	int sinus;
		sinus = rad
		-(rad*rad*rad)/60000
		+(((((rad*rad*rad)/10000)*rad)/1000)*rad)/1200
		-(((((((((rad*rad*rad)/10000)*rad)/1000)*rad)/1000)*rad)/1000)*rad)/504;

	return sinus;
}