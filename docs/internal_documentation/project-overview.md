# Project Overview

## Development & Launch Plan (Milestones, Team, Budget)

No Content yet.

## Major Software Components

### I. Traditional Game Infrastructure (Server/Client)

| No. | Component | Core Responsibility |
| :--- | :--- | :--- |
| 1. | Game Client | Display & Player Input: Renders the game world, handles all player input, executes local game logic (like movement, animation, particle effects), and sends critical actions to the Game Server. |
| 2. | Game Server (Backend) | Core Game Logic & State: Manages the persistent, real-time game world state, verifies all player actions (combat, quest completion, item drops), and handles the high-frequency, non-tokenized internal game loop. |
| 3. | Database (Player Data/Progress) | Data Persistence: Securely stores off-chain player data (character stats, inventory *of non-NFT items*, quest progress, internal currency balances ($PERLAS$), and session history). |

### II. Web3 & Token Infrastructure (Blockchain/Crypto)

| No. | Component | Core Responsibility |
| :--- | :--- | :--- |
| 4. | Smart Contracts (Tokens/NFTs) | Economic Source of Truth: Manages the immutable rules for the token economy. Must handle $BAKU$ token ownership, minting/burning (as initiated by the Crypto Request Router server), NFT ownership/creation, and staking/governance logic (Rules burning mechanism and initial pool so on). |
| 5. | Blockchain Node / API Gateway | Network Connection & Data Access: Provides the scalable interface (via services like Infura or self-hosted nodes) for the Request Router to read real-time data from the blockchain and submit signed transactions securely. |
| 6. | Crypto Request Router | Server-to-Blockchain Bridge: Securely receives validated requests from the Game Server (e.g., "Player X earned $BAKU"), signs the corresponding transaction, and sends it to the Blockchain Node for execution. |
| 7. | Player Wallet Service / SDK | User Asset Management: Allows players to securely connect their external crypto wallet (e.g., MetaMask) to the game, sign transactions (for P2P trading), and display their on-chain token/NFT balances. |

### III. Maintenance & Sustainability

| No. | Component | Core Responsibility |
| :--- | :--- | :--- |
| 8. | Asset Storage (NFT Metadata) | Secure Media Hosting: Stores the permanent, immutable data files for your NFTs (e.g., the JSON metadata, the high-resolution art of the legendary $Filipino Myth Creature$ item), often using decentralized storage like IPFS. |
| 9. | Dedicated Analytics & Monitoring | Economic Health Tracking: Continuously ingests data from the Database (3) and the Blockchain (5) to track key metrics like $Mint/Burn Ratio$, $BAKU$ inflation, player retention, and to detect economic exploits or botting activity. |

## Documentation Overview

### Game Design Documentation Structure

#### I. Core Concept & Vision

* Project Description (Elevator pitch, genre, USP)
* Vision & Goals (What feeling do you want to evoke? What is the Tokenomics goal?)
* Target Audience & Market Analysis (Demographics, Competitors)

#### II. World & Lore (Filipino Mythology Focus)

* Worldbuilding & Lore (The inspired mythology, regions, factions, history)
* Story & Quests (Main storyline, side quests)

#### III. Gameplay Systems

##### Character & Progression

6.1. Races & Classes/Archetypes

6.2. Skills & Stats Systems

6.3. Leveling/Experience System (How does this interact with tokens?) General Idea on the source and use of the token.

##### Core Gameplay Loop

7.1. Movement, Combat Mechanics, User Skills (Controls)

7.2. Massive Multiplayer Features (Guilds, PvP, Dungeons/Raids, Social Hubs)

##### Items, Crafting & Economy

8.1. Types of Items (Weapons, Armor, Cosmetics, Tokenized Assets/NFTs)

8.2. Crafting & Gathering Systems

8.3. In-Game Economy Flow (Where do tokens/resources come from and go?)

##### Losing & Dying

* Consequences
* Token/Item Sink Mechanics

#### IV. Tokenomics & Blockchain Integration

