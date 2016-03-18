// Common header file

#ifndef TLPI_HDR_H
#define TLPI_HDR_H

#include <sys/types.h>	// type definitions
#include <stdio.h>		// I/O functions
#include <stdlib.h>		// commonly used functions + EXIT_SUCCESS &
						// EXIT_FAILURE constants

#include <unistd.h>		// prototypes for system calls
#include <errno.h>		// errno and error constants
#include <string.h>		// string-handling utilites

#include "get_num.h"	// handling numeric arguments (getInt(), getLong())
#include "error_functions.h"	//error-handling functions

typedef enum { FALSE, TRUE } Boolean;

#define min(m, n) ((m) < (n) ? (m) : (n))
#define max(m, n) ((m) > (n) ? (m) : (n))

#endif
