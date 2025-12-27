# Project Overview

## Directory Structure

```md
/Echoes-of-Bathala
│
├── /unity
│   └── Echoes-of-Bathala                 # Game Client & Headless Server
│       ├── /Assets
│       │   ├── /Project
│       │   │   ├── /Art
│       │   │   │   ├── /Characters
│       │   │   │   ├── /Environment
│       │   │   │   └── /UI
│       │   │   ├── /Audio
│       │   │   ├── /Scenes               # Login, Town, Dungeon
│       │   │   └── /Scripts              # Game Logic
│       │   │       ├── /Client           # UI, Input, VFX
│       │   │       ├── /Server           # Simulation, AI, Physics (Authoritative)
│       │   │       └── /Netcode          # NetworkVariables, RPCs
│       │   ├── /Plugins                  # 3rd-party assets
│       │   └── /StreamingAssets          # Runtime configs
│       ├── /Packages                     # Unity manifest (refs shared backend)
│       └── /ProjectSettings              # Tags, Layers, Physics
│
├── /backend
│   ├── GameBackend.sln
│   ├── docker-compose.yml                # SQL Server & service config
│   │
│   └── /src
│       ├── /GameBackend.Shared           # Bridge library (Unity ↔ Backend)
│       │   ├── /DTOs                     # LoginRequest, AuthResponse
│       │   ├── /Enums                    # ZoneType, ItemRarity
│       │   ├── GameBackend.Shared.asmdef # Unity Assembly Definition
│       │   ├── GameBackend.Shared.csproj
│       │   └── package.json              # Unity package manifest
│       │
│       ├── /GameBackend.API              # REST API service
│       │   ├── /Controllers              # AuthController, MarketController
│       │   └── Program.cs
│       │
│       ├── /GameBackend.Worker           # Background / crypto router
│       │   ├── /Jobs                     # MintNFTJob
│       │   ├── /Services                 # NonceManager
│       │   └── Program.cs
│       │
│       ├── /GameBackend.Core             # Internal domain logic
│       │   ├── /Entities                 # User, Item
│       │   └── /Interfaces               # IRepository
│       │
│       ├── /GameBackend.Infra            # Infrastructure & externals
│       │   ├── /Persistence              # EF Core DbContext
│       │   └── /Blockchain               # Nethereum wrappers
│       │
│       └── /test
│           └── /GameBackend.Tests
│
└── /blockchain                           # Hardhat environment
    ├── /contracts
    ├── /scripts
    ├── /test
    └── hardhat.config.ts
```

### Dependencies

| Directory / Project | Role | Depends On (References) | Why? |
| --- | --- | --- | --- |
| **`/backend/src/GameBackend.Shared`** | **The Dictionary** | *None* (Leaf Node) | Contains DTOs, Enums, Constants used by everyone. |
| **`/backend/src/GameBackend.Core`** | **Domain Logic** | `Shared` | Uses Enums to define Entities and Interfaces. |
| **`/backend/src/GameBackend.Infra`** | **External Tools** | `Core`, `Shared` | Implements Repo interfaces from Core; uses Shared types. |
| **`/backend/src/GameBackend.API`** | **Web Service** | `Core`, `Infra`, `Shared` | Orchestrates the app; Inject dependencies. |
| **`/backend/src/GameBackend.Worker`** | **Background Svc** | `Core`, `Infra`, `Shared` | Executes domain logic triggered by events. |
| **`/unity`** | **Game Client & Server** | `Shared` (via csproj ref) | Needs DTOs to talk to API; Enums for game logic. |
| **`/blockchain`** | **Smart Contracts** | *None* | Independent TS/Solidity project. |

## Timeline

[Development Execution Plan](https://drive.google.com/file/d/1qVKfzZTCYacYxvLfTUYaedTA31zdUtcC/view?usp=sharing)

## To start follow the following

- [Development Guideline](development-guide.md)
- [Documentation Guideline](documentation-guide.md)
