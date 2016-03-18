// setenv is better to be use if it can be
// putenv is not very portable and clear

#include <stdlib.h>
#include <stdio.h>
#include <string.h>

#define QUERY_MAX_SIZE	32

void print_evar(const char *variable) {
	char *value = getenv(variable);
	if(value == NULL) {
		printf("Error: '%s' is not set\n", variable);
		return;
	}
	printf("%s=%s\n", variable, value);
}

int main(void) {
	char *query_str = (char *) malloc(QUERY_MAX_SIZE);
	if(query_str == NULL) abort;

	int ret;
	strncpy(query_str, "FOO=foo_value1", QUERY_MAX_SIZE - 1);
	ret = putenv(query_str); // add evnironmental variable
	if(ret != 0) abort;
	print_evar("FOO");

	strncpy(query_str, "FOO=foo_value2", QUERY_MAX_SIZE - 1);	// change environmental variable
	print_evar("FOO");

	strncpy(query_str, "FOO", QUERY_MAX_SIZE - 1);
	ret = putenv(query_str);	// remove environmental variable
	if(ret != 0) abort;
	print_evar("FOO");
	
	free(query_str);
	exit(0);
}

	
