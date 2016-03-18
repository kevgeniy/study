#include <unistd.h>
#include <stdio.h>
#include <stdlib.h>

int main(void) {
	printf("%ld", sysconf(_SC_PAGESIZE));
	exit(EXIT_SUCCESS);
}
