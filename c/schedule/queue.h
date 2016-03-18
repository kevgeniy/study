#ifndef QUEUE_H
#define QUEUE_H

struct queue_element {
    void *element;
    struct queue_element *next;
};

struct queue {
	struct queue_element *head;
	struct queue_element *end;	
	int count;
};

int queue_init(struct queue *queue);

int queue_push_back(struct queue *queue, void *element);
void *queue_front(struct queue *queue);
void *queue_pop(struct queue *queue);

int queue_destroy(struct queue *queue);

int queue_is_empty(struct queue *queue);

#endif

