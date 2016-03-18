#include "compare.h"
#include <string.h>
#include <stdlib.h>

int compare_modulus(const bigInt *first, const bigInt *second) {
    if( first->length < second->length)
        return -1;
    if(first->length > second->length)
        return 1;
    int index = first->length;
    while(index > 0 && first->digits[index] == second->digits[index])
        index--;
    return !index ? 0 : get_sign(first->digits[index] - second->digits[index]);
}

// safe for any numeric type
int get_sign(int number) {
    return (0 < number) - (number < 0);
}

int get_max(int first, int second) {
    return first > second ? first : second;
}



