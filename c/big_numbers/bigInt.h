#ifndef BIGINT_H
#define BIGINT_H
#include <math.h>

#define type unsigned long long
typedef struct {
    int sign;
    int length; // length of the integer
    int length_full; // length of the digits array
    type *digits;
    unsigned int radix;
} bigInt;

// desirable (actualLength - length) for struct bigInt
extern const int length_spare;
extern const unsigned int radix;

/* constructors */
bigInt *new_bigInt(void);

bigInt *new_bigInt_array_length(const int length);

bigInt *new_bigInt_length(const int length);

bigInt *new_bigInt_int(const int number);

bigInt *new_bigInt_array(const type *array, const int length);

/* destructors */
void destruct_bigInt(bigInt* first);

bigInt *deep_copy(const bigInt *first);

bigInt *light_copy(const bigInt *first);

#endif // BIGINT_H
