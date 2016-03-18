#include "../header.h"
#include <fcntl.h>

#define DEBUG

int main(int argc, char **argv) {
	char *file_name = argv[1], *target_name = argv[2];
	int fd, target_fd, buf_index;
	ssize_t buf_size, rd_size, wr_size, hole_size, text_size;
	char *buffer;

	if(unlikely((fd = open(file_name, O_RDONLY)) == -1))
		errExit("opening file %s", file_name);

	if(unlikely((target_fd = open(target_name, O_CREAT | O_TRUNC | O_WRONLY, 
					S_IRUSR | S_IWUSR | S_IROTH | S_IWOTH |
					S_IRGRP | S_IWGRP)) == -1))
		errExit("opening file %s", target_name);

	buf_size = optimal_buf_size(fd);
#ifdef DEBUG
	printf("%ld\n", (long)buf_size);
#endif
	if(unlikely((buffer = (char *)malloc(buf_size)) == NULL))
		errExit("malloc-ing memory for I/O buffer");

	hole_size = 0;
	while((rd_size = read(fd, buffer, buf_size)) > 0) {
		buf_index = 0;

		// do we have continuing hole?
		if(hole_size != 0) {
			while(buffer[buf_index] == 0 && buf_index < rd_size) {
				++buf_index;
				++hole_size;
			}
			if(buf_index == rd_size) // hole doesn't ended
				continue;
#ifdef DEBUG
	printf("continuing hole, hole_size: %ld\n", (long)hole_size);
#endif
			if(unlikely(lseek(target_fd, hole_size, SEEK_CUR) == -1))
				errExit("making hole with lseek");
			hole_size = 0;
		}

		// now last simbol was not 0
		while(buf_index < rd_size) {
			text_size = 0;
#ifdef DEBUG
	printf("before finding text hole_size: %ld buf_index: %d\n", (long)hole_size, buf_index);
#endif
			while(buffer[buf_index] != 0 && buf_index < rd_size) {
				++buf_index;
				++text_size;
			}
#ifdef DEBUG
	printf("after finding text hole_size: %ld buf_index: %d\n", (long)hole_size, buf_index);
#endif
			if(text_size != 0 && unlikely(write(target_fd, &buffer[buf_index - text_size], text_size) != text_size))
				errExit("couldn't write the whole buffer");

			while(buffer[buf_index] == 0 && buf_index < rd_size) {
				++buf_index;
				++hole_size;
			}
#ifdef DEBUG
	printf("ater finding hole hole_size: %ld buf_index: %d\n", (long)hole_size, buf_index);
#endif
			if(hole_size != 0) {
				if(buf_index == rd_size)							// buffer ended with hole
					break;
				else {
					if(unlikely(lseek(target_fd, hole_size, SEEK_CUR) == -1))
						errExit("making hole with lseek");
					hole_size = 0;
				}
			}
		}
	}
	if(hole_size != 0 && unlikely(lseek(target_fd, hole_size, SEEK_CUR) == -1))
		errExit("making hole with lseek");

	if(unlikely(rd_size == -1))
		errExit("reading from %s", file_name);

	if(unlikely(close(fd) == -1))
		errExit("closing %s", file_name);
	if(unlikely(close(target_fd) == -1))
		errExit("closing %s", target_name);

	free(buffer);
	exit(EXIT_SUCCESS);		
}

int optimal_buf_size(int fd) {
	struct stat *fd_info;

	if(unlikely(isatty(fd)))								// terminal
		return BUFSIZ;

	fd_info = (struct stat *)alloca(sizeof(struct stat));

	if(unlikely(fstat(fd, fd_info) == -1))							
		errExit("in fstat");

	if(fd_info->st_mode & S_IFREG != 0 && fd_info->st_size > 0 && fd_info->st_size < fd_info->st_blksize)
		return fd_info->st_size;
	
	return fd_info->st_blksize;
}
