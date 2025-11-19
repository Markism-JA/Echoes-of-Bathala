# Scope and Limitations of the Study

This study focuses on the development of a desktop-based massively
multiplayer online role-playing game (MMORPG) inspired by Philippine
folklore that integrates blockchain-based assets. The primary objective
is to demonstrate the functionality of the game's core gameplay loop and
the tokenomics system, which utilizes in-game tokens and non-fungible
token (NFT) assets to simulate a player-driven economy.

## Scope of the Study

The scope of this study is threefold: designing, developing, and
evaluating a prototype of a Massively Multiplayer Online Role Playing
Game (MMORPG). The study seeks to incorporate traditional folklore,
cultural narratives, customs, and mythological symbols into the game's
core design, environment, and gameplay systems to act as a social sink.

Furthermore, the study defines a dual-currency economic structure to
demonstrate the integration of blockchain technology. This system
distinguishes between Perlas, the primary in-game off-chain resource,
and BAKU, the on-chain cryptocurrency. The acquisition of Perlas acts as
the central economic faucet, obtained through specific distinct gameplay
loops. Players may acquire Perlas by harvesting Giant Taklobo (Clams)
located in aquatic ponds. Alternatively, Perlas can be looted from
hostile entities, but this is restricted to specific high-risk areas:
the Contested Zone (Low Security), which yields lower drop rates, and
the Void Sector (Lawless Null Sec), which offers significantly higher
drop rates to incentivize high-stakes gameplay.

To bridge the off-chain and on-chain economies, the study covers the
development of the Baku Forge mechanics. This system facilitates the
conversion process, allowing players to exchange their accumulated
Perlas for BAKU tokens, effectively transferring value from the game
server to the blockchain.

The following contains the software separated into major components, its
subsystems, and the features of each subsystem:

### Game Server

This is one of the main components of the software. The game server is
the single, authoritative source of truth for all persistent world data
and game logic, responsible for calculating player interactions,
validating movements, managing the state of all characters and
environments, and synchronizing that data across every connected client
in real-time.

1.  **World Simulation**

    ::: decimallist
    Contains the states for the environment, acting as a dedicated
    subsystem responsible for tracking and synchronizing the following
    persistent data.

    1.  Entity - General container for persistent objects.

    2.  Non-Player Characters - Quest givers.

    3.  Player Objects - Hauling cart.

    4.  Resource Nodes - Perlas, Ores.

    5.  Collective Cooldown and Effects - Zone debuffs, Event Cooldowns.

    Contains the data and authoritative engine for all spatial
    interactions and character locomotion.

    1.  Position - Stores their location using X, Y, Z coordinates

    2.  Collision Check - Determine if entities are physically
        overlapping

    3.  Movement Prediction - Validating the client's reported movement
        against the server's predicted state to prevent lag.

    4.  Map Boundaries - Defines the legal perimeter for zones and
        regions.

    5.  Hitbox State - Manages the size, shape and active frames of
        hitboxes.

    Responsible for handling all aspects of instanced, temporary, or
    specialized gameplay zones.

    1.  Dungeon State - Manages the current progress and status of a
        dungeon.

    2.  Spawn - Handles the initial placement and subsequent respawn
        logic for all enemies.

    3.  Timer - Administers all time-related constraints, includes
        cooldown, allowable time.

    4.  Loot Roll Manager - Manages the fair and structured distribution
        of unique rewards dropped within the dungeon.

    Contains processing, execution, and state management of all
    Non-Player Characters (NPCs), monsters, and boss encounters. It
    governs all aspects of enemy and npc behavior.

    1.  Behavior Trees - A hierarchical structure used to model complex,
        conditional enemy decision-making and action sequencing.

    2.  Threat Table - A data structure tracking numerical amount of
        threat or enmity.

    3.  State Machine - System that defines and manages the behavioral
        state of monster.

    4.  Pathfinding - System responsible for calculating the optimal
        route for an monster

    5.  Aggro System - Logic to designate the current target of the
        monster.

    6.  Schedule - Manages the time-based non-combat behaviors of NPC or
        monsters.

    Authoritative core of all player-versus-environment and
    player-versus-player (PvP) conflicts and battles.

    1.  Stat Calculation - Calculates the current numerical values of
        all character attributes.

    2.  Status Effect State - Manages application, duration and ticks of
        current buffs and debuffs.

    3.  Damage Events - Central process that resolves an attack and
        calculates the final damage value.

    4.  Cooldowns - Responsible for checking the timer to determine if a
        skill can be executed.
    :::

