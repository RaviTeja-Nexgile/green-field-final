# Care Clinic Manager

A modern Healthcare Management System (HMS) built as an ASP.NET Core MVC web application with C#, Entity Framework Core and SQLite. It serves four roles — Admin, Doctor, Receptionist and Patient — each with its own dashboard and screens for the clinic's day-to-day work: patient registration, appointment booking and tracking, consultations and medical records, prescriptions, laboratory tests and reports, pharmacy inventory, billing and payments, notifications and system settings.

The app runs entirely locally in Visual Studio. It uses a single SQLite database file, seeded with sample data and one login per role, so a fresh checkout starts with a working, populated clinic. There is no cloud hosting, no external server and no deployment step.

## Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/download) (10.0 or later)

## Build and run

From the repository root:

```
dotnet restore
```

Run the app (runs migrations and seeds an empty database before serving):

```
dotnet run --project src/CareClinicManager
```

The app listens on `127.0.0.1` and reads its port from the `ASPNETCORE_URLS` environment variable (for example `http://127.0.0.1:5000`). By default the `launchSettings.json` profile uses `http://127.0.0.1:5000`.

## Test

```
dotnet test
```

## Test logins

The seed script creates one login per role:

| Role        | User name     | Password           |
| ----------- | ------------- | ------------------ |
| Admin       | `admin`       | `Admin@123`        |
| Doctor      | `doctor`      | `Doctor@123`       |
| Receptionist| `receptionist`| `Receptionist@123` |
| Patient     | `patient`     | `Patient@123`      |

Passwords are stored only as a PBKDF2 hash, never as plain text.

## Project layout

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
  e2e/
tests/CareClinicManager.Tests/
```

## Database

The app uses SQLite with Entity Framework Core. The connection string is in `appsettings.json` (`Data Source=careclinic.db`). On startup the app applies migrations and seeds an empty database; a database that already has data is left as it is.
