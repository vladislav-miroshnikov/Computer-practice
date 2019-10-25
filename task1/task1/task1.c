#include <stdio.h>
#include <malloc.h>
#include <math.h>
#include <string.h>

void loc(long long r,int n)
{
	int *a, i = 0;
	a = (int*)malloc(n * sizeof(int));
	
	while (r != 0)
	{			
		a[i] = r % 2;
		r = r / 2;
		i++;
	}
	
	for (i = 0; i <= (n-1); i++)
		printf("%d", a[(n-1)-i]);
	free(a);
}

int main()
{
	const char name[] = "Vladislav";
	const char surname[] = "Miroshnikov";
	const char patronymic[] = "Igorevich";
	int proizv = strlen(name) * strlen(surname) * strlen(patronymic);
	printf("The product is equal: %d\n", proizv);
	long long b = pow(2, 32) - proizv;
	printf("negative 32bit integer is = ");
	loc(b, 32);

	float f = proizv;
	int x = *((int*)& f);
	printf("\npositive floating point number (single precision, IEEE 754) is = 0"); //0 because positive
	loc(x, 31);

	double g = proizv;
	long long l = *((long long*)& g);
	printf("\nneg. fl. point number (double prec., IEEE 754) is  = 1"); //1 because negative
	loc(l, 63);

	return 0;
}