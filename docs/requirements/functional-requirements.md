# Functional Requirements

These requirements describe the system this project originated from: an educational course
project. They are restated here in the project's own words for reference, and form the baseline
this rebuild works from.

## 1. Overview

The system manages the core services a driving license authority provides to applicants:
issuing, renewing, and replacing driving licenses, running the required tests, and detaining or
releasing licenses. Every service is requested through an **Application**, which is always
linked to a **Person** already registered in the system.

## 2. Core Services

| # | Service | Application Fee |
|---|---|---|
| 1 | Issue a new license (first time) | $5 |
| 2 | Retake a failed test | $5 |
| 3 | Renew a driving license | $5 |
| 4 | Replace a lost license | $5 |
| 5 | Replace a damaged license | $5 |
| 6 | Release a detained license | $5 |
| 7 | Issue an international license | $5 |

Each application additionally carries a service-specific fee (see §4).

## 3. Application Rules

- Every application has: an application number, date, applicant (Person), type, status
  (**New**, **Cancelled**, **Completed**), and fees paid.
- If the application is for a **new license**, the system must:
  - Record the requested license class.
  - Verify the applicant does **not already hold** a license of that same class.
- The system must verify the applicant has **no existing incomplete application** of the same
  type before accepting a new one.
- A person can have multiple applications over time, but each application belongs to exactly
  one person.

## 4. License Classes

| Class | Description | Min Age | Fee | Validity |
|---|---|---|---|---|
| 1 | Small motorcycles | 18 | $15 | 5 years |
| 2 | Heavy motorcycles | 21 | $30 | 5 years |
| 3 | Regular vehicles (cars) | 18 | $20 | 10 years |
| 4 | Commercial (taxi/limousine) | 21 | $200 | 10 years |
| 5 | Agricultural vehicles | 21 | $50 | 10 years |
| 6 | Small/medium buses | 21 | $250 | 10 years |
| 7 | Trucks & heavy vehicles | 21 | $300 | 10 years |

License class attributes stored in the system: `LicenseClassID`, `ClassName`,
`ClassDescription`, `MinimumAllowedAge`, `ValidityLength`, `ClassFees`.

### Eligibility rules per class

- Applicant age must match or exceed the minimum age for the requested class; otherwise the
  system must reject the request.
- Applicant must not already hold an active license of the same class.
- An applicant **may** hold licenses across multiple classes simultaneously (e.g., a car license
  and a motorcycle license).
- Valid personal identification (passport or national ID) is required.
- A training completion certificate is required before the applicant is eligible to sit the
  tests.

## 5. Testing Requirements

Applicants must pass three tests **in sequence** before a license can be issued:

1. **Vision Test**, $10. Medical check of visual acuity. Failure blocks progress until vision
   is corrected (glasses/surgery) and the test is retaken.
2. **Written Test**, $20. Paper-based test on traffic law and road safety, graded out of 100
   and recorded pass/fail in the system.
3. **Practical (Road) Test**, fee varies by license class. Evaluates real driving ability and
   rule compliance.

Rules:
- Each test attempt requires payment before scheduling.
- A failed test may be retaken by scheduling a new appointment and paying again.
- Only one appointment may be open per test type at a time.
- Test fees are determined by the requested license class.
- All three tests must be passed before a license can be issued.

## 6. License Issuance

Once all required tests are passed, the system issues a license containing:

- License number, holder's photo, national number, name, date of birth
- License class, issue date, expiry date (per class validity length)
- Notes/conditions (free text)
- Issuance status: **New**, **Replacement (Lost)**, **Replacement (Damaged)**, **Renewed**

The system supports look-up of a person's licenses by national number or license number.

The first time any license is issued to a person, that person becomes an official **Driver** in
the system and receives a driver record (created once, reused for all future licenses).

## 7. Service-Specific Rules

### 7.1 Retest
- New appointment for the same test type, referencing the original failed test.
- Applicant must have previously failed the test type to be eligible.
- Fee: $5 + the relevant test fee.
- Creates a new application linked to the original.
- Only one open appointment per test type is allowed at a time.

### 7.2 License Renewal
- Fee: $10 at submission.
- Vision test must be passed again (with its fee) before renewal is granted.
- The applicant must surrender the expired license before a renewed one is issued.
- **A license can only be renewed once expired**, an active license cannot be renewed early.

### 7.3 Replace Lost License
- System must verify the license is not currently detained before allowing this.
- Fee: $20.
- Applicant surrenders the lost license record (deactivated, not deleted) before a new license
  ID is issued.

### 7.4 Replace Damaged License
- Applicant surrenders the damaged license before a replacement is issued.
- Fee: $20.
- The system must retain the record of when the damaged replacement was issued.

### 7.5 Release Detained License
- Requires payment of the fine associated with the detention before release.
- Records the release date.

### 7.6 International License
- Available only to holders of a Class 3 (regular car) license, and only if that license is
  neither expired nor detained.
- Fee: $20.
- Duration is a configurable system setting.
- Issuing a new international license while one is already active automatically deactivates the
  old one, all previously issued international licenses must be retained (not deleted).

### 7.7 Detain License
- Records: license ID, fine amount, detention reason, detention date.

## 8. Person Management

Every applicant must be uniquely registered as a **Person** before submitting any application.
Person data (national number, full name, date of birth, address, phone, email, nationality,
photo) is stored once and never duplicated.

Operations: search by national number, view, add, edit, delete. **A national number must be
unique across the system.**

## 9. System/User Administration

- User accounts are linked to a Person record.
- Operations: add user, view, edit, delete, freeze/unfreeze account, assign permissions.
- User record includes: national number, name, DOB, address, phone, email, nationality, photo,
  username, password.

## 10. Reference Data Management

- **Application Types**: fee-only editing.
- **Test Types**: fee-only editing.
- **License Classes**: age, validity length, and fee are editable.

## 11. Cross-Cutting Rule

**Every state-changing action in the system must record which user performed it and when.**
This applies to every add/edit/delete/status-change operation across all modules, this is a
system-wide audit requirement, not specific to any one service.
