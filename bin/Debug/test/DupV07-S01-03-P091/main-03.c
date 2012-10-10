/* Praktikum Maschinenorientiertes Programmiern, Vorlage Versuch 7, D. Pawelczak */ 
#include "vscreen.h"
#include <time.h>
#include <stdlib.h>
#define SIZE_X 800
#define SIZE_Y 522

typedef struct List
{
	double x;				// current X-position
	double y;				// current Y-position
	double dx;				// delta increment of X
	double dy;				// delta increment of Y
	int count;				// number of animation steps
	struct List *next;		// successor (single linked list)
	struct List *children; 	// child list or NULL
} tList;

tList *Base = NULL;
void UpdateList(tList* initialdx, int createChild);
int Animate(tList *star, int createChild);
void AddElement(tList* newElement);

tList* CreateStar(tList* next)
{
	tList* dPtr;
	dPtr = malloc (sizeof(tList));
	dPtr -> x = SIZE_X/2;
	dPtr -> y = SIZE_Y/2;
	dPtr -> dx = ((double) rand()/ RAND_MAX * 2.0 - 1);
	dPtr -> dy = ((double) rand()/ RAND_MAX * 2.0 - 1);
	dPtr -> next = next;
	dPtr -> children = NULL;
	dPtr -> count = 0;

	if (V_ERROR == 1)
	{
		return NULL;
	}
	else
	{
		return dPtr;
	}
}

void AddElement(tList* newElement)
{
	tList* initialdx = Base;

	if (Base == NULL)
	
		Base = newElement;
	
	else
	{
		while (initialdx -> next)
		{
			initialdx = initialdx->next;
		}
	initialdx -> next = newElement;
	}
}




int Animate(tList *star, int createChild)
{
	VSetPixel((int) star -> x, (int) star -> y, vBlack);
	star -> x = star -> x + star -> dx;
	star -> y = star -> y + star -> dy;
	
	/*if ( star -> x >= SIZE_X + 100 ||
		star -> y >= SIZE_Y + 100  ||
		star -> y <= SIZE_Y - 100  ||
		star -> x <= SIZE_X - 100 )
	{
		return 1;
	}*/


	VSetPixel((int) star -> x, (int) star -> y, vWhite);
	(star -> count) ++;

	if ((star -> count ) % 40 == 0 && createChild == 1)
	{
		tList* copy = CreateStar(NULL);
		tList* mem = NULL;

		if ((star -> count ) / 40 > 1)
		{
			mem = star -> children;

		}

		copy -> count = star -> count ;
		copy -> x = star -> x ;
		copy -> y = star -> y ;
		copy -> dx = star -> dx ;
		copy -> dy = star -> dy ;
		copy -> children = NULL;
		copy -> next = mem;
		star -> children = copy ;
		star -> x = star -> x + star -> dx ;
		star -> y = star -> y + star -> dy ;

	}
	else
	{
		UpdateList(star -> children,0);
	}

	return 0;

}

void DeleteList(void)
{
	tList *aktu = Base;
	
	while (aktu)
	{
		tList *ne = aktu;
		tList *now = aktu  -> children;

		while (now)
		{
			tList *net = now;
			now = now -> next;
			free(net);
		}
		aktu = aktu -> next;
		free (ne);
	}

	Base = NULL;
}

void TestAnimate(void)
{
	tList* star = CreateStar(NULL);
	while ( Animate (star, 1) == 0)
	VFlushGraphics();
}

void UpdateList( tList* initialdx, int createChild)
{
	while (initialdx != NULL)
	{
		Animate(initialdx,createChild);
		initialdx = initialdx -> next;
	}
}

void main(void)
{
	
	
	//int i;

	VOpenGraphics("Screen", 0, SIZE_X, SIZE_Y);
	VClear();
	/*for ( i = 0 ; i < 10 ; i++)
	{
		AddElement(CreateStar(NULL));
	}*/

	//TestAnimate();
	
	while (VGetMouseButton() != 2)
	{
		AddElement(CreateStar(NULL));
		UpdateList(Base,1);
		VFlushGraphics();
	}

	DeleteList();


}





