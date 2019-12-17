#pragma once
#include <stdio.h>

void* myMalloc(size_t size);
void myFree(void* array);
void* myRealloc(void* array, size_t newSize);
void init();