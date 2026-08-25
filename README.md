# DVLD: Driving & Vehicle License Department System

A desktop application (WinForms / .NET Framework 4.8 / C# / SQL Server) that manages the core
operations of a driving license authority: registering people, issuing and renewing driving
licenses, scheduling and grading tests, and handling detained/lost/damaged licenses.

## Project Origin & Goal

This project started as a guided educational course project, built to teach layered architecture
(Data Access / Business Logic / UI), delegates, events, and reusable UserControls in a
C#/WinForms context.

The original implementation is functionally complete for the core workflows, but, as is typical
for teaching-oriented code, validation is partial, there is no automated testing, error handling
is minimal, and no security, performance, or scalability concerns were addressed (see the
[baseline audit](docs/architecture/database-design.md#audit-of-the-original-implementation) for
specifics).

**The goal of this repository is different: to take that functional baseline and rebuild it into
a production-grade system**, applying the architecture, security, performance, and testing
practices expected of a real enterprise application, while being transparent about where the
project came from and what changed.

This is an active, ongoing effort, not a finished product. Implementation has not started yet.
The current state of this repo is documentation and planning only: a baseline audit of the
original code, functional requirements, use cases, and the first architecture decision record.
Progress will be tracked phase by phase as implementation begins (see [Roadmap](#roadmap)
below), with each phase reviewed for architecture, security, or performance concerns before
moving to the next.

## Why This Repo Exists

Most portfolio projects either stay a raw tutorial copy, or claim "production-ready" without
evidence. This repo aims to do neither:

- Every non-trivial architectural decision will be documented as an [ADR](docs/decisions/), the
  problem, the decision, and the trade-offs, not just the resulting code.
- The original codebase was audited **before** any rewrite began (see
  [Database & Code Audit](docs/architecture/database-design.md#audit-of-the-original-implementation)),
  so every future "improvement" claim can be backed by a documented *before* state.
- Performance work (planned for a later phase) will be backed by actual before/after benchmarks
  on a seeded dataset, not estimates, tracked under [docs/performance](docs/performance/) once
  that phase begins.

## Tech Stack

| Layer | Current (baseline) | Target (production) |
|---|---|---|
| UI | WinForms, .NET Framework 4.8 | WinForms, .NET Framework 4.8 |
| Data Access | Raw ADO.NET (`SqlConnection`/`SqlCommand`) | Dapper + Repository / Unit of Work |
| Database | SQL Server | SQL Server (indexed, audited schema) |
| Security | Plaintext password comparison | BCrypt hashing, RBAC, audit logging |
| Testing | None | xUnit, Moq |
| CI/CD | None | GitHub Actions |

## Project Structure

```
DVLD/
├── docs/
│   ├── requirements/          Functional requirements
│   ├── use-cases/              Use cases
│   ├── architecture/          System architecture, database design, audit findings
│   ├── decisions/             Architecture Decision Records (ADRs)
│   └── performance/           Baseline and post-optimization benchmark results
├── src/
│   ├── DVLD/                  Presentation layer (WinForms)
│   ├── DVLD_Business/         Business logic layer
│   └── DVLD_DataAccess/       Data access layer
└── tests/                     Unit and integration tests
```

## Roadmap

The rebuild is planned as a series of phases, covering (in order): core setup, people/user
management, applications and licensing workflows, testing/scheduling, an architectural refactor
(Repository/Unit of Work/Dapper), security hardening, performance optimization with measured
benchmarks, and automated testing/CI-CD.

Current phase: **Planning & baseline audit complete, implementation not yet started.**

## Documentation

- [Functional Requirements](docs/requirements/functional-requirements.md)
- [Use Cases](docs/use-cases/use-cases.md)
- [Database Design & Original Codebase Audit](docs/architecture/database-design.md)
- [Architecture Decisions](docs/decisions/)
- [Performance Benchmarks](docs/performance/)

## Mentorship & Review

Major phases of this rebuild, particularly architectural refactoring, security hardening, and
performance work, are reviewed with external senior engineers before moving forward, to
sanity-check decisions before they're implemented rather than only after.

## Status

📋 Planning stage. Requirements, use cases, and the baseline audit are complete. Implementation
has not started yet. This README and the linked docs will be updated as each phase begins and
completes.