2.  **Player Management**

    ::: decimallist
    Handles the initial player login and account verification against
    the database.

    1.  Credential Validation - Validates username/password or other
        login methods.

    2.  Session Token Generation - Creates the unique, temporary
        cryptographic key (Session Token) upon successful login.

    3.  Character List Retrieval - Fetches the list of characters
        associated with the account from the Game Database

    Responsible for tracking all temporary, real-time, and volatile data
    associated with an actively logged-in player character.

    1.  Core Player Statistics - Stores the current values of the
        player's character attributes.

    2.  Status Effects - Tracks all active personal buffs and debuffs
        currently affecting the player character.

    3.  Inventory Change - Records and manages all temporary
        modifications to the player's inventory that occur during a
        session.

    4.  Cooldowns - Manages the current remaining time until a players
        specific skill, ability, or item can be used again.

    5.  Uncommitted Quest Progress - Stores all temporary advancement
        within active quests that has not yet been finalized and saved.

    6.  Temporary Combat Logs - Records a brief, volatile history of
        recent combat events.

    7.  Input Command Queues - A temporary buffer that stores the
        sequence of actions sent by the player's client.

    Manages the server's active record of all logged-in users and their
    authenticated connection status.

    1.  Connected Players - A live list tracking all current users who
        have an established and active network connection to the game
        server.

    2.  Session Tokens - Manages the creation, validation, and
        expiration of unique, temporary cryptographic keys assigned to a
        player upon successful login.

    3.  Player State - Tracks the connection status and current activity
        of a player within the game world.
    :::

