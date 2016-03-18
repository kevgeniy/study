#ifndef CAT
#define CAT

#include <unistd.h>
#include <stdio.h>
#include <stdlib.h>
#include <errno.h>
#include <string.h>
#include <fcntl.h>

typedef enum { TRUE, FALSE } Boolean;

static int process(char *fname);

static void errMsg(char *fileName, char *format, ...);

#endif
