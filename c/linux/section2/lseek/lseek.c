#include <stdio.h>		// fprintf(), stderr, BUFSIZ
#include <errno.h>		// errno
#include <fcntl.h>		// flags for open()
#include <string.h>		// strerror()
#include <stdlib.h>
#include <unistd.h>		// ssize_t
#include <sys/types.h>	// off_t
#include <sys/stat.h>	// mode_t

struct person {
	char name[10];
	char id[10];
	off_t pos;
} people[] = {
	{ .name = "arnold", .id = "123456789", .pos = 0 },
	{ .name = "miriam", .id = "987654321", .pos = 10240 },
	{ .name = "joe", .id = "192837465", 81920 },
};

static void errMsg(char *progName, char *fileName, char *usrMsg);

int main(int argc, char **argv) {
	int fd;
	int i;

	// check for correct arguments
	if(argc < 2) {
		fprintf(stderr, "usage: %s file\n", argv[0]);
		exit(EXIT_FAILURE);
	}

	// correctly open file with name argv[1]
	fd = open(argv[1], O_RDWR | O_CREAT | O_TRUNC, 0666);
	if(fd < 0) {
		errMsg(argv[0], argv[1], "cannot open for read/write:");
		exit(EXIT_FAILURE);
	}

	for(i = 0; i < sizeof(people) / sizeof(people[0]); i++) {
		if(lseek(fd, people[i].pos, SEEK_SET) < 0) {
			errMsg(argv[0], argv[1], "seek error");
			(void) close(fd);
			exit(EXIT_FAILURE);
		}
		if(write(fd, &people[i], sizeof(people[i])) != sizeof(people[i])) {
			errMsg(argv[0], argv[1], "write error");
			(void) close(fd);
			exit(EXIT_FAILURE);
		}
	}
	(void)close(fd);
	exit(EXIT_SUCCESS);
}

static void errMsg(char *progName, char *fileName, char *usrMsg) {
	fprintf(stderr, "%s: %s: %s: %s\n", progName, fileName, usrMsg, strerror(errno));
}	


