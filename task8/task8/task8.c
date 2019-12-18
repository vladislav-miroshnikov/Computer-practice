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

void convolution(RGB** rgb, double* gaborColor1, double* gaborColor2, double* gaborColor3, int number, double f, double theta, int i, int j, ui* color1, ui* color2, ui* color3, const int* matrix)
{
	int u, v;
	if (number == 1) //gabor filter
	{
		double sigma_x = 0.04, sigma_y = 0.04, x, y;
		double gaborMatrix[13][13];
		int wx = 13, wy = 13;
		for (u = 0; u < wy; u++)
		{
			for (v = 0; v < wx; v++)
			{
				x = v * cos(theta) + u * sin(theta);
				y = -v * sin(theta) + u * cos(theta);
				x = x * x;
				y = y * y;
				gaborMatrix[u][v] = exp(-0.5 * ((x / sigma_x) + (x / sigma_y))) * cos(2 * PI * f * x);
				*gaborColor1 += gaborMatrix[u][v] * (double)rgb[i - wy / 2 + u][j - wx / 2 + v].rgbBlue;
				*gaborColor2 += gaborMatrix[u][v] * (double)rgb[i - wy / 2 + u][j - wx / 2 + v].rgbGreen;
				*gaborColor3 += gaborMatrix[u][v]* (double)rgb[i - wy / 2 + u][j - wx / 2 + v].rgbRed;
			}
		}
	}
	else if (number == 2) // grey filter
	{
		*color1 = ((rgb[i][j].rgbGreen + rgb[i][j].rgbBlue + rgb[i][j].rgbRed) / 3);
	}
	else if (number == 3) //average filter
	{
		for (u = -1; u < 2; u++)
		{
			for (v = -1; v < 2; v++)
			{
				*color1 += rgb[i + u][j + v].rgbRed;
				*color2 += rgb[i + u][j + v].rgbGreen;
				*color3 += rgb[i + u][j + v].rgbBlue;
				
			}
		}
	}
	else //gausses and sobels filters
	{
		int c = (int)f;
		int stride = (int)theta;
		for (u = 0; u < c * c; u++)
		{
			RGB* pix = &rgb[i - stride + u / c][j - stride + u % c];
			*color1 += pix->rgbRed * matrix[u];
			*color2 += pix->rgbGreen * matrix[u];
			*color3 += pix->rgbBlue * matrix[u];
		}
	}

			
}

void averageFilter(RGB** rgb, RGB** newRgb, int height, int width)
{
	for (int i = 1; i < height - 1; i++)
	{
		for (int j = 1; j < width - 1; j++)
		{
			ui green = 0, blue = 0, red = 0;
			convolution(rgb, NULL, NULL, NULL, 3, 0, 0, i, j, &red, &green, &blue, NULL, NULL);
			newRgb[i][j].rgbRed = (uc)(red / 9);
			newRgb[i][j].rgbGreen = (uc)(green / 9);
			newRgb[i][j].rgbBlue = (uc)(blue / 9);
		}
	}
}
	
void gaussFilter3x3(RGB** rgb, RGB** newRgb, int height, int width)
{
	ui i, j;
	const int matrix3x3[9] = { 1, 2, 1, 2, 4, 2, 1, 2, 1 };
	for (i = 1; i < height - 1; i++)
	{
		for (j = 1; j < width - 1; j++)
		{
			ui red = 0, green = 0, blue = 0;

			convolution(rgb, NULL, NULL, NULL, 0, 3, 1, i, j, &red, &green, &blue, matrix3x3);

			newRgb[i][j].rgbRed = (uc)(red / 16);
			newRgb[i][j].rgbGreen = (uc)(green / 16);
			newRgb[i][j].rgbBlue = (uc)(blue / 16);
		}
	}
}

