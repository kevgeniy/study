#ifndef MULTIPLY_H
#define MULTIPLY_H

#include "bigInt.h"

bigInt *mult_bigInts(const bigInt *first, const bigInt *second);

type mult_overflow(const type first, const type second, type *result, const type value_max);

void mult_on_type(char *result, type multiplier, int max_length);

#endif // MULTIPLY_H
