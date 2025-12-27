## Quick Start

!!! tip "Quick Start Overview"
    Use this section to quickly run the full stack or individual services during development.

### Run Everything (Full Stack)

Spin up the entire stack (**Database**, **API**, **Worker**, **Blockchain**) using Docker:

```bash
docker-compose up --build
```

---

### Run Individual Services

Use these commands if you only need specific parts of the stack.

#### Database Only (Postgres)

!!! info "Required Dependency"
    The database is required for all backend-related development.

```bash
docker-compose up -d db
```

**Connection String**

```
Host=localhost;Port=5432;Database=echoes_bathala_db;Username=admin;Password=password123
```

---

#### Blockchain Only (Hardhat)

!!! info "Smart Contract Development"
    Required when working on **Smart Contracts** or the **Crypto Worker**.

```bash
docker-compose up -d blockchain
```

* **RPC URL:** `http://localhost:8545`
* **Chain ID:** `31337` (Hardhat default)

---

#### API Only (Hot Reload)

!!! tip "Recommended for C# Iteration"
    Run the database in Docker and the API **natively** for faster hot reloads.

1. Start the database:

   ```bash
   docker-compose up -d db
   ```

2. Run the API:

   ```bash
   cd backend/src/GameBackend.API
   dotnet watch run
   ```

* **Swagger UI:** [http://localhost:5000/swagger](http://localhost:5000/swagger)

---

#### Crypto Worker Only

!!! info "Worker Dependencies"
    The worker requires both the **Database** and **Blockchain** services.

1. Start dependencies:

   ```bash
   docker-compose up -d db blockchain
   ```

2. Run the worker:

   ```bash
   cd backend/src/GameBackend.Worker
   dotnet run
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

#### When to Check the Docs

* Adding new services or dependencies
* Debugging container networking issues
* Managing volumes and persistent data
* Creating environment-specific overrides (`docker-compose.override.yml`)
* Optimizing development vs production setups
