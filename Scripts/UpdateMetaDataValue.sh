#!/usr/bin/env bash

SCRIPT_DIR=$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )
echo "$1 node value will be changed to '$2'"

if [ "$2" ]; then
    pwsh "$SCRIPT_DIR/UpdateMetaDataValue.ps1" -name $1 -value $2
else
    pwsh "$SCRIPT_DIR/UpdateMetaDataValue.ps1" -name $1
fi