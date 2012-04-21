#define _CRT_SECURE_NO_DEPRECATE
#define PI 314
#define MAX_NEG_EINGABE -180
#define MAX_POS_EINGABE 180
#include <stdio.h>
#include <vscreen.h>
int DegToRad(int deg);
int Sin100(int crad);

void main(void)
{
     int crad, deg, vorne, sinus;
     char v;
	for (;;)
	{
		fflush(stdin);

		printf("--------------------------------------------------\n"
			"      Bitte einen Winkel in Grad eingeben:\n\n"
			"Der Winkel muss zwischen %d und %d liegen\n\n"
				"Eingabe:   ",MAX_NEG_EINGABE,MAX_POS_EINGABE);
		scanf("%d", &deg);
		if (deg > MAX_POS_EINGABE || deg < MAX_NEG_EINGABE)
			 printf("\nFehler: Eingabe ist Ungueltig!\n");
		else
		{
			crad = DegToRad (deg);
			sinus = abs(Sin100(crad));
			vorne = (crad/100);
			if (deg <= 0)
			{
				 v = '-';
			}
			else
			{
				 v = ' ';
			}
			printf("\nEin Winkel von %4d\xF8  entspricht ungefaehr \x7E %c%02d,%02d rad\n"
				"Der Sinus  von %4d\xF8  ist %c%02d,%02d.\n\n",deg,v,vorne,crad % 100,deg,v,sinus/100,sinus%100);
		}
	}
}

int DegToRad(int deg)
{
     int crad ;
     crad = (abs(deg)*PI +90)/180;
     return crad;
}
	int Sin100(int crad)
	{
		 int sinus;
		 sinus = crad
			 - ((crad*crad*crad)/60000)
			 + ((((crad*crad/100)*(crad/10)*(crad*crad/100)))/120000)
			 - ((((crad*crad/10000)*(crad/10)*(crad*crad/10000)*(crad*crad/10000)))/504);
		 return sinus;
	}
