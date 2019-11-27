#define _CRT_SECURE_NO_WARNINGS
#include <stdio.h>
#include <stdlib.h>
#include <fcntl.h>
#include <malloc.h>
#include <sys/stat.h>
#include <sys/types.h>
#include "mman.h"
#include "mman.c"
#include <string.h>

int scmp(const void* p1, const void* p2)
{
	const char* s1, * s2;

	s1 = *(char**)p1;
	s2 = *(char**)p2;
	return strcmp(s1, s2);
}

int params(char* map, int size)
{
	int w = 1;
	for (int i = 0; i < size; i++)
	{
		if (map[i] == '\n')
		{
			w++;
		}
	}
	return w;
}

int max_length(char* map, int size)
{
	int e = 0;
	int max = 0;
	for (int i = 0; i < size; i++)
	{
		if (map[i] != '\n')
		{
			e++;
		}
		else
		{
			if (e > max)
			{
				max = e;
			}
			e = 0;
		}
	}
	return max;		
}

int main()
{
	int in = _open("input.txt", O_RDWR);
	int out = _open("output.txt", O_RDWR | O_CREAT | O_TRUNC, S_IWRITE);
	struct stat inform;
	fstat(in, &inform);
	int size = inform.st_size;
	int j, i, ctr;
	int y = 0;
	if (in == -1) {
		printf("unable to open file");
		return 1;
	}

	char* map = mmap(0, size, PROT_READ | PROT_WRITE, MAP_PRIVATE, in, 0);
	if (map == MAP_FAILED)
	{
		printf("error calling mmap function");
		return 1;
	}
	
	j=params(map, size); //numbers of lines
	char** sort = (char**)malloc(j * sizeof(char*));
	j=max_length(map, size);
	char* strings = (char*)malloc(j * sizeof(char));
	int k = 0;
	for (i = 0; i < size; )
	{
		j = 0;

		while (k < size)
		{
			if (map[k] == '\n')
			{
				strings[j] = '\n';
				j++;
				k++;
				break;
			}

			strings[j] = map[k];
			j++;
			k++;
		}
		sort[i++] = _strdup(strings);
		if (k == size)
		{
			break;
		}
	}
	ctr = i;
	qsort(sort, ctr, sizeof(char*), scmp);
	
	for (i = 0; i < ctr; i++)
	{
		char* wow = sort[i];

		while (*wow != '\n')
		{
			y++;
			wow++;
		}

		if (*wow == '\n')
		{
			y++;
		}
	}

	char* sort_map = (char**)malloc(y * sizeof(char*));
	k = 0;

	for (i = 0; i < ctr; i++)
	{
		char* wow = sort[i];

		while (*wow != '\n')
		{
			sort_map[k] = *wow;
			k++;
			wow++;
		}

		if (*wow == '\n')
		{
			sort_map[k] = '\n';
			k++;
		}

		free(sort[i]);
	}
	_write(out, sort_map, y);
	free(sort_map);
	free(sort);	
	munmap(map, size);
	_close(in);
	_close(out);
	return 0;
}
