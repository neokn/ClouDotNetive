#!/bin/sh

export IMAGE_NAME=mcr.microsoft.com/azure-sql-edge
export IMAGE_TAG=latest
export PWD=$(pwd)
set -o allexport && source $PWD/localSetup/database/mssql/overlays/env && set +o allexport && kubectl kustomize $PWD/localSetup/database/mssql/overlays | envsubst | kubectl apply -f -
