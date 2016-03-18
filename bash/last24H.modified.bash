#!/bin/bash

directory=/media/evgeniy/Data/Programming/projects/bash

E_XCD=85
BACKUPFILE=backup-$(date +%m-%d-%Y)

archive=${1-$BACKUPFILE}

cd "$directory" || {
echo "Usage: Can't change directory"
exit E_XCD
}

tar cvf $archive.tar $(find . -mtime -1 -type f -printf %f\\n)
gzip $archive.tar
echo "Directory $PWD backed up in archive file \"$archive.tar.gz\"."