* Token Design (Utility token vs. Governance token, ticker, supply, distribution)
* Play-to-Earn (P2E) Mechanics (How do players earn/use/stake tokens?)
* NFT Strategy (What assets are NFTs? Utility? Marketplace?)
* Security: Design Intent Only, the detailed technical plan belongs to other document.

#### V. Presentation & Technical

* Art Style & Visuals
* Music & Sound Design
* Technical Description Synthesis (Engine, Platform, Blockchain Choice)

___

### Game Economy Design Specification

#### I. Executive Summary & Core Definitions

1.1. Economic Vision: (1-2 paragraphs) The long-term goal for the economy (e.g., self-sustaining, deflationary over 5 years, low barrier to entry).

1.2. Currency Definition & Roles:

* BAKU (Utility/Governance Token): On-chain, high-value, earned from complex/high-level sinks, used for staking/governance. Directly interfaces with Component 4 (Smart Contracts).
* PERLAS (Internal Soft Currency): Off-chain, high-volume, earned from core gameplay loops, used for low-level upgrades/repairs. Stored in Component 3 (Database).

1.3. NFT Asset Categories: (e.g., Myth Creature NFTs, Land NFTs, Equipment NFTs) and their role in the economy.

#### II. Core Economy Flow: Sources and Sinks

This section uses flowcharts and tables to map every resource and currency.

2.1. BAKU - Sources (Minting/Inflation):

* Source 1: Staking Rewards / Emissions (Initial supply schedule, vesting).
* Source 2: PvP Tournament Rewards (High-risk, high-reward).
* Source 3: High-Tier Dungeon/Raid Completion (Very low drop rate).
* Mechanism Note: All BAKU sources must be validated by Component 2 (Game Server) and processed via Component 6 (Crypto Request Router).

2.2. BAKU - Sinks (Burning/Deflation):

* Sink 1: NFT Minting/Upgrading/Crafting (Permanent token burn).
* Sink 2: Access Fees (e.g., entry to exclusive FilipinoMythCreature zones).
* Sink 3: Governance Voting Fees / Treasury Contribution.
* Mechanism Note: Define the specific Smart Contract (Component 4) function calls for each burn event.

2.3. PERLAS - Sources & Sinks: (Traditional In-Game Loop)

* Sources: Mob kills, repeatable quests, selling common items.
* Sinks: Item repairs, fast travel, purchasing consumables, small PERLAS fee on P2P off-chain trades.

#### III. Mathematical & Economic Modeling

3.1. Inflation/Deflation Targets:

* Initial BAKU emission rate (Tokens/Day).
* Target Mint/Burn Ratio (e.g., 1.05:1 initially, trending to 1:1 after Year 2).

3.2. Progression Curve Formulas:

* Formulas for EXP needed per level.
* Formulas for PERLAS needed for equipment upgrades (e.g., exponential curve).

3.3. Value Relationship (BAKU vs. PERLAS):

* The single defined exchange rate or process (e.g., conversion via an in-game "Banker NPC" with a high friction/tax fee).
* Modeling the impact of BAKU price on the effective time-to-earn for PERLAS.

#### IV. NFT Strategy & Utility Deep Dive

4.1. NFT Utility Table: (For each NFT type: MythCreature, Land, Equipment)

* Source: Minting cost (BAKU + PERLAS + resource sinks).
* Utility: Stat bonus, resource generation, passive staking yield.
* Key Sink: Repair cost (PERLAS) or lifespan (BAKU burn to renew).

4.2. Royalty Structure: Percentage split for creator/treasury on secondary marketplace sales.

4.3. Asset Storage Plan: Detailed process for storing metadata (JSON) and visual assets (images/3D models) on Component 8 (Asset Storage/IPFS), ensuring immutability.

#### V. Governance & Treasury Management

5.1. Staking & Locking: Rewards schedule for players who lock BAKU.

5.2. Treasury Management Rules: Rules for how the in-game Treasury (holding BAKU from fees/sinks) will be used (e.g., 50% to liquidity, 30% for development, 20% for ecosystem grants).

5.3. Governance Mechanism: Thresholds required for a vote to pass (e.g., 5% of total BAKU supply must vote).

#### VI. Maintenance & Security (Economics Focus)