3.  **Others**

    ::: decimallist
    Manages the primary progression and social reward structures of the
    game.

    1.  Quest Engine - Manages the lifecycle of all quests and cultural
        narratives, from assignment to completion. Also responsible for
        awarding non-crypto currency, items, and, in specific cases,
        BAKU tokens or Perlas.

    2.  Loot System - Governs the server-side determination and
        distribution of items and currency dropped by defeated enemies
        or contained in discoverable chests. This includes the
        calculation of item drops.

    3.  Crafting Queues - Manages the sequential processing and
        time-gating of all production and crafting orders initiated by
        players.

    4.  Parties - Manages the formation, membership, and social rules of
        temporary player groups.

    5.  Player-to-player Trading System - Manages the synchronous,
        secure, two-player trade interface to exchange non-crypto items
        and currency.

    6.  Talipapa System (Marketplace) - Manages the authoritative
        server-side logic for the asynchronous marketplace, including
        posting listings, processing buyouts, collecting fees, and
        sending items/currency via an in-game mail system.

    Manages the low-level, foundational services that maintain the
    operational timing, and persistence required for the entire Game
    Server to function securely and reliably.

    1.  Scheduler Tick - The central, authoritative clock and heartbeat
        of the entire server simulation.

    This server-side module executes the permanent and irreversible
    logic when a player attempts to enhancement an item.

    1.  Item Validity - Performs the initial critical validation that
        the submitted item or materials are valid.

    2.  Cost Manager - Calculates and deducts all required resources and
        currency before any enhancement attempt begins.

    3.  Success Probability Scaling - Executes the central logic for
        determining the enhancement outcome.

    4.  Attribute applier - Contains the probability scaling logic to
        increase an equipment attributes.

    5.  Item State Manager - Manages the logic that implements the
        consequences of success and failure.

    This server-side module executes the permanent and irreversible
    logic when a player attempts to upgrade or enchant an item.

    1.  Item Validity - Performs the initial critical validation that
        the submitted item or materials are valid.

    2.  Cost Manager -Calculates and deducts all required resources and
        currency before any enhancement attempt begins.

    3.  Success Probability Scaling - Executes the central logic for
        determining the enhancement outcome.

    4.  Tier Applier - Contains the logic to increase an equipment tier.

    5.  Item State Manager - Manages the logic that implements the
        consequences of success and failure.

    6.  Tier Cap - Contains the logic for an item maximum tier level.

    This server-side module handles the complete lifecycle of creating
    new equipment: validating ingredients, consuming resources.

    1.  Recipe Manager - Manages the lookup, validation, and
        prerequisites for every crafting recipe.

    2.  Equipment Creation - Executes the core, non-reversible steps of
        the crafting process, from consuming materials to determining
        the item's final attributes.

        1.  Combine Item - Definitive step where the required input
            materials are simultaneously and authoritatively removed
            from the player's inventory and consumed by the system.

        2.  Apply Stats - Generates the base stats or attributes for the
            new item.

        3.  Success or Failure - Executes the calculation to determine
            the outcome of the craft.

    3.  Equipment Creation Logic - Contains the logic for the entire
        flow of crafting process.

    This server-side module manages the player-driven conversion of
    in-game resources (Perlas) into the on-chain fungible token (BAKU).
    This is a primary economic faucet.

    1.  Context Validation - Verifies the player is interacting with the
        correct NPC or is within the designated "Baku Forge" zone.

    2.  Resource Cost Manager - Calculates and deducts the required
        amount of Perlas from the player's off-chain inventory (Game
        Database).

    3.  Conversion Rate Logic - Applies the current server-defined
        exchange rate.

    4.  Token Request Emitter - Upon successful validation and resource
        deduction, this component generates a secure request to the
        Crypto Request Router to mint or transfer the calculated BAKU
        amount to the player's linked wallet.

    This server-side module handles the player-initiated process of
    converting a specific, eligible in-game item (from the Game
    Database) into a permanent Non-Fungible Token (NFT) on the
    blockchain.

    1.  Item Eligibility Check - Validates that the item submitted by
        the player (e.g., a "Legendary" crafted weapon) is eligible for
        tokenization.

    2.  Minting Cost Manager - Calculates and deducts the required cost
        for minting, which could be BAKU tokens, non-crypto currency, or
        other rare materials.

    3.  Item State Conversion - Authoritatively flags the item in the
        Game Database as "tokenized," linking it to its future on-chain
        ID and making it non-volatile.

    4.  NFT Request Emitter - Securely packages the item's metadata
        (stats, name, rarity, ID) and sends a minting request to the
        Crypto Request Router to create the NFT on the blockchain.

    This is the authoritative server-side counterpart to the client's
    Progression System. It manages the permanent, non-volatile state of
    a character's growth, abilities, and skills, ensuring all
    progression is validated and legitimate.

    1.  Experience and Leveling Manager - Authoritatively handles all
        experience point (XP) gain and the leveling-up process. It
        receives XP "grant" commands from other systems (like Quest
        Engine and Loot System) and calculates when a character levels
        up, triggering the availability of new points or skills.

    2.  Attribute Manager - Manages the "bank" of available attribute
        points (e.g., Strength, Intellect) for a character. It validates
        and processes all player requests to allocate these points, then
        updates the character's base stats in the Game Database.

    3.  Class & Profession Manager (Separate and related) - Contains the
        definitive logic, rules, and skill trees for all player classes
        (Anito's Heir System) and professions (Blacksmith and
        Craftsmith). It manages the character's current level in each
        and validates all requests to learn or upgrade class-specific
        abilities or profession-specific recipes.

    4.  Skill Manager - Manages the authoritative list of skills a
        character has learned. It validates all prerequisites (e.g.,
        level, class, attribute requirements) before unlocking a new
        skill for a character, making it available for the Combat System
        (1.5) to use. It also handles the upgrading logic of skill via
        skill scrolls.
    :::

### Game Client

One of the main components of the software. Its primary role is to
render the virtual world based on data received from the Game Server and
to capture, queue, and transmit player input as commands to the server.

::: decimallist
Responsible for translating the player's physical input into digital
commands and visual representations of the player character's actions on
the local screen.

1.  Movement - Handles the client-side interpretation and execution of
    directional input.

2.  Animation - Manages the visual rendering of the player character's
    actions and state.

3.  Targeting - Manages the player's client-side system for selecting,
    directing and locking onto enemies or interactable objects.

Manages the visual representation of the player's advancement and
character development and details. It displays all progression-related
data received from the Game Server.

