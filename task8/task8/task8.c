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
	RGB** newSet = calloc(sizeof(RGB*), height);
	for (int i = 0; i < height; i++)
	{
		newSet[i] = calloc(sizeof(RGB*), width);
		memcpy(newSet[i], rgb[i], sizeof(RGB) * width);
	}
	return newSet;
}

//function of record bmp into new file
void record(RGB** newSet, int vect, BITMAPFILEHEADER* bmpHeader, BITMAPINFOHEADER* bmpInfo, FILE* output, char palette, ul paletteSize)
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
			fwrite(&newSet[i][j], sizeof(RGB), 1, output);
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

void cleaning(RGB*** rgb, RGB*** newSet, BITMAPFILEHEADER** bmpHeader, BITMAPINFOHEADER** bmpInfo)
{
	free(*bmpHeader);
	free(*bmpInfo);
	int a = strlen(*rgb);
	int b = strlen(*newSet);
	for (int i = 0; i < a; i++)
	{
		free(*rgb[i]);
	}
	free(*rgb);
	for (int i = 0; i < b; i++)
	{
		free(*newSet[i]);
	}
	free(*newSet);
}

// filters are begin from here

void averageFilter(RGB** rgb, RGB** newSet, int height, int width)
{
	for (int i = 1; i < height - 1; i++)
	{
		for (int j = 1; j < width - 1; j++)
		{
			ui green = 0, blue = 0, red = 0;
			for (int k = -1; k < 2; k++)
			{
				for (int m = -1; m < 2; m++)
				{
					red += rgb[i + k][j + m].rgbRed;
					green += rgb[i + k][j + m].rgbGreen;
					blue += rgb[i + k][j + m].rgbBlue;
				}
			}
			
			newSet[i][j].rgbRed = (uc)(red / 9);
			newSet[i][j].rgbGreen = (uc)(green / 9);
			newSet[i][j].rgbBlue = (uc)(blue / 9);
		}
	}
}

void createGauss3x3(RGB** rgb, RGB** newSet, int x, int y)
{
	const int matrix3x3[3][3] = { {1, 2, 1}, {2, 4, 2}, {1, 2, 1} };
	int pop = 16;
	int green = 0, blue = 0, red = 0;

	for (int i = 0; i < 3 * 3; i++)
	{
		RGB* pix = &rgb[y - 1 + i / 3][x - 1 + i % 3];

		blue += pix->rgbBlue * matrix3x3[i / 3][i % 3];
		green += pix->rgbGreen * matrix3x3[i / 3][i % 3];
		red += pix->rgbRed * matrix3x3[i / 3][i % 3];
	}
	newSet[y][x].rgbRed = (uc)(red / pop);
	newSet[y][x].rgbBlue = (uc)(blue / pop);
	newSet[y][x].rgbGreen = (uc)(green / pop);
}

void gaussFilter3x3(RGB** rgb, RGB** newSet, int height, int width)
{
	
	for (int i = 3 / 2; i < height - 3; i++)
	{
		for (int j = 3 / 2; j < width - 3; j++)
		{
			createGauss3x3(rgb, newSet, j, i);
		}
	}
}

void createGauss5x5(RGB** rgb, RGB** newSet, int x, int y)
{
	const int matrix5x5[5][5] = { {1, 4,  6,  4,  1}, {4, 16, 24, 16, 4}, {6, 24, 36, 24, 6}, {4, 16, 24, 16, 4}, {1, 4,  6,  4,  1} };
	int pop = 256;
	int green = 0, blue = 0, red = 0;

	for (int i = 0; i < 5 * 5; i++)
	{
		RGB* pix = &rgb[y - 2 + i / 5][x - 2 + i % 5];

		blue += pix->rgbBlue * matrix5x5[i / 5][i % 5];
		green += pix->rgbGreen * matrix5x5[i / 5][i % 5];
		red += pix->rgbRed * matrix5x5[i / 5][i % 5];
	}

	newSet[y][x].rgbRed = (uc)(red / pop);
	newSet[y][x].rgbBlue = (uc)(blue / pop);
	newSet[y][x].rgbGreen = (uc)(green / pop);
}

void gaussFilter5x5(RGB** rgb, RGB** newSet, int height, int width)
{
	for (int i = 5 / 2; i < height - 5; i++)
	{
		for (int j = 5 / 2; j < width - 5; j++)
		{
			createGauss5x5(rgb, newSet, j, i);
		}
	}
}