6.1. Analytics Requirements: (Directly related to Component 9).
Define 5-10 key metrics to track continuously (e.g., Token Daily Active User (DAU), New Wallet Acquisition, Average Burn per Player).

6.2. Economic Exploits & Prevention:

* Botting/farming countermeasures and detection methods.
* Security plan for the Game Server (Component 2) to prevent unauthorized mint/burn requests from reaching the Crypto Request Router (Component 6).

___

### Technical Design Documentation

#### I. Project Overview & Architecture

Context, Scope, & All Components (1-9)

| Section | Focus Area & Components |
| :--- | :--- |
| 1.1. Introduction & Goals | Define core performance and system requirements. |  
| 1.2. High-Level Architecture Diagram | Visual diagram covering all 9 components and data flow. |  
| 1.3. Technical Stack Selection | Define engine, backend, DB, blockchain, wallet stack. |

##### Checklist — Project Overview & Architecture

* Define latency goal (e.g., < 100ms real-time response)
* Define uptime target (e.g., > 99.99%)
* Define max concurrent players supported
* Define gas optimization target
* Produce labeled architecture diagram showing all 9 components & data flows
* Select game engine
* Select backend language/framework
* Select DB stack (core + cache if needed)
* Select blockchain network / L2
* Select wallet SDK

#### II. Traditional Game Infrastructure (Core Components I. 1, 2, 3)

Client, Server, Database.

| Section | Focus Area & Components |
| :--- | :--- |
| 2.1. Game Server Topology & Scaling | Define sharding, instancing, load balancing. |
| 2.2. Networking Protocol | Define client->server & server->server protocols. |
| 2.3. Database Schema (Off-Chain) | Core data models + persistence rules. |
| 2.4. Game Client Spec | Hardware, asset pipeline, anti-cheat. |

##### Checklist — Game Infrastructure

* Select sharding strategy (zone / instance / hybrid)
* Define game server autoscaling rules
* Define load balancing approach
* Choose protocol for real-time (UDP)
* Choose protocol for transactions (TCP/HTTPS)
* Define internal server messaging (Kafka/RabbitMQ etc.)
* Draw DB high-level schema
* Define persist-to-disk schedule & process
* Specify client hardware targets
* Specify anti-cheat methods

#### III. Web3 & Bridge Infrastructure (II. 4-7)

Smart Contracts, Router, Node, Wallet

| Section | Focus Area & Components |
| :--- | :--- |
| 3.1. Crypto Request Router API | Define secure endpoints for on-chain calls. |
| 3.2. Router Security | ACL, private key handling. |
| 3.3. Smart Contract Spec | Token, NFT, core functions. |
| 3.4. Blockchain Node | Hosting strategy, gas, nonce mgmt. |
| 3.5. Wallet Integration | Wallet UI/UX, connection flow. |

##### Checklist — Web3 / Bridge

* List all Router API endpoints
* Define input validation rules
* Define error codes & HTTP responses
* Configure ACL for game server IPs
* Establish private key vault storage policy
* Choose token standard (ERC-20)
* Choose NFT standard (ERC-721/1155)
* Map out on-chain functions & events
* Select blockchain node provider or self-host
* Define gas pricing & nonce handling strategy
* Select wallet SDK (e.g., WalletConnect)
* Design wallet connect/sign/disconnect flow

#### IV. Maintenance, Storage, & Security (III. 8, 9)

| Section | Focus Area & Components |
| :--- | :--- |
| 4.1. Asset Storage Pipeline | NFT metadata → IPFS → on-chain CID. |
| 4.2. Analytics & Monitoring | Logs, KPIs, dashboards. |
| 4.3. DevOps | Version control, CI/CD, staging envs. |
| 4.4. Code Standards & Testing | Tests, coverage, audits. |

##### Checklist — Ops, Storage, Security

* Define NFT metadata format
* Set IPFS pinning solution (e.g., Pinata/Filecoin)
* Automate CID storage on-chain
* Choose analytics stack
* Define KPIs (tx failures, gas cost, response time etc.)
* Set up CI/CD pipeline
* Configure Dev/Staging/Prod envs
* Define coding standards
* Set unit test coverage threshold
* Plan third-party smart contract audit
* Schedule periodic penetration testing

