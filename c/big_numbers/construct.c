#include "bigInt.h"
#include <stdlib.h>
#include <string.h>
#include "compare.h"

const unsigned int radix = 17600;

const int length_spare = 3;

/* constructors */
bigInt *new_bigInt_array_length(int array_length) {
    bigInt *pointer = (bigInt*)malloc(sizeof(bigInt));
    pointer->sign = 0;
    pointer->length = 0;
    pointer->length_full = array_length + length_spare;
    pointer->radix = radix;
    pointer->digits = (type*)malloc(sizeof(type) * (array_length + length_spare));
    return pointer;
}

bigInt *new_bigInt_length(int length) {
    bigInt *pointer = new_bigInt_array_length(length);
    pointer->length = length;
    return pointer;
}

bigInt *new_bigInt(void) {
    return new_bigInt_length(1);
}

bigInt *new_bigInt_int(const int value) {
    bigInt *pointer = new_bigInt_array_length(1);
    pointer->digits[0] = value;
    pointer->length = 1;
    pointer->sign = get_sign((type)value);
    return pointer;
}

bigInt *new_bigInt_array(const type *array, const int length) {
    bigInt *result = new_bigInt_array_length(length);
    result->length = length;
    result->sign = 1;
    memcpy(result->digits, array, length * sizeof(type));
    return result;
}

bigInt *deep_copy(const bigInt *first) {
    bigInt *result = new_bigInt_array(first->digits, first->length);
    result->sign = first->sign;
    return result;
}

bigInt *light_copy(const bigInt *first) {
    bigInt *second = (bigInt*)malloc(sizeof(bigInt));
    second->digits = first->digits;
    second->length = first->length;
    second->length_full = first->length_full;
    second->radix = first->radix;
    second->sign = first->sign;
    return second;
}

void destruct_bigInt(bigInt* first) {
    free(first->digits);
    free(first);
}
