#!/usr/bin/env bash

if [ $# -lt 1 ]
then
  echo Usage: ./animateGenerations.sh PathToDirectory
  echo        e.g. ./animateGenerations.sh ./Part1
  exit 1
fi

DIRECTORY_PATH=$1

pushd "$DIRECTORY_PATH"
for f in `ls *.txt | sort -V`
do
  clear
  cat "$f"
  sleep 0.1
done
popd
