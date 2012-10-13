/* erstes Programm, die Reihe */

# define _CRT_SECURE_NO_DEPRECATE
# define PI  314
# include <stdio.h>
# include <stdlib.h>


int DegToRad (int deg);

void main (void) {
	/* mein Quelltext*/
	int deg;
	int vkrad;
	int narad;
	int sinvk;
	int sinnk;
	
	while('a'){
	fflush(stdin);
	printf("Bitte geben Sie ein Winkel in Grad ein: ");
	scanf("%d", &deg);
	//printf("%d", deg);			//test
	vkrad = (DegToRad(deg)/100);
	narad = (DegToRad(deg) % 100);
	printf("%d\xF8 in rad: %d.%d rad \n",deg, vkrad, narad);
	}
	return;
}


int DegToRad (int deg){
	int rad;
	rad = deg * PI /180;
	return rad;
}