1.  Quest Tracker - Displays the current status of all active quests.

2.  Levelling - Manages the visual presentation of the player's overall
    level and progression status.

3.  Experience Curve - Displays the percentage of current experience
    points accumulated relative to amount of experience points remaining
    until the next level.

4.  Class System - Renders the visual interface and mechanics related to
    the character's chosen class.

5.  Equipment System - Manages the client-side inventory and character
    gear display.

6.  Skill System - Displays the visual interface for all character
    skills or abilities.

7.  Profession System - Renders the user interface for all non-combat
    economic specializations or skills.

8.  Attributes - Renders the character's core statistics (e.g.,
    Strength, Agility, Intellect, Vitality). This interface displays the
    current value of each attribute, the points available for allocation
    (if any), and tooltips explaining how each attribute contributes to
    secondary stats like attack power, critical strike chance, and
    health.

This client-side module manages the visual presentation and input
mechanisms for all resource acquisition and item production.

1.  Recipe - Manages the display of the player's learned item blueprints
    and crafting formulas.

2.  Creation - Handles the visual interface and confirmation for
    initiating a crafting order.

3.  Crafting Chance - Displays the server-determined percentage
    likelihood of success or failure before the player commits to the
    crafting action.

4.  Upgrade System - Manages the client-side user interface and logic
    for modifying or improving existing items, often requiring
    additional materials or currency.

    1.  Enhance - Renders the interface for increasing an item's base
        statistics or numeric tier.

    2.  Enchant - Renders the interface for applying enhancement to an
        item.

Manages the visual presentation and player interaction with all stored
items, resources, and currency.

1.  Item interaction - This term covers the entire range of things a
    player can do with an item once it is in their inventory, including
    equipping, unequipping, moving, splitting stacks, using, selling, or
    destroying.

2.  Slots - Renders the visual structure of the inventory grid.

3.  Stack - Handles the visual representation and management of item
    quantity within a single inventory slot.

4.  Upgrading - Manages the client-side interface and visual
    confirmation for increasing the player's total storage capacity.

This module manages the visual rendering and information display for all
items in the game world, inventory, and equipment slots.

1.  Equipment - Manages the client-side representation and stat
    comparison interface for items designed to be worn.

2.  Drop Rate - Renders the visual presentation of the probability that
    a specific item, or category of items, will be acquired from an
    enemy or container.

3.  Durability - Manages the visual status and health bar of an item's
    current wear and tear.

4.  Consumables - Handles the visual feedback and prompt for single-use
    items.

5.  Item Metadata - Renders the detailed descriptive information about
    the item in tooltips and inspection windows.

6.  Rarity - Manages the visual and stylistic representation of an
    item's unique quality.

7.  Item Drop UI - Renders the visual window or notification that
    appears when an enemy is defeated or a container is opened,
    displaying the icons, names, and quantities of the items that have
    dropped. This interface allows the player to loot the items,
    transferring them to their inventory.

This client-side module manages all interfaces and functions related to
player-to-player communication, grouping, and cooperative interaction,
drawing upon the authoritative data from the Game Server's Gameplay
System.

1.  Chats - Renders and manages the visual interface for all real-time
    text communication channels.

    1.  Area Chat - A localized communication channel that displays
        messages from players within a specific in-game radius or zone.

    2.  Party Chat - A private communication channel exclusive to
        members of the same party or group.

    3.  World Chat - A global communication channel visible to all
        connected players across the entire game world.

2.  Parties - Displays the visual status of all members within the
    player's current cooperative group.

3.  Item Distribution - Renders the visual interface and mechanics for
    determining how loot is shared among party members.

4.  Friends List - Displays the current connection status and location
    of a player's designated social contacts.

5.  Player-to-Player Trade UI - Renders the secure, synchronous
    interface for two players to exchange items and currency. This
    includes item slots for each player's offer, a currency field, and a
    two-stage "confirm" and "finalize" process to validate the trade on
    the server.

This comprehensive client-side module manages the aesthetic design,
layout, and functionality of all interactive graphical elements
presented to the player.

1.  HUD - Manages the always-visible, non-intrusive overlay that
    presents essential, real-time status information.

2.  Map - Renders the visual interface for the game world's geography
    and current player or NPC location.

