#!/bin/sh

if nerdctl -v; then
    alias docker=nerdctl
fi

export IMAGE_NAME=k8s.io/dotnet-grpc-service
export IMAGE_TAG=$1
docker --namespace k8s.io build -f GrpcService/Dockerfile -t $IMAGE_NAME:$IMAGE_TAG -t $IMAGE_NAME:latest .
kubectl set image --namespace k3s deployment/dotnet-grpc dotnet-grpc=$IMAGE_NAME:$IMAGE_TAG