#define _GNU_SOURCE
#include <stdio.h>
#include <errno.h>

int main(int arc, char **argv) {
	printf("%s\n", argv[0]);
	printf("%s\n", program_invocation_name);
	printf("%s\n", program_invocation_short_name);
	return 0;
}
