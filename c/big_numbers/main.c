#include <stdio.h>
#include <math.h>
#include <stdlib.h>
#include "bigInt.h"
#include "add_sub.h"
#include "multiply.h"
#include "compare.h"
#include "in_out.h"
#include <stdbool.h>

bool add_test1(void);
bool add_test2(void);
bool add_test3(void);
bool add_test4(void);
bool sub_test1(void);
bool sub_test2(void);
bool sub_test3(void);
bool sub_test4(void);

int add_overflow_test(void);
int mult_test(void);


int main(void) {
    bigInt *integer = scan_bigInt("1234");
    print_bigInt(integer);
    destruct_bigInt(integer);

    printf("\n");
    if(add_test1())
        printf("%s\n", "FAIL1");
    else
        printf("%s\n", "RIGHT1");

    if(add_test2())
        printf("%s\n", "FAIL2");
    else
        printf("%s\n", "RIGHT2");

    if(add_test3())
        printf("%s\n", "FAIL3");
    else
        printf("%s\n", "RIGHT3");

    if(add_test4())
        printf("%s\n", "FAIL4");
    else
        printf("%s\n", "RIGHT4");


    if(sub_test1())
        printf("%s\n", "FAIL1");
    else
        printf("%s\n", "RIGHT1");

    if(sub_test2())
        printf("%s\n", "FAIL2");
    else
        printf("%s\n", "RIGHT2");

    if(sub_test3())
        printf("%s\n", "FAIL3");
    else
        printf("%s\n", "RIGHT3");

    if(sub_test4())
        printf("%s\n", "FAIL4");
    else
        printf("%s\n", "RIGHT4");

    return 0;
}

bool add_test1() {
    bigInt *integer = scan_bigInt("12345");
    bigInt *integer2 = scan_bigInt("123456");
    bigInt *result = scan_bigInt("135801");
    bigInt *result2 = add_bigInts(integer, integer2);
    return !(compare_modulus(result, result2) == 0);
}


bool add_test2() {
    bigInt *integer = scan_bigInt("-12345");
    bigInt *integer2 = scan_bigInt("123456");
    bigInt *result = scan_bigInt("111111");
    bigInt *result2 = add_bigInts(integer, integer2);
    return !(compare_modulus(result, result2) == 0 && result2->sign > 0);
}
bool add_test3() {
    bigInt *integer = scan_bigInt("12345");
    bigInt *integer2 = scan_bigInt("-123456");
    bigInt *result = scan_bigInt("-111111");
    bigInt *result2 = add_bigInts(integer, integer2);
    return !(compare_modulus(result, result2) == 0 && result2->sign < 0);
}

bool add_test4() {
    bigInt *integer = scan_bigInt("-12345");
    bigInt *integer2 = scan_bigInt("-123456");
    bigInt *result = scan_bigInt("-135801");
    bigInt *result2 = add_bigInts(integer, integer2);
    return !(compare_modulus(result, result2) == 0 && result2->sign < 0);
}

bool sub_test1() {
    bigInt *integer = scan_bigInt("12345");
    bigInt *integer2 = scan_bigInt("123456");
    bigInt *result = scan_bigInt("-111111");
    bigInt *result2 = sub_bigInts(integer, integer2);
    return !(compare_modulus(result, result2) == 0 && result2->sign < 0);
}


bool sub_test2() {
    bigInt *integer = scan_bigInt("-12345");
    bigInt *integer2 = scan_bigInt("123456");
    bigInt *result = scan_bigInt("-135801");
    bigInt *result2 = sub_bigInts(integer, integer2);
    return !(compare_modulus(result, result2) == 0 && result2->sign < 0);
}
bool sub_test3() {
    bigInt *integer = scan_bigInt("12345");
    bigInt *integer2 = scan_bigInt("-123456");
    bigInt *result = scan_bigInt("135801");
    bigInt *result2 = sub_bigInts(integer, integer2);
    return !(compare_modulus(result, result2) == 0 && result2->sign > 0);
}

bool sub_test4() {
    bigInt *integer = scan_bigInt("-12345");
    bigInt *integer2 = scan_bigInt("-123456");
    bigInt *result = scan_bigInt("111111");
    bigInt *result2 = sub_bigInts(integer, integer2);
    return !(compare_modulus(result, result2) == 0 && result2->sign > 0);
}
