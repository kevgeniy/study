#!/bin/bash

for file in /media/evgeniy/Data/Programming/{,projects/}bash/*up?
do
	if [ -x "$file" ]
	then
	  echo "$file"
	fi
done
