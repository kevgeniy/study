#ifndef GET_NUM_H
#define GET_NUM_H

#define GN_NONNEG	01  // value >= 0
#define GN_GT_0		02	// value > 0
	// by default integers are decimal base
#define GN_ANY_BASE	0100	// any base like strtol(3)
#define GN_BASE_8	0200	// value expressed in octal
#define GN_BASE_16	0400	// value expressed in hex

// return arg in numeric form, providing(unlike of atoi, atol, ...) some
// basic validity checking
int getInt(const char *arg, int flags, const char *name);

long getLong(const char *arg, int flags, const char *name);

#endif


