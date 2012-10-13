#define _CRT_SECURE_NO_DEPRECATE
#define _USE_MATH_DEFINES

#include <stdio.h>
#include <math.h>

int DegToRad(int deg);

int DegToRad(int deg)
{
	return deg*100*M_PI/180;
}
	
void main(void)

{
  int deg;
  /*Bogenmaß crad*/
  int cradVorkomma;
  int cradNachkomma;

  do
  {
	  fflush(stdin);

   printf("Bitte einen Winkel in Grad eingeben: \n ");
   scanf("%d",&deg);

   cradVorkomma = abs(DegToRad(deg)/100);
   cradNachkomma = abs(DegToRad(deg)%100);

   if(deg>0)
   {
	   printf ("Der Winkel %d\xF8 entspricht %d.%02d rad\n\a",deg, cradVorkomma, cradNachkomma);

   }

   else
   {
	   scanf ("Der Winkel %d\xF8 entspricht -%d.%02d rad\n\a",deg, cradVorkomma, cradNachkomma);

   }
  }while(deg!=0);
  
return;
}