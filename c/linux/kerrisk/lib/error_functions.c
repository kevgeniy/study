#include <stdarg.h>
#include "error_functions.h"
#include "tlpi_hdr.h"
#include "ename.c.inc"

#ifdef __GNUC__
__attribute__ ((__noreturn__))
#endif
static void terminate(Boolean useExit3) {
	char *s;

	// Check dump core flag

	s = getenv("EF_DUMPCORE");

	if(s != NULL && *s != '\0')
		abort();
	else if(useExit3)
		exit(EXIT_FAILURE); // exit-3
	else
		_exit(EXIT_FAILURE); // exit-2
}

static void outputError(Boolean useErr, int err, Boolean flushStdout, const char *format, va_list ap) {
#define BUF_SIZE 500
	char buf[2 * BUF_SIZE], userMsg[BUF_SIZE], errText[BUF_SIZE];
	//fill userMsg
	vsnprintf(userMsg, BUF_SIZE, format, ap);
	// fill errText
	if(useErr)
		snprintf(errText, BUF_SIZE, " [%s %s]", (err > 0 && err <= MAX_ENAME) ?
				ename[err] : "?UNKNOWN?", strerror(err));
	else
		snprintf(errText, BUF_SIZE, ":");
	//fill buf
	snprintf(buf, 2 * BUF_SIZE,"ERROR%s %s\n", errText, userMsg);

	if(flushStdout)
		fflush(stdout);
	// display summary error message
	fputs(buf, stderr);
	fflush(stderr);
}

void errMsg_f(const char *format, ...) {
	va_list argList;
	int savedErrno;

	savedErrno = errno; // if we change it here

	va_start(argList, format);
	outputError(TRUE, errno, TRUE, format, argList);
	va_end(argList);

	errno = savedErrno;
}

void errExit_f(const char*format, ...) {
	va_list argList;

	va_start(argList, format);
	outputError(TRUE, errno, TRUE, format, argList);
	va_end(argList);

	terminate(TRUE);
}

void err_exit_f(const char *format, ...) {
	va_list argList;

	va_start(argList, format);
	outputError(TRUE, errno, FALSE, format, argList);
	va_end(argList);

	terminate(FALSE);
}

void errExitEN_f(int errnum, const char *format, ...) {
	va_list argList;

	va_start(argList, format);
	outputError(TRUE, errnum, TRUE, format, argList);
	va_end(argList);

	terminate(TRUE);
}

void fatal_f(const char *format, ...) {
	va_list argList;

	va_start(argList, format);
	outputError(FALSE, 0, TRUE, format, argList);
	va_end(argList);

	terminate(TRUE);
}

void usageErr_f(const char *format, ...) {
	va_list argList;
	
	fflush(stdout); // flush and pending stdout
	
	fprintf(stderr, "Usage: ");
	va_start(argList, format);
	vfprintf(stderr, format, argList);
	va_end(argList);

	fflush(stderr);	// in case stderr is not line-buffered
	exit(EXIT_FAILURE);
}

void cmdLineErr_f(const char *format, ...) {
	va_list argList;

	fflush(stdout);
	
	fprintf(stderr, "Command-line usage error: ");
	va_start(argList, format);
	vfprintf(stderr, format, argList);
    va_end(argList);	

	fflush(stderr);
	exit(EXIT_FAILURE);
}

