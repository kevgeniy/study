#ifndef SCHEDULE_H
#define SCHEDULE_H

struct schedule {
	struct rq *active;
	struct rq *expired;
	// int lock;
	//int last_swap;
};

int schedule_init(struct schedule *schedule);

int schedule_next(struct schedule *schedule);

int schedule_add_task_random(struct schedule *schedule);

int schedule_add_task(struct schedule *schedule, int nice);

int schedule_destroy(struct schedule *schedule);

int schedule_print(struct schedule *schedule);

#endif
