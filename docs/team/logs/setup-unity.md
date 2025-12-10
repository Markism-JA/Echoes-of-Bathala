# Setup Logs Unity

**Project:** Echoes of Bathala  
**Engine Version:** Unity 6 (6000.0.60f1) LTS  
**Date:** December 11, 2025  
**Repo Status:** Initialized with ParrelSync & Netcode

---

## 1. Project Initialization

1. **Engine Selection:** Created project using **Unity 6 (6000.0.60f1)**.  
    *Constraint:* The project is locked to this editor version. Any usage of older versions (2022/2021) will cause asset serialization conflicts.

2. **Version Control Initialization:** * Initialized Git repository.
    * Configured **Git LFS** (Large File Storage) for binary assets (images, audio, models).
    * Created a standard Unity `.gitignore`.

## 2. Networking Implementation (Netcode)

1. **Package Installation:** Installed **Netcode for GameObjects** (NGO) via Unity Package Manager (UPM).
    * *Package ID:* `com.unity.netcode.gameobjects`
2. **Configuration:**
    * Generated `NetworkManager` object.
    * Created `DefaultNetworkPrefabs.asset` to register network-spawnable objects.

## 3. Local Multiplayer Testing Setup (ParrelSync)

To facilitate testing Client-Host connections on a single machine without building executables, we integrated ParrelSync.

### Step 3.1: Installation

Installed ParrelSync directly via UPM using the Git URL:

* **Method:** Window > Package Manager > "+" > Add package from git URL.
* **URL:** `https://github.com/VeriorPies/ParrelSync.git?path=/ParrelSync`
* *Verification:* `Packages/manifest.json` and `packages-lock.json` were updated.

### Step 3.2: Repository Configuration (Crucial)

Modified `.gitignore` to strictly exclude ParrelSync generated clones to prevent repository bloat and massive merge conflicts.

**Added rules:**

```text
# ParrelSync Ignore Rules
*_clone
*_clone_*
```