3.  Talipapa UI - Shows the visual interface or button that accesses the
    talipapa, the auction system.

4.  Menus - Manages the standard hierarchical pop-up windows that
    contain and organize the various other system interfaces.

5.  Pre-Game Menus - Manages the UI flow that occurs before a player
    loads into the game world.

    1.  Login Screen - Renders the interface for account authentication
        (e.g., username, password) and connects to the server's
        Authentication System.

    2.  Character Selection/Creation - Renders the interface to view,
        select, delete, or create new characters. This includes class,
        profession, and appearance customization which communicates with
        the server's Character Progression System.

6.  Settings Menu - Renders the interface for configuring all local
    client options, including:

    1.  Login Screen - Renders the interface for account authentication
        (e.g., username, password) and connects to the server's
        Authentication System.

    2.  Character Selection/Creation - Renders the interface to view,
        select, delete, or create new characters. This includes class,
        profession, and appearance customization which communicates with
        the server's Character Progression System.

7.  Audio Settings - Manages all volume sliders (e.g., Master, Music,
    Sound Effects).

8.  Graphics Settings - Manages resolution, texture quality, and other
    performance-related options.

9.  Keybindings - Manages the mapping of user inputs to in-game actions.

This client-side module manages all visual feedback and special effects
related to conflicts and battles.

1.  PVE - Player Versus Environment blueprints

    1.  Aggro UI - Renders the visual indicator that a monster is
        targeting a player.

    2.  AI Mob Action -Manages the visual rendering of special actions
        or abilities performed by monsters and bosses.

2.  PVP - Player Versus Player

    1.  PK Mode - Manages the client-side visual display of a player's
        current hostility or PvP engagement status.

3.  PVP and PVE - Shared features for both

    1.  Visual Hit Confirmation - Renders visual confirmation that an
        attack has successfully landed on a target.

    2.  Damage or Heal - Manages the rendering of floating numerical
        text for all damage dealt and healing values.

    3.  Status Effects - Displays the visual icons, tooltips, and
        remaining timer bars for all temporary buffs and debuffs.

    4.  Health and MP Bar - Manages the visual rendering of the player
        and target's current Health and Resource (MP) bars.

    5.  Cooldown - Renders the visual overlay on skill buttons in the
        player's HUD to indicate the time remaining until a skill is
        available for use.

This client-side module manages all visual and interfaces related to the
kuchero.

1.  Cart - Manages the visual rendering of the object used for hauling

This client-side module provides the visual interface for the in-game
market.

1.  Search - Manages the input fields and logic for filtering the market
    listings.

2.  Sorting - Handles the client-side arrangement and display order of
    the market listings.

3.  Market Display - Renders the main interface for viewing item
    listings. This includes displaying the item icon, quantity, seller's
    name, the item's metadata, the list price, and any associated
    transaction fees.

This client-side module is responsible for the visual projection and
real-time updating of all player characters, NPCs, objects, and the
static environment.

1.  Models - Manages the loading and display of the 3D character models
    for the player, all other players, and NPCs.

2.  Dialogue - Renders the visual interface for all narrative and
    conversational interactions with NPCs.

3.  Environment - Manages the loading and static rendering of the game's
    physical world, including terrain, buildings, weather or zone
    effects, and skyboxes.

4.  Hostiles or Enemies - Focuses on the unique rendering requirements
    for combat-active entities.

    1.  Animation - Manages the visual sequencing of all movement and
        combat actions performed by hostile NPCs and monsters.

    2.  Targeting - Renders the visual feedback of a monster skill
        designation or range.

5.  Interactable Objects State - Renders the visual state of all
    interactable resource nodes and world objects.

6.  Particle System Rendering - Manages the creation and rendering of
    all dynamic visual effects.

7.  Interaction and Feedback indicators - Renders contextual visual
    elements over entities and world objects.

This module is responsible for managing and playing all audio within the
client.

1.  Background Music (BGM) - Handles the streaming, looping, and
    playback of ambient music for different zones, menus, and boss
    encounters.

2.  Sound Effects (SFX) - Manages the playback of all event-driven
    sounds, including spatial (3D) effects for combat and environment
    (e.g., footsteps, skill hits) and interface (2D) sounds for button
    clicks and notifications.

