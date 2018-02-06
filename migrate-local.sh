#!/usr/bin/env bash

set -e

suffix=$(echo ${1} | tr '[:upper:]' '[:lower:]')

if [[ $# != 1 || !(${suffix} == 'test' || ${suffix} == 'dev') ]]
then
    echo -e "\nusage:\n\n$0 test\n$0 dev\n"
    exit 1
fi

for dir in Databases/*/
do
    dir=${dir%*/}
    dir=${dir##*/}
    dbName=tracker_${dir/-database/}_dotnet_${suffix}

    flyway -user=tracker_dotnet \
    -password=password \
    -url="jdbc:mysql://localhost:3306/${dbName}" \
    -locations=filesystem:Databases/${dir} migrate;
done
