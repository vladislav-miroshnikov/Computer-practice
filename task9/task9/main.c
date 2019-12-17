#include <stdio.h>
#include "allocator.h"

int main()
{
	//allocate memory for the array
	int* a = myMalloc(sizeof(int) * 30);
	int i;
	for (i = 0; i < 30; i++)
	{
		a[i] = i;
	}
	printf("print the filled array:\n");
	for (i = 0; i < 30; i++)
	{
		printf("a[%d] = %d\n", i, a[i]);
	}
	printf("\nallocate more memory and replenish the array:\n");
	//allocate more memory and print a new array
	a = myRealloc(a, sizeof(int) * 50);
	for (i = 30; i < 50; i++)
	{
		a[i] = i;
	}
	for (i = 0; i < 50; i++)
	{
		printf("a[%d] = %d\n", i, a[i]);
	}
	myFree(a);
	printf("the program works correctly");
	return 0;
}
