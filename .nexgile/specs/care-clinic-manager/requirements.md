# Requirements: Care Clinic Manager

## Requirement 1: Sign in and role-based access

**User Story:** As a user of the clinic, I want to sign in with my own account, so that I see only the screens and records my role allows.

_Source: R1, R2, R3, R4, BR1, BR2, F6, F23_

### Acceptance Criteria

1. WHEN a person opens the app at `/` THE SYSTEM SHALL show the sign-in page.
2. WHEN a person enters a correct user name and password THE SYSTEM SHALL sign them in and open the dashboard for their role: Admin, Doctor, Receptionist or Patient.
3. WHEN a person enters a wrong user name or password THE SYSTEM SHALL show the message "Invalid username or password" and stay on the sign-in page.
4. WHEN a person signs in THE SYSTEM SHALL store their password only as a hash, never as plain text.
5. IF a person who is not signed in opens any page other than the sign-in page THEN THE SYSTEM SHALL send them to the sign-in page.
6. WHEN a Patient signs in THE SYSTEM SHALL open the Patient dashboard and never show another patient's records.
7. WHEN a Receptionist signs in THE SYSTEM SHALL open the Receptionist dashboard and never show clinical notes.
8. WHEN a Doctor signs in THE SYSTEM SHALL open the Doctor dashboard and show patient records and clinical notes.
9. WHEN an Admin signs in THE SYSTEM SHALL open the Admin dashboard with system control, staff, departments, patients, appointments, financial reports and settings.
10. WHEN a person signs out THE SYSTEM SHALL return them to the sign-in page.

## Requirement 2: Patient registration and management

**User Story:** As a Receptionist, I want to register a new patient and manage patient records, so that the clinic has an accurate, up-to-date patient list.

_Source: W1, E1, F18, F24, F11_

### Acceptance Criteria

1. WHEN a Receptionist opens the patient list THE SYSTEM SHALL show every patient with search, sorting, filtering and pagination.
2. WHEN a Receptionist opens "Register patient" THE SYSTEM SHALL show a form with the patient's name, date of birth, gender, phone, email and address.
3. WHEN a Receptionist submits the register form with a required field empty THE SYSTEM SHALL show a validation message and not save the patient.
4. WHEN a Receptionist submits a valid register form THE SYSTEM SHALL save the patient and show the patient in the list.
5. WHEN a Receptionist edits a patient and saves THE SYSTEM SHALL update the patient's record.
6. WHEN a Receptionist deletes a patient THE SYSTEM SHALL remove the patient from the list.
7. WHEN a Receptionist opens a patient's record THE SYSTEM SHALL show the patient's profile and medical history.

## Requirement 3: Doctor and staff management

**User Story:** As an Admin, I want to manage doctors and staff, so that the clinic's team and their roles are correct.

_Source: E2, E3, F18, F24, F11_

### Acceptance Criteria

1. WHEN an Admin opens the staff list THE SYSTEM SHALL show every doctor and staff member with search, sorting, filtering and pagination.
2. WHEN an Admin opens "Add doctor" or "Add staff" THE SYSTEM SHALL show a form with name, role, department, specialization and contact details.
3. WHEN an Admin submits the form with a required field empty THE SYSTEM SHALL show a validation message and not save the member.
4. WHEN an Admin submits a valid form THE SYSTEM SHALL save the doctor or staff member and show them in the list.
5. WHEN an Admin edits a doctor or staff member and saves THE SYSTEM SHALL update the record.
6. WHEN an Admin deletes a doctor or staff member THE SYSTEM SHALL remove them from the list.

## Requirement 4: Department and specialization management

**User Story:** As an Admin, I want to manage departments and specializations, so that doctors and appointments are organized correctly.

_Source: E4, E5, F18, F24, F11_

### Acceptance Criteria

1. WHEN an Admin opens the departments list THE SYSTEM SHALL show every department with search, sorting, filtering and pagination.
2. WHEN an Admin opens the specializations list THE SYSTEM SHALL show every specialization with search, sorting, filtering and pagination.
3. WHEN an Admin adds a department or specialization with a name THE SYSTEM SHALL save it and show it in the list.
4. WHEN an Admin submits a department or specialization with an empty name THE SYSTEM SHALL show a validation message and not save it.
5. WHEN an Admin edits a department or specialization and saves THE SYSTEM SHALL update the record.
6. WHEN an Admin deletes a department or specialization THE SYSTEM SHALL remove it from the list.

## Requirement 5: Appointment booking and scheduling

**User Story:** As a Receptionist or a Patient, I want to book and schedule an appointment, so that a patient gets a time with the right doctor.

_Source: W2, E6, F17, F31, F24, F11_

### Acceptance Criteria