void createSobelX(RGB** rgb, RGB** newSet, int x, int y)
{
	const int matrix[3][3] = { {1,  2,  1}, {0,  0,  0}, {-1, -2, -1} };
	int green = 0, blue = 0, red = 0;

	for (int i = 0; i < 9; i++)
	{
		RGB* pix = &rgb[y - 1 + i / 3][x - 1 + i % 3];

		blue += pix->rgbBlue * matrix[i / 3][i % 3];
		green += pix->rgbGreen * matrix[i / 3][i % 3];
		red += pix->rgbRed * matrix[i / 3][i % 3];
	}

	if (red < 0) red = 0;
	if (blue < 0) blue = 0;
	if (green < 0) green = 0;

	if (blue > 255) blue = 255;
	if (green > 255) green = 255;
	if (red > 255) red = 255;

	newSet[y][x].rgbRed = (uc)(red);
	newSet[y][x].rgbBlue = (uc)(blue);
	newSet[y][x].rgbGreen = (uc)(green);
}

void sobelXFilter(RGB** rgb, RGB** newSet, int height, int width)
{
	for (int i = 1; i < height - 3; i++)
	{
		for (int j = 1; j < width - 3; j++)
		{
			createSobelX(rgb, newSet, j, i);
		}
	}
}

void createSobelY(RGB** rgb, RGB** newSet, int x, int y)
{
	const int matrix[3][3] = { {-1, 0, 1}, {-2, 0, 2}, {-1, 0, 1} };
	int green = 0, blue = 0, red = 0;

	for (int i = 0; i < 9; i++)
	{
		RGB* pix = &rgb[y - 1 + i / 3][x - 1 + i % 3];

		blue += pix->rgbBlue * matrix[i / 3][i % 3];
		green += pix->rgbGreen * matrix[i / 3][i % 3];
		red += pix->rgbRed * matrix[i / 3][i % 3];
	}

	if (red < 0) red = 0;
	if (blue < 0) blue = 0;
	if (green < 0) green = 0;

	if (blue > 255) blue = 255;
	if (green > 255) green = 255;
	if (red > 255) red = 255;

	newSet[y][x].rgbRed = (uc)(red);
	newSet[y][x].rgbBlue = (uc)(blue);
	newSet[y][x].rgbGreen = (uc)(green);
}


void sobelYFilter(RGB** rgb, RGB** newSet, int height, int width)
{
	for (int i = 1; i < height - 3; i++)
	{
		for (int j = 1; j < width - 3; j++)
		{
			createSobelY(rgb, newSet, j, i);
		}
	}
}

void greyFilter(RGB** rgb, RGB** newSet, int height, int width)
{
	for (int i = 0; i < height; i++)
	{
		for (int j = 0; j < width; j++)
		{
			uc color = (uc)((rgb[i][j].rgbGreen + rgb[i][j].rgbBlue + rgb[i][j].rgbRed) / 3);

			newSet[i][j].rgbBlue = color;
			newSet[i][j].rgbGreen = color;
			newSet[i][j].rgbRed = color;
		}
	}
}

