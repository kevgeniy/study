int uadd_ok(unsigned x, unsigned y) {
    return x + y >= x;
}

int tadd_ok(unsigned x, unsigned y) {
    return !(x > 0 && y > 0 && x + y <= 0 ||
           x < 0 && y < 0 && x + y >= 0);
}

int tsub_ok(unsigned x, unsigned y) {
    int UMax = 21;
    int sign = y == UMax ? -1 : 1;
    return sign * tadd_ok(x, -y);
}

#include<stdio.h>

int main(void) {
     int a = 9223372036854775808 > -1;
     printf("%d", a);
}
