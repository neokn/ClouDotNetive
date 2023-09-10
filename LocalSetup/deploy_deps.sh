#!/bin/sh

export PWD=$(pwd)
sh $PWD/LocalSetup/deploy_mssql.sh
sh $PWD/LocalSetup/deploy_mongodb.sh
