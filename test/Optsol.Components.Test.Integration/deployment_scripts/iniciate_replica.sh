#!/bin/bash

echo "Starting replica set initialize"
until mongosh --host mongodb1 --eval "print(\"waited for connection\")"
do
    sleep 2
done
echo "Connection finished"
echo "Creating replica set"
mongosh --host mongodb1 <<EOF
var cfg = {
    "_id": "rs0",
    "protocolVersion": 1,
    "version": 1,
    "members": [
        {
            "_id": 0,
            "host": "mongodb1:27017",
            "priority": 2
        },
        {
            "_id": 1,
            "host": "mongodb2:27017",
            "priority": 0
        },
        {
            "_id": 2,
            "host": "mongodb3:27017",
            "priority": 0,
        }
    ]
};
rs.initiate(cfg, { force: true });
rs.secondaryOk();
db.getMongo().setReadPref('primary');
rs.status();
EOF
echo "replica set created"