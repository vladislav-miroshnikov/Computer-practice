#define _CRT_SECURE_NO_WARNINGS
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <Windows.h>
#include < math.h >
#include <time.h>
#define PI 3.141592

typedef unsigned short us;
typedef unsigned int ui;
typedef unsigned char uc;
typedef unsigned long ul;

//explained in Windows.h

//typedef struct BITMAPFILEHEADER
//{
//	us bfType;
//	ui bfSize;
//	us bfReserved1;
//	us bfReserved2;
//	ui bfOffBits;
//}BITMAPFILEHEADER;
//
//typedef struct BITMAPINFOHEADER 
//{
//	ui biSize;
//	ui biWidth;
//	ui biHeight;
//	us biPlanes;
//	us biBitCount;
//	ui biCompression;
//	ui biSizeImage;
//	ui biXPelsPerMeter;
//	ui biYPelsPerMeter;
//	ui biClrUsed;
//	ui biClrImportant;
//}BITMAPINFOHEADER;

//palette of image
typedef struct RGB
{
	uc rgbBlue;
	uc rgbGreen;
	uc rgbRed;
}RGB;

RGB** processingBmp(BITMAPFILEHEADER* bmpHeader, BITMAPINFOHEADER* bmpInfo, FILE* input, int* vect, char** palette, ul *paletteSize)
{
	fread(bmpHeader, sizeof(BITMAPFILEHEADER), 1, input);
	fread(bmpInfo, sizeof(BITMAPINFOHEADER), 1, input);
	if (bmpInfo->biBitCount != 24 && bmpInfo->biBitCount != 32)
	{
		printf("wrong picture format, try again");
		exit(-1);
	}

	ul image = bmpHeader->bfOffBits;
	*paletteSize = image - sizeof(BITMAPFILEHEADER) - sizeof(BITMAPINFOHEADER);
	if (*paletteSize)
	{
		*palette = malloc(paletteSize);	
		fread(*palette, *paletteSize, 1, input);
	}
	RGB** pixel = calloc(sizeof(RGB*), bmpInfo->biHeight);
	for (int i = 0; i < bmpInfo->biHeight; i++)
	{
		pixel[i] = calloc(sizeof(RGB*), bmpInfo->biWidth);
	}

	*vect = (4 - (bmpInfo->biWidth * (bmpInfo->biBitCount / 8)) % 4) & 3;
	for (int i = 0; i < bmpInfo->biHeight; i++)
	{
		for (int j = 0; j < bmpInfo->biWidth; j++)
		{
			pixel[i][j].rgbBlue = (uc)getc(input);
			pixel[i][j].rgbGreen = (uc)getc(input);
			pixel[i][j].rgbRed = (uc)getc(input);
			if (bmpInfo->biBitCount == 32)
			{
				getc(input);
			}
		}
		for (int k = 0; k < *vect; k++)
		{
			getc(input);
		}
	}

	fclose(input);
	if (pixel != NULL)
	{
		return pixel;
	}
	else
	{
		printf("error reading file, try again");
		exit(-1);
	}
}

//function of copy rgb

RGB** copyBmp(RGB** rgb, int height, int width)
{
	RGB** newRgb = calloc(sizeof(RGB*), height);
	for (int i = 0; i < height; i++)
	{
		newRgb[i] = calloc(sizeof(RGB*), width);
		memcpy(newRgb[i], rgb[i], sizeof(RGB) * width);
	}
	return newRgb;
}

//function of record bmp into new file
void record(RGB** newRgb, int vect, BITMAPFILEHEADER* bmpHeader, BITMAPINFOHEADER* bmpInfo, FILE* output, char palette, ul paletteSize)
{
	fwrite(bmpHeader, sizeof(BITMAPFILEHEADER), 1, output);
	fwrite(bmpInfo, sizeof(BITMAPINFOHEADER), 1, output);

	if (paletteSize)
	{
		fwrite(palette, paletteSize, 1, output);
	}

	for (int i = 0; i < bmpInfo->biHeight; i++)
	{
		for (int j = 0; j < bmpInfo->biWidth; j++)
		{
			fwrite(&newRgb[i][j], sizeof(RGB), 1, output);
			if (bmpInfo->biBitCount == 32)
			{
				putc(0, output);
			}
		}

		for (int k = 0; k < vect; k++)
		{
			fputc(0, output);
		}
	}

	fclose(output);
}

