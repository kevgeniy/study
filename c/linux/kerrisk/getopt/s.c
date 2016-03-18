#include <stdio.h>

int main(void) {
	int count = 0, nSum = 0;
	char input = 0;
	double dAvg;
	while(scanf("%c", &input) != -1) {
		if(input == 'N') break;
		if(input != 0) {
			nSum += input - '0';
			++count;
		}
	}
	if(count != 0) dAvg = nSum * 1.0 / count;
	printf("%f", dAvg);
}

