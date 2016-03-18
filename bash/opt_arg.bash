#!/bin/bash
#Simple program with case

opt=
dat=

for i in $@; do
  case "$i" in
    -*) opt="$opt $i" ;;
    *) dat="$dat $i" ;;
  esac
done
echo "Options: $opt"
echo "Data: $dat"
