#define _CRT_SECURE_NO_DEPRECATE
#include <stdio.h>
#include <math.h>

int DegToRad(int deg);
int Sin100(int rad);

void main(void){
        int deg = 0;
        int rad = 0;
        while (0 != 1){
                fflush(stdin);
                printf("Bitte einen Winkel in Grad eingeben: ");
                scanf("%d",&deg);
                if (deg>360 || deg<-360)
                        printf("Fehler: Wert muss zw. 180\xf8 und -180\xf8 liegen!\n");
                else {
                        if (deg>=0){
                                printf("Ein Winkel von %d\xf8 entspricht %d.%02d rad\n",deg,abs(DegToRad(deg)/100),abs(DegToRad(deg)%100));
                                printf("Der Sinus von %d\xf8 ist %d.%02d\n",deg,Sin100(DegToRad(deg))/100,(abs(Sin100(DegToRad(deg))))%100);
                        } else {
                                printf("Ein Winkel von %d\xf8 entspricht -%d.%02d rad\n",deg,abs(DegToRad(deg)/100),abs(DegToRad(deg)%100));
                                printf("Der Sinus von %d\xf8 ist -%d.%02d\n",deg,Sin100(DegToRad(deg))/100,(abs(Sin100(DegToRad(deg))))%100);
                        }
                }
        }
        return;
}

int DegToRad(int deg)
{
        //maximale Anzahl an Multiplikationen ohne Überlauf
        return ((deg*314+90)/180);
}

int Sin100(int rad)
{
        return
((rad)-(rad*rad*rad)/60000+(rad*rad*rad)/100000*(rad*rad)/120000-(rad*rad*rad)/50400*(rad*rad)/100000*(rad*rad)/1000000);
}

