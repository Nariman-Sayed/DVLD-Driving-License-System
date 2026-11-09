# DVLD: Driving & Vehicle License Department System

DVLD is a driving license authority system rebuilt from an educational WinForms project into a
layered .NET solution: an ASP.NET Core Web API, a WinForms desktop client, shared DTOs, and
integration tests. The API works against the existing DVLD SQL Server database.

This repository is an active rebuild. The API currently covers authentication plus read/write
access to people, application types, test types, and base applications. The desktop client
currently implements login, session expiry handling, and a read-only API smoke test.

## Architecture

```
src/
├── DVLD.Api/                  ASP.NET Core Web API (net10.0)
│   ├── Controllers/           HTTP endpoints
│   ├── Services/              Business logic
│   ├── Data/                  EF Core DbContext, entities, configurations, repositories
│   ├── Options/               Validated Jwt and RateLimiting options
│   ├── Extensions/            Service registration
│   └── Middleware/            ProblemDetails exception handler, rate-limit response
├── DVLD.Desktop/              WinForms client (net10.0-windows)
│   ├── Login/                 Login form
│   └── Services/              Singleton ApiClient, bearer handler, ApiException
├── DVLD.Shared/               DTOs shared by API and desktop (no data access)
└── DVLD.Api.IntegrationTests/ xUnit integration tests with WebApplicationFactory
db/scripts/                    Read-only database verification scripts
docs/                          Requirements, use cases, architecture notes, ADRs
```

Request flow in the API is controller, then service, then repository, then EF Core. The API uses
JWT bearer authentication, returns RFC 7807 ProblemDetails for errors, logs with Serilog, and
rate limits requests with ASP.NET Core's built-in rate limiter. The desktop client owns one
long-lived `HttpClient` behind `ApiClient.Instance`, attaches the current session token on every
request, and converts API errors into messages a user can act on.

EF Core maps to the existing DVLD schema. There are no migrations in this repository and the API
does not create or alter the database.

## Prerequisites

- .NET 10 SDK.
- Windows to run the desktop project.
- A SQL Server instance with the DVLD database restored (tables include `People`, `Users`,
  `ApplicationTypes`, `TestTypes`, and `Applications`).
- A user row in `Users` whose `Password` column contains a BCrypt hash.

## Configure secrets

The API reads its connection string and JWT signing key from user-secrets during development.
Neither value belongs in `appsettings.json`, and neither should ever be committed.

From the repository root:

```powershell
cd src/DVLD.Api
dotnet user-secrets set "ConnectionStrings:DVLDConnection" "<your DVLD connection string>"
```

Generate a random signing key and store it without printing it:

```powershell
$key = [Convert]::ToBase64String([System.Security.Cryptography.RandomNumberGenerator]::GetBytes(48))
dotnet user-secrets set "Jwt:Key" $key
```

The key must be at least 32 bytes. The API validates `Jwt:Key`, `Jwt:Issuer`, and `Jwt:Audience`
at startup and fails fast with a clear message when the key is missing or too short. Issuer and
audience already have defaults in `src/DVLD.Api/appsettings.json`.

## Run the API

From the repository root:

```powershell
dotnet run --project src/DVLD.Api --launch-profile https
```

The HTTPS profile listens on `https://localhost:7226` and `http://localhost:5250`. In Development,
the OpenAPI document and Scalar UI are available at `/openapi/v1.json` and `/scalar/v1`.

`POST /api/auth/login` accepts `{ "username": "...", "password": "..." }` and returns a JWT.
The other endpoints require `Authorization: Bearer <token>`. The login endpoint has a stricter
fixed-window rate limit than the rest of the API: five requests per 60 seconds per client by
default, configurable through the `RateLimiting` section. Exceeding it returns a 429
ProblemDetails response.

## Run the desktop client

Start the API first, then from the repository root:

```powershell
dotnet run --project src/DVLD.Desktop
```

The desktop base URL is the `ApiBaseUrl` application setting in
`src/DVLD.Desktop/Properties/Settings.settings`, defaulting to `https://localhost:7226/`. If the
API runs on a different address, change that setting and rebuild.

After a successful login the main form opens. When the API returns 401, the client clears the
session and returns to the login form with an explanatory message.

## Build and test

```powershell
dotnet build src/DVLD.slnx -c Release
dotnet test src/DVLD.slnx -c Release
```

The build is expected to produce zero warnings. The test suite currently has 27 integration tests
and does not require a database: repository fakes are injected into the test host. Tests cover
authentication outcomes (unknown user, wrong password, inactive account, malformed stored hash,
valid login), login rate limiting, and the existing controller behaviour.

CI is defined in `.github/workflows/build-and-test.yml`. It runs on `windows-latest` for every
push and every pull request, restores the solution, builds it in Release (including the desktop
project), runs the tests with coverage, and uploads the coverage artifact.

## Verify EF Core mappings

`db/scripts/verify-ef-mappings.sql` is a read-only script that lists columns, types, nullability,
identity columns, constraints, and foreign keys for the mapped tables. Run it with your normal
SQL client, for example:

```powershell
sqlcmd -S <server> -d DVLD -E -i db/scripts/verify-ef-mappings.sql
```

## Known gaps

- No role-based access control. Any valid JWT can call every authorized endpoint.
- No audit trail for authentication or data changes.
- No concurrency control. Concurrent updates follow last-write-wins.
- No SQL Server integration tests in CI. CI uses repository fakes only.
- Login rate limiting is per client IP and in-memory, so it is not shared across API instances.
- No refresh tokens or token revocation; clients re-authenticate when the JWT expires.
- The desktop client does not yet implement the licensing workflows behind the main menu.

## Project origin and documentation

The original course project taught layered architecture, delegates, events, and reusable
UserControls in WinForms. The baseline audit, requirements, and use cases are preserved so the
rebuild can be compared with the starting point.

- [Functional requirements](docs/requirements/functional-requirements.md)
- [Use cases](docs/use-cases/use-cases.md)
- [Database design and baseline audit](docs/architecture/database-design.md)
- [Architecture decision records](docs/decisions/)