void gaborFilter(RGB** rgb, RGB** newSet,  int height, int width, double begin)
{
	double end = clock();
	double theta = (double)((end - begin) / CLOCKS_PER_SEC);
	double f = theta;
	double sum1, sum2, sum3;
	int i, j, wx = 13, wy = 13, u, v;
	double sigma_x = 0.04, sigma_y = 0.04, x, y;
	double G;
	for (i = wy/2; i < height - wy / 2; i++)
	{
		for (j = wx / 2; j < width - wx / 2; j++)
		{
			sum1 = 0; sum2 = 0; sum3 = 0;
			for (u = 0; u < wy; u++)
			{
				for (v = 0; v < wx; v++)
				{
					x = v * cos(theta) + u * sin(theta);
					y = -v * sin(theta) + u * cos(theta);
					x = x * x;
					y = y * y;
					G = exp(-0.5 * ((x / sigma_x) + (x / sigma_y))) * cos(2 * PI * f * x);
					sum1 += G * (double)rgb[i - wy / 2 + u][j - wx / 2 + v].rgbBlue;
					sum2 += G * (double)rgb[i - wy / 2 + u][j - wx / 2 + v].rgbGreen;
					sum3 += G * (double)rgb[i - wy / 2 + u][j - wx / 2 + v].rgbRed;
				}
			}
			/*sum1 = (int)sum1;   //????? 
			sum2 = (int)sum2;
			sum3 = (int)sum3;*/
			newSet[i][j].rgbBlue = (uc)sum1;
			newSet[i][j].rgbGreen = (uc)sum2;
			newSet[i][j].rgbRed = (uc)sum3;
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
	if ((input=(fopen(argv[1], "rb"))) == NULL)
	{
		printf("Unable to open input file");
		fclose(input);
		exit(-1);
	}
	if ((output=(fopen(argv[3], "wb"))) == NULL)
	{
		printf("Unable to open output file");
		fclose(output);
		exit(-1);
	}

	int vect;
	char* palette;
	ul paletteSize;
	BITMAPFILEHEADER* bmpHeader = malloc(sizeof(BITMAPFILEHEADER));
	BITMAPINFOHEADER* bmpInfo = malloc(sizeof(BITMAPINFOHEADER));

	RGB** rgb = processingBmp(bmpHeader, bmpInfo, input, &vect, &palette, &paletteSize);
	if (strcmp(argv[2], "average") == 0)
	{
		RGB** newSet = copyBmp(rgb, bmpInfo->biHeight, bmpInfo->biWidth);
		averageFilter(rgb, newSet, bmpInfo->biHeight, bmpInfo->biWidth);
		record(newSet, vect, bmpHeader, bmpInfo, output, palette, paletteSize);
		cleaning(&rgb, &newSet, &bmpHeader, &bmpInfo);
	}
	else if (strcmp(argv[2], "gauss3") == 0)
	{
		RGB** newSet = copyBmp(rgb, bmpInfo->biHeight, bmpInfo->biWidth);
		gaussFilter3x3(rgb, newSet, bmpInfo->biHeight, bmpInfo->biWidth);
		record(newSet, vect, bmpHeader, bmpInfo, output, palette, paletteSize);
		cleaning(&rgb, &newSet, &bmpHeader, &bmpInfo);
	}
	else if (strcmp(argv[2], "gauss5") == 0)
	{
		RGB** newSet = copyBmp(rgb, bmpInfo->biHeight, bmpInfo->biWidth);
		gaussFilter5x5(rgb, newSet, bmpInfo->biHeight, bmpInfo->biWidth);
		record(newSet, vect, bmpHeader, bmpInfo, output, palette, paletteSize);
		cleaning(&rgb, &newSet, &bmpHeader, &bmpInfo);
	}
	else if (strcmp(argv[2], "sobelX") == 0)
	{
		RGB** newSet = copyBmp(rgb, bmpInfo->biHeight, bmpInfo->biWidth);
		sobelXFilter(rgb, newSet, bmpInfo->biHeight, bmpInfo->biWidth);
		record(newSet, vect, bmpHeader, bmpInfo, output, palette, paletteSize);
		cleaning(&rgb, &newSet, &bmpHeader, &bmpInfo);
	}
	else if (strcmp(argv[2], "sobelY") == 0)
	{
		RGB** newSet = copyBmp(rgb, bmpInfo->biHeight, bmpInfo->biWidth);
		sobelYFilter(rgb, newSet, bmpInfo->biHeight, bmpInfo->biWidth);
		record(newSet, vect, bmpHeader, bmpInfo, output, palette, paletteSize);
		cleaning(&rgb, &newSet, &bmpHeader, &bmpInfo);
	}
	else if (strcmp(argv[2], "grey") == 0)
	{
		RGB** newSet = copyBmp(rgb, bmpInfo->biHeight, bmpInfo->biWidth);
		greyFilter(rgb, newSet, bmpInfo->biHeight, bmpInfo->biWidth);
		record(newSet, vect, bmpHeader, bmpInfo, output, palette, paletteSize);
		cleaning(&rgb, &newSet, &bmpHeader, &bmpInfo);

	}
	else if (strcmp(argv[2], "gabor") == 0)
	{
		RGB** newSet = copyBmp(rgb, bmpInfo->biHeight, bmpInfo->biWidth);
		gaborFilter(rgb, newSet, bmpInfo->biHeight, bmpInfo->biWidth, begin);
		record(newSet, vect, bmpHeader, bmpInfo, output, palette, paletteSize);
		cleaning(&rgb, &newSet, &bmpHeader, &bmpInfo);
	}
	else
	{
		printf("Invalid input! Check the name of filter");
		fclose(input);
		fclose(output);
		exit(-1);
	}

	return 0;
}


