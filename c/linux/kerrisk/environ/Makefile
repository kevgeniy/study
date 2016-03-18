LIB="../lib"
CC=gcc -I $(LIB)

modify_env: modify_env.o error_functions.o
	$(CC) modify_env.o error_functions.o -o modify_env
	rm -f modify_env.o error_fucntions.o

modify_env.o:
	$(CC) -c modify_env.c -o modify_env.o

error_functions.o:
	$(CC) -c $(LIB)/error_functions.c -o error_functions.o

clean:
	rm -f modify_env
