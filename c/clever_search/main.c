#include <stdio.h>
#include "list.h"
#include <stdlib.h>
#include "search.h"

word_t *word1();
word_t *read(FILE *);
word_t *read_words(FILE *);

int main(void) {
    int *documents = NULL;
    char *first = (char *)malloc(sizeof(char) * 6);
    sprintf(first, "%s", "first");
    char *second = (char *)malloc(sizeof(char) * 7);
    sprintf(second, "%s", "second");
    char **words = (char **)malloc(sizeof(char *) * 2);
    words[0] = first;
    words[1] = second;

    FILE *file = (FILE *)malloc(sizeof(FILE));
    file = fopen("1", "r");
    word_t *res = read_words(file);

    int count;
    count = search(res, (const char **)words, 2, &documents);
    for(int i = 0; i < count; i++)
        printf("%d ", documents[i]);
    free(documents);

    return 0;
}

word_t *read_words(FILE *file) {
    int count_words;
    fscanf(file, "%d", &count_words);
    if(count_words <= 0)
        return NULL;
    word_t *word = read(file);
    word->next = word->previous = word;
    --count_words;
    for(int i = count_words; i > 0; i--) {
        word_t *new_word = read(file);
        new_word->previous = word;
        new_word->next = word->next;
        word->next->previous = new_word;
        word->next = new_word;
    }
    return word;
}

word_t *read(FILE *file) {
    char *name = (char *)malloc(sizeof(char) * 100);
    fscanf(file, "%c", name);
    fgets(name, 100, file);

    int pos = 0;
    while(name[pos] != '\0')
          ++pos;
    name[pos - 1] = '\0';

    int info_count;
    fscanf(file, "%d", &info_count);
    word_t *word = (word_t *)malloc(sizeof(word_t));
    word->name = name;
    word->info_count = info_count;
    word->info = (word_info **)malloc(sizeof(word_info *) * word->info_count);

    for(int i = 0; i < word->info_count; i++)
        word->info[i] = (word_info *)malloc(sizeof(word_info));

    for(int i = 0; i < info_count; i++) {

    fscanf(file, "%d", &(word->info[i]->document));
    fscanf(file, "%d", &(word->info[i]->count_in_doc));
    word->info[i]->positions = (int *)malloc(sizeof(int) * word->info[i]->count_in_doc);

    for(int j = 0; j < word->info[i]->count_in_doc; j++)
        fscanf(file, "%d", &(word->info[i]->positions[j]));
    }

    return word;
}

// old method for tests
word_t *word1() {
    int *p1 = (int *) malloc(sizeof(int) * 3);
    p1[0] = 1;
    p1[1] = 3;
    p1[2] = 5;
    int *p2 = (int *) malloc(sizeof(int) * 3);
    p2[0] = 1;
    p2[1] = 4;
    p2[2] = 12;
    int *p3 = (int *) malloc(sizeof(int) * 3);
    p3[0] = 1;
    p3[1] = 4;
    p3[2] = 7;
    word_info **b1 = (word_info **)malloc(sizeof(word_info*) * 3);
    b1[0] =  new_word_info(0, p1, 3);
    b1[1] = new_word_info(1, p2, 3);
    b1[2] = new_word_info(2, p3, 3);
    word_t *word = (word_t *)malloc(sizeof(word_t));
    word->info = b1;
    word->info_count = 3;
    char *first = (char *)malloc(sizeof(char) * 6);
    sprintf(first, "%s", "first");
    word->name = first;
    word->next = word->previous = word;

    int *p4 = (int *) malloc(sizeof(int) * 3);
    p4[0] = 2;
    p4[1] = 4;
    p4[2] = 6;
    int *p5 = (int *) malloc(sizeof(int) * 3);
    p5[0] = 2;
    p5[1] = 5;
    p5[2] = 6;
    int *p6 = (int *) malloc(sizeof(int) * 3);
    p6[0] = 2;
    p6[1] = 5;
    p6[2] = 6;
    word_info **b2 = (word_info **)malloc(sizeof(word_info*) * 3);
    b2[0] =  new_word_info(0, p4, 3);
    b2[1] = new_word_info(1, p5, 3);
    b2[2] = new_word_info(2, p6, 3);
    char *second = (char *)malloc(sizeof(char) * 7);
    sprintf(second, "%s", "second");
    add_word(word, b2, 3, second);
    return word;
}

