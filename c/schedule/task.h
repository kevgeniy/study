#ifndef TASK_H
#define TASK_H

#define HIST_SIZE	3

#define MAX_PRIO	39
#define MIN_PRIO	0

#define DELTA_PRIO	5

#define MIN_TIME	10
#define MAX_TIME	200
#define DEF_TIME	100

struct task {
	int nice;
	int priority;
	int timeslice;
	// io_part + cpu_part = 100
	int io_part, cpu_part;
	int history[2 * HIST_SIZE];
};

int task_init(struct task *task, int nice, int io_part, int cpu_part);

int task_init_random(struct task *task, int nice);

int task_execute(struct task *task);

int task_destroy(struct task *task);

int task_recalculate_priority(struct task *task);

int task_recalculate_timeslice(struct task *task);

int task_is_interactive(struct task *task);

int task_print(struct task *task);
#endif
