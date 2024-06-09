# NATS publisher telemetry spike

## Overview

This is an example of consuming telemetry messages from OPC Publisher using a NATS client. This example is also shows, how messages can be consumed with multiple clients without duplicating them, with [Queue Subscriptions](https://docs.nats.io/using-nats/developer/receiving/queues).

## Usage

The example can be run in docker. There is a docker `docker-compose.yml` file in the root folder. The following command can run the containers:

```Bash
docker compose up -d
```

The containers can be shut down, with the following command:

```Bash
docker compose down
```

### How it works

The following services participate in the example solution:

* [OPC-PLC](https://github.com/Azure-Samples/iot-edge-opc-plc) is an OPC UA Server for simulating OPC UA Nodes.
* [OPC Publisher](https://github.com/Azure/Industrial-IoT) Is an OPC UA Client. It collects data changes from the OPC PLC and sends the data in PubSub messages to an MQTT broker. In this example the MQTT broker is the NATS server.
* [NATS](https://nats.io) is the messaging server in this example. It is exposing an MQTT broker and also enables the same topic as a NATS subject.
* [Redis](https://redis.io) is an in-memory database. The `NatsTelemetryExample` client apps, save the telemetry received on the NATS subject to this database.
* `NatsTelemetryExample` is the example application. Two instance of this application deployed to process the messages parallel as an example of client-side load balancing.

The `NatsTelemetryExample` subscribes on the NATS subject where the `OPC Publisher` publishes the `PubSub` messages. It saves them into a `Redis` database. In case the record is already there, the application logs a warning message and puts a new record into the `Error` `SortedSet`. The apps are using the same `Queue Group`, and the messages are always changing. It means, the two clients should not insert the same record twice. In case, the clients use different `Queue Groups` they will process the same messages, so error records should appear in the DB. The results can be checked on the `http://localhost:8001` address.