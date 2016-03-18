#ifndef LIST_H
#define LIST_H

#include <pthread.h>

struct list {
	void **head;
	int current_length;
	int full_length;
	pthread_mutex_t *mutex;
};


int list_init(struct list *list);

int list_add(struct list *list, void *element);

int list_destroy(struct list *list);

#endif
