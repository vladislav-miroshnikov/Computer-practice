#include <stdio.h>
#include <math.h>

double checkinput(double w)
{
	char e;
	while (1)
	{
		if (!scanf_s("%lf", &w) || sqrt(w) == (int)(sqrt(w)) || (w <= 1) || (w != (int)w) || getchar() != '\n')
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
	int a0 = (int)sqrt(w);
	int i = 0, a1;
	double x0 = sqrt(w) - a0;
	printf("[%d; {", a0);

	do
	{
		a1 = (int)(1 / x0);
		x0 = (1 / x0) - a1;
		printf(" %d,", a1);
		i++;
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