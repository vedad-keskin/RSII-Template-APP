# RSII Template — ManiFest

Backend solution for **ManiFest**: an ASP.NET Core **8.0** Web API with Entity Framework Core, SQL Server, Swagger/OpenAPI, HTTP Basic authentication, and an optional **RabbitMQ** worker for notifications.

## What’s in the repo

| Path | Purpose |
|------|---------|
| `ManiFest/ManiFest.sln` | Visual Studio / `dotnet` solution |
| `ManiFest/ManiFest.WebAPI` | REST API, Swagger UI, auth, startup migration check |
| `ManiFest/ManiFest.Services` | EF Core `DbContext`, services, migrations |
| `ManiFest/ManiFest.Model` | DTOs, requests, responses, search objects |
| `ManiFest/ManiFest.Subscriber` | Background worker (RabbitMQ consumer, email-related services) |
| `Documentation/` | Command cheatsheets (C#, Docker, Flutter) |
| `Utils/` | Extra command notes |

### API surface

CRUD-style controllers (plus search/paging via shared base types) include **Users**, **Role**, **Gender**, **City**, and **Category**. Swagger is enabled in the pipeline so you can explore routes interactively.

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- SQL Server (local instance, Azure SQL, or the SQL Server container from Docker Compose)
- (Optional) [Docker Desktop](https://www.docker.com/products/docker-desktop/) for the full stack

## Run locally (API only)

1. **Connection string** — Set `ConnectionStrings:DefaultConnection` in `ManiFest/ManiFest.WebAPI/appsettings.json`, or override with environment variables / [user secrets](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets) so credentials are not committed.

2. From the solution folder:

   ```bash
   cd ManiFest
   dotnet restore
   dotnet run --project ManiFest.WebAPI
   ```

3. Open **Swagger** (HTTP profile uses port **5130**):

   - `http://localhost:5130/swagger`

The API applies **pending EF Core migrations** on startup when the database is reachable.

### Authentication

The API uses **Basic** authentication. Use the **Authorize** control in Swagger with a valid `username:password` (handled by `BasicAuthenticationHandler`).

## Run with Docker Compose

Compose brings up SQL Server, RabbitMQ (management UI on **15672**), the **Web API** (**5130**), and the **notification subscriber** (**7111** mapped to container port 80).

1. Create `ManiFest/.env` (Compose reads variables from there) with at least:

   - `SQL__PASSWORD`, `SQL__PID`, `SQL__DATABASE`, `SQL__USER`
   - `RABBITMQ__HOST`, `RABBITMQ__USERNAME`, `RABBITMQ__PASSWORD`

   Match `SQL__*` values to what SQL Server expects; `RABBITMQ__HOST` should resolve to the RabbitMQ service name on the Compose network (e.g. `rabbitmq` when using the bundled service).

2. From `ManiFest`:

   ```bash
   docker compose up --build
   ```

Individual images are built from `Dockerfile` (API) and `Dockerfile.notifications` (subscriber).

## Entity Framework migrations

Migrations live under `ManiFest/ManiFest.Services/Migrations`. To add a new migration after model changes:

```bash
cd ManiFest
dotnet ef migrations add <MigrationName> --project ManiFest.Services --startup-project ManiFest.WebAPI
```

## Tech stack (summary)

- ASP.NET Core 8, minimal hosting (`Program.cs`)
- EF Core + SQL Server
- Mapster for mapping
- Swashbuckle (Swagger + XML comments)
- RabbitMQ-backed subscriber for async/notification workloads

## Security note

Do not commit real database or RabbitMQ passwords. Prefer environment variables, user secrets, or a local `.env` that stays out of version control.
