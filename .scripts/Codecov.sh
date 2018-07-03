#!/bin/sh

# Get absolute path this script is in and use this path as a base for all other (relatve) filenames.
# !! Please make sure there are no spaces inside the path !!
# Source: https://stackoverflow.com/questions/242538/unix-shell-script-find-out-which-directory-the-script-file-resides
SCRIPT=$(readlink -f "$0")
SCRIPTPATH=$(dirname "$SCRIPT")
ROOT_PATH=$(cd ${SCRIPTPATH}/../; pwd)


cd {SCRIPTPATH}
CODECOVS_EXE=$(find . -type f -name codecov.exe)

echo ${#CODECOVS_EXE[@]}

if [ "${#CODECOVS_EXE[@]}" -eq 1 ]; then
    echo "Exactly one item found! Yeahj"
fi

CODECOV_EXE=${CODECOVS_EXE[0]}
echo ${CODECOV_EXE}
