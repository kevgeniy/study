#ifndef LIST_H
#define LIST_H

#include <stdbool.h>

typedef struct inf {
    int document;
    int *positions;
    int count_in_doc;
} word_info;

typedef struct dict {
    struct dict *previous;
    struct dict *next;
    word_info **info;
    int info_count;
    char *name;
} word_t;

bool contains(const word_t *, const char *);

word_t *get(const word_t *, const char *);

bool is_name_equal(const word_t *, const char *);

bool is_word_equal(const word_t *, const word_t *);

word_t *remove_word(word_t *, const char *word_name);

word_t *add_word(word_t *, word_info **, int, char*);

word_info *new_word_info(int , int*, int);

void free_word(word_t *);

#endif
