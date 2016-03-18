#include <stdio.h>
#include "list.h"
#include <stdlib.h>
#include <string.h>
#include <pthread.h>

void *find_words(void*);

int main(void) {
	struct list *list = (struct list *)malloc(sizeof(struct list));
	list_init(list);
	char *string = "abc 123, 135,,,,34,4!sdf";
	void **arg = (void **)malloc(sizeof(void) * 3);
	arg[0] = list;
	arg[1] = string;
	arg[2] = string + strlen(string) - 1;
	map_reduce(1, string);
	find_words((void*)arg);

	int i;
	for(i = 0; i < list->current_length; ++i) {
		printf("%s\n", (char*)list->head[i]);
	}

}

int is_separator(char *simbol, char *separators) {
	while(*separators != 0) {
		if(*simbol == *separators)
			return 1;
		++separators;
	}
	return 0;
}

void *find_words(void *arg) {
	struct list *list;
	char *begin, *end;
	char *new_begin, *new_str;
	int i, is_unique;
	void **pointer;

	pointer = (void **)arg;

	list = (struct list *)(pointer[0]);
	begin = (char *)(pointer[1]);
	end = (char *)(pointer[2]);

	while(begin != end) {
		while(begin <= end && is_separator(begin, ",.?!"))
			++begin;
		if(begin > end)
			return NULL;
		new_begin = begin;
		while(new_begin <= end && !is_separator(new_begin, ",.?!"))
			++new_begin;

		--new_begin;

		pthread_mutex_lock(list->mutex);
		is_unique = 0;
		for(i = 0; i < list->current_length; ++i) {
			if(strncmp(list->head[i], begin, new_begin - begin + 1) == 0) {
				is_unique = 1;
				break;
			}
		}

		if(is_unique == 0) {
			new_str = (char *)malloc(sizeof(char) * (new_begin - begin + 2));
			memcpy(new_str, begin, new_begin - begin + 1);
			new_str[new_begin - begin + 1] = '\0';
			list_add(list, new_str);
		}
		pthread_mutex_unlock(list->mutex);
	}
	free(arg);
	return NULL;
}

int map_reduce(int number_threads, char *string) {
	int length, delta_length, i, begin_offset, end_offset, cur_pthread;
	struct list *list;
	void **arg;
	pthread_t *pthreads;

	list = (struct list*)malloc(sizeof(struct list));
	list_init(list);

	pthreads = (pthread_t *)malloc(sizeof(pthread_t)*number_threads);
   
	length = strlen(string);
	delta_length = length / number_threads;

	begin_offset = 0;
	cur_pthread = 0;
	for(i = 0; i < number_threads; ++i) {
		if(i == number_threads - 1)
			end_offset = length;
		else {
			end_offset = delta_length * i;
			while(end_offset >= begin_offset && !is_separator(string + end_offset, ",.?!"))
				--end_offset;
		}
		if(end_offset >= begin_offset) {
			arg = (void **)malloc(sizeof(void) * 3);
			arg[1] = list;
			arg[2] = string + begin_offset;
			arg[3] = string + end_offset;
			pthread_create((pthreads + cur_pthread), NULL, find_words, (void*)arg);
		}
		begin_offset = end_offset;
	}

	for(i = 0; i < number_threads; ++i)
		pthread_join(pthreads[i], NULL);
	free(pthreads);
	

	for(i = 0; i < list->current_length; ++i)
		free(list->head + i);
	list_destroy(list);
	free(list);
}

