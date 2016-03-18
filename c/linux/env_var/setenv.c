#include <stdlib.h>
#include <stdio.h>


int main(int arg, char **argv) {
	if(arg < 3) {
		fprintf(stderr, "%s: Too few arguments\n", argv[0]);
		fprintf(stderr, "Usage: %s <name> <value>\n", argv[0]);
	}
	if(setenv(argv[1], argv[2], 0) != 0) {
		fprintf(stderr, "%s: Cannot set '%s'\n", argv[0], argv[1]);
		exit(1);
	}
	printf("%s=%s\n", argv[1], getenv(argv[1]));
	exit(0);
}
