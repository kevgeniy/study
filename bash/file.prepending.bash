#!/bin/bash

#Prepending file using cat -

file=/media/evgeniy/Data/Programming/projects/bash/file.txt
title="This is a"
cat - "$file" <<< $title > "$file.bat"
mv "$file.bat" "$file"

