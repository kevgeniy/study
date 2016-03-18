#include <string.h>
#include <stdbool.h>
#include <stdlib.h>
#include "list.h"

bool contains(const word_t *words, const char *word) {
    return get(words, word) != NULL;
}

word_t *get(const word_t *words, const char *word) {
    const word_t *head = words;
    do {
        if (is_name_equal(words, word))
            return (word_t *)words;
        words = words->next;
    } while(!is_word_equal(head, words));
    return NULL;
}

bool is_name_equal(const word_t *word, const char *word_name) {
    return strcmp(word->name, word_name) == 0;
}

bool is_word_equal(const word_t *first, const word_t *second) {
    return is_name_equal(first, second->name);
}

word_t *remove_word(word_t *words, const char *word_name) {
    word_t *head = words;
    words = get(words, word_name);
    if(words == NULL)
        return head;
    if(is_word_equal(words, words->next)) {
        free_word(words);
        return NULL;
    }
    else {
        words->next->previous = words->previous;
        words->previous->next = words->next;
        word_t *ans = words->next;
        free_word(words);
        return ans;
    }
}

word_t *add_word(word_t *words, word_info **info, int info_count, char *name) {
    word_t *new_word = (word_t *)malloc(sizeof(word_t));
    new_word->name = name;
    new_word->info_count = info_count;
    new_word->info = info;

    new_word->next = words;
    new_word->previous = words->previous;
    words->previous->next = new_word;
    words->previous = new_word;
    return new_word;
}

word_info *new_word_info(int document, int *positions, int count_in_doc) {
    word_info *new_word_info = (word_info *)malloc(sizeof(word_info));
    new_word_info->document = document;
    new_word_info->positions = positions;
    new_word_info->count_in_doc = count_in_doc;
    return new_word_info;
}

void free_word(word_t *current) {
    free(current->name);
    for(int i = 0; i < current->info_count; i++) {
        free(current->info[i]->positions);
        free(current->info[i]);
    }
    free(current);
}





