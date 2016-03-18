#include "../lib/tlpi_hdr.h"


int main(int argc, char **argv) {
	int xcnt = 0, opt;
	char *pstr = NULL;

	while((opt = getopt(argc, argv, ":p:x")) != -1) {
		switch(opt) {
		case ':':
			printf("Usage error: [-%c] argument expected", optopt);
			exit(EXIT_FAILURE);
		case '?':
			printf("Usage error: unrecognized option [-%c]", optopt);
			exit(EXIT_FAILURE);
		case 'p':
			pstr = optarg;
			break;
		case 'x':
			xcnt++;
			break;
		default:
			fatal("Unexpected case in switch()");
		}
	}

	if(xcnt != 0)
		printf("-x was specified (count=%d)\n", xcnt);
	if(pstr != NULL)
		printf("-p was specified with argument \"%s\"\n", pstr);
	if(optind < argc)
		printf("First nonoption argument is \"%s\" at argv[%d]\n", argv[optind], optind);

	exit(EXIT_SUCCESS);
}
	
		
