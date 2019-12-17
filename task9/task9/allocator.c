#include <stdio.h>
#include "allocator.h"

typedef unsigned char uc;
typedef struct forMemory
{
	size_t size;
	struct forMemory* previousNode, * nextNode;
}forMemory;

forMemory* find(size_t size);
forMemory* memoryList;
const size_t sizeM = 1024;  //const of max memory size, you can change this setting depending on the task 
void* memorySize = 0;    

forMemory* find(size_t size)
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
	memorySize = malloc(sizeM * sizeof(size_t));
	if (memorySize == 0)
	{
		exit(-1);
	}
	memoryList = (forMemory*)memorySize;
	memoryList->size = sizeM;
	memoryList->nextNode = 0; 
	memoryList->previousNode = memoryList->nextNode;
}

void* myMalloc(size_t size)
{
	if (memorySize == 0)
	{
		init();
	}
	size_t row = size + sizeof(size_t);
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
		return (uc*)pop + sizeof(forMemory);
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
		return (uc*)set + sizeof(forMemory);
	}
}

void* myRealloc(void* array, size_t newSize)
{
	if (memorySize == 0)
	{
		printf("error of memory size");
		exit(-1);
	}
	forMemory* pop = (uc*)(void*)array - sizeof(size_t);
	if (pop->size >= newSize + sizeof(size_t))
	{
		return array;
	}
	else
	{
		void* newArray = myMalloc(newSize);
		if (newArray == 0) return 0;
		memcpy(newArray, array, pop->size - sizeof(size_t));
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
	forMemory* pop = (forMemory*)(void*)array - sizeof(size_t);
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
