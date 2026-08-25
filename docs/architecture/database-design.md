# Database Design

## Schema Overview

The database consists of 13 core tables, plus a small number of reporting views.

| Table | Purpose |
|---|---|
| `People` | Every individual known to the system (applicants, drivers, users) |
| `Countries` | Nationality reference data |
| `Users` | System login accounts, linked 1:1 to a `Person` |
| `ApplicationTypes` | The 7 service types and their base fees |
| `Applications` | Every service request submitted by a person |
| `LicenseClasses` | The 7 license classes (age, fee, validity rules) |
| `LocalDrivingLicenseApplications` | Local license applications, linked to `Applications` |
| `TestTypes` | The 3 test types (Vision, Written, Road) and their fees |
| `TestAppointments` | Scheduled test appointments |
| `Tests` | Recorded test results, linked to an appointment |
| `Drivers` | Created once per person on their first issued license |
| `Licenses` | Local driving licenses (new/renewed/replaced), linked to a `Driver` |
| `InternationalLicenses` | International licenses, linked to a `Driver` and `Application` |
| `DetainedLicenses` | Detention records linked to a `Licenses` row |

Two reporting views (`Drivers_View`, `LocalDrivingLicenseApplications_View`) pre-join commonly
displayed fields (e.g., driver full name) for list screens.

### Core relationships

```
Countries 1───* People
People    1───1 Users
People    1───* Applications
People    1───1 Drivers            (created on first license only)
Applications 1───1 LocalDrivingLicenseApplications  (for local license requests)
LocalDrivingLicenseApplications 1───* TestAppointments
TestAppointments 1───1 Tests
Drivers   1───* Licenses
Drivers   1───* InternationalLicenses
Licenses  1───0..1 DetainedLicenses
LicenseClasses 1───* LocalDrivingLicenseApplications
```

**TODO:** generate a full ER diagram from the live schema (SQL Server Management Studio's
Database Diagram tool, or dbdiagram.io) and add it here as `database-erd.png`, once the local
dev database is set up. This has not been done yet.

---

## Audit of the Original Implementation

This section documents specific issues found in the original `DVLD_DataAccess` source, prior
to any changes. Each finding references the actual code it comes from.

### 1. Passwords are stored and compared in plaintext

```sql
SELECT * FROM Users WHERE Username = @Username and Password = @Password;
```
*(`clsUserData.GetUserInfoByUsernameAndPassword`)*

Login credentials are compared directly against the database with no hashing at all. This is
the single highest-priority item for the security-hardening phase (BCrypt).

### 2. No connection reuse discipline, a new `SqlConnection` per call

Every Data Access method opens its own `SqlConnection` in a local variable and closes it in a
`finally` block. There is no `using` statement, so a connection can leak if an exception occurs
before `finally` is reached in edge cases, and there's no shared/pooled access pattern beyond
what ADO.NET's connection pool does implicitly.

### 3. Exceptions are silently swallowed

```csharp
catch (Exception ex)
{
    //Console.WriteLine("Error: " + ex.Message);
    isFound = false;
}
```
This pattern repeats across essentially every Data Access method. Errors are never logged;
callers only see a generic `false`/`null`/`-1` return value with no way to distinguish "not
found" from "the database call failed." This will be replaced with structured logging
(Serilog) and meaningful exception propagation.

### 4. `AddWithValue` used throughout

Every parameterized query uses `command.Parameters.AddWithValue(...)`, which is a known SQL
Server anti-pattern: it infers parameter types from the .NET value at runtime, which can cause
execution plan cache bloat and implicit conversions. This will be replaced with explicitly
typed parameters when the data access layer is rewritten with Dapper.

### 5. `SELECT *` and full-table reads with no pagination

```csharp
string query = "SELECT * FROM Drivers_View order by FullName";
```
List screens (`GetAllPeople`, `Drivers_View`, etc.) load the entire table into memory with no
`OFFSET`/`FETCH`, and select all columns rather than only the ones the UI displays. This is
functionally fine at the small data volumes used for teaching, but will not scale, addressed in
the performance-optimization phase with server-side pagination and column-scoped queries.

### 6. No indexes beyond primary keys

The schema, as delivered, defines no explicit non-clustered indexes on frequently filtered
columns (`NationalNo`, `Username`, foreign keys used in JOINs). This will be measured and
addressed with a documented before/after benchmark in the performance phase.

### 7. Fully synchronous data access

All data access is synchronous ADO.NET (`ExecuteReader`, `ExecuteNonQuery`, `ExecuteScalar`).
On a single-user teaching setup this is invisible; under concurrent load it blocks the calling
thread for the duration of each round-trip. Addressed via `async`/`await` in the DAL rewrite.

---

## What Changes and Why

Each of the seven findings above will be resolved in a specific phase, and the decision behind
each fix, not just the resulting code, will be recorded as an [ADR](../decisions/). This audit
section is the traceable "before" state referenced by those ADRs and by the performance
benchmark reports.
