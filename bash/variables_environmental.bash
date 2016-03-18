#!/bin/bash

(echo $var)
var=3
export var
(echo $var)
