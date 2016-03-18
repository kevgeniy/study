#!/bin/bash
echo "Let's begin!"

echo

var=1
echo "The value of \"var\" is $var."

echo

let "var=5 +43"
echo "The value of \"var\" is $var."

echo

echo -n "Values of \"a\" in the loop are: "
for a in 6 7 8 9 10
do
  echo -n "$a "
done

echo

echo "Enter \"a\":"
read a
echo "The value of \"a\" is now $a."

echo

echo "Good luck!"
exit 0
