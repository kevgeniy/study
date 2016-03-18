#include <stdio.h>
#include <unistd.h>
#include <pthread.h>
#include <stdlib.h>
#include <string.h>

char *message = "Hello World!";

void *thread_function(void *);

int main(void) {
	int res;
	pthread_t thread_id;
	void *thread_result;

	res = pthread_create(&thread_id, NULL, thread_function, (void *)message);
	
	if(res != 0) {
		perror("Thread creation failed");
		exit(EXIT_FAILURE);
	}
	printf("Waiting for thread to finish...\n");
	res = pthread_join(thread_id, &thread_result);
	if(res != 0) {
		perror("Thread join failed");
		exit(EXIT_FAILURE);
	}
	printf("Thread joined, it returned %s\n", (char *)thread_result);
	printf("Message is now %s\n", message);
	exit(EXIT_SUCCESS);
}

void *thread_function(void *arg) {
	printf("thread_function is running. Argument was %s\n", (char *)arg);
	sleep(3);
	pthread_exit("Thank you for the CPU time");
}
