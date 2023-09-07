#!/bin/sh

if nerdctl -v; then
    alias docker=nerdctl
fi

export IMAGE_NAME=k8s.io/dotnet-gql-service
export IMAGE_TAG=$1
nerdctl --namespace k8s.io build -f GraphQLApplication/Dockerfile -t $IMAGE_NAME:$IMAGE_TAG -t $IMAGE_NAME:latest .
set -o allexport && source GraphQLApplication/overlays/env && set +o allexport && kubectl kustomize GraphQLApplication/overlays | envsubst | kubectl apply -f -
