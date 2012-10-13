#define _CRT_SECURE_NO_DEPRECATE
#include <stdio.h>
#include <stdlib.h>
#include <math.h>


int DegToRad(int deg)
{
int crad =  0;
if (deg == 0)
	return 0;
else 
crad = (deg * 314 + deg/abs(deg) * 90) /180;
return crad;
}


int Sin100(int rad)
{
int x =0;
 x = rad -(rad*rad* rad/60000) + (rad *rad*rad/60000 *rad*rad/200000)- (rad*rad*rad/50400 * rad*rad /100000 *rad*rad/1000000);
return x;
}


void main(void)
{
int winkeleingabe = 0;
int winkel = 0;
int vorkomma = 0;
int nachkomma = 0;


     while(1 != 0)
     {

         printf("Bitte einen Winkel eingeben: ");
         fflush(stdin);
         scanf("%d", &winkeleingabe);

         winkel = DegToRad(winkeleingabe);
         vorkomma = winkel / 100;
         nachkomma = winkel % 100;


         if (winkeleingabe < 0)
             printf("%d%c = %c%d.%02d rad\n",  winkeleingabe, 0xf8, 0x2D , abs(vorkomma), abs(nachkomma));
         else
             printf("%d%c = %d.%02d rad\n",  winkeleingabe, 0xf8, abs(vorkomma), abs(nachkomma));

         if(winkeleingabe < -180 || winkeleingabe > 180)printf("Wert fuer sinus-Berechnung ungueltig! \nBitte einen Wert zwischen -180 und 180 eingeben \n");
         else
         {
             int x = Sin100(winkel);
             printf("sinus %d = %d.%02d \n",  winkeleingabe, x/100, abs(x%100));
         }
     }


return;
}