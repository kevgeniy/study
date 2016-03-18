#ifndef ADD_SUB_H
#define ADD_SUB_H

#include "bigInt.h"

bigInt *add_bigInts(const bigInt *first, const bigInt *second);

bigInt *sub_bigInts(const bigInt *first, const bigInt *second);

bigInt *add_with_sign(const bigInt *first, const bigInt *second, int sign_operation);

bigInt *add_modulus(const bigInt *first, const bigInt *second);

bigInt *add_modulus_fast(bigInt *first, const bigInt *second);

bigInt *sub_modulus(const bigInt *first, const bigInt *second);

bigInt *sub_modulus_fast(bigInt *first, const bigInt *second);

int sub_overflow(type *first, const type second, const type value_max);

int add_overflow(type *first, const type second, const type value_max);

void add_to_stringInt(char *result, type number, int max_length);

#endif // ADD_SUB_H
