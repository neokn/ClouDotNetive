#!/bin/sh

export IMAGE_NAME=k8s.io/dotnet-grpc-service
export IMAGE_TAG=$1
set -o allexport && source GrpcService/overlays/env && set +o allexport && kubectl kustomize GrpcService/overlays | envsubst | kubectl apply -f -