### Smart Contract Specification

#### I. Contract Architecture & Scope

##### 1.1 Introduction & Goals

**What to write here:**

* Define why blockchain is used in the game economy (e.g., scarcity, verifiable ownership).
* Explain the role of on-chain logic vs off-chain game logic.
* Clarify the **research scope** (testnet deployment, not production-ready).
* List PoC goals, e.g.:

  * Token mint/burn under admin control
  * NFT mint/transfer for game assets
  * Secure access control and player asset ownership

##### 1.2 Chosen Standards

Explain chosen token standards and why they fit game economics.

Document:

* Token standard used (ERC-20) — economic currency
* NFT standard used (ERC-721 or ERC-1155) — in-game items/characters
* Compatibility rationale (wallet support, EVM standardization)

Also include:

* Standard references (EIPs)
* Interoperability goals (wallets, explorers, testnet bridges)

##### 1.3 Access Control List (ACL)

Document who can perform privileged actions and why.

Include:

* Contract roles (e.g., Owner, Game Router, Player)
* Function permissions (e.g., admin-only minting)
* Rationale: prevent cheating, enforce economic integrity
* Future upgrade consideration (ex: role-based governance post-thesis)

##### 1.4 Audit & Testing Requirements

Explain testing methodology and quality metrics.

Document:

* Minimum testing goal — **unit + integration tests**
* **Target code coverage** rationale (e.g., 70%-90% for academic rigor)
* Smart contract testing tools (Hardhat/Foundry)
* Internal code review approach
* Security considerations acknowledgment

  * Reentrancy
  * Overflow/underflow
  * Access control vulnerabilities

#### II. BAKU Token (ERC-20) — Documentation Requirements

##### 2.1 Token Parameters

Document:

* Name, symbol, decimals
* Initial supply & distribution logic
* Testnet wallet allocation (treasury, dev)
* Governance choices (e.g., fixed vs mintable supply — selected rationale)

##### 2.2 Standard Token Functions

Explain and justify inclusion of:

* `transfer`
* `approve`
* `transferFrom`

Show that ERC-20 interface conformity was validated.

##### 2.3 Game-Specific Token Functions

Document game-economy logic choices:

* Mint rules (admin only)
* Burn rules (player actions or admin control)
* Gas considerations and efficiency
* Abuse prevention strategies

  * No player-side unlimited mint
  * Controlled sinks/sources only

Include example game use cases:

* Reward distribution
* Crafting economy sinks
* Marketplace fees

##### 2.4 Out-of-Scope Features (Academic Justification)

Document what you **will not implement** and why:

* Vesting schedules
* DAO governance
* Cross-chain features
* Real-money trading

Show awareness & future direction.

#### III. NFT Contract(s) — Documentation Requirements

##### 3.1 Asset Categories

Document:

* NFT types included (Character, Item, etc.)
* Scope limit — **minimal assets for PoC**
* Rationale for chosen assets (simplest to prove utility)

##### 3.2 Metadata Design

Document metadata strategy:

* Metadata server (dynamic)
* JSON metadata structure
* Traits supported in PoC
* No IPFS / no permanent storage — test environment only

##### 3.3 NFT Functions

Describe implemented functions & restrictions:

* Mint NFT (admin or router only)
* Transfer
* Approve

State why no full game logic on chain:

> Game logic stays off-chain to avoid gas cost and latency issues.

##### 3.4 On-Chain vs Off-Chain Logic

Explain separation of concerns:

* Blockchain = asset ownership + minting
* Server = game stats, combat logic, item drops
* Anti-cheat justification for server authority

Include diagram reference (for thesis later).

#### IV. Staking / Governance (Research-Only Section)

##### 4.1 Staking Concept (Not Implemented)

Define theoretical staking mechanics relevant to tokenomics literature.

Clarify **not built in PoC** but described academically.

##### 4.2 Reward Formulation

Describe formula conceptually (math expression for thesis), but no code required.

##### 4.3 Governance Model Thought Exercise

Describe future role:

* Player-driven decisions
* Token vs item-weighted governance considerations
