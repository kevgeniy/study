//#include "bigInt.h"
#include "string.h"
#include "stdlib.h"
#include "add_sub.h"
#include "compare.h"

/* operations */
bigInt *add_bigInts(const bigInt *first, const bigInt *second) {
    return add_with_sign(first, second, 1);
}

bigInt *sub_bigInts(const bigInt *first, const bigInt *second) {
    return add_with_sign(first, second, -1);
}

// redirection to subtraction and addition of modules
bigInt *add_with_sign(const bigInt *first, const bigInt *second, int sign_of_operation) {
    int sign_first = get_sign(first->sign);
    int sign_second = get_sign(second->sign);

    if(!sign_second)
        return deep_copy(first);
    if(!sign_first) {
        bigInt *result = deep_copy(second);
        result->sign *= sign_of_operation;
        return result;
    }

    if(sign_first + sign_second)
        return sign_of_operation < 0 ? sub_modulus(first, second) : add_modulus(first, second);
    return sign_of_operation < 0 ? add_modulus(first, second) : sub_modulus(second, first);
}


bigInt *add_modulus(const bigInt *first, const bigInt *second) {
    if(!first || !first->sign)
        return deep_copy(second);
    if(!second || !second->sign)
        return deep_copy(first);

    int length = get_max(first->length_full, second->length);
    if(second->length >= first->length_full || first->length == first->length_full)
        length += length_spare;
    bigInt *result = light_copy(first);
    result->length_full = length;
    result->digits = (type *)calloc(result->length_full, sizeof(type));
    memcpy(result->digits, first->digits, sizeof(type) * first->length);

    return add_modulus_fast(result, second);
}

// addition of modulus of bigInts without saving first bigInt
bigInt *add_modulus_fast(bigInt *first, const bigInt *second) {
    int transfer = 0;
    int digit_index;
    for(digit_index = 0; digit_index < second->length; digit_index++) {
        transfer = add_overflow(first->digits + digit_index, second->digits[digit_index] + transfer, first->radix);
    }
    while(transfer) {
        transfer = add_overflow(first->digits + digit_index, transfer, first->radix);
        digit_index++;
    }
    first->length = digit_index;

    return first;
}

bigInt *sub_modulus(const bigInt *first, const bigInt *second) {
    if(!first || !first->sign) {
        bigInt *result = deep_copy(second);
        result->sign *= -1;
        return result;
    }
    if(!second || !second->sign)
        return deep_copy(first);

    int compare_first_second = compare_modulus(first, second);
    if(!compare_first_second)
        return new_bigInt(); // return 0
    if(compare_first_second < 0) {
        const bigInt *temp = first;
        first = second;
        second = temp;
    }

    bigInt *result = light_copy(first);
    result->digits = (type *)calloc(result->length_full, sizeof(type));
    memcpy(result->digits, first->digits, first->length * sizeof(type));
    sub_modulus_fast(result, second);

    result->sign *= get_sign(compare_first_second);
    return result;
}

//first >= second
bigInt *sub_modulus_fast(bigInt *first, const bigInt *second) {
    int transfer = 0;
    int digit_index;
    for(digit_index = 0; digit_index < second->length; digit_index++)
        transfer = sub_overflow(first->digits + digit_index, second->digits[digit_index] + transfer, first->radix);
    while(transfer) {
        transfer = sub_overflow(first->digits + digit_index, transfer, first->radix);
        digit_index++;
    }

    return first;
}


/* overflow operations, return overflow if it is */
int add_overflow(type *first, const type second, const type value_max) {
    if(value_max - second > *first) {
        *first += second;
        return 0;
    }
    *first -= (value_max - second);
    return 1;
}

int sub_overflow(type *first, const type second, const type value_max) {
    if(*first > second) {
        *first -= second;
        return 0;
    }
    *first += value_max - second;
    return 1;
}


void add_to_stringInt(char *result, type number, int max_length) {
    int index = 0;
    int transfer_digit = 0;
    while((index < max_length) && (number > 0 || transfer_digit > 0)) {
        int sum =  number % 10 + result[index] + transfer_digit;
        transfer_digit = sum / 10;
        result[index] = sum % 10;
        number /= 10;
        index++;
    }
}
