/* Projektvorlage MOP-Praktikum Versuch 4 */
#include "vscreen.h"
#define SIZE_X                  640
#define SIZE_Y                  480
#define MAX_ITERATION   256
#define MAX_BETRAG              (4*4)

static char* VideoRam;

typedef struct
{
       double Re, Im;
} tComplex;

typedef struct
{   /* RGB-Color components  */
       unsigned char blue, green, red, alpha;
} tColor;

tColor colorList[MAX_ITERATION+1] = {
       { 0x10, 0x00, 0x00 }, { 0x20, 0x00, 0x00 }, { 0x30, 0x00, 0x00 }, { 0x40,
0x00, 0x00 }, { 0x50, 0x00, 0x00 }, { 0x60, 0x00, 0x00 }, { 0x70, 0x00,
0x00 }, { 0x80, 0x00, 0x00 },
       { 0x90, 0x00, 0x00 }, { 0xa0, 0x00, 0x00 }, { 0xb0, 0x00, 0x00 }, { 0xc0,
0x00, 0x00 }, { 0xd0, 0x00, 0x00 }, { 0xe0, 0x00, 0x00 }, { 0xf0, 0x00,
0x00 }, { 0xff, 0x00, 0x00 },
       { 0xff, 0x10, 0x10 }, { 0xff, 0x20, 0x20 }, { 0xff, 0x30, 0x30 }, { 0xff,
0x40, 0x40 }, { 0xff, 0x50, 0x50 }, { 0xff, 0x60, 0x60 }, { 0xff, 0x70,
0x70 }, { 0xff, 0x80, 0x80 },
       { 0xff, 0x90, 0x80 }, { 0xff, 0xa0, 0x80 }, { 0xff, 0xb0, 0x80 }, { 0xff,
0xc0, 0x80 }, { 0xff, 0xd0, 0x80 }, { 0xff, 0xe0, 0x80 }, { 0xff, 0xf0,
0x80 }, { 0xff, 0xff, 0x80 },
       { 0xf0, 0xff, 0x80 }, { 0xe0, 0xff, 0x80 }, { 0xd0, 0xff, 0x80 }, { 0xc0,
0xff, 0x80 }, { 0xb0, 0xff, 0x80 }, { 0xa0, 0xff, 0x80 }, { 0x90, 0xff,
0x80 }, { 0x80, 0xff, 0x80 },
       { 0x80, 0xff, 0x80 }, { 0x70, 0xff, 0x70 }, { 0x60, 0xff, 0x60 }, { 0x50,
0xff, 0x50 }, { 0x40, 0xff, 0x40 }, { 0x30, 0xff, 0x30 }, { 0x20, 0xff,
0x20 }, { 0x10, 0xff, 0x10 },
       { 0x00, 0xf0, 0x00 }, { 0x00, 0xe0, 0x00 }, { 0x00, 0xd0, 0x00 }, { 0x00,
0xc0, 0x00 }, { 0x00, 0xb0, 0x00 }, { 0x00, 0xa0, 0x00 }, { 0x00, 0x90,
0x00 }, { 0x00, 0x80, 0x00 },
       { 0x00, 0x80, 0x10 }, { 0x00, 0x80, 0x20 }, { 0x00, 0x80, 0x30 }, { 0x00,
0x80, 0x40 }, { 0x10, 0x80, 0x50 }, { 0x20, 0x80, 0x60 }, { 0x30, 0x80,
0x70 }, { 0x40, 0x80, 0x80 },
       { 0x40, 0x90, 0x90 }, { 0x50, 0xa0, 0xa0 }, { 0x60, 0xb0, 0xb0 }, { 0x60,
0xc0, 0xc0 }, { 0x70, 0xd0, 0xd0 }, { 0x70, 0xe0, 0xe0 }, { 0x70, 0xf0,
0xf0 }, { 0x70, 0xff, 0xff },
       { 0x80, 0xff, 0xff }, { 0x80, 0xf0, 0xff }, { 0x80, 0xd0, 0xff }, { 0x80,
0xc0, 0xff }, { 0x80, 0xb0, 0xff }, { 0x80, 0xa0, 0xff }, { 0x80, 0x90,
0xff }, { 0x80, 0x80, 0xff },
       { 0x70, 0x70, 0xff }, { 0x60, 0x60, 0xff }, { 0x50, 0x50, 0xff }, { 0x40,
0x40, 0xff }, { 0x30, 0x30, 0xff }, { 0x20, 0x20, 0xff }, { 0x10, 0x10,
0xff }, { 0x00, 0x00, 0xff },
       { 0x00, 0x00, 0xf0 }, { 0x00, 0x00, 0xe0 }, { 0x00, 0x00, 0xd0 }, { 0x00,
0x00, 0xc0 }, { 0x00, 0x00, 0xb0 }, { 0x00, 0x00, 0xa0 }, { 0x00, 0x00,
0x90 }, { 0x00, 0x00, 0x80 },
       { 0x10, 0x00, 0x80 }, { 0x20, 0x00, 0x80 }, { 0x30, 0x00, 0x80 }, { 0x40,
0x00, 0x80 }, { 0x50, 0x00, 0x80 }, { 0x60, 0x00, 0x80 }, { 0x70, 0x00,
0x80 }, { 0x80, 0x00, 0x80 },
       { 0x80, 0x10, 0x80 }, { 0x90, 0x10, 0x80 }, { 0xa0, 0x10, 0x90 }, { 0xb0,
0x10, 0xa0 }, { 0xc0, 0x10, 0xa0 }, { 0xd0, 0x10, 0xb0 }, { 0xe0, 0x20,
0xc0 }, { 0xf0, 0x20, 0xd0 },
       { 0xff, 0x20, 0xf0 }, { 0xff, 0x30, 0xf0 }, { 0xff, 0x40, 0xf0 }, { 0xff,
0x50, 0xe0 }, { 0xff, 0x60, 0xd0 }, { 0xff, 0x70, 0xc0 }, { 0xff, 0x80,
0xb0 }, { 0xff, 0x80, 0xa0 },
       { 0xff, 0x80, 0x90 }, { 0xff, 0x90, 0x80 }, { 0xff, 0xa0, 0x80 }, { 0xff,
0xb0, 0x80 }, { 0xff, 0xc0, 0x80 }, { 0xff, 0xd0, 0x80 }, { 0xff, 0xe0,
0x80 }, { 0xff, 0xf0, 0x80 },
       { 0xff, 0xff, 0x80 }, { 0xf0, 0xf0, 0x70 }, { 0xe0, 0xe0, 0x60 }, { 0xd0,
0xd0, 0x50 }, { 0xc0, 0xc0, 0x40 }, { 0xb0, 0xb0, 0x30 }, { 0xa0, 0xa0,
0x20 }, { 0x90, 0x90, 0x10 },
       { 0x80, 0x80, 0x00 }, { 0x90, 0x70, 0x00 }, { 0xa0, 0x70, 0x00 }, { 0xb0,
0x60, 0x00 }, { 0xc0, 0x50, 0x00 }, { 0xd0, 0x40, 0x30 }, { 0xe0, 0x40,
0x10 }, { 0xf0, 0x50, 0x20 },
       { 0xf0, 0x50, 0x30 }, { 0xe0, 0x50, 0x40 }, { 0xd0, 0x60, 0x40 }, { 0xc0,
0x60, 0x50 }, { 0xb0, 0x70, 0x50 }, { 0xa0, 0x70, 0x60 }, { 0x90, 0x70,
0x60 }, { 0x80, 0x80, 0x70 },
       { 0x70, 0x80, 0x80 }, { 0x60, 0x80, 0x80 }, { 0x50, 0x80, 0x80 }, { 0x50,
0x90, 0x90 }, { 0x55, 0xa0, 0xa0 }, { 0x55, 0xb0, 0xb0 }, { 0x55, 0xc0,
0xc0 }, { 0x55, 0xd0, 0xd0 },
       { 0x55, 0xe0, 0xe0 }, { 0x55, 0xf0, 0xf0 }, { 0x55, 0xff, 0xff }, { 0x60,
0xf0, 0xf0 }, { 0x70, 0xe0, 0xe0 }, { 0x80, 0xd0, 0xd0 }, { 0x90, 0xc0,
0xc0 }, { 0xa0, 0xb0, 0xb0 },
       { 0xa0, 0xa0, 0xa0 }, { 0x90, 0x90, 0x90 }, { 0x80, 0x80, 0x80 }, { 0x70,
0x70, 0x70 }, { 0x60, 0x60, 0x60 }, { 0x50, 0x50, 0x50 }, { 0x40, 0x40,
0x40 }, { 0x30, 0x30, 0x30 },
       { 0xa0, 0xa0, 0xa0 }, { 0x90, 0x90, 0x90 }, { 0x80, 0x80, 0x80 }, { 0x70,
0x70, 0x70 }, { 0x60, 0x60, 0x60 }, { 0x50, 0x50, 0x50 }, { 0x40, 0x40,
0x40 }, { 0x30, 0x30, 0x30 },
       { 0x30, 0x40, 0x30 }, { 0x30, 0x50, 0x30 }, { 0x35, 0x60, 0x35 }, { 0x40,
0x70, 0x40 }, { 0x45, 0x80, 0x45 }, { 0x50, 0x90, 0x50 }, { 0x55, 0xa0,
0x55 }, { 0x60, 0xa0, 0x55 },
       { 0x65, 0x90, 0x55 }, { 0x70, 0x80, 0x55 }, { 0x80, 0x80, 0x55 }, { 0x90,
0x80, 0x55 }, { 0xa0, 0x80, 0x55 }, { 0xb0, 0x80, 0x55 }, { 0xc0, 0x80,
0x55 }, { 0xd0, 0x80, 0x55 },
       { 0xd0, 0x80, 0x65 }, { 0xd0, 0x75, 0x75 }, { 0xd0, 0x70, 0x85 }, { 0xd0,
0x65, 0x95 }, { 0xd0, 0x60, 0xa5 }, { 0xd0, 0x55, 0xb5 }, { 0xd0, 0x55,
0xc5 }, { 0xd0, 0x55, 0xd5 },
       { 0xd0, 0x55, 0xe5 }, { 0xd0, 0x55, 0xf5 }, { 0xd0, 0x65, 0xff }, { 0xc0,
0x75, 0xff }, { 0xa0, 0x85, 0xff }, { 0x95, 0x95, 0xff }, { 0xb5, 0xb5,
0xff }, { 0xc5, 0xc5, 0xff },
       { 0xc5, 0xd5, 0xff }, { 0xc0, 0xe5, 0xff }, { 0xb5, 0xf5, 0xff }, { 0xb0,
0xf0, 0xf8 }, { 0xa5, 0xe5, 0xf0 }, { 0x95, 0xe0, 0xe8 }, { 0x85, 0xd8,
0xe0 }, { 0x75, 0xd0, 0xd8 },
       { 0x70, 0xd0, 0xd0 }, { 0x70, 0xd0, 0xc0 }, { 0x70, 0xd0, 0xb0 }, { 0x70,
0xd0, 0xa0 }, { 0x70, 0xd0, 0x90 }, { 0x70, 0xd0, 0x80 }, { 0x70, 0xd0,
0x70 }, { 0x70, 0xd0, 0x60 },
       { 0x80, 0xd0, 0x60 }, { 0x90, 0xd0, 0x60 }, { 0xa0, 0xd0, 0x60 }, { 0xb0,
0xd0, 0x60 }, { 0xc0, 0xd0, 0x60 }, { 0xd0, 0xd0, 0x60 }, { 0xe0, 0xd0,
0x60 }, { 0xf0, 0xd0, 0x60 },
       { 0xf0, 0xd0, 0x70 }, { 0xf0, 0xd0, 0x80 }, { 0xf0, 0xd0, 0x90 }, { 0xf0,
0xd0, 0xa0 }, { 0xf0, 0xd0, 0xb0 }, { 0xf0, 0xd0, 0xc0 }, { 0xf0, 0xc8,
0xd0 }, { 0xe8, 0xc0, 0xe0 },
       { 0xe0, 0xb8, 0xe0 }, { 0xd0, 0xb0, 0xe0 }, { 0xc0, 0xa0, 0xe0 }, { 0xb0,
0x90, 0xd8 }, { 0xa0, 0x80, 0xd0 }, { 0x90, 0x70, 0xc8 }, { 0x80, 0x60,
0xc0 }, { 0x70, 0x50, 0xb8 },
       { 0x00, 0x00, 0x00 },
       };

