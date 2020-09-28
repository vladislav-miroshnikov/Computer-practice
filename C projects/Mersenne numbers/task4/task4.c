#include <stdio.h>
#include <math.h>

int simple(long long n)
{
	for (long long i = 2; i * i <= n; i++)
		if ((n % i) == 0)
			return 0;
	return 1;
}

int main()
{
	double c = 2;
	for (int j = 2; j <= 31; j++)
	{
		if (simple(j))
		{
			long long a = pow(c, j) - 1;
			if (simple(a))
				printf("%lld\n", a);
		}
	}
	return 0;
}