#include <stdio.h>
#include <string.h>

void Ticker (char *s, int len);
void _stdcall Sleep (long ms);




void Ticker (char *s, int len)
{

int i;


char speicher[2];

if (s[0] == '\0')
	
	return;

speicher[0] = s[0];

for (i=0;(i+1)<len; i++)
{
	s[i]=s[i+1];

}

s[i] = speicher[0];

	return;
}


void main (void)
{
	int i;
	int z;
	char name[79];
	char s1[4]="Max";
	char s2[1]="";
	printf("Bitte geben Sie Ihren Namen ein:");
	scanf("%78[^\n]s", name);

	for (i=strlen(name);i<=79;i++)
	{
		strcat(name," ");
	}

	for (z=0; z<500; z++);
	{
		Ticker (name, (int)strlen(name));
		printf("%s\r",name);

	}
	Ticker (s1, (int)strlen(s1));
	Ticker (s2, (int)strlen(s2));
	Sleep (50);

}