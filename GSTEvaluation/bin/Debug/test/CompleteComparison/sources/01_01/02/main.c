#define _CRT_SECURE_NO_DEPRECATE
  #define _USE_MATH_DEFINES
  #include <math.h>
  #include <stdio.h>

  int DegToRad(int deg);
  //int crad;
  int i;
  int Sin100(int rad);
  //int sin;
  void main(void)
  {
       int deg;
       int i=1;
			while (i<2)
       {
       printf("Bitte einen Winkel in Grad eingeben:\n");
       scanf("%d", &deg);
			if ((deg<-180) || (deg>180))
       {
       printf("Der Winkel muss zwischen -180 und 180 liegen!\n");
       fflush(stdin);
       }
			else
       {
			if (deg<0)
				printf("Ein Winkel von %d\xf8 entspricht -%-i,%02i rad\n", deg,
				abs(DegToRad(deg)/100), abs(DegToRad(deg)%100) );
			if (deg>=0)
				printf("Ein Winkel von %d\xf8 entspricht %-i,%02i rad\n", deg,
				abs(DegToRad(deg)/100), abs(DegToRad(deg)%100) );
       //printf("Test %d \n", Sin100(DegToRad(deg)));
			if (deg<0)
				printf("Der Sinus von %d\xf8 ist -%d.%02d\n", deg,
				abs(Sin100(DegToRad(deg))/100),(abs(Sin100(DegToRad(deg))))%100);
			if (deg>=0)
				printf("Der Sinus von %d\xf8 ist %d.%02d\n", deg,
				Sin100(DegToRad(deg))/100,(abs(Sin100(DegToRad(deg))))%100);
          //printf("Der Sinus von %d\xf8 ist %-i,%02i\n", deg,
				abs(Sin100(DegToRad(deg))/100), abs(Sin100(DegToRad(deg))%100);
       //printf("Die Loesung ist: %-i,%02i rad\n",
				abs(DegToRad(deg)/100), abs(DegToRad(deg)%100);
       }
       }
       return;
  }
  int DegToRad(int deg)
  {

    int crad;
    if (deg>=0)
		crad = (deg*314+90)/180;

	if (deg<0)
		crad = (deg*314-90)/180;

    return crad;
  }
/*int Sin100(int rad)
{
     int sin;
     sin = (rad - rad*rad*rad/60000 +
((rad*rad*rad/100000)*rad*rad)/120000 -
((((rad*rad*rad/50400)*rad*rad)/100000)*rad*rad)/10000000);
     return sin;
}
*/

int Sin100(int rad)
{
	int sin;
		sin =(rad-rad*rad*rad/60000 + ((rad*rad*rad/10000)*rad*rad)/1200000 -
		(rad*rad*rad)/1000000*(rad*rad*rad)/100000*(rad)/50400);

return sin;
}
