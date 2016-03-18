#ifndef ERROR_FUNCTIONS
#define ERROR_FUNCTIONS


// Errors from system calls and library functions

void errMsg_f(const char *format, ...);	// prints error message to stderr, work
										// like printf with auto \n, prints text
										// based on errno value

#ifdef __GNUC__

	// This macro stops 'gcc -Wall' complaining that "control reaches end of
	//  non-void function" if we use the following functions to
	// terminate main() or some other non-void function. 
#define NORETURN __attribute__ ((__noreturn__))
#else
#define NORETURN
#endif

void errExit_f(const char *format, ...) NORETURN; // errMsg + exit |
												// (if EF_DUMPCORE != null) abort()

void err_exit_f(const char *format, ...) NORETURN; // similar to errExit but doesn't flush output before error message
												 // call _exit : terminate without flushing stdio/invoking exit handlers
												 // especially useful if we write a library function that creates 
												 // a child process that needs to terminate because of an error

void errExitEN_f(int errnum, const char *format, ...) NORETURN; // specifies error number. Useful for posix functions,
															  // they return positive errors or 0 on success.
															  // we couldn't simply assign it to errno because in posix
															  // errno is a global macro


// Other errors

void fatal_f(const char *format, ...) NORETURN;	// general errors without errno. prints the formated
												// output and call errExit

void usageErr_f(const char *format, ...) NORETURN; // errors in command-line argument usage
												 // terminates with exit()

void cmdLineErr_f(const char *format, ...) NORETURN; // similar to usageErr but diagnose errors
												   // in specified command-line arguments
#define PRINT_DEBUG printf("%s, %s: %d: ", __FILE__, __PRETTY_FUNCTION__, __LINE__)

#define DEBUG_WRAPPER(func, format, ...)	\
	do {									\
		PRINT_DEBUG;						\
		func(format, ##__VA_ARGS__);		\
	} while(0)								

#define errExit(format, ...) DEBUG_WRAPPER(errExit_f, format, ##__VA_ARGS__);
#define err_exit(format, ...) DEBUG_WRAPPER(err_exit_f, format, ##__VA_ARGS__);
#define fatal(format, ...) DEBUG_WRAPPER(fatal_f, format, ##__VA_ARGS__);
#define usageErr(format, ...) DEBUG_WRAPPER(usageErr_f, format, ##__VA_ARGS__);
#define cmdLineErr(format, ...) DEBUG_WRAPPER(cmdLineErr_f, format, ##__VA_ARGS__);

#define errExitEN(errnum, format, ...)				\
	do {											\
		PRINT_DEBUG;								\
		errExitEN_f(errnum, format, ##__VA_ARGS_);	\
	} while(0)										
#endif
