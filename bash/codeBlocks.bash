#!/bin/bash

SUCCESS=0
E_NOARGS=65

if [ -z "$1" ]
then
  echo "Usage: `basename $0` rpm-file"
  exit $E_NOARGS
fi

#Output stream redirection, intput redirection is also enabled
{
  echo &&  echo "Archive Description:"
  rpm -qpi "$1"
} > "$1.test"

echo "Results of rpm test in file $1.test"

exit 0
