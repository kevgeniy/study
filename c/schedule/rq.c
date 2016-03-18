#include "rq.h"
#include "task.h"
#include <stdlib.h>
#include <stdio.h>

int rq_init(struct rq *rq, int count) {
	int i;

	if(rq == NULL || count <= 0)
		return EXIT_FAILURE;
	
	rq->queue_count = count;
	rq->task_count = 0;

	rq->head = (struct queue*)malloc(sizeof(struct queue) * count);
    if(rq->head == NULL)
		return EXIT_FAILURE;

	for(i = 0; i < count; ++i)
		if(queue_init(rq->head + i) < 0)
			return EXIT_FAILURE;

	return EXIT_SUCCESS;
}

int rq_add(struct rq *rq, struct task *task) {
	if(rq == NULL || rq->queue_count <= task->priority - MIN_PRIO || task->priority < MIN_PRIO)
		return EXIT_FAILURE;

	queue_push_back(rq->head + task->priority - MIN_PRIO, (void *)task);

	++rq->task_count;
	return rq->task_count;
}

struct task *rq_pop(struct rq *rq, int priority) {
	if(rq == NULL || rq->queue_count <= priority || priority < 0)
		return NULL;

	--rq->task_count;
	return (struct task *)queue_pop(rq->head + priority);
}

int rq_first_queue(struct rq *rq) {
	int index = 0;

	if(rq == NULL)
		return EXIT_FAILURE;

	while(index < rq->queue_count && rq->head + index != NULL && queue_is_empty(rq->head + index))
		++index;

	return index;
}

int rq_destroy(struct rq *rq) {
	int i;
    struct queue_element *queue_element;

	if(rq == NULL)
		return EXIT_FAILURE;

    for(i = 0; i < rq->queue_count; ++i) {
        queue_element = (rq->head + i)->head;
        while(queue_element != NULL) {
            task_destroy(queue_element->element);
            queue_element = queue_element->next;
        }
		queue_destroy(rq->head + i);
    }

	free(rq->head);
	free(rq);

	return EXIT_SUCCESS;
}

int rq_print(struct rq *rq) {
	struct queue_element *queue_element;
	int i;

	for(i = 0; i < MAX_PRIO - MIN_PRIO; ++i) {
        printf("Queue № %d, %d elements\n", i, (rq->head + i)->count);
        queue_element = rq->head[i].head;
		while(queue_element != NULL) {
			task_print((struct task *)queue_element->element);
			printf("\n");
            queue_element = queue_element->next;
		}
	}

	return EXIT_SUCCESS;
}
