#define _CRT_SECURE_NO_DEPRECATE
#define MAX_EINGABE 180
#define _USE_MATH_DEFINES

#include <stdio.h>
#include <math.h>

int DegToRad(int deg);
int Sin100(int rad);

void main(void)
{
	int deg;
	int cradvorkomma;
	int cradnachkomma;
	int sin100vorkomma;
	int sin100nachkomma;

	do

	{
		printf("Bitte einen Winkel in Grad eingeben:\n");

		fflush(stdin);
		scanf("%d",&deg);

		cradvorkomma = abs(DegToRad(deg)/100);
		cradnachkomma = abs(DegToRad(deg)%100);
		sin100vorkomma = abs(DegToRad(deg)/100);
		sin100nachkomma = abs(DegToRad(deg)%100);



		if (deg>0) {

		printf("Der Winkel %d\xF8 betraegt umgerechnet %d,%02d rad \n",deg,cradvorkomma,cradnachkomma);
		printf("Der Sinus dieses rad-Wertes ist: %d,%02d rad \n",sin100vorkomma,sin100nachkomma);

		}
		else {
			printf("Der Winkel %d\xF8 betraegt umgerechnet -%d,%02d rad \n",deg,cradvorkomma,cradnachkomma);
			printf("Der Sinus dieses rad-Wertes ist: -%d,%02d rad \n",sin100vorkomma,sin100nachkomma);
		}

	}
	while (1);


return;
}

int DegToRad (int deg)
{
	int crad;
		
		crad = deg*100*M_PI/180;

		return crad;
}

int Sin100 (int rad) 
{
	int sin100;

	sin100 = rad - ((rad*rad*rad)/60000)+(rad*rad*rad)/60000)*((rad*rad)/200000);

	return sin100;
}

