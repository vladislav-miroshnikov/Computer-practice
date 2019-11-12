#include <stdio.h>
#include <math.h>

double check_input(double w)
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
	int b = (int)sqrt(w), i = 0, c, num = 1, den = 0;
	printf("[%d; {", b);

	do
	{
		c = b - den;
		den = c + b;
		num = (w - c * c) / num;
		c = (int)(den / num);
		den = den % num;
		i++;
		printf(" %d,", c);

	} while (c != (2 * b));

	printf("} ]\nperiod is = %d ", i);
}

int main()
{
	double a = 0;
	printf("Please, enter the number\n");
	a = check_input(a);
	prog(a);

	return 0;
}