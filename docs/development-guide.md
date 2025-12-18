# Development Guide

## Setup

### Dependency

- SSH: For authentication and deployment
- GIT: For version control
- GitHub CLI: Project and developer suite management
- Dotnet: Backend
- Unity: Game Client and Simulation (Service 1)
- Docker: Containerization for deployment and testing
- Hardhat: Smart Contract writing and simulating Testnet
- Volta: Manages NodeJS package.

#### Editor

You can use what suits you however it must meet the following:

- General editor for writing documentation and managing codebase.
- For developing the smart contract it must have an LSP (Plugin) for Solidity and Typescript.
- Good integration with Unity for writing script.
- For Backend, you need an editor with great support for C# language features.

```markdown
Ex. Lead Dev's
- General Editor: Neovim (Lazyvim distribution) with customized configurations.
- Smart Contract: Neovim or Vscode
- Unity Scripts: Rider
- Backend: Nvim and Rider
```

> [!NOTE]
> If you choose to go with Visual Studio. I have no idea how that works, so you're on your own.

#### Instructions

##### GitHub CLI

```pwsh
winget install GIT
```
