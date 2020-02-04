#include <stdio.h>
#include <stdlib.h>
#include <math.h>

int main()
{
	int n = (1 + (log(3) / log(16)) * 5000); //number of characters in hexadecimal notation
	int* a;
	a = (int*)malloc(n * sizeof(int));
	int i,j;
	for (i = 0; i <= n - 1; i++)
	{
		a[i] = 0;
	}
	a[0] = 1;
	for (i = 0; i <= 4999; i++)
	{
		for (j = 0; j <= n - 1; j++)
		{
			a[j] = a[j] * 3;	
		}
		for (j = 0; j <= n - 2; j++)
		{
			if (a[j] > 15)
			{
				a[j + 1] = (a[j] / 16) + a[j + 1];
				a[j] = a[j] % 16;
			}
		}
	}
	
	for (i = n - 1; i >= 0; i--)
	{
		printf("%x", a[i]);
	}
	free(a);

	return 0;
}
