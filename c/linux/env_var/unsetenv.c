#include <stdlib.h>
#include <stdio.h>

char *variable = "USER";

int main(void) {
	int ret = unsetenv(variable);
	if(ret) {
		fprintf(stderr, "Cannot unset '%s'\n", variable);
		exit(1);
	}
	char *result = getenv(variable);
	if(!result)
		printf("'%s' has removed from the environment\n", variable);
	else
		printf("'%s' hasn't removed\n", variable);
	exit(0);
}
