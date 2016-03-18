#include <stdarg.h>
#include "error_functions.h"
#include "header.h"

#ifdef __GNUC__
__attribute__ ((__noreturn__))
#endif
static void
terminate(Boolean useExit3) {
	exit(EXIT_FAILURE);
}

static void
outputError(Boolean useErr, int err, const char *format, va_list ap) {
#define BUF_SIZE 100
	char buf[2 * BUF_SIZE], userMsg[BUF_SIZE], errText[BUF_SIZE];

	vsnprintf(userMsg, BUF_SIZE, format, ap);
	if(useErr) {
		snprintf(errText, BUF_SIZE, " [%s] ", strerror(err));
	}

	snprintf(buf, BUF_SIZE,"ERROR:%s %s\n", errText, userMsg);

	fflush(stdout);

	fputs(buf, stderr);
	fflush(stderr);
}

void 
errMsg_f(const char *format, ...) {
	va_list argList;
	int savedErrno;

	savedErrno = errno;

	va_start(argList, format);
	outputError(TRUE, errno, format, argList);
	va_end(argList);

	errno = savedErrno;
}

void
errExit_f(const char *format, ...) {
	va_list argList;

	va_start(argList, format);
	outputError(TRUE, errno, format, argList);
	va_end(argList);

	terminate(TRUE);
}

void
fatal_f(const char *format, ...) {
	va_list argList;
	va_start(argList, format);
	outputError(FALSE, 0, format, argList);
	va_end(argList);

	terminate(TRUE);
}

void
usageError_f(const char *format, ...) {
	va_list argList;

	fflush(stdout);
	fprintf(stderr, "Usage: ");
	va_start(argList, format);
	vfprintf(stderr, format, argList);
	va_end(argList);

	fflush(stderr);
	exit(EXIT_FAILURE);
}