void gaussFilter5x5(RGB** rgb, RGB** newRgb, int height, int width)
{
	
	const int matrix5x5[25] =
	{
	1, 4, 6, 4, 1,
	4, 16, 24, 16, 4,
	6, 24, 36, 24, 6,
	4, 16, 24, 16, 4,
	1, 4, 6, 4, 1
	};
	for (int i = 2; i < height - 2; i++)
	{
		for (int j = 2; j < width - 2; j++)
		{
			
			int pop = 256;
			int green = 0, blue = 0, red = 0;

			convolution(rgb, NULL, NULL, NULL, 0, 5, 2, i, j, &red, &green, &blue, matrix5x5);

			newRgb[i][j].rgbRed = (uc)(red / pop);
			newRgb[i][j].rgbBlue = (uc)(blue / pop);
			newRgb[i][j].rgbGreen = (uc)(green / pop);
		}
	}
}


void sobelXFilter(RGB** rgb, RGB** newRgb, int height, int width)
{
	for (int i = 1; i < height - 1; i++)
	{
		for (int j = 1; j < width - 1; j++)
		{
			const int matrix3x3[9] = { 1,  2,  1, 0,  0,  0, -1, -2, -1 };
			int green = 0, blue = 0, red = 0;

			convolution(rgb, NULL, NULL, NULL, 0, 3, 1, i, j, &red, &green, &blue, matrix3x3);
			if (red < 0) red = 0;
			if (blue < 0) blue = 0;
			if (green < 0) green = 0;

			if (blue > 255) blue = 255;
			if (green > 255) green = 255;
			if (red > 255) red = 255;

			newRgb[i][j].rgbRed = (uc)(red);
			newRgb[i][j].rgbBlue = (uc)(blue);
			newRgb[i][j].rgbGreen = (uc)(green);
		}
	}
}

void sobelYFilter(RGB** rgb, RGB** newRgb, int height, int width)
{
	for (int i = 1; i < height - 3; i++)
	{
		for (int j = 1; j < width - 3; j++)
		{
			const int matrix3x3[9] = { -1, 0, 1, -2, 0, 2, -1, 0, 1 };
			int green = 0, blue = 0, red = 0;

			convolution(rgb, NULL, NULL, NULL, 0, 3, 1, i, j, &red, &green, &blue, matrix3x3);

			if (red < 0) red = 0;
			if (blue < 0) blue = 0;
			if (green < 0) green = 0;

			if (blue > 255) blue = 255;
			if (green > 255) green = 255;
			if (red > 255) red = 255;

			newRgb[i][j].rgbRed = (uc)(red);
			newRgb[i][j].rgbBlue = (uc)(blue);
			newRgb[i][j].rgbGreen = (uc)(green);
		}
	}
}

void greyFilter(RGB** rgb, RGB** newRgb, int height, int width)
{
	for (int i = 0; i < height; i++)
	{
		for (int j = 0; j < width; j++)
		{
			ui color;
			convolution(rgb, NULL, NULL, NULL, 2, 0, 0, i, j, &color, NULL, NULL, NULL, NULL);
			newRgb[i][j].rgbBlue = (uc)color;
			newRgb[i][j].rgbGreen = (uc)color;
			newRgb[i][j].rgbRed = (uc)color;
		}
	}
}

void gaborFilter(RGB** rgb, RGB** newRgb, int height, int width, double begin)
{
	double end = clock();
	double theta = (double)((end - begin) / CLOCKS_PER_SEC);
	double f = theta;
	double sum1, sum2, sum3;
	int i, j, wx = 13, wy = 13, u, v;
	for (i = wy / 2; i < height - wy / 2; i++)
	{
		for (j = wx / 2; j < width - wx / 2; j++)
		{
			sum1 = 0; sum2 = 0; sum3 = 0;
			convolution(rgb, &sum1, &sum2, &sum3, 1, theta, f, i, j, NULL, NULL, NULL, NULL, NULL);
			newRgb[i][j].rgbBlue = (uc)sum1;
			newRgb[i][j].rgbGreen = (uc)sum2;
			newRgb[i][j].rgbRed = (uc)sum3;
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
		sobelXFilter(rgb, newRgb, bmpInfo->biHeight, bmpInfo->biWidth);
	}
	else if (strcmp(argv[2], "sobelY") == 0)
	{
		sobelYFilter(rgb, newRgb, bmpInfo->biHeight, bmpInfo->biWidth);
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
