#include <stdio.h>
#include "list.h"
#include <stdlib.h>
#include <string.h>
#include <pthread.h>
#include <unistd.h>
#include <fcntl.h>
#include <sys/stat.h>

void *find_words(void*);

int map_reduce(int number_threads, char *string, struct list *list);

int main(void) {
    char *string;
    int cur_time, i, j;
    struct list *list;
    struct stat *buf;

    buf = (struct stat *) malloc(sizeof(struct stat));
    stat("./Don_Quixote.txt", buf);

    int fd = open("./Don_Quixote.txt", O_RDONLY);
    string = (char *)malloc(sizeof(char) * buf->st_size);
    read(fd, string, buf->st_size);

    list = (struct list *)malloc(sizeof(struct list));
    for(j = 2; j < 4; ++j) {
		list_init(list);
	
		cur_time = clock();
        map_reduce(j, string, list);
		cur_time = clock() - cur_time;

		printf("%d\n\n", cur_time);
		//for(i = 0; i < list->current_length; ++i)
		//	printf("%s\n", (char*)list->head[i]);

		for(i = 0; i < list->current_length; ++i)
			free(list->head[i]);
		list_destroy(list);
    }

	free(string);
    free(list);
    exit(0);
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
        while(begin <= end && is_separator(begin, " ,.?!"))
			++begin;
		if(begin > end)
			return NULL;
		new_begin = begin;
        while(new_begin <= end && !is_separator(new_begin, " ,.?!"))
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
        usleep(1000);
        begin = new_begin + 1;
	}
    pthread_exit(0);
}

int map_reduce(int number_threads, char *string, struct list *list) {
	int length, delta_length, i, begin_offset, end_offset, cur_pthread;
	void ***arg;
	pthread_t *pthreads;

    pthreads = (pthread_t *)malloc(sizeof(pthread_t)*number_threads);
	arg = (void ***)malloc(sizeof(void *) * number_threads);
   
	length = strlen(string);
	delta_length = length / number_threads;

	begin_offset = 0;
	cur_pthread = 0;
	for(i = 0; i < number_threads; ++i) {
		if(i == number_threads - 1)
			end_offset = length;
		else {
            end_offset = delta_length * (i + 1);
            while(end_offset >= begin_offset && !is_separator(string + end_offset, " ,.?!"))
				--end_offset;
		}
		if(end_offset >= begin_offset) {
			arg[cur_pthread] = (void **)malloc(sizeof(void *) * 3);
            arg[cur_pthread][0] = list;
            arg[cur_pthread][1] = string + begin_offset;
            arg[cur_pthread][2] = string + end_offset - 1;
			pthread_create((pthreads + cur_pthread), NULL, find_words, (void*)(arg[cur_pthread]));
            ++cur_pthread;
		}
		begin_offset = end_offset;
	}

    for(i = 0; i < cur_pthread; ++i) {
		pthread_join(pthreads[i], NULL);
		free(arg[i]);
	}

	free(pthreads);
	free(arg);

    return 0;
}
