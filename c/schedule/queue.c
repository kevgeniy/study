#include "queue.h"
#include <stdlib.h>

int queue_init(struct queue *queue) {
    queue->head = queue->end = NULL;
	queue->count = 0;
	return 0;
}

int queue_push_back(struct queue *queue, void *element) {
	struct queue_element *queue_element;
 
	if(queue == NULL || element == NULL)	
		return -1;

    queue_element = (struct queue_element *)malloc(sizeof(struct queue_element));
    queue_element->element = element;
    queue_element->next = NULL;

	if(queue->head == NULL)
		queue->head = queue_element;
	else if(queue->end == NULL) {
		queue->end = queue_element;
		queue->head->next = queue_element;
	} else {
		queue->end->next = queue_element;
		queue->end = queue_element;
	}

	return ++(queue->count);
}

void *queue_front(struct queue *queue) {
	return queue->head == NULL || queue == NULL ? NULL : queue->head->element;
}

void *queue_pop(struct queue *queue) {
	void *element;
    struct queue_element *queue_element;

	if(queue == NULL || queue->head == NULL) 
		return NULL;

    queue_element = queue->head;
    element = queue_element->element;
    queue->head = queue_element->next;

    free(queue_element);
	
	--(queue->count);

	return element;
}

int queue_is_empty(struct queue *queue) {
	return queue == NULL || queue->count == 0;
}

int queue_destroy(struct queue *queue) {
	struct queue_element *next;

	if(queue == NULL)
		return 0;

	while(queue->head != NULL) {
		next = queue->head->next;
		free(queue->head);
		queue->head = next;
	}

	return 0;
}





