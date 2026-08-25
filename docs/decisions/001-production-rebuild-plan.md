# ADR 001: Rebuilding a Course Project into a Production-Grade System

## Status
Accepted

## Context
This project began as a guided course implementation (WinForms/.NET Framework 4.8/C#/SQL
Server), built for teaching layered architecture. An audit of the original source (see
[database-design.md](../architecture/database-design.md#audit-of-the-original-implementation))
found several gaps typical of teaching-scale code: plaintext password comparison, no
connection-handling discipline, silently swallowed exceptions, no indexing strategy, no
pagination, and fully synchronous data access.

The goal is to use this codebase as a foundation for a portfolio project that demonstrates
legacy modernization skills, not to pretend it was production-grade from the start.

## Decision
As of this writing, implementation has not started, this ADR records the plan before any code
is written. The rebuild will proceed in two stages:

1. **Complete the original functional scope** (weeks 1–12) following the course structure,
   while applying safe, low-risk improvements incrementally as they're encountered (e.g.,
   BCrypt hashing as soon as the login/user module is reached, rather than deferring it) so
   fixes don't compound into a larger rewrite later.
2. **Structural hardening** (weeks 13–16): architectural refactor (Repository + Unit of Work +
   Dapper), full security hardening (RBAC, audit trail), measured performance optimization
   (indexing, pagination, caching, async I/O, load testing), and automated testing + CI/CD.

Every non-trivial decision made in stage 2 will be recorded as its own ADR, referencing the
specific audit finding it addresses.

## Consequences
- **Positive:** Improvements are traceable to a documented "before" state, so claims made about
  this project (e.g., in a CV) are defensible with evidence, not estimates.
- **Positive:** Spreading some fixes (e.g., password hashing) earlier avoids a large one-time
  rewrite and reduces the risk of regressions late in the project.
- **Trade-off:** The functional/course-following phase intentionally does not fix every issue
  immediately (e.g., the DAL stays on raw ADO.NET until the stage-2 refactor), this is a
  deliberate scope decision to avoid redesigning the data access layer twice.
