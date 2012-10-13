/* Projektvorlage MOP-Praktikum Versuch 3 - Übung 2 */ 
#define _CRT_SECURE_NO_WARNINGS
#define STR_SIZE 256
#define SSTR_SIZE 32
#include <stdio.h>
#include <string.h>
#include<stdlib.h>


void strreplace(char* dest, const char* src, const char* find, const char* replace);


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

void strreplace(char* dest, const char* src, const char* find, const char* replace)
{

	char *searchResult;

	dest[0]= 0;
	searchResult = strstr(src, find);
	if(searchResult == NULL)
	{
		printf("Das was du suchst ist ueberhaupt nicht vorhanden");
		return;
	}

	while (searchResult!=NULL)
	{
		strncpy(dest, src, searchResult-src);
		dest += searchResult-src;
		src = searchResult;
		strcpy(dest,replace);
		dest += strlen(replace);
		src += strlen(find);

		searchResult = strstr(src, find);

	}
	
	strcpy(dest, src);
		

}
	


	

	

	
		


	















