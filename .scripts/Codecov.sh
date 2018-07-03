#!/bin/sh

# Get absolute path this script is in and use this path as a base for all other (relatve) filenames.
# !! Please make sure there are no spaces inside the path !!
# Source: https://stackoverflow.com/questions/242538/unix-shell-script-find-out-which-directory-the-script-file-resides
SCRIPT=$(readlink -f "$0")
SCRIPTPATH=$(dirname "$SCRIPT")
ROOT_PATH=$(cd ${SCRIPTPATH}/../; pwd)

cd ${SCRIPTPATH}
CODECOV_EXE=$(find . -type f -name codecov.exe)
CODECOV_PATH=$(dirname "$CODECOV_EXE")
echo ${CODECOV_PATH}
echo ${CODECOV_EXE}

cd ${CODECOV_PATH}
echo mono codecov.exe -h
mono codecov.exe -h
