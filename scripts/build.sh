#!/usr/bin/env bash
set -ex

build_output="/tmp/build-output"
artifacts_path="./artifacts"
version=$1

mkdir -p $build_output
mkdir -p $artifacts_path

# compile apps 
(cd Applications/AllocationsServer && dotnet publish  --configuration Release --output $build_output/allocations/Applications/AllocationsServer/bin/Release/netcoreapp2.1/publish)
(cd Applications/BacklogServer && dotnet publish --configuration Release --output $build_output/backlog/Applications/BacklogServer/bin/Release/netcoreapp2.1/publish)
(cd Applications/RegistrationServer && dotnet publish --configuration Release --output $build_output/registration/Applications/RegistrationServer/bin/Release/netcoreapp2.1/publish)
(cd Applications/TimesheetsServer && dotnet publish --configuration Release --output $build_output/timesheets/Applications/TimesheetsServer/bin/Release/netcoreapp2.1/publish)

# bundle cf manifest with app
cp manifest-allocations.yml $build_output/allocations/
cp manifest-backlog.yml $build_output/backlog/
cp manifest-registration.yml $build_output/registration/
cp manifest-timesheets.yml $build_output/timesheets/

cp scripts/migrate-databases.sh $build_output/allocations/
cp scripts/migrate-databases.sh $build_output/backlog/
cp scripts/migrate-databases.sh $build_output/registration/
cp scripts/migrate-databases.sh $build_output/timesheets/

cp -r Databases/allocations-database/migrations $build_output/allocations/migrations
cp -r Databases/backlog-database/migrations $build_output/backlog/migrations
cp -r Databases/registration-database/migrations $build_output/registration/migrations
cp -r Databases/timesheets-database/migrations $build_output/timesheets/migrations

# build artifacts
tar -cvzf $artifacts_path/allocations-server-$version.tgz --directory=$build_output/allocations --remove-files . 
tar -cvzf $artifacts_path/backlog-server-$version.tgz --directory=$build_output/backlog --remove-files .
tar -cvzf $artifacts_path/registration-server-$version.tgz --directory=$build_output/registration --remove-files .
tar -cvzf $artifacts_path/timesheets-server-$version.tgz --directory=$build_output/timesheets --remove-files .