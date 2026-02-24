# Testing & Continuous Integration

!!! tip "Testing Overview"
    This guide covers the testing philosophy for Echoes of Bathala, local testing tools, and the automated CI pipeline that protects the `master` branch.

## Test-Driven Development (TDD)

We employ a Test-Driven Development (TDD) approach for the backend. This ensures our game logic, APIs, and infrastructure are robust, reliable, and well-documented through code.

### What TDD Implies in Our Workflow

1. **Red:** Write a failing test for a new feature or bug fix *before* writing the actual implementation.
2. **Green:** Write the minimum amount of code necessary to make the test pass.
3. **Refactor:** Clean up the code, optimize, and ensure all formatting aligns with project standards, keeping the test green.

*Always write your tests alongside or ahead of your features to maintain high coverage and confidence when merging.*

---

### Local Testing: The Test Runner Script

!!! info "Common Baseline Utility"
    We provide a cross-platform PowerShell script (`Test-Backend.ps1`) to standardize testing and coverage reporting across the team.

The script makes it easy to fuzzy-find projects, filter specific tests, and instantly generate an HTML coverage report.

**Run a Specific Project**

```bash
# Fuzzy matches projects like "GameBackend.API.Tests"
./Test-Backend.ps1 -Project "api"

```

**Run a Specific Test or Class**

```bash
./Test-Backend.ps1 -Project "api" -Filter "TradeTest"

```

**Generate a Coverage Report**

```bash
# Runs tests and opens an HTML coverage report in your browser
./Test-Backend.ps1 -Project "api" -Report

```

---

### Alternative Testing Methods

!!! note "Use What Works For You"
    The `Test-Backend.ps1` script is entirely optional. It exists to provide a common baseline, but you are free to use your preferred tools.

#### Direct .NET CLI

You can bypass the script entirely and use standard .NET CLI commands:

```bash
# Run all tests in the solution
dotnet test backend/GameBackend.sln

# Run a specific test project
dotnet test backend/tests/GameBackend.API.Tests

```

#### IDE Integrations

Modern IDEs offer excellent built-in visual test runners that integrate perfectly with our setup:

* **Visual Studio / Rider:** Use the built-in "Test Explorer" to run, debug, and view coverage for individual tests directly from the gutter next to your code.
* **VS Code:** Install the `.NET Core Test Explorer` extension to run and debug tests directly from the sidebar.

---

### Backend CI Pipeline (GitHub Actions)

!!! warning "Pull Request Requirements"
    All code must pass the **Backend CI** workflow before it can be merged into `master`.

When you open a Pull Request modifying any files in the `backend/` directory, GitHub Actions automatically spins up an environment to verify your code.

#### CI Workflow Steps

1. **Environment Setup:** Checks out the code and sets up `.NET 8.0.x` with caching for faster subsequent runs.
2. **Restore:** Restores NuGet dependencies and local tools (like `CSharpier` and `reportgenerator`).
3. **Quality & Formatting Gates:**

* **CSharpier:** Verifies code layout. *(Run `dotnet csharpier backend/` locally to fix).*
* **Code Style:** Verifies `.editorconfig` style rules. *(Run `dotnet format style backend/GameBackend.sln` locally to fix).*
* **Analyzers:** Checks for compiler and structural warnings. *(Run `dotnet format analyzers backend/GameBackend.sln` locally to fix).*

1. **Build:** Compiles the solution in `Release` configuration.
2. **Test & Coverage:** Runs all tests across the backend and collects Cobertura XML coverage data.
3. **Coverage Reporting:** Generates a markdown summary of your code coverage. If coverage drops below the acceptable thresholds (70% minimum, 80% target), the pipeline will fail.
4. **PR Comment:** Automatically posts or updates a sticky comment on your Pull Request showing the coverage badge and statistics.

#### When to Check the CI

* Before requesting a review, ensure the CI pipeline is green.
* If the pipeline fails on formatting, apply the suggested `dotnet format` or `csharpier` commands locally and push the fixes.
* If the pipeline fails on coverage, you need to write more tests for your new features before merging.
