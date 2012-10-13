#define _CRT_SECURE_NO_DEPRECATE
#define _USE_MATH_DEFINES
#define MAX_EINGABE 180
//#define PIDURCH180 0.0174532925199432975
#include <stdio.h>
#include <math.h> 
#include <stdlib.h>
//4200000; 200000; 60000
int DegToRad(int deg);
int Sin100(int rad);

void main(void) 
{ 
	
	
	int deg;
    int cradvk;
    int cradnk;
    int Sin100vk;
    int Sin100nk;


   do
	{
		printf("Bitte ein Winkel in Grad eingeben:\n");
		
		fflush(stdin);
		
		scanf("%d",&deg);
				
			if (deg == 666)
				printf("Ende!\n \n");
				
			else
				if (deg >= 181)
					printf("Falsche Eingabe!\n \n");
				else{
				 	
				    if (deg <= -181)
						printf("Falsche Eingabe!\n \n");

				
					else{
							cradvk = DegToRad(deg)/100;
							cradnk = DegToRad(deg)%100;
							Sin100vk = Sin100(DegToRad(deg))/100;
							Sin100nk = Sin100(DegToRad(deg))%100;

							if (Sin100nk < 0)
							{	
								printf("Der Winkel %d\xF8 betraegt umgerechnet -%ld.%ld rad \n",deg, abs(cradvk), abs(cradnk));
								printf("Die Sinusfunktion dieses rad Wertes ist: -%ld.%02ld \n \n \n \n ",Sin100vk, abs(Sin100nk));
							}
							else
									if (cradnk < 0)
							{	
								printf("Der Winkel %d\xF8 betraegt umgerechnet %ld.%ld rad \n",deg, cradvk, abs(cradnk));
								printf("Die Sinusfunktion dieses rad Wertes ist: %ld.%02ld \n \n \n \n ",Sin100vk, Sin100nk);
							}
									else
							{
									printf("Der Winkel %d\xF8 betraegt umgerechnet %ld.%ld rad \n",deg, cradvk, abs(cradnk));
									printf("Die Sinusfunktion dieses rad Wertes ist: %ld.%02ld \n \n \n \n ",Sin100vk, Sin100nk);

							}
							
	

							}
				}
				
				
				

	

    
	
	}while (deg != 666);
	return;
}

	int DegToRad(int deg)
	{
		
		
		int crad;
		
		crad = deg*314/180;
		
		
		return crad;
	}

	int Sin100(int rad)
	{
		int sin100;
		sin100 = rad 
			- ((rad*rad*rad)/60000) 
			+ ((((((rad*rad*rad)/10000)*rad)/1000)*rad)/1200) 
			- ((((((((rad*rad*rad)/10000)*rad)/100)*rad)/100*rad)/100)*rad)/504000;
			
	return sin100;
	}

 