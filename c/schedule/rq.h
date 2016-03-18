#ifndef RQ_H
#define RQ_H
#include "queue.h"
#include "task.h"

struct rq {
	struct queue *head;
	int queue_count;
	int task_count;
};

int rq_init(struct rq *rq, int count);

int rq_add(struct rq *rq, struct task *element);
struct task *rq_pop(struct rq *rq, int priority);

int rq_first_queue(struct rq *rq);

// TODO
int rq_print(struct rq *rq);

int rq_destroy(struct rq *rq);
#endif
