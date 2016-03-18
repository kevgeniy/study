#!/bin/bash

E_WRONG_ARGS=85

#NUMBER_OF_PARAMETERS=2
number_of_expected_parameters=2
SRIPT_PARAMETERS="-a -h"

#Number of parameters from the keyboard
#case "$1" in
#  ""	)
#	number_of_expected_parameters=NUMBER_OF_PARAMETERS;;
#  *[!0-9]*)
#	echo "Usage: $(basename $0) uncorrect first argument (number of parameters)."
#	exit E_WRONG_ARGS;;
#  *	)
#	number_of_expected_parameters=$1
# esac

case "$*" in
  "" )
	script_parameters=SCRIPT_PARAMETERS;;
  [!-]* | -*[!a-zA-Z0-9\ ]	)
	echo "Usage: $(basename $0) uncorrect script parameters."
	exit E_WRONG_ARGS;;
  * )
	script_parameters=$*;;
esac

if [ "$#" -ne "$number_of_expected_parameters" ] then
  echo "Usage: $(basename $0) uncorrect number of parameters $script_parameters"
  exit E_WRONG_ARGS fi
