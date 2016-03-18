#include <stdio.h>
#include <stdlib.h>
#include "list.h"
#include <string.h>
#include <pthread.h>

void *sample1(void *arg);
void *sample2(void *arg);
void *sample3(void *arg);

void *print(void *element);
void *print_int(void *element);
void char_free(void *element);

int int_cmp(void *el1, void *el2);
int char_cmp(void *el1, void *el2);
void int_free(void *element);

int main(void) {
    struct list *list;
    pthread_t *pthread;
    int i;

    pthread = (pthread_t *)malloc(sizeof(pthread_t) * 3);
    list = (struct list *)malloc(sizeof(struct list));
    list_init(list);

    pthread_create(pthread + 0, NULL, sample1, (void *)list);
    pthread_create(pthread + 1, NULL, sample2, (void *)list);
    //pthread_create(pthread + 2, NULL, sample3, (void *)list);

    for(i = 0; i < 3; ++i)
        pthread_join(pthread[i], NULL);

    list_destroy(list, NULL);
    free(pthread);
    return 0;
}

void *sample1(void *arg) {
    struct list *list;

    list = (struct list *)arg;

    while(1) {
        int *a = malloc(sizeof(int));
        *a = 1;
        list_add(list, a);
        list_for_each(list, print_int);
        list_remove(list, a, int_cmp, int_free);
    }
}

void *sample2(void *arg) {
    struct list *list;

    list = (struct list *)arg;

    while(1) {
        int *a = malloc(sizeof(int));
        *a = 2;
        list_add(list, a);
        list_for_each(list, print_int);
        list_remove(list, a, int_cmp, int_free);
    }
}

void *sample3(void *arg) {
    struct list *list;

    list = (struct list *)arg;

    while(1) {
        int *a = malloc(sizeof(int));
        *a = 3;
        list_add(list, a);
        list_remove(list, a, int_cmp, int_free);
    }
}

int int_cmp(void *el1, void *el2) {
    if(el1 == NULL || el2 == NULL)
        return -1;
    return !(*((int*)el1) == *((int *)el2));
}

void int_free(void *element) {
    free((int*)element);
}

int char_cmp(void *el1, void *el2) {
    if(el1 == NULL || el2 == NULL)
        return -1;
    return strcmp((char *)el1, (char *)el2);\
}

void char_free(void *element) {
    free((char *)element);
}

void *print(void *element) {
    printf("%s\n", (char *)element);
    return NULL;
}

void *print_int(void *element) {
    printf("%d\n", *(int *)element);
    return NULL;
}
