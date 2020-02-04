#include <stdio.h>
#include <stdlib.h>

int digital_root(int a)
{
	if ((a % 9) == 0)
	{
		return 9;
	}
	else
	{
		return (a % 9);
	}
}

int main()
{
	int* a;
	a = (int*)malloc(1000000 * sizeof(int));
	int i, j, sum;
	for (i = 2; i <= 999999; i++)
	{
		sum = 0;
		for (j = 2; j * j <= i; j++)
		{
			if ((i % j) == 0)
			{
				a[i] = a[i / j] + a[j];
			}
			if (a[i] > sum)
			{
				sum = a[i];
			}
		}
		if (sum > digital_root(i))
		{
			a[i] = sum;
		}
		else
		{
			a[i] = digital_root(i);
		}
	}
	int mdrs = 0;
	for (i = 2; i <= 999999; i++)
	{
		mdrs = mdrs + a[i];
	}
	printf("%d", mdrs);

	return 0;
}

