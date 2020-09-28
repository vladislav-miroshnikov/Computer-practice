#include <stdio.h>
#include <stdlib.h>

int check_input(int w)
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
	n = check_input(n);
	int coins[8] = { 1, 2, 5, 10, 20, 50, 100, 200 };
	long long* a;
	int i, j;
	a = (long long*)calloc((n + 1), sizeof(long long));
	a[0] = 1;
	for (i = 0; i <= 7; i++)
	{
		for (j = coins[i]; j <= n; j++)
		{
			a[j] = a[j] + a[j - coins[i]];
		}
	}
	printf("number of ways is %lld", a[n]);
	free(a);

	return 0;
}
