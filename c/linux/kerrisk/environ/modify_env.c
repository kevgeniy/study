#define _BSD_SOURCE
#define _SVID_SOURCE
#include "tlpi_hdr.h"

extern char **environ;

int main(int argc, char **argv) {
	int i;
	char **ep;
		
	clearenv();

	for(i = 1; i < argc; i++)
		if(putenv(argv[i]) != 0)
			errExit("putenv: %s", argv[i]);

	if(setenv("GEEK", "Hello, World!", 0) != 0)
		errExit("setenv: GEEK");

	if(unsetenv("BYE") != 0)
		errExit("unsetenv");

	for(ep = environ; *ep != NULL; ep++)
		puts(*ep);

	exit(EXIT_SUCCESS);
}



