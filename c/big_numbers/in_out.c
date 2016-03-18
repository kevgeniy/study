//#include "bigInt.h"
#include "stdlib.h"
#include "stdio.h"
#include "string.h"
#include "stdbool.h"
#include "multiply.h"
#include "add_sub.h"
#include "math.h"

const int max_length = 36;

int decimal_to_radix_array(unsigned long long int *digits, int begin_index, int end_index, type **result_digits);

//int get_length(type first);

void print_bigInt(bigInt* first) {
    printf("%s %d:\n", "bigInt with radix", first->radix);
    if(first->sign < 0)
        printf("%c", '-');
    else if(first->sign == 0) {
        printf("%d", 0);
        return;
    }

    char *result = (char*)calloc(sizeof(char), max_length);

    for(int i = first->length - 1; i > 0; i--) {
        add_to_stringInt(result, (type)first->digits[i], max_length);
        mult_on_type(result, first->radix, max_length);
    }
    add_to_stringInt(result, (type)first->digits[0], max_length);

    int index;
    for(index = max_length - 1; result[index] == 0 && index > 0; index--);
    for(; index >= 0; index--)
        printf("%c", result[index] + '0');
    printf(" ");
    free(result);
}

bigInt* scan_bigInt(char *str) {
    if(str == NULL)
    printf("%s %d %s", "Please, enter bigInt no more than ", max_length,
           "simbols, any string begining with 0 consider as 0:\n");
    unsigned long long int digits[max_length];
    int end_index = -1;
    if(str == NULL) {
    char ch;
    while((ch = getchar()) != '\n' && ch != '\0')
        digits[++end_index] = ch != '-' ? ch - '0' : ch;
    }
    else {
        char ch;
        int i = 0;
        while((ch = str[i]) != '\n' && ch != '\0') {
            digits[++end_index] = ch != '-' ? ch - '0' : ch;
            ++i;
        }
    }
    int sign;
    int begin_index = 0;
    if(digits[0] == '-') {
        sign = -1;
        begin_index++;
    }
    else if(digits[1] == 0) {
        bigInt *result = new_bigInt_length(1);
        result->sign = 0;
        return result;
    }
    else
        sign = 1;

    type *result_digits = NULL;
    int count_result_digits = decimal_to_radix_array(digits, begin_index, end_index, &result_digits);

    bigInt *result = new_bigInt_length(count_result_digits);
    memcpy(result->digits, result_digits, count_result_digits * sizeof(type));
    result->sign = sign;
    free(result_digits);
    return result;
}

int decimal_to_radix_array(unsigned long long int *digits, int begin_index, int end_index, type **result_digits) {
    int max_result_length = (int)((end_index - begin_index + 1) * (log(10) / log(radix))) + 1;
    *result_digits = (type*)malloc(sizeof(type) * max_result_length);
    int count_result_digit = 0;

    type scored_int;
    while(begin_index < end_index) {
        scored_int = 0;
        int last_div_index = begin_index;
        int last_digit_index = begin_index;
        bool is_first = true, is_wait = false;
        while(last_digit_index <= end_index || scored_int >= radix) {
            if (scored_int < radix) {
                scored_int = scored_int * 10 + digits[last_digit_index];
                last_digit_index++;
                if(!is_first && is_wait) {
                    digits[last_div_index] = 0;
                    last_div_index++;
                }
                is_wait = true;
            }
            else {
                is_first = is_wait = false;
                digits[last_div_index] = scored_int / radix;
                last_div_index++;
                scored_int %= radix;
            }
        }
        if(is_wait && end_index != 0) {
            digits[last_div_index] = 0;
            last_div_index++;
        }
        (*result_digits)[count_result_digit] = scored_int;
        count_result_digit++;
        end_index = last_div_index - 1;
    }
    (*result_digits)[count_result_digit] = digits[begin_index];
    count_result_digit++;
    return count_result_digit;
}

//int get_length(type first) {
//    int result = 0;
//    while(first != 0) {
//        result++;
//        first /= 10;
//    }
//    return result;
//}