void PutPixel(int x, int y, tColor c){
       tColor *ptr = (tColor*) VideoRam;
       ptr = ptr+x+(y*SIZE_X);
       *ptr = c;
}

void FillScreen(tColor c){
       int y = 0;
       int x = 0;
       for (y=0; y<SIZE_Y; y++){
               for (x=0; x<SIZE_X; x++){
                       PutPixel(x,y,c);
               }
       }
}
double Betrag (tComplex c){
       return((c.Im*c.Im)+(c.Re*c.Re));
}


int Iteration (tComplex c){
       tComplex zAlt = {0, 0};
       tComplex zNeu = {0, 0};
       int i = 0;
       for (i = 0; i < MAX_ITERATION; ++i ){
               if (Betrag(zAlt) > (MAX_BETRAG)){
                       break;
               }
               zNeu.Re = zAlt.Re*zAlt.Re - (zAlt.Im*zAlt.Im) + c.Re;
               zNeu.Im = 2*zAlt.Re*zAlt.Im + c.Im;
               zAlt = zNeu;
       } return i;
}


void Mandelbrot(tComplex c){
       int y = 0;
       int x = 0;
       double RE = c.Re;
       for (y=0; y<SIZE_Y; y++){
               c.Re = RE;
               c.Im = c.Im + 0.004;
               for (x=0; x<SIZE_X; x++){
                       c.Re = c.Re + 0.004;
                       PutPixel (x, y, colorList[Iteration(c)]);
               }
       }
}

int main(void)
{
       tColor deepBlue = {0x80, 0x0F, 0x17};
       tComplex c = {-2.0, -1.0};
       int i = 0;
       VOpenGraphics("Mandelbrot", 1, SIZE_X, SIZE_Y);
       VideoRam = VGetGraphicsPointer();

       VClear();
       //FillScreen(deepBlue);
       //Iteration (c);
       Mandelbrot (c);
       printf ("%d", Iteration (c));

       VWait();
       return 0;
}
