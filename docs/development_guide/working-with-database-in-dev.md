## dotnet-ef & Migrations

!!! tip "Quick Start Overview"
    Use this section to manage your database schema using **EF Core CLI**. Includes creating, updating, and applying migrations.

### Installing dotnet-ef

!!! info "Local Tool Recommended"
    To ensure all developers use the same version, install `dotnet-ef` as a **local tool** in your repo root.

```bash
# Restore tools defined in the manifest
dotnet tool restore

# Or install explicitly (global install optional)
dotnet tool install --global dotnet-ef --version 8.0.11
```

### Creating a Migration

!!! info "Database Schema Versioning"
    Migrations track changes to your EF Core models and apply them to the database.

```bash
# From the backend directory
dotnet tool run dotnet-ef migrations add <MigrationName> \
  --project src/GameBackend.Infra \
  --startup-project src/GameBackend.API
```

* Replace `<MigrationName>` with a descriptive name (e.g., `InitialCreate`, `AddAccountsTable`).
* The `--project` option points to your **DbContext project**.
* The `--startup-project` option points to the **API project**, used for configuration and DI.

### Applying Migrations to the Database

!!! tip "Update the Database"
    Apply pending migrations to bring the database schema up-to-date.

```bash
dotnet tool run dotnet-ef database update \
  --project src/GameBackend.Infra \
  --startup-project src/GameBackend.API
```

* This will execute all pending migrations in order.
* Ensure the database service is running (see Docker Quick Start).

### Viewing Migration Status

!!! info "Check Database State"
    See which migrations have been applied and which are pending.

```bash
dotnet tool run dotnet-ef migrations list \
  --project src/GameBackend.Infra \
  --startup-project src/GameBackend.API
```

---

### Removing or Reverting a Migration

!!! warning "Caution"
    Only revert migrations that have **not been applied** to a production database.

```bash
# Remove the last migration (not yet applied)
dotnet tool run dotnet-ef migrations remove \
  --project src/GameBackend.Infra \
  --startup-project src/GameBackend.API

# Revert database to a previous migration
dotnet tool run dotnet-ef database update <PreviousMigrationName>
```

!!! note "Migration applied in Prod"
    Migrations can be exported into production databases. This caution would only be necessary in the future.

---

### Other Useful Commands & References

!!! note "Beyond Quick Start"
    EF Core CLI can do more than migrations. Refer to the official docs for advanced scenarios.

```bash
# Scaffold a DbContext from an existing database
dotnet tool run dotnet-ef dbcontext scaffold "<ConnectionString>" Npgsql.EntityFrameworkCore.PostgreSQL \
  --project src/GameBackend.Infra \
  --startup-project src/GameBackend.API \
  --output-dir Models

# Generate SQL script for pending migrations
dotnet tool run dotnet-ef migrations script \
  --project src/GameBackend.Infra \
  --startup-project src/GameBackend.API
```

#### Official Documentation

* **EF Core CLI Reference**
  [https://learn.microsoft.com/ef/core/cli/dotnet](https://learn.microsoft.com/ef/core/cli/dotnet)

* **EF Core Migrations Overview**
  [https://learn.microsoft.com/ef/core/managing-schemas/migrations/](https://learn.microsoft.com/ef/core/managing-schemas/migrations/)

* **Working with DbContext**
  [https://learn.microsoft.com/ef/core/dbcontext-configuration/](https://learn.microsoft.com/ef/core/dbcontext-configuration/)
