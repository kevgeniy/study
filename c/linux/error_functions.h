#ifndef ERROR_FUNCTIONS_H
#define ERROR_FUNCTIONS_H

void errMsg_f(const char *format, ...);

#ifdef __GNUC__
	#define NORETURN __attribute__ ((__noreturn__))
#else
	#define NORETURN 
#endif

void errExit_f(const char *format, ...) NORETURN ;
void fatal_f(const char *format, ...) NORETURN ;
void usageErr_f(const char *format, ...) NORETURN ;

#define PRINT_DEBUG	printf("%s, %s:%d: ", __FILE__, __PRETTY_FUNCTION__, __LINE__)

#define DEBUG_WRAPPER(func, format, ...)	\
	do {									\
		PRINT_DEBUG;						\
		func(format, ##__VA_ARGS__);		\
	} while(0)							

#define errMsg(format, ...)	DEBUG_WRAPPER(errMsg_f, format, ##__VA_ARGS__)
#define errExit(format, ...) DEBUG_WRAPPER(errExit_f, format, ##__VA_ARGS__)
#define usageErr(format, ...) DEBUG_WRAPPER(usageErr_f, format, ##__VA_ARGS__)

#endif

