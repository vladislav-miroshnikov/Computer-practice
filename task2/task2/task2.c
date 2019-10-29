#include <stdio.h>

int gcd(int a, int b)
{
	while ((a != 0) && (b != 0))
	{
		if (a > b)
		{
			a = a % b;
		}
		else b = b % a;
	}

	return (a + b);
}

int checkinput(int w)
{
	char e;
	while (1)
	{
		if (!scanf_s("%d", &w) || w==0 || getchar() != '\n')
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
	int x = 0, y = 0, z = 0, t;
	printf("Please, enter three numbers \n");
	x = checkinput(x), y = checkinput(y), z = checkinput(z);
	if (x > z)
	{
		t = z;
		z = x;
		x = t;
	}
	if (y > z)
	{
		t = z;
		z = y;
		y = t;
	}
	x = x * x; y = y * y; z = z * z;
	if ((x + y) == z)
	{
		printf("This is the Pythagorean triple ");
		if ((gcd(x, y) == 1) && (gcd(x, z) == 1) && (gcd(y, z) == 1))
		{
			printf("and also simple triple\n");
		}
		else printf("and also not simple triple\n");
	}
	else printf("This is't the Pythagorean triple\n");

	return 0;
}