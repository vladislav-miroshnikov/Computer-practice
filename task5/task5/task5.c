#include <stdio.h>
#include <math.h>

double checkinput(double w)
{
	char e;
	while (1)
	{
		if (!scanf_s("%lf", &w) || sqrt(w) == (int)(sqrt(w)) || (w != (int)w) || getchar() != '\n')
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

void prog(double w)
{
	int a0 = (int)sqrt(w), i = 0, a1, num = 1, den = 0;
	printf("[%d; {", a0);

	do
	{
		a1 = a0 - den;
		den = a1 + a0;
		num = (w - a1 * a1) / num;
		a1 = (int)(den / num);
		den = den % num;
		i++;
		printf(" %d,", a1);

	} while (a1 != (2 * a0));

	printf("} ]\nperiod is = %d ", i);
}

int main()
{
	double a = 0;
	printf("Please, enter the number\n");
	a = checkinput(a);
	prog(a);

	return 0;
}