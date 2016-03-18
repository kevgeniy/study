#ifndef SHEDULE_H
#define SHEDULE_H

#include "schedule.h"
#include "task.h"
#include "rq.h"
#include <stdlib.h>
#include <stdio.h>

int schedule_next(struct schedule *schedule) {
	int queue;
	struct task *task;

	if(schedule == NULL || schedule->active == NULL || schedule->expired == NULL)
		return EXIT_FAILURE;

	if(schedule->active->task_count == 0) {
		void *temp = schedule->active;
		schedule->active = schedule->expired;
		schedule->expired = temp;
	}

	if((queue = rq_first_queue(schedule->active)) < 0)
		return EXIT_FAILURE;

	if((task = rq_pop(schedule->active, queue)) == NULL)
		return EXIT_FAILURE;

	if(task_execute(task) < 0)
		return EXIT_FAILURE;

	if(task_recalculate_priority(task) < 0)
		return EXIT_FAILURE;

	if(task_recalculate_timeslice(task) < 0)
		return EXIT_FAILURE;

	if(task_is_interactive(task) != 0)
		rq_add(schedule->active, task);
	else
		rq_add(schedule->expired, task);

	return EXIT_SUCCESS;
}

int schedule_init(struct schedule *schedule) {
	int queue_count;
   
	if(schedule == NULL)
		return EXIT_FAILURE;

	if((schedule->active = (struct rq*)malloc(sizeof(struct rq))) == NULL)
		return EXIT_FAILURE;	
	if((schedule->expired = (struct rq*)malloc(sizeof(struct rq))) == NULL)
		return EXIT_FAILURE;

	queue_count	= MAX_PRIO - MIN_PRIO + 1;
	if(rq_init(schedule->active, queue_count) < 0)
		return EXIT_FAILURE;
	if(rq_init(schedule->expired, queue_count) < 0)
		return EXIT_FAILURE;

	return EXIT_SUCCESS;
}

int schedule_add_task(struct schedule *schedule, int nice) {
	struct task *task;

    if(schedule == NULL)
		return EXIT_FAILURE;

	task = (struct task *)malloc(sizeof(struct task));
	task_init_random(task, nice);

	return rq_add(schedule->active, task);
}

int schedule_add_task_random(struct schedule *schedule) {
	int nice;

	nice = rand() % (MAX_PRIO - MIN_PRIO + 1) + MIN_PRIO;
	return schedule_add_task(schedule, nice);
}

int schedule_destroy(struct schedule *schedule) {
	if(rq_destroy(schedule->active) < 0)
		return EXIT_FAILURE;
	if(rq_destroy(schedule->expired))
		return EXIT_FAILURE;

	free(schedule);

	return EXIT_SUCCESS;
}

int schedule_print(struct schedule *schedule) {
	printf("ACTIVE\n");
	rq_print(schedule->active);
	printf("EXPIRED\n");
    rq_print(schedule->expired);

	return EXIT_SUCCESS;
}

#endif
