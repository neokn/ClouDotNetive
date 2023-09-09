#!/bin/sh

export IMAGE_NAME=k8s.io/dotnet-gql-service
export IMAGE_TAG=$1
set -o allexport && source GraphQLApplication/overlays/env && set +o allexport && kubectl kustomize GraphQLApplication/overlays | envsubst | kubectl apply -f -
