#include <stdio.h>
#include <stdlib.h>

int checkinput(int w)
{
	char e;
	while (1)
	{
		if (!scanf_s("%d", &w) || w == 0 || getchar() != '\n')
		{
			while ((e = getchar()) != '\n' && e != EOF);
			printf_s("Entered incorrectly\nTry again: \n");
		}
		else
		{
			return w;
		}
	}

}

int main()
{
	int n = 0;
	printf("Please, enter the number\n");
	n = checkinput(n);
	int coins[8] = { 1, 2, 5, 10, 20, 50, 100, 200 };
	int* a;
	int i, j;
	a = (int*)malloc((n + 1) * 10 * sizeof(int));
	for (i = 0; i <= n; i++)
	{
		*(a + 0 * (n + 1) + i) = 0;
	}
	for (j = 0; j <= 9; j++)
	{
		*(a + j * (n + 1) + 0) = 1;
	}
	for (j = 0; j <= 7; j++)
	{
		for (i = coins[j]; i <= n; i++)
		{
			*(a + (j + 1) * (n + 1) + i) = *(a + j * (n + 1) + i) + *(a + (j + 1) * (n + 1) + (i - coins[j]));
		}

		for (i = 1; i <= n; i++)
		{
			*(a + (j + 2) * (n + 1) + i) = *(a + (j + 1) * (n + 1) + i);
		}
	}
	printf("number of ways is %d", *(a + (j + 1) * (n + 1) + n));
	free(a);

	return 0;
}
