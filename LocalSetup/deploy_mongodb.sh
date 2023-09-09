#!/bin/sh

export IMAGE_NAME=mongo
export IMAGE_TAG=7.0
export PWD=$(pwd)
set -o allexport && source $PWD/localSetup/database/mongodb/overlays/env && set +o allexport && kubectl kustomize $PWD/localSetup/database/mongodb/overlays | envsubst | kubectl apply -f -
