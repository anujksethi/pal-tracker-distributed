#!/usr/bin/env bash


for topDir in Applications Components
do
    for subDir in ./${topDir}/*
    do
        subDir=${subDir%*/}
        subDir=${subDir##*/}
        echo -e "\n${subDir}"
        dotnet list "./${topDir}/${subDir}/${subDir}.csproj" reference
    done
done