# Design: Care Clinic Manager

## Overview

Care Clinic Manager is a modern Healthcare Management System (HMS) built as an ASP.NET Core MVC web application with C#, Entity Framework Core and SQLite. It serves four roles — Admin, Doctor, Receptionist and Patient — each with its own dashboard and a set of screens for the clinic's day-to-day work: patient registration, appointment booking and tracking, consultations and medical records, prescriptions, laboratory tests and reports, pharmacy inventory, billing and payments, notifications and system settings.

The app runs entirely locally in Visual Studio. It uses a single SQLite database file, seeded with sample data and one login per role, so a fresh checkout starts with a working, populated clinic. There is no cloud hosting, no external server and no deployment step.

## Architecture

The app follows Clean MVC architecture with Dependency Injection, ViewModels, Services and the Repository pattern where it adds value.

- **Controllers** — thin, one per module, handle HTTP, authorization and validation, and delegate to services.
- **Services** — hold the business rules (appointment status transitions, the pharmacy inventory decrease on prescription, invoice totals and payment status, role-based access checks).
- **Repositories** — wrap `DbContext` access for the records that need reusable queries (patients, appointments, medicines).
- **ViewModels** — shape data for each view and carry form input with validation attributes.
- **Views** — Razor views with a shared layout, partial views and tag helpers, styled with Bootstrap 5 and a project stylesheet.
- **Data** — Entity Framework Core with a SQLite provider, migrations, and a seed routine that runs on an empty database.

### Project layout

```
CareClinicManager.sln
src/CareClinicManager/
  Program.cs
  appsettings.json
  appsettings.Development.json
  Data/
    AppDbContext.cs
    DbSeeder.cs
    Migrations/
  Models/            (entities)
  ViewModels/
  Services/
  Repositories/
  Controllers/
  Views/
    Shared/
    Account/
    Admin/
    Doctor/
    Receptionist/
    Patient/
    Home/
  wwwroot/
    css/site.css
    js/site.js
    lib/
  e2e/.gitkeep
tests/CareClinicManager.Tests/
```

## Components and Interfaces

### Authentication and authorization

- ASP.NET Core Identity is not used; a lightweight application user model is used instead, because the four roles are fixed and the app runs locally.
- Passwords are stored only as a hash (PBKDF2 via `Microsoft.AspNetCore.Identity.PasswordHasher<T>`), never as plain text.
- Sign-in is cookie-based. The `[Authorize]` attribute and role checks (`[Authorize(Roles = "Admin")]`, etc.) gate every controller.
- The four roles are `Admin`, `Doctor`, `Receptionist` and `Patient`.

### Data models

Entities and their relationships (foreign keys):

- **User** — `Id`, `UserName`, `PasswordHash`, `Role`, `PatientId?` (links a Patient login to its Patient record).
- **Patient** — `Id`, `Name`, `DateOfBirth`, `Gender`, `Phone`, `Email`, `Address`.
- **Doctor** — `Id`, `Name`, `Role`, `DepartmentId`, `SpecializationId`, `Phone`, `Email`.
- **Staff** — `Id`, `Name`, `Role`, `DepartmentId`, `Phone`, `Email`.
- **Department** — `Id`, `Name`.
- **Specialization** — `Id`, `Name`.
- **Appointment** — `Id`, `PatientId`, `DoctorId`, `DepartmentId`, `Date`, `Time`, `Status` (`Scheduled`, `Checked in`, `In progress`, `Completed`, `Cancelled`).
- **MedicalRecord** — `Id`, `PatientId` (one per patient).
- **Consultation** — `Id`, `MedicalRecordId`, `AppointmentId`, `DoctorId`, `Symptoms`, `Diagnosis`, `ClinicalNotes`, `Date`.
- **Prescription** — `Id`, `MedicalRecordId`, `ConsultationId`, `MedicineId`, `Dosage`, `Quantity`, `Issued` (bool).
- **LaboratoryTest** — `Id`, `MedicalRecordId`, `PatientId`, `DoctorId`, `TestType`, `Status` (`Requested`, `Completed`).
- **LaboratoryReport** — `Id`, `LaboratoryTestId`, `ResultData`, `ResultFilePath`.
- **Medicine** — `Id`, `Name`, `StockQuantity`.
- **PharmacyInventory** — `Id`, `MedicineId`, `StockQuantity` (the single stock quantity per medicine).
- **Invoice** — `Id`, `PatientId`, `Items` (JSON or child rows), `TotalAmount`, `Status` (`Unpaid`, `Paid`).
- **Payment** — `Id`, `InvoiceId`, `Amount`, `Date`.
- **Notification** — `Id`, `PatientId`, `Type`, `Message`, `SentAt`.
- **SystemSetting** — `Id`, `Key`, `Value`.

