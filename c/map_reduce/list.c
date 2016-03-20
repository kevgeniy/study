#include <stdlib.h>
#include <string.h>
#include "list.h"

int list_init(struct list *list) {
	if(list == NULL)
		return EXIT_FAILURE;	
	list->head = (void **)malloc(sizeof(void *) * 4);
	list->current_length = 0;
	list->full_length = 4;
    list->mutex = (pthread_mutex_t *)malloc(sizeof(pthread_mutex_t));
	pthread_mutex_init(list->mutex, NULL);
	return EXIT_SUCCESS;
}

int list_add(struct list *list, void *element) {
    int i;
	if(list->current_length == list->full_length) {
        list->full_length *= 2;
        void **new_array = (void **)malloc(sizeof(void *) * list->full_length);

        for(i = 0; i < list->current_length; ++i)
            new_array[i] = (list->head)[i];
        //memcpy(new_array, list->head, list->current_length);
		free(list->head);
		list->head = new_array;
	}	

	*(list->head + list->current_length) = element;
    ++(list->current_length);
	return EXIT_SUCCESS;
}


int list_destroy(struct list *list) {
	pthread_mutex_destroy(list->mutex);
	free(list->mutex);
	free(list->head);
    //free(list);
    return 0;
}