//function of memory cleaning

void cleaning(RGB*** rgb, RGB*** newRgb, BITMAPFILEHEADER** bmpHeader, BITMAPINFOHEADER** bmpInfo)
{
	free(*bmpHeader);
	free(*bmpInfo);
	int a = strlen(*rgb);
	int b = strlen(*newRgb);
	for (int i = 0; i < a; i++)
	{
		free(*rgb[i]);
	}
	free(*rgb);
	for (int i = 0; i < b; i++)
	{
		free(*newRgb[i]);
	}
	free(*newRgb);
}

// filters are begin from here

void convolution(RGB** rgb, RGB** newRgb, int sobel, int size, int stride, int i, int j, double* matrix, double divisionRatio)
{
	double color1 = 0, color2 = 0, color3 = 0;
	if (matrix == NULL) // grey filter
	{
		color1 = color2 = color3 = ((rgb[i][j].rgbRed + rgb[i][j].rgbGreen + rgb[i][j].rgbBlue) / divisionRatio);
	}
	else // gausses, sobels, average and gabor filters 
	{
		int u, v;
		for (u = 0; u < size * size; u++)
		{
			RGB* pix = &rgb[i - stride + u / size][j - stride + u % size];
			color1 += pix->rgbRed * matrix[u] / divisionRatio;
			color2 += pix->rgbGreen * matrix[u] / divisionRatio;
			color3 += pix->rgbBlue * matrix[u] / divisionRatio;
		}
	}	
	if (sobel == 1) // only for sobel filters
	{
		if (color1 < 0) color1 = 0;
		if (color2 < 0) color2 = 0;
		if (color3 < 0) color3 = 0;
		if (color1 > 255) color1 = 255;
		if (color2 > 255) color2 = 255;
		if (color3 > 255) color3 = 255;
	}
	newRgb[i][j].rgbRed = (uc)(color1);
	newRgb[i][j].rgbGreen = (uc)(color2);
	newRgb[i][j].rgbBlue = (uc)(color3);
}

void averageFilter(RGB** rgb, RGB** newRgb, int height, int width)
{
	ui i, j;
	double matrix[9] = { 1, 1, 1, 1, 1, 1, 1, 1, 1 };
	for (i = 1; i < height - 1; i++)
	{
		for (j = 1; j < width - 1; j++)
		{
			convolution(rgb, newRgb, 0, 3, 1, i, j, matrix, 9);
		}
	}
}
	
void gaussFilter3x3(RGB** rgb, RGB** newRgb, int height, int width)
{
	ui i, j;
	double matrix[9] = { 1, 2, 1, 2, 4, 2, 1, 2, 1 };
	for (i = 1; i < height - 1; i++)
	{
		for (j = 1; j < width - 1; j++)
		{
			convolution(rgb, newRgb, 0, 3, 1, i, j, matrix, 16);
		}
	}
}

void gaussFilter5x5(RGB** rgb, RGB** newRgb, int height, int width)
{
	ui i, j;
	double matrix[25] =
	{
	1, 4, 6, 4, 1,
	4, 16, 24, 16, 4,
	6, 24, 36, 24, 6,
	4, 16, 24, 16, 4,
	1, 4, 6, 4, 1
	};
	for (i = 2; i < height - 2; i++)
	{
		for (j = 2; j < width - 2; j++)
		{
			convolution(rgb, newRgb, 0, 5, 2, i, j, matrix, 256);
		}
	}
}

void sobelFilter(RGB** rgb, RGB** newRgb, int height, int width, int number)
{
	ui i, j;
	double matrix[9];
	if (number == 1) // sobelX
	{
		matrix[0] = 1;
		matrix[1] = 2;
		matrix[2] = 1;
		matrix[3] = 0;
		matrix[4] = 0;
		matrix[5] = 0;
		matrix[6] = -1;
		matrix[7] = -2;
		matrix[8] = -1;
	}
	else // sobelY
	{
		matrix[0] = -1;
		matrix[1] = 0;
		matrix[2] = 1;
		matrix[3] = -2;
		matrix[4] = 0;
		matrix[5] = 2;
		matrix[6] = -1;
		matrix[7] = 0;
		matrix[8] = 1;
	}
	for (i = 1; i < height - 1; i++)
	{
		for (j = 1; j < width - 1; j++)
		{
			convolution(rgb, newRgb, 1, 3, 1, i, j, matrix, 1);			
		}
	}
}