This module manages all client-side UIs that interact directly with the
server's Token Infrastructure and related economy systems.

1.  Baku-Forge UI - Renders the dedicated interface and world object
    (inside NPC towns) for converting in-game resources (e.g., Perlas)
    into the BAKU fungible token. This UI displays the current
    server-defined conversion rate, the player's available resources,
    and a confirmation button that sends the conversion request to the
    server's Baku Forge System.

2.  NFT-Minting UI - Renders the interface that allows a player to
    select an eligible, server-validated in-game item (e.g., a
    "Legendary" crafted item) and initiate the process to mint it into
    an on-chain NFT. This UI displays the item, the BAKU or resource
    cost, and a final confirmation to send the request to the server's
    NFT Minting System.

3.  Wallet Management UI - Renders the interface in the settings menu
    that allows a player to securely link, verify, or view their
    external cryptocurrency wallet address with their game account. This
    UI communicates with the server's Wallet Service.
:::

### Game Database

A main component of the software. This is the non-volatile storage
system where the Game Server persists all critical information.

1.  **Database API (ASP.NET RESTful Service)**

    This is the secure application layer that manages all data
    transactions. It exposes a series of RESTful endpoints that the Game
    Server consumes.

    ::: decimallist
    \- Provides endpoints for user authentication (login, registration),
    session validation, and character management (creation, selection,
    retrieval).

    \- Provides endpoints for saving and loading a character's
    persistent state, including their inventory, equipment, quest
    progress, and attributes (e.g., /character/{id}/save,
    /character/{id}/inventory).

    \- Provides endpoints for managing all persistent economic actions,
    such as posting/canceling Talipapa listings, retrieving crafting
    history, and logging Baku Forge conversions.

    \- Provides endpoints for the Game Server to load static,
    non-volatile game rules, such as NPC definitions, item blueprints,
    and quest definitions, on startup.
    :::

2.  **SQL Instance**

    This is the non-volatile storage layer. It contains the actual data
    schemas, tables, and stored procedures managed by the API.

    ::: decimallist
    Tables containing the authoritative, persistent record for all user
    and character data.

    1.  Account Tables - Stores hashed credentials, account status, and
        associated wallet addresses.

    2.  Character Tables - Stores the persistent state for each
        character (e.g., level, XP, class, attributes, current zone).

    3.  Inventory & Equipment Tables - Stores all persistent item
        instances, their quantity, durability, and which character or
        storage container they belong to.

    4.  Quest Progression Tables - Stores the state (e.g.,
        "in-progress," "completed") of all quests for each character.

    5.  Social Tables - Stores persistent social data (e.g., Friends
        List).

    Tables containing the static, read-only "blueprint" data for the
    game.

    1.  Entity Definitions - Stores the base stats, models, and
        abilities for all NPCs and enemies.

    2.  World Definitions - Stores static world data (e.t., zone names,
        map boundaries).

    3.  Game Rule Tables - Stores the comprehensive rules for combat
        (e.g., damage formulas), progression (e.g., XP curves), and
        crafting (e.g., recipes, probability scales).

    Tables that manage the persistent state of all economic and
    item-related transactions.

    1.  Item Instance Table - The authoritative table for every unique
        item in the game, including those with NFT linkage.

    2.  Manufacturing & Transaction Logs - Stores the audit trail for
        all crafting, enhancement, and Baku Forge conversions.

    3.  Talipapa Listing Table - Stores all active, volatile listings
        for the player-to-player marketplace.

    <!-- -->

    1.  Event Log Tables - Stores detailed, time-stamped logs of
        significant player and system actions (e.g., trades, boss kills,
        high-value-item drops).
    :::

### Token Infrastructure

This module defines the smart contract architecture, token standards,
and core utility for the digital assets and currency that form the
foundation of the game's economy and unique ownership mechanics. It acts
as the definitive bridge between the centralized Game Server and the
decentralized blockchain.

::: decimallist
This is the low-level middleware service responsible for all direct
communication with the blockchain's RPC nodes. It abstracts the
complexity of on-chain transactions, acting as the "last stop" for
requests.

1.  Transaction Submission: Takes finalized, signed transaction requests
    from the Crypto Request Router and broadcasts them to the blockchain
    network.

