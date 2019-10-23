#include <stdio.h>
#include <math.h>
#include <string.h>

int main()
{
	const char name[] = "Vladislav";
	const char surname[] = "Miroshnikov";
	const char patronymic[] = "Igorevich";
	int proizv = strlen(name) * strlen(surname) * strlen(patronymic);
	printf("Composition is equal: %d\n", proizv);
	long long b = pow(2, 32) - proizv;
	int t[32], i = 0;
	
	while (b != 0)
	{
		t[i] = b % 2;
		b = b / 2;
		i++;
	}
	printf("negative 32bit integer is =");
	for (int i = 0; i<=31 ; i++)
	{
		printf("%d", t[31-i]);

	}
	
	float f = proizv;
	int x = *((int*)& f);
	
	i = 0;
	
	while (x)
	{
		t[i] = x % 2;
		x= x / 2;
		i++;
	}
	printf("\npositive floating point number (single precision, IEEE 754) is = 0");
	
	for (i = 1; i <= 31; i++)
	{
		printf("%d",t[31-i]);
	}
	
	double g = 891;
	long long l = *((long long*)& g);
	i = 0;
	int z[63];
	while (l)
	{
		z[i] = l % 2;
		l = l / 2;
		i++;
	}
	printf("\nneg. fl. point number (double prec., IEEE 754) is  = 1");
	
	for (i = 1; i <= 63; i++)
	{
		printf("%d", z[63 - i]);
	}
	
	return 0;
}