void greyFilter(RGB** rgb, RGB** newRgb, int height, int width)
{
	ui i, j;
	for (i = 0; i < height; i++)
	{
		for (j = 0; j < width; j++)
		{
			convolution(rgb, newRgb, 0, 0, 0, i, j, NULL, 3);
		}
	}
}

void gaborFilter(RGB** rgb, RGB** newRgb, int height, int width, double begin)
{
	double end = clock();
	double theta = (double)((end - begin) / CLOCKS_PER_SEC);
	double f = theta;
	double x, y;
	double divisionRatio = 1;
	int wx = 13, wy = 13, q = 0;
	int i, j, u, v;
	double sigmaX = 0.012, sigmaY = 0.012;
	double auxiliaryMatrix[13][13];
	double gaborMatrix[169];

	for (u = 0; u < wy; u++)  //I use an additional two - dimensional array, then I translate it into one - dimensional
	{
		for (v = 0; v < wx; v++)
		{	
			x = v * cos(theta) + u * sin(theta);
			y = -v * sin(theta) + u * cos(theta);
			x = x * x;
			y = y * y;
			auxiliaryMatrix[u][v] = exp(-0.5 * ((x / sigmaX) + (x / sigmaY))) * cos(2 * PI * f * x);
			gaborMatrix[q] = auxiliaryMatrix[u][v];
			q++;
		}
	}
	//I send a one-dimensional array of type double to the function in the same way as with other filters
	for (i = wy / 2; i < height - wy / 2; i++)
	{
		for (j = wx / 2; j < width - wx / 2; j++)
		{
			convolution(rgb, newRgb, 0, 13, 6, i, j, gaborMatrix, 1);
		}
	}
}


int main(int argc, char* argv[])
{
	
	double begin = clock();
	FILE* input;
	FILE* output;
	if (argc != 4)
	{
		printf("Invalid input");
		exit(-1);
	}
	if ((input = (fopen(argv[1], "rb"))) == NULL)
	{
		printf("Unable to open input file");
		fclose(input);
		exit(-1);
	}
	if ((output = (fopen(argv[3], "wb"))) == NULL)
	{
		printf("Unable to open output file");
		fclose(output);
		exit(-1);
	}
	//system("chcp 1251");
	int vect;
	char* palette;
	ul paletteSize;
	BITMAPFILEHEADER* bmpHeader = malloc(sizeof(BITMAPFILEHEADER));
	BITMAPINFOHEADER* bmpInfo = malloc(sizeof(BITMAPINFOHEADER));
	RGB** rgb = processingBmp(bmpHeader, bmpInfo, input, &vect, &palette, &paletteSize);
	RGB** newRgb = copyBmp(rgb, bmpInfo->biHeight, bmpInfo->biWidth);
	if (strcmp(argv[2], "average") == 0)
	{
		averageFilter(rgb, newRgb, bmpInfo->biHeight, bmpInfo->biWidth);
	}
	else if (strcmp(argv[2], "gauss3x3") == 0)
	{
		gaussFilter3x3(rgb, newRgb, bmpInfo->biHeight, bmpInfo->biWidth);
	}
	else if (strcmp(argv[2], "gauss5x5") == 0)
	{
		gaussFilter5x5(rgb, newRgb, bmpInfo->biHeight, bmpInfo->biWidth);
	}
	else if (strcmp(argv[2], "sobelX") == 0)
	{
		sobelFilter(rgb, newRgb, bmpInfo->biHeight, bmpInfo->biWidth, 1);
	}
	else if (strcmp(argv[2], "sobelY") == 0)
	{
		sobelFilter(rgb, newRgb, bmpInfo->biHeight, bmpInfo->biWidth, 2);
	}
	else if (strcmp(argv[2], "grey") == 0)
	{
		greyFilter(rgb, newRgb, bmpInfo->biHeight, bmpInfo->biWidth);

	}
	else if (strcmp(argv[2], "gabor") == 0)
	{
		gaborFilter(rgb, newRgb, bmpInfo->biHeight, bmpInfo->biWidth, begin);
	}
	else
	{
		printf("Invalid input! Check the name of filter");
		fclose(input);
		fclose(output);
		exit(-1);
	}
	record(newRgb, vect, bmpHeader, bmpInfo, output, palette, paletteSize);
	cleaning(&rgb, &newRgb, &bmpHeader, &bmpInfo);
	return 0;
}
