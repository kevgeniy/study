#include <unistd.h>
#include <stdio.h>
#include <string.h>
#include <stdlib.h>

extern char **environ;

int main(int argc, char **argv) {
    int i;
    if(argc < 2) {
        fprintf(stderr, "environ: Too few arguments\n");
        fprintf(stderr, "Usage: environ <varialbe>\n");
        exit(1);
    }
    
    for(i = 0; environ[i] != NULL; i++) {
        if(!strncmp(environ[i], argv[1], strlen(argv[1]))) {
           printf("'%s' found\n", environ[i]);
		   exit(0);
        }
    }
    printf("'%s' not found\n", environ[i]);
    exit(0);
}
    
