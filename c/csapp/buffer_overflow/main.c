#include<stdio.h>
#include<stdlib.h>

char *getsim(char *s) {
    int c;
    char *dest = s;
    while((c = getchar()) != '\n' && c != EOF)
        *dest++ = c;
    if(c == EOF && dest == s) // Nothing read
        return NULL;
    return s;
}

void echo() {
    char buf[8];
    getsim(buf);
    puts(buf);
}

int main(void) {
    return 0;
}