### Business rules (in services)

- **Appointment status transitions** — check-in sets `Checked in`; a Doctor starting a consultation sets `In progress`; ending a consultation sets `Completed`; cancelling sets `Cancelled`.
- **Pharmacy inventory decrease** — issuing a prescription decreases the medicine's stock by the prescribed quantity; cancelling a prescription before it is issued leaves stock unchanged.
- **Invoice totals and payment** — an invoice is saved as `Unpaid` with a computed total; recording a payment sets it to `Paid`.
- **Role-based access** — a Receptionist never sees clinical notes; a Patient never sees another patient's records.

### API endpoints

The app is server-rendered MVC; there is no JSON API. The "interfaces" between screens are the routes below, which the controllers expose. Each screen's task builds the controller actions and views for its routes.

| Method | Path | Purpose |
| --- | --- | --- |
| GET | `/` | Sign-in page (redirects to the role dashboard when already signed in) |
| POST | `/Account/Login` | Sign in |
| POST | `/Account/Logout` | Sign out |
| GET | `/Account/Register` | Patient self-registration page |
| POST | `/Account/Register` | Create a Patient account |
| GET | `/Admin/Dashboard` | Admin dashboard |
| GET/POST | `/Admin/Staff` | Staff and doctor list and CRUD |
| GET/POST | `/Admin/Departments` | Department CRUD |
| GET/POST | `/Admin/Specializations` | Specialization CRUD |
| GET/POST | `/Admin/Medicines` | Medicine and pharmacy inventory CRUD |
| GET/POST | `/Admin/Settings` | System settings |
| GET | `/Admin/Notifications` | Notifications list |
| GET | `/Admin/Reports` | Financial reports |
| GET | `/Doctor/Dashboard` | Doctor dashboard |
| GET/POST | `/Doctor/Consultations` | Consultation CRUD |
| GET/POST | `/Doctor/Prescriptions` | Prescription CRUD |
| GET/POST | `/Doctor/LabTests` | Lab test request and report |
| GET | `/Doctor/Patients/{id}` | Patient record and history |
| GET | `/Receptionist/Dashboard` | Receptionist dashboard |
| GET/POST | `/Receptionist/Patients` | Patient registration and management |
| GET/POST | `/Receptionist/Appointments` | Appointment booking, calendar and status |
| GET/POST | `/Receptionist/Invoices` | Invoice and payment management |
| GET | `/Patient/Dashboard` | Patient dashboard |
| GET/POST | `/Patient/Appointments` | Patient self-booking |
| GET | `/Patient/MedicalHistory` | Patient medical history |
| GET | `/Patient/Invoices` | Patient invoices |

### Shared state and events

- The signed-in user's role and id are read from the authentication cookie by every controller; screens do not pass them around.
- A consultation completion sets the linked appointment to `Completed`; a prescription issue decreases inventory; a payment sets the invoice to `Paid`. These are service-side effects, not events between screens.

## Data Models (details)

The entities above map to SQLite tables via EF Core migrations. Foreign keys enforce the relationships: a Patient has one MedicalRecord; an Appointment belongs to a Patient, a Doctor and a Department; a Consultation belongs to a MedicalRecord and an Appointment; a Prescription belongs to a MedicalRecord and a Medicine; a LaboratoryReport belongs to a LaboratoryTest; a Payment belongs to an Invoice.

## Error Handling

- A global exception handler logs the error and shows a friendly error page.
- Server-side validation (data annotations on ViewModels) and client-side validation (jQuery unobtrusive validation) both run on every form.
- Unauthorized access redirects to the sign-in page or the role's dashboard, per the role rules.

## Testing Strategy

- **Unit tests** (xUnit) cover the services: appointment status transitions, the pharmacy inventory decrease and the cancel-before-issue rule, invoice totals and payment status, and password hashing.
- **Integration tests** cover the controllers and the `DbContext` against a throwaway SQLite file in a temporary folder, so no running database or fixed port is needed.
- **End-to-end journeys** are saved by Greenfield's Tester in `e2e/` with Playwright and run through `run_journeys`; the unit test runner excludes that folder.

