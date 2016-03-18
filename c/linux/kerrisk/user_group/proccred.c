#include "../lib/tlpi_hdr.h"
#include <grp.h>
#include <pwd.h>
#include <sys/types.h>
#include <limits.h>

#define SG_SIZE (NGROUPS_MAX + 1)

void print_uid(const char *, uid_t);
void print_gid(const char *, gid_t);

int main(void) {
	uid_t ruid, euid, suid, fsuid;
	gid_t rgid, egid, sgid, fsgid;
	gid_t *group_list;
	int grp_size, i;

	if(getresuid(&ruid, &euid, &suid) == -1)
		errExit("getresuid");
	if(getresgid(&rgid, &egid, &sgid) == -1)
		errExit("getresgid");

	fsuid =	setfsuid(0);
	fsgid = setfsgid(0);

	printf("UID:\n");
	print_uid("real", ruid);
	print_uid("effective", euid);
	print_uid("saved", suid);
	print_uid("file-system", fsuid);
	
	printf("\nGID:\n");
	print_gid("real", rgid);
	print_gid("effective", egid);
	print_gid("saved", sgid);
	print_gid("file-system", fsgid);

	group_list = (gid_t *)malloc(sizeof(gid_t) * SG_SIZE);
	if((grp_size = getgroups(SG_SIZE, group_list)) == -1)
		errExit("getgroups");

	printf("\nSupplementary groups (%d):\n", grp_size);
	for(i = 0; i < grp_size; ++i)
		print_gid("", group_list[i]);	

	free(group_list);
	exit(EXIT_SUCCESS);
}
void print_uid(const char *name, uid_t uid) {
	struct passwd *pwd;

	errno = 0;
	pwd = getpwuid(uid);
	if(errno != 0)
		errExit("getpwuid");

	printf("%s=%s (%ld); ", name, pwd == NULL ? "???" : pwd->pw_name, (long)uid);
}

void print_gid(const char *name, gid_t gid) {
	struct group *grp;

	errno = 0;
	grp = getgrgid(gid);
	if(errno != 0)
		errExit("getgrgid");

	printf("%s=%s (%ld); ", name, grp == NULL ? "???" : grp->gr_name, (long)gid);
}


