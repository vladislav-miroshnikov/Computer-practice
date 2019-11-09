#include <stdio.h>
#include <stdlib.h>

int main()
{
	int* a;
	a = (int*)malloc(5000 * sizeof(int));
	int i,j;
	for (i = 0; i <= 4999; i++)
	{
		a[i] = 0;
	}
	a[0] = 1;
	for (i = 0; i <= 4999; i++)
	{
		for (j = 0; j <= 4999; j++)
		{
			a[j] = a[j] * 3;	
		}
		for (j = 0; j <= 4998; j++)
		{
			if (a[j] > 15)
			{
				a[j + 1] = (a[j] / 16) + a[j + 1];
				a[j] = a[j] % 16;
			}
		}
	}
	int k = 5000;
	for (i = 4999; i >= 0; i--)
	{
		k = k - 1;
		if (a[i] > 0)
		{
			for (k; k >= 0; k--)
			{
				printf("%x", a[k]);
			}		
			break;
		}
	}
	free(a);

	return 0;
}
