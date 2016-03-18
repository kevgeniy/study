#!/bin/bash
# Cleanup, version 2.0
# Author: Kluchikov Evgeniy

#Variables
LOG_DIR=/var/log
ROOT_UID=0
LINES=50
E_NOTROOT=87
E_XCD=86

#Only the root can execute this file
if [ "$UID" -ne "$ROOT_UID" ]
then
  echo "You must be root to run this script."
  exit $E_NOTROOT
fi

#Check if the first argument is correct
E_WRONGARGS=85
case "$1" in
""	) lines=$LINES;;
*[!0-9]*) echo "Usage: $(basename $0)  lines-to-cleanup"; exit E_WRONGARGS;;
*	) lines=$1;;
esac

#Try to change the current directory to the directory where the logs are
cd $LOG_DIR || {
  echo "Can't change directory to $LOG_DIR."
  exit $E_XCD
}

#Delete all lines except $lines from the end
tail -n $lines messages > mesg.temp
mv mesg.temp messages

#Clean up the logs from wtmp
: > wtmp
echo "Logs cleaned up."

exit 0

