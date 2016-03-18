#ifndef HEADER_H
#define HEADER_H

#include <sys/types.h>	// type definitions

#include <unistd.h>		// system calls
#include <errno.h>		// errno, error constants

#include <stdio.h>		// standard I/O
#include <stdlib.h>		// common functions, EXIT_{SUCCESS, FAILURE}
#include <string.h>		// string operations

#include "error_functions.h"

#define min(m, n) ((m) < (n) ? m : n)
#define max(m, n) ((m) > (n) ? m : n)

#ifdef __GNUC__
#define likely(x) (__builtin_expect(!!(x), 1))
#define unlikely(x) (__builtin_expect(!!(x), 0))
#else
#define likely(x) (x)
#define unlikely(x) (x)
#endif

typedef enum { FALSE, TRUE } Boolean;

#endif

