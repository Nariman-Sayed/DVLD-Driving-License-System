# Use Cases

Each use case lists the primary actor, preconditions, main flow, and key business rules
enforced.

## Actors
- **System User**, an employee of the license authority operating the system (requires login).
- **Applicant / Driver**, the person the application/license is about (not a system actor;
  acted on by the System User).

---

### UC-01: Add New Person
**Actor:** System User
**Preconditions:** National number does not already exist in the system.
**Main flow:**
1. User opens "Manage People" and selects "Add New Person."
2. User enters national number, name, DOB, address, phone, email, nationality, photo.
3. System validates uniqueness of national number.
4. System saves the person record and returns a `PersonID`.
**Business rules:** National number must be unique; email is optional; photo is optional.

### UC-02: Search / Filter People
**Actor:** System User
**Main flow:** User filters the people list by PersonID, national number, first name, or other
fields. System returns matching records.

### UC-03: Add System User
**Actor:** System User (with admin rights)
**Preconditions:** The target Person already exists.
**Main flow:** User links a new system account (username/password) to an existing Person and
sets active status.

### UC-04: Login
**Actor:** System User
**Main flow:** User enters username/password. System validates credentials, checks the account
is active, and opens the main application window on success.
**Current baseline issue:** credentials are compared as plaintext against the database (see
[audit findings](../architecture/database-design.md#audit-of-the-original-implementation)).
This will be addressed in the security-hardening phase.

### UC-05: Apply for New Local Driving License
**Actor:** System User (on behalf of an Applicant)
**Preconditions:** Applicant exists as a Person; applicant does not already hold a license of
the requested class; applicant meets the minimum age for the requested class.
**Main flow:**
1. User selects/looks up the applicant by national number.
2. User selects the requested license class.
3. System validates age eligibility and absence of a duplicate active license/open application.
4. System creates the Application record (status: New).

### UC-06: Schedule a Test
**Actor:** System User
**Preconditions:** Applicant has an open, eligible Application; the required prior test in the
sequence (Vision → Written → Road) has already been passed, where applicable.
**Main flow:** User selects the test type, sets an appointment date, and records the fee paid.
**Business rules:** Vision must be passed before Written can be scheduled; Written must be
passed before the Road test can be scheduled; only one open appointment per test type is
allowed.

### UC-07: Take / Grade a Test
**Actor:** System User
**Main flow:** User records the result (Pass/Fail) for a scheduled test appointment. On Pass,
the next test in the sequence becomes eligible for scheduling. On Fail, the applicant may
schedule a retest (see UC-08).

### UC-08: Retake a Failed Test
**Actor:** System User
**Preconditions:** Applicant has previously failed the given test type.
**Main flow:** A new test appointment is created, linked to the original failed test, with the
retest fee applied.

### UC-09: Issue Driving License (First Time)
**Actor:** System User
**Preconditions:** Applicant has passed all three required tests for the requested class.
**Main flow:**
1. User confirms all three tests are passed.
2. System issues a new License record (status: New) with a generated LicenseID.
3. If this is the applicant's first license ever, the system creates a Driver record for them
   and links it to their Person record (created once, reused for future licenses).
4. The originating Application is marked Completed.

### UC-10: Renew Driving License
**Actor:** System User
**Preconditions:** Existing license is expired (not renewable while still active); vision test
retaken and passed.
**Main flow:** User submits a renewal application, applicant retakes and passes the vision test,
old license is deactivated, and a new license record is issued (status: Renewed) preserving the
old license ID in history.

### UC-11: Replace Lost License
**Actor:** System User
**Preconditions:** License is not currently detained.
**Main flow:** User submits the replacement application, old license is deactivated (not
deleted), a new License record is issued (status: Replacement–Lost) with a new License ID.

### UC-12: Replace Damaged License
**Actor:** System User
**Main flow:** Same as UC-11 but for a damaged license (status: Replacement–Damaged); the date
of the damaged replacement is retained.

### UC-13: Detain License
**Actor:** System User
**Main flow:** User records a license as detained, entering the fine amount, the reason, and the
detention date. The license becomes ineligible for renewal/replacement/international issuance
until released.

### UC-14: Release Detained License
**Actor:** System User
**Preconditions:** The applicable fine has been paid.
**Main flow:** User releases the license, records the release date, and the license returns to
normal (active) status.

### UC-15: Issue International License
**Actor:** System User
**Preconditions:** Applicant holds an active, non-detained Class 3 (regular car) license.
**Main flow:** User submits the request; if an active international license already exists for
this applicant, it is deactivated and a new one is issued; all past international licenses are
retained in history.

### UC-16: View Person's License History
**Actor:** System User
**Main flow:** User looks up a person (by PersonID or national number) and views all local and
international licenses ever issued to them, including inactive/superseded ones.

### UC-17: Manage Reference Data (Application Types, Test Types, License Classes)
**Actor:** System User (admin)
**Main flow:** User edits fee, and, for license classes only, minimum age and validity length.
These entities are otherwise fixed/seeded.

---

## Cross-cutting requirement affecting all use cases

Every use case that creates, edits, deletes, or changes the status of a record must persist
**which user performed the action and when**. In the baseline code, this is only partially
in place: `Applications` records the creating user (`CreatedByUserID`) and a status timestamp,
but `People` records neither. Making this consistent across all entities is part of the
security-hardening phase.
