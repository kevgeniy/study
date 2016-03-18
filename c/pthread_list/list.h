#ifndef LIST_H
#define LIST_H
#include <pthread.h>

struct list {
	struct list_element *head;
	struct list_element *tail;
	int current_length;
	pthread_mutex_t *mutex;
};

struct list_element {
	pthread_mutex_t *mutex;
	void *element;
	struct list_element *next;
};

int list_init(struct list *list);

int list_add(struct list *list, void *element);
int list_remove(struct list *list, void *element, int (*compare) (void *, void *), void (*el_free)(void *));
struct list_element *list_find(struct list *list, void *element, int (*compare)(void *, void*));
int list_for_each(struct list *list, void *(*func)(void *));

int list_destroy(struct list *list, void (*el_free)(void *));
#endif

