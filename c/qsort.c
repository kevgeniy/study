#include <stdio.h>
#include <stdlib.h>

static int Qsort(int *, int, int);

int main(void) {
    int i;
    int array[] = {7, 3, 4, 1, 2, 9, 10, 4};

    if(Qsort(array, 0, 7) != 0)
			exit(EXIT_FAILURE);

    for(i = 0; i < 8; ++i)
        printf("%d ", array[i]);
	 
	exit(EXIT_SUCCESS);
}

int Qsort(int *array, int left, int right) {
    int middle_value;
    int i, j;

    if(right <= left)
        return 0;

    middle_value = array[left + rand() % (right - left  + 1)];
    i = left, j = right;
    while(i < j) {
        while(array[i] < middle_value)
            ++i;
        while(array[j] > middle_value)
            --j;
        if(i < j) {
            int temp = array[i];
            array[i] = array[j], array[j] = temp;
            ++i, --j;
        }
    }
    if(array[i] > middle_value)
        --i;
    if(Qsort(array, left, i) != 0)
		return 1;
	if(Qsort(array, i + 1, right) != 0)
		return 1;
    return 0;
}

