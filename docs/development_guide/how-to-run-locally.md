# Working with the Docker Containers in Development

## Quick Start

!!! tip "Quick Start Overview"
    Use this section to quickly run the full stack or individual services during development.

### Run Everything (Full Stack)

!!! note
    Check the `docker-compose.yml` file for more information about the specific configuration of the full stack.

Spin up the entire stack using Docker:

```bash
docker-compose up --build
```

---

### Run Individual Services

Use these commands if you only need specific parts of the stack.

```bash
docker-compose up -d <container-name>
```

---

### Other Docker Commands & References

!!! note "Beyond Quick Start"
    This guide only covers the most common development workflows.
    For advanced usage (attaching, cleanup, rebuilding, logs, profiles, overrides), refer to the official Docker documentation below.

#### Useful Docker Commands

```bash
# Stop all running services
docker-compose down

# Stop and remove volumes (⚠ deletes database data)
docker-compose down -v

# Rebuild a specific service
docker-compose build api

# View logs for a service
docker-compose logs -f api

# List running containers
docker-compose ps
```

#### Official Documentation

* **Docker Compose Overview**
  [https://docs.docker.com/compose/](https://docs.docker.com/compose/)

* **Docker Compose CLI Reference**
  [https://docs.docker.com/compose/reference/](https://docs.docker.com/compose/reference/)

* **Docker Volumes (Data Persistence)**
  [https://docs.docker.com/storage/volumes/](https://docs.docker.com/storage/volumes/)

* **Docker Networking**
  [https://docs.docker.com/network/](https://docs.docker.com/network/)