1. WHEN a Receptionist opens the appointment calendar THE SYSTEM SHALL show appointments in a calendar view with status badges.
2. WHEN a Receptionist opens "Book appointment" THE SYSTEM SHALL show a form to pick a patient, a doctor, a department, a date and a time.
3. WHEN a Patient signs in and opens their dashboard THE SYSTEM SHALL let them pick a time and book their own appointment.
4. WHEN the appointment form is submitted with a required field empty THE SYSTEM SHALL show a validation message and not book the appointment.
5. WHEN a valid appointment is booked THE SYSTEM SHALL save it with the status "Scheduled" and show it in the calendar.
6. WHEN a Receptionist edits an appointment and saves THE SYSTEM SHALL update the appointment.
7. WHEN a Receptionist cancels an appointment THE SYSTEM SHALL set its status to "Cancelled".
8. WHEN a Patient books an appointment THE SYSTEM SHALL show the appointment in the Patient's dashboard.

## Requirement 6: Appointment status tracking

**User Story:** As a Receptionist, I want to track the status of each appointment, so that the clinic knows who has checked in, is waiting, or has been seen.

_Source: E6, F17, F18_

### Acceptance Criteria

1. WHEN a Receptionist opens the appointment list THE SYSTEM SHALL show each appointment with its status badge.
2. WHEN a Receptionist checks a patient in THE SYSTEM SHALL set the appointment status to "Checked in".
3. WHEN a Doctor starts a consultation THE SYSTEM SHALL set the appointment status to "In progress".
4. WHEN a consultation ends THE SYSTEM SHALL set the appointment status to "Completed".
5. WHEN an appointment is cancelled THE SYSTEM SHALL set its status to "Cancelled".

## Requirement 7: Electronic medical records and consultation history

**User Story:** As a Doctor, I want to record a consultation and see a patient's history, so that I can treat the patient with full context.

_Source: W3, E7, E8, BR2, F18, F24, F11_

### Acceptance Criteria

1. WHEN a Doctor opens a patient's record THE SYSTEM SHALL show the patient's consultation history.
2. WHEN a Doctor opens "New consultation" THE SYSTEM SHALL show a form for symptoms, diagnosis and clinical notes.
3. WHEN a Doctor submits a consultation with a required field empty THE SYSTEM SHALL show a validation message and not save it.
4. WHEN a Doctor submits a valid consultation THE SYSTEM SHALL save it to the patient's medical record and show it in the history.
5. WHEN a Doctor edits a consultation and saves THE SYSTEM SHALL update the record.
6. WHEN a Receptionist opens a patient's record THE SYSTEM SHALL not show clinical notes.

## Requirement 8: Prescription management

**User Story:** As a Doctor, I want to prescribe medicine, so that the patient receives the right treatment and the pharmacy stock stays accurate.

_Source: W4, E9, BR3, F18, F24, F11_

### Acceptance Criteria

1. WHEN a Doctor opens "New prescription" THE SYSTEM SHALL show a form to pick a medicine, a dosage and a quantity.
2. WHEN a Doctor submits a prescription with a required field empty THE SYSTEM SHALL show a validation message and not save it.
3. WHEN a Doctor prescribes a quantity of a medicine THE SYSTEM SHALL save the prescription and decrease the pharmacy inventory by that quantity.
4. WHEN a Doctor prescribes 10 tablets of a medicine THE SYSTEM SHALL decrease the stock of that medicine by 10.
5. WHEN a prescription is cancelled before it is issued THE SYSTEM SHALL not change the pharmacy inventory.
6. WHEN a Doctor opens a patient's record THE SYSTEM SHALL show the patient's prescriptions.

## Requirement 9: Laboratory tests and reports

**User Story:** As a Doctor, I want to request a lab test and see its report, so that the patient's results inform the diagnosis.

_Source: W5, E10, E11, F32, F18, F24, F11_

### Acceptance Criteria

1. WHEN a Doctor opens "Request lab test" THE SYSTEM SHALL show a form to pick a patient and a test type.
2. WHEN a Doctor submits a lab request with a required field empty THE SYSTEM SHALL show a validation message and not save it.
3. WHEN a Doctor submits a valid lab request THE SYSTEM SHALL save it with the status "Requested".
4. WHEN a Receptionist records a lab result THE SYSTEM SHALL let them upload a result file and enter structured result data.
5. WHEN a lab report is recorded THE SYSTEM SHALL set the test status to "Completed" and show the report in the patient's record.
6. WHEN a Patient opens their dashboard THE SYSTEM SHALL show their laboratory reports.

## Requirement 10: Medicine and pharmacy inventory management

**User Story:** As an Admin, I want to manage medicines and pharmacy inventory, so that the clinic knows what is in stock.

_Source: E12, E13, BR3, F18, F24, F11, F16_

### Acceptance Criteria

