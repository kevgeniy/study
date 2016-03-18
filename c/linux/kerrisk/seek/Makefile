all:
	gcc -c seek_io.c -o seek_io.o
	gcc seek_io.o ../lib/error_functions.o ../lib/get_num.o -o seek_io
	rm -f seek_io.o

clean:
	rm -f seek_io
