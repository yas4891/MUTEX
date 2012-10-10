/* Projektvorlage MOP-Praktikum Versuch 3 - Übung 2 */
#define _CRT_SECURE_NO_WARNINGS
#define STR_SIZE 256
#define SSTR_SIZE 32
#include <stdio.h>
#include <string.h>
void strreplace(char* dest, const char* src, const char* find, const char*
replace);

void main(void)
{
		char *s="Fischers Fritz fischt frische Fische,"
				" frische Fische fischt Fischers Fritz.";

		char target[STR_SIZE];
		char toFind[SSTR_SIZE];
		char toReplace[SSTR_SIZE];

		printf("%s\n\nSuche.....:", s);
		scanf("%31s", toFind);
		printf("Ersetze...:");
		scanf("%31s", toReplace);

		if (strstr(toReplace, toFind))
		printf("Das zu ersetzende Wort darf nicht im Suchwort enthalten sein!");

		else
		strreplace(target, s, toFind, toReplace);

		printf("\nNeuer String:\n%s\n", target);
}

void strreplace(char* dest,const char* src, const char* find,const char*
replace)
{
		int i =0;
		const char* z1 = src;
		const char* z2 = src;
		dest [0]=0;

			while ((z1 = strstr (z2,find)) !=0)
			{
				i = z1-z2;
				strncat (dest,z2,i);
				strcat (dest,replace);
				z2 = z1+strlen(find);
			}

		strcat(dest,z2);
}

