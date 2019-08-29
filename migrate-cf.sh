#!/usr/bin/env bash

set -e

cfMigrate() {
    trap "kill 0" EXIT
        appName=$1
        migrationsPath=Databases/${appName/tracker-/}-database
        
        appGuid=$(cf app $1 --guid)
        credentials=$(cf curl v2/apps/${appGuid}/env | jq -r .system_env_json'.VCAP_SERVICES["p-mysql"][].credentials')
        
        database=$(echo ${credentials} | jq -r .name)
        username=$(echo ${credentials} | jq -r .username)
        hostname=$(echo ${credentials} | jq -r .hostname)
        password=$(echo ${credentials} | jq -r .password)
    
        openTunnel ${hostname} ${appName}
        migrate ${username} ${password} ${database} ${migrationsPath}
        closeTunnel $!
    wait
}

function openTunnel() {
    echo -e "\nOpening tunnel..."
    local hostname=$1
    local appName=$2
    
    cf ssh -N -L 63306:${hostname}:3306 ${appName} &
    sleep 5
}

function closeTunnel() {
    echo -e "\nClosing tunnel..."
    kill -9 $1
    wait $1 2>/dev/null
}

function migrate() {
    echo -e "\nRunning migrations...\n"
    local username=$1
    local password=$2
    local database=$3
    local migrationsPath=$4
    
    flyway -user=${username} \
        -password=${password} \
        -url="jdbc:mysql://127.0.0.1:63306/${database}" \
        -locations=filesystem:${migrationsPath} migrate
}

cfMigrate tracker-allocations &&
cfMigrate tracker-backlog &&
cfMigrate tracker-registration &&
cfMigrate tracker-timesheets
