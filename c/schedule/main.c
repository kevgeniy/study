#include "schedule.h"
#include <stdio.h>
#include <stdlib.h>

int main(void) {
    struct schedule *schedule;

    schedule = (struct schedule *)malloc(sizeof(struct schedule));

    schedule_init(schedule);
    schedule_add_task(schedule, 14);
    schedule_add_task(schedule, 16);
    schedule_add_task(schedule, 15);

    schedule_next(schedule);

    schedule_print(schedule);

    schedule_destroy(schedule);

    return EXIT_SUCCESS;
}
