// our own cat version
// file I/O and error handling
	
#include "cat.h"
#include <stdarg.h>

int main(int argc, char **argv) {
	int i;

	if(argc == 1 && process("-") != 0)	// if no args, read from stdin
		errMsg(argv[0], "in 'process' on file -");

	for(i = 1; i < argc; ++i)
		if(process(argv[i]))
			errMsg(argv[0], "in 'process' on file %s", argv[i]);

	exit(EXIT_SUCCESS);
}

#define BUF_SIZE 500
static char bufStr[BUF_SIZE], usrStr[BUF_SIZE], errStr[BUF_SIZE];

static void errMsg(char *fileName, char *format, ...) {
	va_list argList;

	va_start(argList, format);
	vsnprintf(usrStr, BUF_SIZE, format, argList);
	va_end(argList);

	snprintf(errStr, BUF_SIZE, " '%s'", strerror(errno));

	snprintf(bufStr, BUF_SIZE, "%s: ERROR%s %s\n", fileName, errStr, usrStr);

	fflush(stdout);
	fputs(bufStr, stderr);
	fflush(stderr);
}

static char buf[BUFSIZ];
static int process(char *fname) {
	int fd;
	ssize_t wcount, rcount;

	if(strcmp(fname, "-") == 0)
		fd = 0;
	else if((fd = open(fname, O_RDONLY)) < 0) {
		close(fd);
		return EXIT_FAILURE;
	}
	
	while((rcount = read(fd, buf, BUFSIZ)) > 0)
		if(write(STDOUT_FILENO, buf, rcount) != rcount) {
			close(fd);
			return EXIT_FAILURE;
		}

	if(close(fd) < 0)
		return EXIT_FAILURE;
	if(rcount < 0)
		return EXIT_FAILURE;
	return EXIT_SUCCESS;
}

