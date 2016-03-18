#include <unistd.h>
#include <fcntl.h>
#include <stdlib.h>
#include "../lib/tlpi_hdr.h"

int main(void) {
	int fd1, fd2, fd3;
	char *file;

	file = "1.txt";

	fd1 = open(file, O_WRONLY | O_CREAT | O_TRUNC | O_EXCL, S_IRUSR | S_IWUSR);
	if(fd1 == -1)
		errMsg("open");
	if((fd2 = dup(fd1)) == -1)
		errMsg("dup");
	if((fd3 = open(file, O_RDWR)) == -1)
		errMsg("open");

	write(fd1, "Hello", 6);
	write(fd2, " world", 6);
	lseek(fd1, 0, SEEK_SET);
	write(fd1, "HELLO", 6);
	write(fd3, "Gidday", 6);

	if(close(fd1) == -1)
		errMsg("close1");
	if(close(fd2) == -1)
		errMsg("close2");
	if(close(fd3) == -1)
		errMsg("close3");

	exit(EXIT_SUCCESS);
}
