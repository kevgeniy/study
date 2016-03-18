#include "task.h"
#include <stdlib.h>
#include <unistd.h>
#include <stdio.h>

int task_init(struct task *task, int nice, int io_part, int cpu_part) {
	int i;

	if(task == NULL || io_part < 0 || nice < MIN_PRIO || nice > MAX_PRIO)
		return EXIT_FAILURE;

	task->nice = nice;
	task->priority = nice;

	task->timeslice = DEF_TIME;

	task->io_part = io_part;
	task->cpu_part = cpu_part;

	for(i = 0; i < 2 * HIST_SIZE; ++i)
		task->history[i] = 0;

	return EXIT_SUCCESS;
}

int task_init_random(struct task *task, int nice) {
	int io_part;
	
	io_part = rand() % 1000;
	
	return task_init(task, nice, io_part, 1000 - io_part); 
}

int task_execute(struct task *task) {
	int i;

	if(task == NULL)
		return EXIT_FAILURE;

	if(usleep(1000 * task->timeslice) < 0)
		return EXIT_FAILURE;

	for(i = 2 * HIST_SIZE - 1; i > 1; --i)
		task->history[i] = task->history[i - 2];

	task->history[0] = task->io_part;
	task->history[1] = task->cpu_part;

	return EXIT_SUCCESS;
}

int task_destroy(struct task *task) {
	if(task == NULL)
		return EXIT_FAILURE;

	// free(task->history); TODO
    //free(task);
	return EXIT_SUCCESS;
}

static double avg_io_cpu(struct task *task) {
	double delta;
	int i;

	if(task == NULL)
		return EXIT_FAILURE;

	delta = 0;
	for(i = 0; i < HIST_SIZE; ++i)
        if(!(task->history[2 * i] + task->history[2 * i + 1] == 0))
		delta += (task->history[2 * i] - task->history[2 * i + 1]) / (task->history[2 * i] + task->history[2 * i + 1]);

	return delta / HIST_SIZE;
}

int task_recalculate_priority(struct task *task) {
	if(task == NULL)
		return EXIT_FAILURE;

	task->priority += (int)(avg_io_cpu(task) * DELTA_PRIO);
	
	if(task->priority < MIN_PRIO)
		task->priority = MIN_PRIO;
	if(task->priority > MAX_PRIO)
		task->priority = MAX_PRIO;
	
	return task->priority;	

}

int task_recalculate_timeslice(struct task *task) {
	if(task == NULL)
		return EXIT_FAILURE;

	task->io_part = rand() % 1000;
	task->cpu_part = 1000 - task->io_part;

	task->timeslice = (MAX_PRIO - task->priority) * (MAX_TIME - MIN_TIME) / (MAX_PRIO - MIN_PRIO) + MIN_TIME;

	return task->timeslice;
}

int task_is_interactive(struct task *task) {
	double inter_priority;

	inter_priority = avg_io_cpu(task) * (MAX_PRIO - MIN_PRIO) + task->nice;

	if(inter_priority < MIN_PRIO + (MAX_PRIO - MIN_PRIO) / 4.0)
		return 1;

	return 0;
}

int task_print(struct task *task) {
	int i;

	printf("\t--nice: %d; priority: %d; timeslice: %d; io_part: %d; cpu_part: %d;", task->nice, task->priority, task->timeslice, task->io_part, task->cpu_part);
	printf("\n\t--");
	for(i = 0; i < HIST_SIZE; ++i)
		printf("io_part%d: %d, cpu_part%d: %d", i, task->history[2 * i], i, task->history[2 * i + 1]);

	return EXIT_SUCCESS;
}
