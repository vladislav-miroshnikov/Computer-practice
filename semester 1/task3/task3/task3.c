#include <stdio.h>
#include <math.h>

double checkinput(double w)
{
	char e;
	while (1)
	{
		if (!scanf_s("%lf", &w) || getchar() != '\n')
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

void uniq(double q)
{
	int degrees = (int)q;
	int minutes = (int)((q - degrees) * 60);
	int seconds = (int)(((q - degrees) * 60 - minutes) * 60);
	printf("\n%d degrees %d minutes %d seconds\n",degrees,minutes,seconds);
}

int main()
{
	double p = 180/3.141592;
	double x = 0, y = 0, z = 0;
	printf("Please, enter three sides \n");
	x = checkinput(x), y = checkinput(y), z = checkinput(z);
	if (((x + y) > z) && ((x + z) > y) && ((y + z) > x))
	{
		double xangle = acos((y * y + z * z - x * x) / (2 * y * z)) * p;
		double yangle = acos((x * x + z * z - y * y) / (2 * x * z)) * p;
		double zangle = acos((y * y + x * x - z * z) / (2 * x * y)) * p;
		uniq(xangle); uniq(yangle); uniq(zangle);
	}
	else
	{
		printf("Cannot build a non-degenerate triangle");
	}

	return 0;
}