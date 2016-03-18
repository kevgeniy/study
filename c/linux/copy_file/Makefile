all clean: copy.o error_functions.o
	echo "HELLO"
	gcc copy.o error_functions.o -o copy
copy.o cpd: copy.c
	echo "HELLO1"
	gcc -c copy.c -o copy.o
error_functions.o: ../error_functions.c
	gcc -c ../error_functions.c -o error_functions.o
