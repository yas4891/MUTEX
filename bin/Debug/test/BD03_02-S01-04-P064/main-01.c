#define _ CRT_SECURE_NO_WARNINGS
#include <stdio.h>
#include <string.h>
void _stdcall Sleep(long ms);


void Ticker(char *s, int len); 

void main (void){
	int i = 1;
	int j = 0;
	char name[80] = "";
	char s1[4] = "Max";
	char s2[1] = "";

	while(i = 1)
	{
	printf("Bitte einen Namen eingeben: \n");
	fflush(stdin);
	scanf("%79[^\n]s", name);
	if (strlen(name)== 0)
		printf("Fehler\n\n");
	else
	{
		for (j=strlen(name);j<=78;j++)
		{
			strcat(name," ");
		}
	

	//Ticker(s1, strlen(s1));
	//Ticker(s2, strlen(s2));


		for(j=0; j <= 100; j++)
			{
			Ticker(name, strlen(name));
			
			printf("%4s\r",name);
			Sleep(50);

			}

	}
   /*printf("s1 ist >	%4s < \n" , s1);
   printf("s2 ist > %4s < \n" , s2);*/
   

   


	}
}


void Ticker(char *s, int len){

	int j = 0;
	int counter = 0;
	if (len > 1)
	{

		

			int i = 0;
			char t;

			for(i;i<=77;i++)
			{
				t = s[i];
				s[i] = s[i+1];
				s[i+1] = t;
			}
			
		
	}
	else return;		
		
}










	//char sp1;
	//char sp2;
	//char sp3;


	// sp1 = s[len-1]	;  /*Platz 3 wird gespeichert = x*/
	// sp2 = s[len-2] ;  /*Platz 2 wird gespeichert = a*/				 
	// sp3 = s[len-3] ;  /*Platz 1 wird gespeichert = m*/
	




 //    s[0] = sp2;
	// s[1] = sp1; 
	// s[2] = sp3; 
	// 
	// 
	//char sp0 = s[0];
	//char sp1 = s[1];
	//char sp2 = s[2];
	//char sp3 = s[3];
	//char sp4 = s[4];
	//char sp5 = s[5];
	//char sp6 = s[6];
	//char sp7 = s[7];
	//char sp8 = s[8];
	//char sp9 = s[9];
	//char sp10 = s[10];
	//char sp11 = s[11];
	//char sp12 = s[12];
	//