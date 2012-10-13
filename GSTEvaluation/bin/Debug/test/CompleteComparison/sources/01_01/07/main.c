#define _CRT_SECURE_NO_DEPRECATE
#include <stdio.h>
int DegToRad(int deg){
    if(deg>0)
        return ((deg*314+180/2))/180;
    else
        return ((deg*314-180/2))/180;
}
int sin100(int rad){
    return
(rad-(rad*rad*rad/60000)+(rad*rad*rad/1200000*rad*rad/10000)-(rad*rad*rad/5040000*rad*rad/100000*rad*rad/10000));
}

void main(void){
    int winkel=0;
   
    while(1){
        printf("Bitte einen Winkel in Grad eingeben: ");
        scanf("%d",&winkel);
        if(-180<winkel<180){
            if((DegToRad(winkel)/100)==0)
                printf("%d\xF8 - -%d.%.2drad \n", winkel, DegToRad(winkel)/100, abs(DegToRad(winkel)%100));
            else
                printf("%d\xF8 - %d.%.2drad \n", winkel, DegToRad(winkel)/100, abs(DegToRad(winkel)%100));
            if(winkel<0)
                printf("sin(%d\xF8) - -%d.%.2d \n",winkel, sin100(DegToRad(winkel))/100, abs(sin100((DegToRad(winkel)))%100));
            else
                printf("sin(%d\xF8) - %d.%.2d \n",winkel, sin100(DegToRad(winkel))/100, abs(sin100((DegToRad(winkel)))%100));   
        }
        else
            printf("falsche Eingabe zwischen -180 und 180\n");
    }
    return;
}
