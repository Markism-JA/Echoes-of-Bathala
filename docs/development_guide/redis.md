# Working with the Cache Layer

The cache layer is divided into two main components: **Pub/Sub** and **Buffer**. Each serves a different purpose in the system depending on whether the data needs to persist or only exist temporarily.

!!! info "Important"
    Redis can be inspected and debugged using **Redis Insight**. Download and install it here: [https://redis.io/insight/](https://redis.io/insight/). Redis official documentation is on here: [Redis Documentation for v. 8.6](https://redis.io/docs/latest/commands/redis-8-6-commands/#string-commands).

## Overview

!!! note "Two Cache Modes"
    The cache layer operates in two distinct modes:

- **Pub/Sub** – Used for ephemeral messaging between services.
- **Buffer** – Used for temporary storage with persistence enabled via an append file.

## Pub/Sub

!!! warning "Ephemeral Data"
    The **Pub/Sub** system is designed for **real-time message passing** between services.

- Messages are **not persisted**.
- If a subscriber is not connected when the message is published, the message is **lost**.
- Used for **live updates, event broadcasting, and notifications** between API and Game Server.

!!! example "Typical Use Cases"

- Game event broadcasting
- Real-time service notifications
- Inter-service communication

## Buffer

!!! tip "Persistent Buffer"
The **Buffer** is used when messages or data must be **retained temporarily**.

- Uses **Append Only File (AOF)** persistence.
- Data is written to disk so it can survive service restarts.
- Acts as a **temporary durable queue or storage layer**.

!!! example "Typical Use Cases"

- Event buffering
- Temporary job queues
- Data that must survive service restarts

## Key Distinction

!!! summary "Pub/Sub vs Buffer"
    | Feature | Pub/Sub | Buffer |
    |------|------|------|
    | Persistence | No | Yes (AOF) |
    | Message Lifetime | Ephemeral | Temporary but durable |
    | Delivery Guarantee | Only to active subscribers | Stored until consumed |
    | Primary Use | Real-time events | Buffered processing |
