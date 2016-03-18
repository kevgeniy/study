//#include "bigInt.h"
#include "stdlib.h"
#include "multiply.h"
#include "add_sub.h"
#include "string.h"

bigInt *mult_bigInts(const bigInt *first, const bigInt *second) {
    bigInt *inter_result = new_bigInt_array_length(first->length + second-> length);
    inter_result->sign = 1;
    bigInt *result = NULL;

    for(int i = 0; i < first->length; i++) {
        type transfer_digit;
        for(int j = 0; j < second->length; j++) {
            type temp = transfer_digit;
            transfer_digit = mult_overflow(first->digits[i], second->digits[i], &inter_result->digits[i + j], inter_result->radix);
            transfer_digit += add_overflow(&inter_result->digits[i], temp, first->radix);
        }
        inter_result->length = i + second->length;
        if(transfer_digit > 0) {
            inter_result->digits[inter_result->length] = transfer_digit;
            inter_result->length++;
        }
        if(result == NULL)
            result = deep_copy(inter_result);
        else
            add_modulus_fast(result, inter_result);
    }
    bigInt *end_result = light_copy(result);
    end_result->digits = (type *)malloc(sizeof(type) * (result->length + length_spare));
    memcpy(end_result->digits, result->digits, sizeof(type) * result->length);
    free(result);
    return end_result;
}

type mult_overflow(const type first, const type second, type *result, const type value_max) {
    unsigned long long multiplication = first * second;
    *result = multiplication % value_max;
    return multiplication / value_max;
}

void mult_on_type(char *result, type multiplier, int max_length) {
    int index = 0;
    char *step_result = (char *)calloc(sizeof(char), max_length);
    while(index < max_length) {
        add_to_stringInt(&step_result[index], result[index] * multiplier, max_length);
        index++;
    }
    memcpy(result, step_result, max_length);
    free (step_result);
}

