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

## Timeline

[Development Execution Plan](https://drive.google.com/file/d/1qVKfzZTCYacYxvLfTUYaedTA31zdUtcC/view?usp=sharing)

## To start follow the following

- [Development Guideline](development-guide.md)
- [Documentation Guideline](documentation-guide.md)
