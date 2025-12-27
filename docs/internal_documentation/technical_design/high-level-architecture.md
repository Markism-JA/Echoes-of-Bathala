* **Client:** Unity 6 (IL2CPP)
* **Backend:** .NET 8 Microservices (API + Worker)
* **Database:** PostgreSQL 15
* **Blockchain:** Polygon (EVM) / Hardhat (Local)

---

## Infrastructure & Deployment Strategy

| Component | Local Development (Docker) | Production (Hybrid) |
| :--- | :--- | :--- |
| **Backend API** | Docker Container (`api`) | Docker Container (Cloud/App Service) |
| **Crypto Worker** | Docker Container (`worker`) | Docker Container (Cloud/App Service) |
| **Game Database** | Docker Container (`postgres`) | Managed Cloud SQL / Dedicated VM |
| **Unity Server** | Editor / Local Build | Dedicated Linux VM (Bare Metal) |
| **Blockchain** | Local Hardhat Node | Polygon Amoy Testnet |

### Network Topology

![High Level Architecture](/assets/internal/topology.svg)

---

## Service Breakdown

### Backend API (`/backend/src/GameBackend.API`)

* **Role:** Handles HTTP/REST requests from the Client (Auth, Inventory, Marketplace).
* **Tech:** ASP.NET Core 8.
* **Access:** Publicly accessible via Load Balancer.
* **Dependencies:** `GameBackend.Core`, `GameBackend.Shared`.

### Crypto Worker (`/backend/src/GameBackend.Worker`)

* **Role:** Background service for blockchain event listening and state reconciliation.
* **Tech:** .NET Worker Service (HostedService).
* **Access:** **Internal Only.** No public ports exposed.
* **Key Behavior:**
  * Listens to `MintNFT` events from Blockchain.
  * Updates `UserInventory` in PostgreSQL.

### Unity Headless Server (`/unity`)

* **Role:** Authoritative Game Server (Movement, Physics, Combat).
* **Tech:** Unity Dedicated Server (Linux Build).
* **Deployment:** Deployed manually/via script to Dedicated VM.
* **Communication:** UDP (Netcode for GameObjects) on Port 7777.

---

## Key Architectural Decisions (ADRs)

### ADR-001: Separation of Worker and API

* **Decision:** The Crypto Worker is a distinct microservice from the API.
* **Context:** Blockchain operations are slow and event-based.
* **Consequence:** We must maintain two Dockerfiles. We gain the ability to scale the API (user load) independently of the Worker (transaction load).

### ADR-002: Shared Project Dependency

* **Decision:** Both `API` and `Worker` depend on `GameBackend.Shared`.
* **Reasoning:** To share `Enums` (ItemRarity, etc.) and ensure the "Dictionary of the Universe" is consistent across all services.
* **Trade-off:** Minimal tight coupling in exchange for type safety.
