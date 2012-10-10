#define _CRT_SECURE_NO_WARNINGS
#include<stdio.h>
#include<string.h>

void Ticker(char *s, int len)
{
     int i;
     char tausch;

     if (s[0] != '\0')
     {
         for(i = 0; s[i+1] != '\0' ; i++)
         {
             tausch = s[i];
             s[i] = s[i+1];
             s[i+1]=tausch;
         }
         printf("%79s\r", s);
     }
}

void __stdcall Sleep(long ms);

void main(void)
{
     int i = 0;
     int j = 0;
     char s1[4] = "Max";
     char s2[1] = "";
     char name[80] = "";

     //Ticker(s1,(int) strlen(s1));
     //Ticker(s2,(int) strlen(s2));

     printf("Bitte Namen eingeben: ");
     fflush(stdin);
     scanf_s("%79[^\n]s", name, sizeof(name));

     for(j=strlen(name);j<=78;j++)
     {
         strcat(name," "); //auffüllen mit Leerzeichen
     }

     for(i=0;i<=30;i++)
     {

         Ticker(name,(int) strlen(name));
         Sleep(60);
     }
}