2.  Gas Fee Management: Handles gas price estimation to ensure
    transactions are processed in a timely and cost-effective manner.

3.  Nonce Management: Manages the sequential transaction nonce for the
    server's "hot wallet" to prevent transaction re-ordering or
    re-sends.

4.  Receipt Confirmation: Listens for transaction confirmations from the
    network and reports the status (success or failure) back to the
    Router for logging.

These are the on-chain, deployed contracts that define the properties
and logic of the game's assets.

1.  **BAKU (Fungible Token):** The game's primary economic token,
    compliant with the ERC-20 standard.

    -   **Key Functions:** Exposes standard `transfer()`, `approve()`,
        and `balanceOf()` functions, as well as a privileged `mint()`
        function callable only by the Crypto Request Router for
        distributing tokens from the Baku Forge.

2.  **Game Assets (Non-Fungible Token):** The contract for unique
    in-game items (legendary equipment), compliant with the ERC-721
    standard.

    -   **Key Functions:** Exposes standard `safeTransferFrom()` and
        `ownerOf()` functions.

    -   **Metadata Management:** A privileged `mint()` function
        (callable only by the Router) that creates a new token and
        assigns its `tokenURI` (describing the item's stats).

    -   **Metadata Update:** A privileged `setTokenURI()` function to
        allow the Router to update an NFT's metadata when it is enhanced
        on the Game Server.

This is the primary security, queuing, and validation layer that sits
between the Game Server and the Blockchain API Gateway. It is the only
component authorized to call privileged mint or update functions on the
Smart Contracts.

1.  Request Validation & Security: Authenticates all incoming requests
    from the Game Server (via a secure API key) to ensure they are
    legitimate.

2.  Transaction Queuing: A persistent queue that holds all transaction
    requests, ensuring they are processed reliably even if the Gateway
    or blockchain is slow.

3.  Retry Logic: Manages failed transactions, automatically retrying
    them with new gas prices or logging them for manual inspection if
    they fail repeatedly.

4.  Prioritization: Prioritizes high-value transactions (a player
    minting an NFT) over lower-value ones (batch-distributing BAKU
    rewards).

This is a dedicated service (part of the Database API or a standalone
microservice) that manages the secure link between a player's in-game
account and their external blockchain wallet.

1.  Wallet Linking: Provides an endpoint for a player to link their
    wallet address (MetaMask) to their in-game account ID in the Game
    Database.

2.  Ownership Verification (Message Signing): Secures the linking
    process by requiring the player to sign a unique, server-provided
    message (a "nonce") with their wallet's private key. The service
    then verifies this signature on the backend to prove ownership
    without ever seeing the private key.

3.  Read-Only Access: Provides the Game Server with a secure way to look
    up a player's linked wallet address for sending BAKU or NFTs.
:::

## Limitations

These limitations are recognized as necessary constraints due to time,
technical resources, and the project's focus on demonstrating
feasibility rather than commercial readiness. Further testing and
broader deployment are recommended to validate the system's reliability
and scalability.

-   The game application does not support cross-platform compatibility
    such as web or mobile versions. It will run exclusively as a desktop
    game or application.

-   The future demonstration limits the player to 75 levels, which
    restricts the scope of content available for evaluation. Additional
    levels may introduce new mechanics or challenges not represented in
    this version.

-   The demonstration is limited up to Chapter Two of the narrative,
    limiting exposure to later story developments. This prevents
    assessment of the full narrative flow and character progression.
    Other chapters may present themes or mechanics that are not
    reflected in this showcase.

-   The system lacks enterprise-level security measures typically
    required for commercial deployment. Only basic or prototype-tier
    protections are implemented.

-   All blockchain, NFT, and token functionalities are confined to the
    Polygon testnet rather than a live mainnet. This limits the realism
    of performance, cost, and security assessments.

-   The study does not include large-scale player participation needed
    to evaluate system stability and player-driven economies. long-term
    balance metrics such as inflation, resource scarcity, and market
    behavior are not analyzed.

-   The narrative serves as a creative adaptation of Filipino mythology;
    however, the researcher has exercised creative liberties to align
    these cultural narratives with the game's core systems.
