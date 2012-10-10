/* Praktikum Maschinenorientiertes Programmiern, Vorlage Versuch 7, D. Pawelczak */ 
#include "vscreen.h"
#include <time.h>
#include <stdlib.h>
#define SIZE_X 640
#define SIZE_Y 480


typedef struct List
{
   double x;                // current X-position
   double y;                // current Y-position
   double dx;                // delta increment of X
   double dy;                // delta increment of Y
   int count;                // number of animation steps
   struct List *next;        // successor (single linked list)
   struct List *children;    // child list or NULL
} tList;

tList *Basis = NULL;
int Animate(tList *star, int createChild);
void AddElement(tList* newElement);
void UpdateList(tList* idx, int createChild);



tList* CreateStar(tList* next)
{
   tList* dZeiger;
   dZeiger = malloc (sizeof(tList));
   dZeiger->x = SIZE_X/2;
   dZeiger->y = SIZE_Y/2;
   dZeiger->dx = ((double)rand()/ RAND_MAX * 2.0 - 1);
   dZeiger->dy = ((double)rand()/ RAND_MAX * 2.0 - 1);
   dZeiger->next = next;
   dZeiger->children = NULL;
   dZeiger->count = 0;

   if(V_ERROR == 1)
   {
       return NULL;
   }
   else
   {
       return dZeiger;
   }
}

void AddElement(tList* newElement)
{
tList* idx = Basis;

if (Basis == NULL)
   Basis = newElement;
else
   {
       while (idx->next)
       {
       idx=idx->next;
       }
idx->next=newElement;
   }


}


int Animate(tList *star, int createChild){
  VSetPixel((int)star->x,(int)star->y,vBlack);
  star->x=star->x+star->dx;
  star->y=star->y+star->dy;
  if(star->x>= SIZE_X + 100 || star->y>= SIZE_Y + 100||star->y<=
-100||star->x<= -100  ){ return 1;}
  VSetPixel((int)star->x,(int)star->y,vWhite);
  (star->count)++;
  if((star->count) % 40 == 0 && createChild == 1){
      tList* copy = CreateStar(NULL);
      tList* mem = NULL;
      if((star->count)/40>1){
          mem=star->children;
      }
      copy->count=star->count;
      copy->x=star->x;
      copy->y=star->y;
      copy->dx=star->dx;
      copy->dy=star->dy;
      copy->children=NULL;
      copy->next=mem;
      star->children=copy;
      star->x=star->x+star->dx;
      star->y=star->y+star->dy;
  }else{
      UpdateList(star->children,0);
  }

  return 0;
}


void DeleteList(void){
  tList *akut =Basis;
while(akut){
  tList *ne = akut;
  tList *now = akut->children;
  while(now){
      tList *net = now;
      now=now->next;
      free(net);
  }
  akut=akut->next;
  free(ne);
}
Basis= NULL;
}


void TestAnimate(void)
{
tList* star = CreateStar(NULL);
while( Animate(star, 1) == 0)
VFlushGraphics();
}


void UpdateList(tList* idx, int createChild)
{
   while(idx!= NULL){
  Animate(idx,createChild);
  idx=idx->next;
  }


}


void main(void)
{
   int i = 0;

   VOpenGraphics("Screen", 0, SIZE_X, SIZE_Y);
   VClear();
   //for(i; i<10; i++)
   //{
   //AddElement(CreateStar(NULL));
   //}

//TestAnimate();

 while(VGetMouseButton()!=2)
 {
      AddElement(CreateStar(NULL));
      UpdateList(Basis,1);
      VFlushGraphics();
  }

 DeleteList();
#ifdef _DEBUG
_CrtDumpMemoryLeaks();
#endif


}