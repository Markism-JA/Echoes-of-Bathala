# Project Overview

To ensure the longevity, stability and seamless cooperation within our project, we adhere to [Clean Architecture](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html). This approach prioritizes Dependency Inversion, ensuring our business logic (The "Domain") remains isolated from volatile implementation details like databases, network protocols, or UI frameworks.

The Core Rules

- Dependencies point INWARD: Inner layers (Domain/Application) must have zero knowledge of Outer layers (Infrastructure/UI/Delivery).
- Interfaces over Implementations: The Application layer defines what it needs via interfaces. The Infrastructure layer provides how it happens by implementing those interfaces.
- Data Crosses Boundaries as Simple Structures: When passing data across boundaries, use simple DTOs (Data Transfer Objects) or primitive structures. Never pass Entity Framework models or UI-specific objects into the Domain.

![The Clean Architecture Diagram](https://blog.cleancoder.com/uncle-bob/images/2012-08-13-the-clean-architecture/CleanArchitecture.jpg)

- Enterprise Business Rules : `Echoes.Domain`
- Application Business Rules : `Echoes.Application`
- Interface Adapters : `Shared.Network`
- Frameworks & Drivers : Entry points (instances), `Echoes.GameServer`, `Echoes.PersistenceWorker` and `Echoes.CryptoWorker`

## System Architecture

![System Architecture](assets/eob_architecture.png)

## Codebase Breakdown & Guidelines

### Backend

```md
├── Backend
│   ├── Echoes.API
│   │   ├── Controllers
│   │   └── Properties
│   ├── Echoes.Application
│   ├── Echoes.CryptoWorker
│   │   └── Properties
│   ├── Echoes.Domain
│   ├── Echoes.GameServer
│   │   └── Properties
│   ├── Echoes.Infrastructure
│   └── Echoes.PersistenceWorker
│       └── Properties
```

---

#### `Echoes.API`

| Project Type | ASP.NET Core Web API (.NET 8) |
| -------------- | --------------- |
| Dependencies | `Shared.Domain`, `Shared.Network`, `Shared.Abstraction`, `Echoes.Application`, `Echoes.Infrastructure` |
| Grouped by | Feature Context |

##### Scope & Responsibility

This is the HTTP delivery mechanism of the system. It exposes REST endpoints and translates HTTP requests into Application use cases.

- Controllers: Accept HTTP requests and convert them into Commands/Queries.
- DTO Mapping: Maps incoming request models to `Shared.Network` DTOs or Application commands.
- Authentication & Authorization: JWT configuration and middleware setup.
- Dependency Injection Configuration: Registers Infrastructure implementations to Application interfaces.

##### Rules

- Thin Controllers: No business logic inside controllers.
- No Direct Database Access: Always go through `Echoes.Application`.
- Framework Boundary: This project may depend on ASP.NET Core, but nothing inward may depend on it.

---

#### `Echoes.Application`

| Project Type | Class Library (.NET 8) |
| -------------- | --------------- |
| Dependencies | `Echoes.Domain`, `Echoes.Shared.Abstraction`, `Echoes.Shared.Domain` |
| Grouped by | Feature Context |

##### Scope & Responsibility

This layer contains the Application Business Rules (Use Cases). It orchestrates domain behavior and defines required infrastructure through interfaces.

- Use Cases: `RegisterPlayer`, `MintItem`, `ProcessMatchResult`, etc.
- MediatR Handlers / Command Handlers
- Repository Interfaces: `IPlayerRepository`, `IMatchRepository`
- External Service Interfaces: `ICryptoService`, `INotificationService`
- Transaction Coordination

##### Rules

- No Infrastructure Code: Only define interfaces — never implement them here.
- Orchestration Only: Coordinates Domain objects but does not contain core domain rules.
- Deterministic: Should be testable using mocked interfaces.

---

#### `Echoes.CryptoWorker`

| Project Type | Worker Service (.NET 8) |
| -------------- | --------------- |
| Dependencies | `Echoes.Shared.Domain`, `Echoes.Application`, `Echoes.Infrastructure`, `Echoes.Domain` |
| Grouped by | Feature Context |

##### Scope & Responsibility

Handles blockchain-related operations and heavy cryptographic workloads outside the HTTP lifecycle.

- Blockchain Event Listening
- Transaction Signing
- Batch Minting
- Event Publishing (e.g., CryptoTransactionCompletedEvent)

##### Rules

- Background Processor Only
- No Business Rule Ownership
- Trigger Application Use Cases when necessary
- Prefer Event-Driven Communication over Direct Calls

---

#### `Echoes.Domain`

| Project Type | Class Library (.NET 8) |
| -------------- | --------------- |
| Dependencies | None (Only standard C# libraries). |
| Grouped by | Feature Context |

##### Scope & Responsibility

This is the heart of the system. Code here should represent the core business concepts independent of how they are persisted or displayed. This should contain the core concepts that are used only in the backend if its shared with unity, put into `Echoes.Shared.Domain`.

- Entities: Core business objects (e.g., `Player`, `Match`).
- Value Objects: Domain concepts defined by properties rather than identity (e.g., `WalletAddress`).
- Domain Exceptions: Business-specific error states.

##### Rules

To keep the system modular and testable, adhere to these strict boundaries:

- Zero Infrastructure: No references to databases, ORMs, JSON serialization, or web frameworks.
- State-Focused: Logic must be purely state-oriented.
- Mock-Free Testing: If you cannot test the logic without a mock framework, the logic belongs in a different layer.

---

#### `Echoes.GameServer`

| Project Type | Worker Service (.NET 8) |
| -------------- | --------------- |
| Dependencies | `Shared.Domain`, `Shared.Network`, `Shared.Abstraction`, `Echoes.Application`, `Echoes.Infrastructure`, `Echoes.Domain` |
| Grouped by | Execution Context |

##### Scope & Responsibility

This layer acts as the orchestrator. It manages the lifecycle of the game simulation and facilitates communication between external inputs and internal business logic.

- Game Loop Orchestration: Manages the fixed-interval "Tick" (e.g., 60Hz) that advances simulation time.
- Composition Root: Acts as the centralized point for Dependency Injection, connecting abstractions to their concrete implementations.
- Adapter Wiring: Acts as the bridge between raw network protocols (WebSockets/UDP) and your `Application` service handlers.
- Environment Management: Handles configuration (ports, tick rates, secrets) and exposes system health checks.

##### Rules

- Stay Thin: This is a delivery mechanism, not a business engine.
- Zero Logic: Do not place `if/then` statements here that determine game rules or business outcomes.
- Delegate Everything: Every incoming packet or tick update must immediately trigger an action in the `Echoes.Application` layer.

---

#### `Echoes.Infrastructure`

| Project Type | Class Library (.NET 8) |
| -------------- | --------------- |
| Dependencies | `Echoes.Application`, `Echoes.Domain`, `Echoes.Shared.Domain`, `Echoes.Shared.Network`, `Echoes.Shared.Abstraction` |
| Grouped by | Execution Context |

##### Scope & Responsibility

Implements the contracts defined in the Application layer. This is the technical “How.”

- Entity Framework DbContext
- Repository Implementations
- Redis Caching
- Blockchain RPC Clients
- External API Clients
- File Storage Services
- Dependency injection

##### Rules

- Implements Interfaces Only
- May Depend on Frameworks
- Translates Persistence Models ⇄ Domain Models
- Pluggable: Should be replaceable without touching Application or Domain

---

#### `Echoes.PersistenceWorker`

| Project Type | Worker Service (.NET 8) |
| -------------- | --------------- |
| Dependencies | `Echoes.Shared.Domain`, `Echoes.Shared.Abstraction`, `Echoes.Application`, `Echoes.Infrastructure`, `Echoes.Domain` |
| Grouped by | Execution Context |

##### Scope & Responsibility

Handles asynchronous persistence and background maintenance tasks.

- Batch Saves
- Database Cleanup Jobs
- Scheduled Tasks (Cron-like jobs)
- Event Processing for Durable Storage

##### Rules

- No Core Business Logic
- Acts as a Delivery Mechanism
- Must Delegate to Application Use Cases

---

### Shared

```md
└── Shared
    ├── Echoes.Shared.Abstraction
    ├── Echoes.Shared.Domain
    └── Echoes.Shared.Network
```

#### `Echoes.Shared.Abstraction`

| Project Type | Class Library (.NET Standard 2.1) |
| -------------- | --------------- |
| Dependencies | `Echoes.Shared.Domain`, `Echoes.Shared.Network` |
| Grouped by | Feature Context |

##### Scope & Responsibility

Defines cross-boundary contracts used by both Backend and Unity. This is mainly to allow us to swap quickly between Unity Headless Server and a custom worker service implementation.

- Shared Interfaces (`IEventMessage`, `ICommandMessage`)
- Lightweight Abstractions used across systems

##### Rules

- No framework dependencies
- No business logic
- Keep minimal and stable

#### `Echoes.Shared.Domain`

| Project Type | Class Library (.NET Standard 2.1) |
| -------------- | --------------- |
| Dependencies | None (Only standard C# libraries). |
| Grouped by | Feature Context |

##### Scope & Responsibility

Contains shared enums, constants, and extremely lightweight value objects that must exist identically in both Backend and Unity.

- `ItemRarity`
- `MatchState`
- `MaxInventorySize`
- Simple deterministic calculations shared across environments

##### Rules

- No persistence logic
- No framework references
- Must remain deterministic and platform-safe

#### `Echoes.Shared.Network`

| Project Type | Class Library (.NET Standard 2.1) |
| -------------- | --------------- |
| Dependencies | `Echoes.Shared.Domain` |
| Grouped by | Feature Context |

##### Scope & Responsibility

Defines the exact shape of data crossing process or network boundaries.

- HTTP Request/Response DTOs
- WebSocket/SignalR payloads
- Binary packet contracts
- Event message contracts

##### Rules

- Pure POCOs only
- No business logic
- No validation rules beyond basic structural integrity
- Version carefully (network contracts are fragile)

### Unity

```md
└── Echoes.Game
    ├── Application
    │   ├── Client
    │   └── Server
    ├── Domain
    ├── Entrypoints
    │   ├── Client
    │   └── Server
    ├── Infrastructure
    │   ├── Client
    │   └── Server
    └── Presentation
```

### `Echoes.Game.App.Client`

| Project Type | Assembly Definition |
| --- | --- |
| **Dependencies** | `Echoes.Shared.Network`, `Echoes.Shared.Domain`, `Echoes.Shared.Abstraction` |
| **Grouped by** | Client Intent |

##### Scope & Responsibility

Orchestrates player actions and intent. It defines the *Use Cases* for the client.

- Movement prediction logic
- User input handling logic (Command pattern)
- UI navigation flow control

##### Rules

- Contains "What the client wants to do."
- Does not care *how* it's implemented (networking agnostic).
- Should not contain visual code (MonoBehaviours).

---

### `Echoes.Game.App.Server`

| Project Type | Assembly Definition |
| --- | --- |
| **Dependencies** | `Echoes.Shared.Network`, `Echoes.Shared.Domain`, `Echoes.Shared.Abstraction` |
| **Grouped by** | Server Authority |

##### Scope & Responsibility

The gatekeeper of game state. It processes requests, validates state, and executes authoritative rules.

- Request validation (Did the player actually have enough mana?)
- Server-side state transition triggers
- Authority-based logic

##### Rules

- High security: Never trust the client.
- Excludes UI/Visual logic.
- Pure Application Logic only.

---

### `Echoes.Game.Infrastructure.Client`

| Project Type | Assembly Definition |
| --- | --- |
| **Dependencies** | `Echoes.Game.App.Client`, `Echoes.Shared.Abstraction` |
| **Grouped by** | Client Plumbing |

##### Scope & Responsibility

Implementation details for the client-side plumbing.

- NGO / LiteNetLib Client implementation
- Serialization adapters (Packet -> Domain object)
- Local storage/caching

##### Rules

- **The Pivot Point:** This is where you swap libraries (e.g., NGO to LiteNetLib).
- Must implement the interfaces from `Shared.Abstraction`.

---

### `Echoes.Game.Infrastructure.Server`

| Project Type | Assembly Definition |
| --- | --- |
| **Dependencies** | `Echoes.Game.App.Server`, `Echoes.Shared.Abstraction` |
| **Grouped by** | Server Plumbing |

##### Scope & Responsibility

Implementation details for the server-side plumbing.

- NGO / LiteNetLib Server implementation
- Database repositories (if applicable)
- Server-side transport logic

##### Rules

- Must be optimized for headless execution.
- Linux/Dedicated Server optimized.

---

### `Echoes.Game.Presentation`

| Project Type | Assembly Definition |
| --- | --- |
| **Dependencies** | `Echoes.Game.App.Client` |
| **Grouped by** | View / Input |

##### Scope & Responsibility

Everything the user sees and touches.

- MonoBehaviours
- UI Framework (UI Toolkit / UGUI)
- Camera/Animation controllers

##### Rules

- **The "Thin" Layer:** Should only delegate commands to the `App.Client` services.
- Never write complex business logic here.

---

### `Echoes.Game.Entrypoints.Client`

| Project Type | Assembly Definition |
| --- | --- |
| **Dependencies** | `Echoes.Game.Presentation`, `Echoes.Game.Infrastructure.Client`, `Echoes.Game.App.Client`, `Echoes.Shared.Domain`, `Echoes.Shared.Abstraction`, `Echoes.Shared.Network` |
| **Grouped by** | Composition Root |

##### Scope & Responsibility

Wiring the client together at startup.

- DI Container setup
- Initializing the client-side Network Client
- Bootstrapping the UI

##### Rules

- **Platform Restriction:** Exclude Dedicated Server.
- This is the "Main" entry point.

---

### `Echoes.Game.Entrypoints.Server`

| Project Type | Assembly Definition |
| --- | --- |
| **Dependencies** | `Echoes.Game.Infrastructure.Server`, `Echoes.Game.App.Server`, `Echoes.Shared.Domain`, `Echoes.Shared.Abstraction`, `Echoes.Shared.Network` |
| **Grouped by** | Composition Root |

##### Scope & Responsibility

Wiring the server together at startup.

- Configuring the server-side Network Transport
- Initializing the Server authoritative loop

##### Rules

- **Platform Restriction:** Include Dedicated Server only.
- Physical firewall: No reference to `Presentation` allowed.
