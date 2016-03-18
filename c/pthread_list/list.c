#include <stdlib.h>
#include "list.h"
#include <pthread.h>

int list_init(struct list *list) {
	struct list_element *list_element;

	list_element = (struct list_element *)malloc(sizeof(struct list_element));
	list_element->mutex = (pthread_mutex_t *)malloc(sizeof(pthread_mutex_t));
    pthread_mutex_init(list_element->mutex, NULL);
	list_element->element = NULL;
	list_element->next = NULL;
	list->head = list->tail = list_element;
	list->mutex = (pthread_mutex_t *)malloc(sizeof(pthread_mutex_t));
    pthread_mutex_init(list->mutex, NULL);
    return 0;
}

int list_add(struct list *list, void *element) {
    struct list_element *list_element, *tail;

	pthread_mutex_lock(list->mutex);
    tail = list->tail;
    pthread_mutex_lock(tail->mutex);

    if(tail->element == NULL)
		tail->element = element;
	else {
		list_element = (struct list_element *)malloc(sizeof(struct list_element));
		list_element->mutex = (pthread_mutex_t *)malloc(sizeof(pthread_mutex_t));
        pthread_mutex_init(list_element->mutex, NULL);
		list_element->element = element;
		list_element->next = NULL;

		tail->next = list_element;
        list->tail = list_element;
	}

    pthread_mutex_unlock(tail->mutex);
    pthread_mutex_unlock(list->mutex);

	return 0;
}

int list_remove(struct list *list, void *element, int (*compare)(void *, void *), void (*el_free)(void *)) {
	struct list_element *list_element, *del_element;

	pthread_mutex_lock(list->mutex);
	pthread_mutex_lock(list->head->mutex);
	
	list_element = list->head;
	if(compare(element, list_element->element) == 0) {
		if(list_element->next == NULL) {
            if(el_free != NULL)
                el_free(list_element->element);
			list_element->element = NULL;
            pthread_mutex_unlock(list->head->mutex);
		}
		else {
			el_free(list_element->element);
            pthread_mutex_unlock(list_element->mutex);
            pthread_mutex_destroy(list_element->mutex);
			free(list_element->mutex);
			list->head = list_element->next;
			free(list_element);
		}
        pthread_mutex_unlock(list->mutex);
		return 0;
	}

    if(list_element->next == NULL) {
        pthread_mutex_unlock(list->mutex);
        pthread_mutex_unlock(list->head->mutex);
		return -1;
    }

	del_element = list_element->next;
    if(del_element->next != NULL)
        pthread_mutex_unlock(list->mutex);
	pthread_mutex_lock(del_element->mutex);

	while(del_element->next != NULL) {
		if(compare(element, del_element->element) == 0) {
			list_element->next = del_element->next;
            if(el_free != NULL)
                el_free(del_element->element);
            pthread_mutex_unlock(del_element->mutex);
            pthread_mutex_destroy(del_element->mutex);
			free(del_element->mutex);
			free(del_element);
            pthread_mutex_unlock(list_element->mutex);
			return 1;
		}
		if(del_element->next->next == NULL)
			pthread_mutex_lock(list->mutex);
		pthread_mutex_lock(del_element->next->mutex);
		pthread_mutex_unlock(list_element->mutex);
		list_element = del_element;
		del_element = del_element->next;
	}
	if(compare(element, del_element->element) == 0) {
			list_element->next = NULL;
			pthread_mutex_unlock(del_element->mutex);
            if(el_free != NULL)
                el_free(del_element->element);
            pthread_mutex_destroy(del_element->mutex);
			free(del_element->mutex);
			free(del_element);
			list->tail = list_element;
            pthread_mutex_unlock(list_element->mutex);
			pthread_mutex_unlock(list->mutex);
			return 1;
		}
	return -1;
}

struct list_element *list_find(struct list *list, void *element, int (*compare)(void *, void *)) {
	struct list_element *list_element, *next;

	pthread_mutex_lock(list->mutex);
	pthread_mutex_lock(list->head->mutex);
	pthread_mutex_unlock(list->mutex);

	list_element = list->head;
	while(list_element->next != NULL) {
		if(compare(element, list_element->element) == 0)
			return list_element;

		pthread_mutex_lock(list_element->next->mutex);
		next = list_element->next;
		pthread_mutex_unlock(list_element->mutex);
		list_element = next;
	}
	if(compare(element, list_element->element) == 0)
		return list_element;

	return NULL;
}

int list_for_each(struct list *list, void *(*func)(void *)) {
	struct list_element *list_element, *next;

	pthread_mutex_lock(list->mutex);
	pthread_mutex_lock(list->head->mutex);
	pthread_mutex_unlock(list->mutex);

	list_element = list->head;
	while(list_element->next != NULL) {
		func(list_element->element);

        pthread_mutex_lock(list_element->next->mutex);
        next = list_element->next;
		pthread_mutex_unlock(list_element->mutex);
		list_element = next;
	}

    func(list_element->element);
	pthread_mutex_unlock(list_element->mutex);

	return 0;
}

int list_destroy(struct list *list, void (*el_free)(void*)) {
	struct list_element *head, *next;
    head = list->head;

    while(head != NULL) {
		next = head->next;
        pthread_mutex_destroy(head->mutex);
		free(head->mutex);
        if(el_free != NULL)
            el_free(head->element);
		free(head);
		head = next;
	}

    pthread_mutex_destroy(list->mutex);
	free(list->mutex);
	free(list);
    return 0;
}
