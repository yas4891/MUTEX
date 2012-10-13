#define _CRT_SECURE_NO_DEPRECATE

#include <stdio.h>
#include <math.h>

int DegToRad(int deg) {

	if(deg < 0){
		return ((deg * 314 - 90) / 180);
	}else{
		return ((deg * 314 + 90) / 180);
	}
}

int Sin100(int rad) {

return rad	- rad*rad*rad / 60000 + rad*rad /24000 * rad*rad*rad /500000 - rad*rad*rad / 500000 * rad*rad*rad / 500000 *rad / 25500;
}

void main(void) {

	int deg = 0;
	int rad = 0;
	int sinus = 0;

	while (1) {
		while (1) {
		printf("Bitte einen Winkel in Grad eingeben: ");
		scanf("%d", &deg);
			if (deg > 180 || deg <-180){
			printf("Eingabe falsch\n");
			}
			else{
				break;
			}
		}
	rad = DegToRad(deg);
	sinus = Sin100(abs(rad));

	if(rad < 0){
		printf("Ein Winkel von %d%c entspricht -%d,%02d rad \n", deg, 0xF8, abs(rad / 100), abs(rad % 100));
		printf("Der Sinus von %d%c entspricht -%d,%02d\n", deg, 0xF8, sinus / 100, abs(sinus % 100));

	}else{

		printf("Ein Winkel von %d%c entspricht %d,%02d rad \n", deg, 0xF8, rad / 100, abs(rad % 100));
		printf("Der Sinus von %d%c entspricht %d,%02d\n", deg, 0xF8, sinus / 100, abs(sinus % 100));

	}
}
return;

}