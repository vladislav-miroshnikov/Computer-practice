#include <stdio.h>
#include <stdlib.h>

typedef unsigned char uc;
void* myMalloc(int size); 
void myFree(void* array);
void* myRealloc(void* array, int newSize); 
void init();

typedef struct forMemory
{
	int size;
	struct forMemory* previousNode, * nextNode;
}forMemory;

forMemory* find(int size);
forMemory* memoryList;
const int sizeM = 1024;  //const of max memory size, you can change this setting depending on the task 
void* memorySize = 0;    

forMemory* find(int size)
{
	forMemory* array = memoryList;
	while (array && array->size < size)
	{
		array = array->nextNode;
	}
	return array;
}
    
void init()
{
	if (memorySize)
	{
		printf("error of memory size");
		exit(-1);
	}
	memorySize = malloc(sizeM * sizeof(int));
	if (memorySize == 0)
	{
		exit(-1);
	}
	memoryList = (forMemory*)memorySize;
	memoryList->size = sizeM;
	memoryList->nextNode = 0; 
	memoryList->previousNode = memoryList->nextNode;
}

void* myMalloc(int size)
{
	if (memorySize == 0)
	{
		init();
	}
	int row = size + sizeof(int);
	if (row < sizeof(forMemory))
	{
		row = sizeof(forMemory);
	}
	forMemory* set = find(row);
	if (!set)
	{
		return 0;
	}
	if (set->size > row + sizeof(forMemory))
	{
		set->size -= row;
		forMemory* pop = (forMemory*)((uc*)set + set->size);
		pop->size = row;
		return (uc*)pop + sizeof(int);
	}
	else
	{
		if (set->previousNode == 0)
		{
			memoryList = set->nextNode;
		}
		else
		{
			set->previousNode->nextNode = set->nextNode;
		}
		if (!set->nextNode)
		{
			set->nextNode->previousNode = set->previousNode;
		}
		return (uc*)set + sizeof(int);
	}
}

void* myRealloc(void* array, int newSize)
{
	if (memorySize == 0)
	{
		printf("error of memory size");
		exit(-1);
	}
	forMemory* pop = (uc*)(void*)array - sizeof(int);
	if (pop->size >= newSize + sizeof(int))
	{
		return array;
	}
	else
	{
		void* newArray = myMalloc(newSize);
		if (newArray == 0) return 0;
		memcpy(newArray, array, pop->size - sizeof(int));
		return newArray;
	}
}

void myFree(void* array)
{
	if (memorySize == 0)
	{
		printf("error of memory size");
		exit(-1);
	}
	forMemory* pop = (forMemory*)(void*)array - sizeof(int);
	if (memoryList == 0)
	{
		memoryList = pop;
		pop->previousNode = pop->nextNode = 0;
	}
	else
	{
		forMemory* prd = 0, *nxt = memoryList;
		while (nxt && nxt->nextNode < pop)
		{
			prd = nxt;
			nxt = nxt->nextNode;
		}
		if (!prd)
		{
			pop->nextNode = memoryList;
			memoryList = pop;
		}
		else
		{
			prd->nextNode = pop;
			pop->previousNode = prd;
		}
		if (nxt)
		{
			nxt->previousNode = pop;
		}
		pop->nextNode = nxt;

		if ((uc*)pop + pop->size == (uc*)nxt)
		{
			pop->nextNode = nxt->nextNode;
			pop->size += nxt->size;
		}
		if (prd && (uc*)prd + prd->size == (uc*)pop)
		{
			prd->nextNode = pop->nextNode;
			prd->size += pop->size;
		}
	}
}

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


