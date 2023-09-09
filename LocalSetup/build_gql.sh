#!/bin/sh

if nerdctl -v; then
    alias docker=nerdctl
fi

export IMAGE_NAME=k8s.io/dotnet-gql-service
export IMAGE_TAG=$1
docker --namespace k8s.io build -f GraphQLApplication/Dockerfile -t $IMAGE_NAME:$IMAGE_TAG -t $IMAGE_NAME:latest .
kubectl set image --namespace k3s deployment/dotnet-gql dotnet-gql=$IMAGE_NAME:$IMAGE_TAG