1. WHEN an Admin opens the medicine list THE SYSTEM SHALL show every medicine with search, sorting, filtering and pagination.
2. WHEN an Admin adds a medicine with a name and a stock quantity THE SYSTEM SHALL save it and show it in the list.
3. WHEN an Admin submits a medicine with an empty name THE SYSTEM SHALL show a validation message and not save it.
4. WHEN an Admin edits a medicine's stock quantity and saves THE SYSTEM SHALL update the inventory.
5. WHEN a prescription is issued THE SYSTEM SHALL decrease the inventory of the prescribed medicine by the prescribed quantity.
6. WHEN an Admin deletes a medicine THE SYSTEM SHALL remove it from the list.

## Requirement 11: Billing, invoices and payment management

**User Story:** As a Receptionist, I want to create invoices and record payments, so that the clinic gets paid for its services.

_Source: W6, E14, E15, F33, F18, F24, F11_

### Acceptance Criteria

1. WHEN a Receptionist opens the invoice list THE SYSTEM SHALL show every invoice with search, sorting, filtering and pagination.
2. WHEN a Receptionist opens "New invoice" THE SYSTEM SHALL show a form to pick a patient and the services or items to bill.
3. WHEN a Receptionist submits an invoice with a required field empty THE SYSTEM SHALL show a validation message and not save it.
4. WHEN a Receptionist submits a valid invoice THE SYSTEM SHALL save it with the status "Unpaid" and show the total amount.
5. WHEN a Receptionist records a payment for an invoice THE SYSTEM SHALL set the invoice status to "Paid".
6. WHEN a Patient opens their dashboard THE SYSTEM SHALL show their invoices and their payment status.

## Requirement 12: Patient profile and medical history

**User Story:** As a Patient, I want to see my profile and medical history, so that I can review my own care.

_Source: E1, E7, E8, E9, E11, E14, BR1_

### Acceptance Criteria

1. WHEN a Patient opens their dashboard THE SYSTEM SHALL show their profile information.
2. WHEN a Patient opens "Medical history" THE SYSTEM SHALL show their consultations, prescriptions and laboratory reports.
3. WHEN a Patient opens "Invoices" THE SYSTEM SHALL show their invoices and payment status.
4. IF a Patient tries to open another patient's record THEN THE SYSTEM SHALL not show it.

## Requirement 13: Notifications and system settings

**User Story:** As an Admin, I want to manage notifications and system settings, so that the clinic runs the way it wants.

_Source: E16, E17, NT1, F18, F24, F11_

### Acceptance Criteria

1. WHEN an Admin opens "System settings" THE SYSTEM SHALL show the settings with their current values.
2. WHEN an Admin changes a setting and saves THE SYSTEM SHALL update the setting.
3. WHEN an appointment is coming up THE SYSTEM SHALL tell the patient by email or SMS.
4. WHEN an Admin opens the notifications list THE SYSTEM SHALL show the notifications that were sent.
5. WHEN an Admin submits a setting with an empty required value THE SYSTEM SHALL show a validation message and not save it.

## Requirement 14: Dashboards and analytics

**User Story:** As a signed-in user, I want a dashboard with the statistics for my role, so that I can see the state of the clinic at a glance.

_Source: G1, F15, F27, F2, F13, F14, F16, F17, F22, F25, F26, F12

### Acceptance Criteria

1. WHEN a user signs in THE SYSTEM SHALL show a dashboard with KPI cards and statistics for their role.
2. WHEN an Admin opens their dashboard THE SYSTEM SHALL show statistics for staff, departments, patients, appointments and financial reports.
3. WHEN a Doctor opens their dashboard THE SYSTEM SHALL show today's appointments, patient records, consultation history, prescriptions and laboratory requests.
4. WHEN a Receptionist opens their dashboard THE SYSTEM SHALL show patient registration, appointment scheduling, check-in, billing and payment management.
5. WHEN a Patient opens their dashboard THE SYSTEM SHALL show appointment booking, medical history, prescriptions, laboratory reports and invoices.
6. WHEN a user opens a data table THE SYSTEM SHALL show search, sorting, filtering and pagination controls.
7. WHEN a user opens the app on a phone, tablet or desktop THE SYSTEM SHALL lay the screen out to fit that device.

## Later releases

- Hosting and deployment: not relevant, the user said so.

## Assumptions

- The sign-in page is the first screen at `/`, and each role lands on its own dashboard after signing in.
- Appointment reminders are sent by email or SMS; the exact lead time is left to a default of 24 hours before the appointment.
- "Staff" covers non-doctor employees such as receptionists and nurses, managed by the Admin.
- A consultation is linked to an appointment, and completing a consultation marks the appointment "Completed".
- The pharmacy inventory is a single stock quantity per medicine, decreased by prescriptions and edited by the Admin.
