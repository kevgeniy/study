#include "search.h"
#include "list.h"
#include <stdlib.h>
#include <string.h>
#include <stdio.h>

int match_words(const word_t *, const char **, int, word_t ***);

int get_priorities(const word_t **, int, double **);

int get_documents(const word_t **, int, int **);

int get_step_words(const word_t **, int, int , int *, int **);

int get_measure(const word_t **, int *, double *, int *, int);

int search(const word_t *pre_words, const char **pat_words, int pat_count, int **documents) {
    // find common words
    word_t **words;
    int num_of_words = match_words(pre_words, pat_words, pat_count, &words);
    // find priorities of these words
    double *priorities = NULL;
    get_priorities((const word_t **)words, num_of_words, &priorities);
    // get used documents
    int num_of_doc = get_documents((const word_t **)words, num_of_words, documents);
    
    
    int *measures = (int *)malloc(sizeof(int) * num_of_doc);
    int *step_words = NULL;
    // i.e. initializing with 0s
    int *step_info_position = (int *)calloc(sizeof(int), num_of_words);
    int cur_doc_num = -1;
    int cur_doc = -1;
    int num_step_words = 0;
    
    while((num_step_words = get_step_words((const word_t **)words, num_of_words,
                                           cur_doc, step_info_position, &step_words)) != -1) {
        int measure = get_measure((const word_t **)words, step_info_position,
                                  priorities, step_words, num_step_words);
        ++cur_doc_num;
        cur_doc = (*documents)[cur_doc_num];
        for(int i = 0; i < num_of_doc; i++)
            if(cur_doc == (*documents)[i]) {
                measures[i] = measure;
                break;
            }
        if(cur_doc_num == num_of_doc - 1)
            break;
    }
    for(int i = 0; i < num_of_doc - 1; i++) {
        int max = 0;
        for(int j = 1; j < num_of_doc - i; j++) {
            if(measures[j] < measures[max]) {
                max = j;
            }
        }
        int temp = measures[num_of_doc - i - 1];
        measures[num_of_doc - i - 1] = measures[max];
        measures[max] = temp;
        temp = (*documents)[num_of_doc - i - 1];
        (*documents)[num_of_doc - i - 1] = (*documents)[max];
        (*documents)[max] = temp;
    }
    free(step_info_position);
    free(priorities);
    free(step_words);
    for(int i = 0; i < num_of_doc; i++)
        printf("%d ", measures[i]);
    printf("\n");
    free (measures);
    free(words);
    return num_of_doc;
}

// words -- all available words
// pat_words -- pattern words
// num_of_words -- returned number of common words in pat_words and words
int match_words(const word_t *words, const char ** pat_words, int pat_words_count, word_t ***match_words) {
    int num_of_words = 0;
    for(int i = 0; i < pat_words_count; i++)
        if(contains(words, pat_words[i]))
            num_of_words += 1;
    *match_words = (word_t **)malloc(sizeof(word_t *) * num_of_words);
    int res_count = 0;
    word_t *res;
    for(int i = 0; i < pat_words_count && res_count < num_of_words; i++)
        if((res = get(words, pat_words[i])) != NULL) {
            (*match_words)[res_count] = res;
            res_count++;
        }
    return num_of_words;
}

// returns number of priorities
int get_priorities(const word_t **words, int num_of_words, double **priorities) {
    *priorities = (double *)malloc(sizeof(double) * num_of_words);
    int max = words[0]->info_count;
    for(int i = 1; i < num_of_words; i++)
        if(max < words[i]->info_count)
            max = words[i]->info_count;
    for(int i = 0; i < num_of_words; i++)
        (*priorities)[i] = words[i]->info_count / max;
    return num_of_words;
}

int get_documents(const word_t **words, int num_of_words, int **documents) {
    int max_count = 4;
    int num_of_doc = 0;
    // we will imitate list using this array
    *documents = (int *)malloc(sizeof(int) * max_count);
    
    for(int i = 0; i < num_of_words; i++)
        for(int j = 0; j < words[i]->info_count; j++) {
            bool is = false;
            for(int k = 0; k < num_of_doc; k++)
                if(words[i]->info[j]->document == (*documents)[k]) {
                    is = true;
                    break;
                }
            // list resizing
            if(!is) {
                if(max_count == num_of_doc) {
                    max_count *= 2;
                    int *temp = (int *)malloc(sizeof(int) * max_count);
                    memcpy(temp, (*documents), num_of_doc);
                    free(*documents);
                    *documents = temp;
                }
                (*documents)[num_of_doc] = words[i]->info[j]->document;
                num_of_doc++;
            }
        }
    return num_of_doc;
}

// words -- words for searching
// cur_doc -- number of last watched document
// step_info_position -- position of word_info in i word
// step_words -- numbers of step_words
int get_step_words(const word_t **words, int num_of_words, int cur_doc, int *step_info_position, int **step_words) {
    int min_next_doc = -1;
    for(int i = 0; i < num_of_words; i++) {
        int i_doc = words[i]->info[step_info_position[i]]->document;
        if(i_doc == cur_doc && words[i]->info_count > step_info_position[i] + 1) {
            step_info_position[i]++;
            i_doc = words[i]->info[step_info_position[i]]->document;
        }
        if(i_doc > cur_doc && (min_next_doc < 0 || min_next_doc > i_doc))
            min_next_doc = i_doc;
    }
    
    int num_step_words = 0;
    for(int i = 0; i < num_of_words; i++)
        if(words[i]->info[step_info_position[i]]->document == min_next_doc)
            num_step_words++;
    
    if(*step_words != NULL)
        free(*step_words);
    *step_words = (int *)malloc(sizeof(int) * num_step_words);
    
    for(int i = 0, index = 0; i < num_of_words; i++)
        if(words[i]->info[step_info_position[i]]->document == min_next_doc) {
            (*step_words)[index] = i;
            index++;
        }
    return cur_doc == min_next_doc ? -1 : num_step_words;
}




// words = array of words for current search from pattern
// priorities = array of priorities for words
// step_words = numbers of step words
int get_measure(const word_t **words, int *word_info_position, double *priorities, int *step_words, int num_step_words) {
    int measure = 0;
    // part1 : count_in_doc * priority
    for(int i = 0; i < num_step_words; i++) {
        int j = step_words[i];
        measure += words[j]->info[word_info_position[j]]->count_in_doc * priorities[j];
    }
    
    // part2 : proximity of words in pattern
    int *positions_number = (int *)calloc(sizeof(int), num_step_words);
    int stage_step_word_number = 0;
    int word_number = step_words[stage_step_word_number];
    word_info *stage_word_info = words[word_number]->info[word_info_position[word_number]];
    while(positions_number[stage_step_word_number] < stage_word_info->count_in_doc) {
        int st_position = stage_word_info->positions[positions_number[stage_step_word_number]];
        int min = -1;
        int max = st_position;
        for(int i = 0; i < num_step_words; ++i) {
            int j = step_words[i];
            word_info *info = words[j]->info[word_info_position[j]];
            while(info->count_in_doc > positions_number[i] + 1 &&
                  abs(info->positions[positions_number[i]] - st_position) >
                  abs(info->positions[positions_number[i] + 1] - st_position)) {
                ++positions_number[i];
            }

            if (info->positions[positions_number[i]] < min && min == -1)
                min = info->positions[positions_number[i]];
            if (info->positions[positions_number[i]] > max)
                max = info->positions[positions_number[i]];
        }
        measure -= max - min;
        ++positions_number[stage_step_word_number];
    }
    free(positions_number);
    return measure;
}