## Build and run

```
## Build and run
Install: dotnet restore
Test: dotnet test
Run locally: yes
Dev: dotnet run --project src/CareClinicManager
Port variable: ASPNETCORE_URLS
Health path: /health
Devices: both
Deploy: none
```

The app reads its port from `ASPNETCORE_URLS` (for example `http://127.0.0.1:5000`), which Greenfield sets to a free port each time it starts the app. The server listens on `127.0.0.1`. `Dev` runs the migrations and seeds an empty database before serving, and leaves a seeded database as it is.

## Test logins

The seed script creates one login per role:

| Role | User name | Password |
| --- | --- | --- |
| Admin | `admin` | `Admin@123` |
| Doctor | `doctor` | `Doctor@123` |
| Receptionist | `receptionist` | `Receptionist@123` |
| Patient | `patient` | `Patient@123` |

## Look and feel

A premium, modern healthcare SaaS interface, not a default Bootstrap template.

- **Colours** — background `#f4f6f9` (soft grey), surface `#ffffff`, text `#1f2a44` (navy), primary `#0f4c81` (navy blue), accent `#14b8a6` (teal), error `#dc3545`, success `#198754`.
- **Font** — system UI stack: `-apple-system, "Segoe UI", Roboto, "Helvetica Neue", Arial, sans-serif`.
- **Spacing** — a 4px base scale (8, 12, 16, 24, 32px); cards use 24px padding.
- **Corner radius** — 10px for cards and buttons, 6px for inputs.
- **Buttons** — primary buttons are navy blue with white text; hover darkens them, focus shows a teal outline, pressed darkens further, disabled is greyed and non-interactive.
- **Fields** — white inputs with a 1px grey border, a teal border on focus, and red borders with a message on validation errors.
- **Lists and tables** — cards with a header, a search box, sortable columns, filter controls and pagination; status badges are coloured pills (`Scheduled` grey, `Checked in` teal, `In progress` blue, `Completed` green, `Cancelled` red).
- **Navigation** — a collapsible sidebar with icons on the left, and a responsive top bar with a profile dropdown and a notifications bell.
- **Dashboards** — KPI cards in a row, then charts and a data table.

### Screen layouts

- **Sign-in / Register** — a centred card on a navy gradient background, with the clinic name, the form, and a link between sign-in and registration.
- **Dashboards** — on desktop, a sidebar on the left and a content area of KPI cards above charts and tables; on a phone, the sidebar collapses into a hamburger menu and the cards stack vertically.
- **List screens** (patients, staff, departments, specializations, medicines, invoices, notifications) — a card with a toolbar (search, filters, "Add" button) above a paginated table; on a phone the table scrolls horizontally and the toolbar wraps.
- **Form screens** (register patient, add doctor/staff, book appointment, new consultation, new prescription, request lab test, new invoice, settings) — a card with labelled fields, validation messages, and a primary submit button; on a phone the fields stack full-width.
- **Appointment calendar** — a month grid with status badges on each appointment; on a phone it becomes a day list.
- **Patient record** — a profile header with tabs for history, consultations, prescriptions, lab reports and invoices.

## Where the parts meet

Each screen's task builds its own controller actions and views, and its own stylesheet (a CSS file under `wwwroot/css/` or a scoped style block) in the shared style above. Task 1 writes the shared layout, the shared `site.css` with the colour and font CSS variables, the `DbContext`, the entities, the migrations, the seed script and the test logins. Screens add their routes to their own controllers, so no two screen tasks change the same routes file.

## Assumptions

- The app uses a lightweight application user model (not ASP.NET Core Identity), because the four roles are fixed and the app runs locally.
- A Patient login is linked to a Patient record via `PatientId`, so a Patient sees only their own records.
- The pharmacy inventory is a single stock quantity per medicine, decreased by prescriptions and edited by the Admin.
- Appointment reminders are sent by email or SMS with a default lead time of 24 hours; the exact channel is a system setting.
- A consultation is linked to an appointment, and completing a consultation marks the appointment `Completed`.
- "Staff" covers non-doctor employees such as receptionists and nurses, managed by the Admin.
