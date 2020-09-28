#define _CRT_SECURE_NO_WARNINGS
#include <stdio.h>
#include <malloc.h>

typedef struct Dawn
{
	int key;
	int value;
	struct Dawn* next;
} Dawn;


int hash_func(int key)
{

	key *= key;

	key >>= 11;

	return key % 1024;

}

typedef struct Hash_table
{
	int size;
	int array_size;
	int limit;
	Dawn** array;
	float percent;
	int increase;
} Hash_table;

Hash_table* create(int limit, float percent, int increase)
{
	Hash_table* card = (Hash_table*)malloc(sizeof(Hash_table));
	card->size = 0;
	card->increase = increase;
	card->array_size = limit;
	card->percent = percent;
	card->limit = (int)(card->percent * card->array_size);
	card->array = (Dawn * *)calloc(card->array_size, sizeof(Dawn*));
	return card;
}

void insert(Hash_table** card, int key, int value)
{
	Hash_table* table = *card;
	unsigned long long hash = hash_func(key);
	int index = (hash % table->array_size);
	if (table->size < table->limit)
	{
		if (table->array[index] == NULL)
		{
			Dawn* couple = (Dawn*)malloc(sizeof(Dawn));
			couple->next = NULL;
			couple->key = key;
			couple->value = value;
			table->array[index] = couple;
		}

		else
		{
			Dawn* assist = table->array[index];
			while (assist->next)
			{

				if (assist->key == key)
				{
					printf("please, check the key");
					exit(-5);
				}
				assist = assist->next;
			}
			Dawn* couple = NULL;
			couple = (Dawn*)malloc(sizeof(Dawn));
			couple->next = NULL;
			couple->value = value;
			couple->key = key;
			assist->next = couple;
		}

	}
	else
	{

		*card = rebalancing(card, key, value);
	}
	(*card)->size++;
}


int rebalancing(Hash_table** card, int key, int value)
{
	Hash_table* table = (*card);
	Hash_table* new_table = create((int)(*card)->array_size * (*card)->increase, (*card)->percent, (*card)->increase);
	Dawn* assist = NULL;
	int size = table->array_size;
	for (int i = 0; i < size; i++) 
	{
		assist = table->array[i];
		while (assist) 
		{
			insert(&new_table, assist->key, assist->value);
			assist = assist->next;

		}
		free(assist);
	}

	free(table->array);
	free(*card);
	*card = new_table;
	insert(&new_table, key, value);
	return new_table;
}

void find(Hash_table* card, int key)
{
	unsigned long long hash = hash_func(key);
	int index = (hash % card->array_size);
	int c = 0;
	if (card->array[index] != NULL)
	{
		if ((card->array[index]->key) == key)
		{
			printf("for key = %d value is %d\n", key, card->array[index]->value);
			c = 1;
		}
		else 
		{
			Dawn* assist = card->array[index]->next;
			while (assist != NULL)
			{
				if (assist->key == key)
				{
					printf("for key = %d value is %d\n", key, assist->value);
					c = 1;
				}
				assist = assist->next;
			}
		}

	}

	if (c == 0)
	{
		printf("nothing was found for this key = %d\n", key);
	}
}


void remove_element(Hash_table* card, int key) 
{
	unsigned long long hash = hash_func(key);
	int index = (hash % card->array_size);
	if (card->array[index] != NULL) 
	{
		if (card->array[index]->key == key) 
		{
			printf("delete key %d and value %d\n", key, card->array[index]->value);
			card->array[index] = card->array[index]->next;
			card->size--;
		}
		else
		{

			Dawn* assist = card->array[index]->next;
			while (assist)
			{

				if (assist->key == key)
				{
					printf("delete key %d and value %d\n", key, assist->value);
					card->array[index]->next = assist->next;
					card->size--;
					break;
				}
				assist = assist->next;
			}
		}
	}
}

void clean(Hash_table** card) 
{
	Hash_table* table = *card;
	int i, size;
	Dawn* assist = NULL;
	Dawn* couple = NULL;

	size = table->array_size;

	for (i = 0; i < size; i++)
	{
		assist = table->array[i];
		while (assist)
		{
			couple = assist;
			assist = assist->next;
			free(couple);
		}
		free(assist);
	}

	free(table->array);
	free(*card);
	*card = NULL;
	printf("\ntable cleanup was successful");
}

void main()
{
	Hash_table* map = create(100, 0.72f, 3);
	for (int i = 0; i <= 100000; i++)
	{
		insert(&map, i, i);
	}
	printf("all elements are inserted\n\n");
	find(map, 43);
	find(map, 23214);
	find(map, 554);
	remove_element(map, 554);
	find(map, 554);
	find(map, 555);
	find(map, 909923);
	remove_element(map, 102000);
	remove_element(map, 45645);
	remove_element(map, 13123);
	int k = 0;
	while (scanf("%d", &k) != -1)
	{
		if (k == -1)
		{
			break;
		}
		find(map, k);
	}
	while (scanf("%d", &k) != -1)
	{
		if (k == -1)
		{
			break;
		}
		remove_element(map, k);
	}
	clean(&map);
}