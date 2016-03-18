#include <stdio.h>
#include <unistd.h>
#include <fcntl.h>
#include <stdlib.h>

int main(void) {
	pid_t pid;
	int i, fd, count, return_status, write_status;
	char *filename = "1.txt", *message;


	fd = open(filename, O_CLOEXEC | O_CREAT | O_EXCL | O_WRONLY, S_IRWXU);

	pid = fork();
	switch(pid) {
		case -1:
			perror("fork failed");
			exit(1);
			break;
		case 0:
			message = "FORKED ";
			count = 10;
			break;
		default:
			message = "PARENT ";
			count = 2;
			break;
	}

	for(i = 0; i < count; ++i) {
		write_status = write(fd, message, 7);
		if(write_status < 0)
			perror("write failed");
		sleep(1);
	}

	exit(return_status);
}
