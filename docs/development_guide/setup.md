## Setup

### Dependencies

* **SSH** — Authentication and deployment
* **Git** — Version control
* **GitHub CLI** — Project and developer suite management
* **.NET** — Backend
* **Unity** — Game client and simulation (Service 1)
* **Docker** — Containerization for deployment and testing
* **Hardhat** — Smart contract development and local testnet simulation
* **Volta** — Node.js toolchain and version management

---

### Editor Requirements

You may use any editor as long as it meets the following requirements:

* General-purpose editor for documentation and codebase management.
* **Solidity + TypeScript LSP support** for smart contract development.
* **Good Unity integration** for writing and debugging scripts.
* **Strong C# language support** for backend development.

**Example (Lead Developer Setup):**

* **General Editor:** Neovim (LazyVim distribution)
* **Smart Contracts:** Neovim or VS Code
* **Unity Scripts:** JetBrains Rider
* **Backend:** Neovim and Rider

!!! danger "Visual Studio Support"
    If you choose to use **Visual Studio**, setup and usage are **unsupported** in this guide. This is due to specific project configurations that favor Rider or Neovim/LSP workflows.

---

## Setup Instructions

!!! info "Terminal Requirement"
    Run all commands using **PowerShell** unless stated otherwise. You **must** restart PowerShell after any step involving an installation to refresh your environment variables (`$PATH`).

### Git

Documentation: [https://git-scm.com/docs/git](https://git-scm.com/docs/git)

#### 1. Install & Setup Identity

```powershell
winget install Git.Git
git config --global user.name "your_name"
git config --global user.email "your_email@example.com"
git config --global init.defaultBranch main

```

#### 2. Set Default Editor

The editor must be lightweight and invokable from the terminal. **Avoid heavy IDEs** like Visual Studio or Rider for Git operations.

```powershell
# Example for VS Code
git config --global core.editor "code --wait"

```

#### 3. Advanced Configuration

Ensures linear history and auto-stashes local changes during rebase.

```powershell
git config --global pull.rebase true
git config --global rebase.autoStash true

```

#### 4. Enable SSH Commit Signing

!!! note "Why sign commits"
    Signing keys verify commit authorship. Without signing, anyone can spoof your identity by simply matching your email address in their local config.

```powershell
# Use SSH for signing
git config --global gpg.format ssh

# Sign commits by default
git config --global commit.gpgsign true

```

#### 5. Verify

```powershell
git config --list --show-origin
```

!!! note
    This should show the configuration you just made.

---

### Git Large File Storage (LFS)

Git LFS is required for handling large binary assets such as Unity files, textures, audio, and other non-text resources. This prevents repository bloat and keeps clone/pull operations fast.

#### 1. Initialize

```powershell
git lfs install
```

#### 2. Verify

```powershell
git lfs --version
```

!!! note
    This should show the version.

---

### GitHub & GitLab CLI

#### 1. Installation

```powershell
winget install GitHub.cli
winget install GLab.GLab

```

#### 2. Authentication

!!! warning "GitLab Access"
    Before authenticating GitLab, ensure you have created an account and contacted the **Lead Developer** to be added to the project organization.

```powershell
gh auth login
glab auth login

```

---

### SSH Key Setup

#### 1. Generate Key

```powershell
ssh-keygen -t ed25519 -C "username@device"

```

???+ abstract "SSH File Locations"
    By default, keys are generated in `~/.ssh/` (On Windows, this is `C:\Users\YourName\.ssh\`).

* **Private key:** `id_ed25519` (NEVER SHARE THIS)
* **Public key:** `id_ed25519.pub` (This is the one you upload)

#### 2. Register Keys

```powershell
# GitHub Signing Key
gh ssh-key add ~/.ssh/id_ed25519.pub --type signing --title "username@device"

# GitHub Auth Key
gh ssh-key add ~/.ssh/id_ed25519.pub --title "username@device"

# GitLab (Uses one key for both)
glab ssh-key add ~/.ssh/id_ed25519.pub --title "username@device"

# Point Git to your public key (Adjust path if not using default)
git config --global user.signingkey ~/.ssh/id_ed25519.pub

```

#### 3. Configure SSH Config

You must manually create or edit the file at `~/.ssh/config` to ensure SSH knows which key to use for which service. Copy the following to `~/.ssh/config`:

```ssh
Host github.com
    User git
    IdentityFile ~/.ssh/id_ed25519

Host gitlab.com
    User git
    IdentityFile ~/.ssh/id_ed25519

```

#### 4. Verify

```powershell
ssh -T git@gitlab.com
```

```powershell
ssh -T git@github.com
```

!!! success
    If successful, the command will return a welcome or successful message.

---

### Clone the Repo

#### 1. Cloning

```powershell
git clone https://github.com/Markism-JA/Echoes-of-Bathala
```

!!! warning
    Without setting up Git LFS and GitLab. Cloning the repo would pull just from the GitHub repo and not include the binary Files.

#### 2. Verify binary files

```powershell
git lfs ls-files
```

!!! note
    This should list all the binary file tracked by Git LFS
---

### Docker

!!! warning "BIOS Requirement"
    Ensure **Virtualization (VT-x / AMD-V)** is enabled in your BIOS settings before installation. Docker Desktop will fail to start without it.

```powershell
winget install Docker.DockerDesktop

```

---

### Volta (Node.js Management)

Volta pins Node.js versions to the project, preventing "it works on my machine" issues.

#### 1. Install & Pin

```powershell
winget install Volta.Volta
# RESTART TERMINAL HERE
volta install node

```

#### 2. Verify Toolchain

Inside the `blockchain` directory of the repo, run:

```powershell
volta list

```

!!! success "Project Pinned"
    You should see the **project-pinned** Node.js and npm versions marked as active. Volta automatically switches versions when you `cd` into the project folder.

---

### .NET 8 SDK

Required for the backend services. We strictly use **.NET 8** for this project.

#### 1. Install SDK

```powershell
winget install Microsoft.DotNet.SDK.8

```

#### 2. Verify Installation

Navigate to the backend directory and restore dependencies to ensure the SDK is correctly recognized.

```powershell
# Navigate to backend
cd backend/

# Restore dependencies
dotnet restore

```

!!! success "Restore Completed"
    If successful, the command will output "Restore completed" or "All projects are up-to-date."

#### 3. .NET Tools

All dotnet tools are localized in the root of the repo in `.config/dotnet-tools.json`. To restore in local dev environment run:

```powershell
dotnet tool restore
```

!!! success "Restore Completed"
    If successful, the command will list all tools available and output "Restore was successful."

---

### Unity

The game client and simulation engine run on **Unity 6**.

#### 1. Install Unity Hub

```powershell
winget install Unity.UnityHub

```

#### 2. Install Unity 6 LTS

1. Launch **Unity Hub**.
2. Go to the **Installs** tab.
3. Click **Install Editor**.
4. Select **Unity 6 (LTS)** from the "Official Releases" list.
5. Click **Install**.

!!! tip "Modules"
    When prompted for modules, ensure **Windows Build Support** (IL2CPP) is selected.

#### 3. Restore package for unity

!!! info
    Unity does not handle Nuget packages on its own so we are using [NugetForUnity](https://github.com/GlitchEnzo/NuGetForUnity) as a tool for managing dependencies we need. `NugetForUnity` is available both as a CLI tool `dotnet nugetforunity` and a plugin in Unity Editor.

Before opening the project in Unity Editor run at project root:

```powershell
dotnet nugetforunity restore /unity/Echoes-of-Bathala
```

Were going to use NugetForUnity mainly in Service 1 (Game Server).
