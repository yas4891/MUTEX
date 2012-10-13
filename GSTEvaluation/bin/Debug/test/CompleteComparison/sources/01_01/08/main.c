#define _CRT_SECURE_NO_DEPRECATE
#include <stdio.h>
#include <math.h>


	int DegToRad(int deg);
	int Sin100(int rad);
	int Fac( int i);


void main(void)
{
		/* ab hier folgt meine Lösung */

		int n;
		
	while (1)
	{
		printf(	"Gradmass zu Bogenmass Converta \n"
			"Bitte geben sie den Winkel im Gradmass ein : ");
		scanf("%d",&n);

		
		if (n < -180 || n> 180)
			printf("Fehler Winkel zw. (-1)*180 und 180 Grad eingeben \n");
		else	
			
			printf("Das Bogenmass fuer %d Grad betraegt %d , %02d rad \n", n , DegToRad(n)/100 , abs(DegToRad(n)%100) );

		printf("Der Sinus fuer %d , %02d rad betraegt %d , %02d \n"  , DegToRad(n)/100 , abs(DegToRad(n)%100) ,Sin100(DegToRad(n))/100 , abs(Sin100(DegToRad(n))%100) );

		
			
	}

	return;


}

int DegToRad(int deg) 
{

	int rad;

	if (deg>=0)
		rad = (314*deg+90)/180;
	else
		rad = (314*deg-90)/180;


	return rad;
}

int Fac(int i)
{

	int c;
	int f = 1;

	for (c = 1;c <= i;c++)
		f *= c;


	return f;
}
		
int Sin100(int rad)
{
	int s;
	
	s = rad - (rad*rad*rad/60000) + (rad*rad*rad/120000)*rad*rad/100000 - (rad*rad*rad/120000) * rad*rad/420000 * rad/100000;




	return s;
}



	
