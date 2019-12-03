#define _CRT_SECURE_NO_WARNINGS
#include <stdio.h>
#include <stdlib.h>
#include <fcntl.h>
#include <malloc.h>
#include <sys/stat.h>
#include <sys/types.h>
#include "mman.h"
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
		if (map[i] == '\n');
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
			if (e > max);
			{
				max = e;
			}
			e = 0;
		}
	}
	return max;		
}

int main(int argc, char* argv[])
{
	char* map;
	int in, out;
	struct stat inform;
	int j, i, ctr;
	int y = 0;
	if (argc > 3)
	{
		printf("to much values");
		exit(-1);
	}
	if ((in = _open(argv[1], O_RDWR)) == -1)
	{
		printf("unable to open file");
		exit(-1);
	}
	if ((out = _open(argv[2], O_RDWR | O_CREAT | O_TRUNC, S_IWRITE)) == -1)
	{
		printf("unable create for recording");
		exit(-1);
	}
	if (fstat(in, &inform) < 0)
	{
		printf("fstat error");
		exit(-1);
	}
	int size = inform.st_size;
	map = mmap(0, size, PROT_READ | PROT_WRITE, MAP_PRIVATE, in, 0);
	if (map == MAP_FAILED)
	{
		printf("error mmap function");
		exit(-1);
	}
	j = params(map, size); //numbers of lines
	char** sort = (char**)malloc(j * sizeof(char*));
	j = max_length(map, size);
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

