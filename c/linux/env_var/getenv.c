#include <stdio.h>
#include <stdlib.h>

int main(int argc, char **argv) {
	if(argc < 2) {
		fprintf(stderr, "getenv: Too few arguments\n"); 
		fprintf(stderr, "Usage: getenv <variable name>\n");
		exit(1);
	}
	char *result=getenv(argv[1]);
	if(result==NULL)
		printf("'%s' not found\n", argv[1]);
	else
		printf("'%s=%s' found\n", argv[1], result);
	exit(0);
}
