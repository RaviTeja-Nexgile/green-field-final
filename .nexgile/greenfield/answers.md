# Requirements brief — Care Clinic Manager
## Profile
A complete healthcare management system for a clinic or hospital, with role-based dashboards for admin, doctors, receptionists and patients, covering appointments, medical records, prescriptions, lab tests, pharmacy inventory, billing and notifications. · web app · Healthcare management · size: large · depth: thorough · money, personalData, regulated: health, publicUsers, notifies
- **Has a user interface?** Yes (It is an ASP.NET Core MVC web app with views, dashboards, forms and pages.)
- **Runs locally?** Yes (It is a web app that serves pages, and the description says it must run locally in Visual Studio with SQLite.)
## The app
A complete healthcare management system for a clinic or hospital, with role-based dashboards for admin, doctors, receptionists and patients, covering appointments, medical records, prescriptions, lab tests, pharmacy inventory, billing and notifications.
## Ruled out by the person
- OUT1 cloud hosting or deployment: "must run locally in Visual Studio without cloud hosting, external servers, or deployment requirements" (description) [no requirement, design, task or code for it]
## Goals and success
- G1 Deliver a production-style, modern Healthcare Management System with professional business application UI/UX, functional backend, secure authentication, and a properly integrated SQLite database. (description)
- M1 Receptionists book appointments faster (No number given yet) [assumed] (Q8)
- M2 Doctors find patient history without searching through paper (No number given yet) [assumed] (Q8)
- M3 The clinic gets paid on time (No number given yet) [assumed] (Q8)
## Roles
- R1 Admin (description)
- R2 Doctor (description)
- R3 Receptionist (description)
- R4 Patient (description)
## Workflows
### W1 Patient registration (release 1) — actors: R3 Receptionist; trigger: A new patient arrives; outcome: Patient is registered; module: Patient Registration and Management (description)
### W2 Appointment booking (release 1) — actors: R3 Receptionist, R4 Patient; trigger: A patient needs an appointment; outcome: Appointment is booked; module: Appointment Booking and Scheduling (description, Q2)
1. Patient selects a time from their dashboard
2. Appointment is booked
### W3 Consultation (release 1) — actors: R2 Doctor; trigger: Patient arrives for appointment; outcome: Consultation recorded; module: Electronic Medical Records and Consultation History (description)
### W4 Prescription management (release 1) — actors: R2 Doctor; trigger: Doctor prescribes medicine; outcome: Prescription issued; module: Prescription Management (description)
### W5 Laboratory test request and report (release 1) — actors: R2 Doctor, R3 Receptionist; trigger: Doctor requests a lab test; outcome: Lab report available; module: Laboratory Tests and Reports (description)
### W6 Billing and payment (release 1) — actors: R3 Receptionist; trigger: Services provided; outcome: Invoice paid; module: Billing, Invoices, and Payment Management (description)
## Records
- E1 Patient (description)
- E2 Doctor (description)
- E3 Staff (description)
- E4 Department (description)
- E5 Specialization (description)
- E6 Appointment (description)
- E7 MedicalRecord (description)
- E8 Consultation (description)
- E9 Prescription (description)
- E10 LaboratoryTest (description)
- E11 LaboratoryReport (description)
- E12 Medicine (description)
- E13 PharmacyInventory (description)
- E14 Invoice (description)
- E15 Payment (description)
- E16 Notification (description)
- E17 SystemSetting (description)
## Rules
- BR1 Role-based authorization controls access. Applies: A doctor can see patient records. Does not apply: A patient cannot see another patient's records (description)
- BR2 Prevent unauthorized access to sensitive patient information. Applies: A receptionist cannot see clinical notes. Does not apply: A doctor can see clinical notes (description)
- BR3 Pharmacy inventory decreases automatically when a prescription is issued. Applies: A doctor prescribes 10 tablets of a medicine: the stock decreases by 10. Does not apply: A prescription is cancelled before it is issued: the stock does not change (Q3)
## Notifications
- NT1 The patient is told by email or SMS when their appointment is coming up (Q6)
## Decisions
- **Stack:** ASP.NET Core MVC, Entity Framework Core and SQLite (Bootstrap 5) (as the user's description or documents say, description: "Backend: ASP.NET Core MVC with C# … Database: SQLite with Entity Framework Core … Frontend: HTML5, CSS3, Bootstrap 5, JavaScript" Greenfield picked Bootstrap 5, as you named no framework.) (Q1)
- **Deploy target:** No deploy for now (as the user's description or documents say, description: "must run locally in Visual Studio without cloud hosting, external servers, or deployment requirements") (Q7)
## Other facts
- F1 The app must run locally in Visual Studio without cloud hosting, external servers, or deployment requirements [hosting] (description)
- F2 The UI must look like a real-world commercial healthcare SaaS product, not a basic student project or a default Bootstrap template [scope] (description)
- F3 Include sample/demo data for all major modules [migration] (description)
- F4 Provide sample login credentials for each role [security] (description)
- F5 Include a detailed README with installation and execution instructions [operations] (description)
- F6 Use secure password hashing [security] (description)
- F7 Use Entity Framework Core migrations [stack] (description)
- F8 Use reusable services, ViewModels, and dependency injection [stack] (description)
- F9 Use Clean MVC architecture with Dependency Injection, ViewModels, Services, and Repository Pattern where appropriate [stack] (description)
- F10 Implement exception handling, logging, and user-friendly error pages [operations] (description)
- F11 Implement server-side and client-side validation [security] (description)
- F12 Responsive layouts for desktop, tablet, and mobile [devices] (description)
- F13 Professional healthcare color palette (navy blue, white, teal, and soft grey) [scope] (description)
- F14 Modern sidebar navigation with icons and collapsible menus [scope] (description)
- F15 Interactive charts and analytics [reports] (description)
- F16 Professional data tables with search, sorting, filtering, and pagination [reports] (description)
- F17 Attractive appointment calendars and status badges [workflows] (description)
- F18 Complete CRUD operations for all modules [scope] (description)
- F19 Configure proper database relationships and foreign keys [entities] (description)
- F20 Provide a clean, organized Visual Studio solution [operations] (description)
- F21 Include SQLite database setup and migration instructions [migration] (description)
- F22 Reusable layouts, partial views, and consistent UI components [scope] (description)
- F23 Professional login, registration, and profile pages [security] (description)
- F24 Modern forms with validation and user-friendly controls [scope] (description)
- F25 Consistent spacing, typography, shadows, rounded cards, and subtle animations [scope] (description)
- F26 Responsive top navigation bar with profile dropdown and notifications [scope] (description)
- F27 Beautiful dashboard cards displaying KPIs and statistics [reports] (description)
- F28 Follow clean coding standards and maintainable project architecture [operations] (description)
- F29 The entire application must run locally in Visual Studio [hosting] (description)
- F30 Ensure every dashboard, button, form, navigation link, CRUD operation, and workflow works correctly [scope] (description)
- F31 Patients book their own appointments from their patient dashboard [workflows] (Q2)
- F32 Lab results can be uploaded as files and entered as structured data [entities] (Q4)
- F33 Invoices are recorded as paid by the receptionist, not paid online [money] (Q5)
## Out of scope and later releases
- Hosting and deployment: not relevant, the user said so [out of scope]
## The interview
- Q2 [workflows · owner] Should patients book their own appointments from their dashboard? → Yes, patients book themselves
- Q3 [rules · owner] Should the pharmacy inventory decrease automatically when a prescription is issued? → Yes, automatically
- Q4 [files · owner] Should lab results be uploaded as files or entered as structured data? → Both
- Q5 [money · owner] Should invoices be paid online or recorded as paid by the receptionist? → Recorded by receptionist
- Q6 [notifications · owner] Should the system send appointment reminders to patients? → Yes, by email or SMS
- Q8 [success · owner] When this system is running in your clinic, what would make you say it is working well? → The receptionists book appointments faster, doctors find patient history without searching through paper, and the clinic gets paid on time (Greenfield's draft, sent unchanged)
- Q9 [notifications · role:R1] ¿Con cuánta antelación debe enviarse el recordatorio de la cita al paciente? → Decide for me
- Q10 [current · role:R1] ¿Cómo se gestionan hoy las citas y las historias clínicas en la clínica? → Decide for me
