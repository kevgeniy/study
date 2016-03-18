#include "../header.h"
#include <fcntl.h>

int main(void) {

}

int dup(int oldfd) {
	return fcntl(oldfd, F_DUPFD, 0);
}

int dup2(int oldfd, int newfd) {
	if(fcntl(oldfd, F_GETFL) == -1)
		return -EBADF;

	if(oldfd == newfd)
		return newfd;

	close(newfd);
	return fcntl(oldfd, F_DUPFD, newfd);
}
