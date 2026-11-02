# Tasks: Care Clinic Manager

- [ ] 1. Project setup
  - _Requirements: 1.1, 1.2, 1.3, 1.4, 1.5, 1.10, 14.7_
  - _Area: setup_
  - _Files: CareClinicManager.sln, src/CareClinicManager/Program.cs, src/CareClinicManager/appsettings.json, src/CareClinicManager/appsettings.Development.json, src/CareClinicManager/Data/AppDbContext.cs, src/CareClinicManager/Data/DbSeeder.cs, src/CareClinicManager/Data/Migrations/**, src/CareClinicManager/Models/**, src/CareClinicManager/Views/Shared/**, src/CareClinicManager/wwwroot/css/site.css, src/CareClinicManager/wwwroot/js/site.js, src/CareClinicManager/wwwroot/lib/**, src/CareClinicManager/e2e/.gitkeep, tests/CareClinicManager.Tests/**, README.md, .gitignore_
  - _Verify: dotnet restore_
  - _Verify: dotnet test_
  - _Issue: #13_

- [ ] 2. Sign in, registration and role-based access
  - _Requirements: 1.1, 1.2, 1.3, 1.4, 1.5, 1.6, 1.7, 1.8, 1.9, 1.10, 14.7_
  - _Depends: 1_
  - _Area: api_
  - _Files: src/CareClinicManager/Controllers/AccountController.cs, src/CareClinicManager/ViewModels/Account/**, src/CareClinicManager/Views/Account/**, src/CareClinicManager/Services/AuthService.cs, src/CareClinicManager/wwwroot/css/account.css, tests/CareClinicManager.Tests/AccountTests.cs_
  - _Verify: dotnet test --filter AccountTests_
  - _Issue: #14_

- [ ] 3. Admin dashboard and analytics
  - _Requirements: 14.1, 14.2, 14.6, 14.7_
  - _Depends: 1_
  - _Area: ui_
  - _Files: src/CareClinicManager/Controllers/AdminController.cs, src/CareClinicManager/ViewModels/Admin/DashboardViewModel.cs, src/CareClinicManager/Views/Admin/Dashboard.cshtml, src/CareClinicManager/wwwroot/css/admin-dashboard.css, tests/CareClinicManager.Tests/AdminDashboardTests.cs_
  - _Verify: dotnet test --filter AdminDashboardTests_
  - _Issue: #15_

- [ ] 4. Doctor dashboard
  - _Requirements: 14.1, 14.3, 14.6, 14.7_
  - _Depends: 1_
  - _Area: ui_
  - _Files: src/CareClinicManager/Controllers/DoctorController.cs, src/CareClinicManager/ViewModels/Doctor/DashboardViewModel.cs, src/CareClinicManager/Views/Doctor/Dashboard.cshtml, src/CareClinicManager/wwwroot/css/doctor-dashboard.css, tests/CareClinicManager.Tests/DoctorDashboardTests.cs_
  - _Verify: dotnet test --filter DoctorDashboardTests_
  - _Issue: #16_

- [ ] 5. Receptionist dashboard
  - _Requirements: 14.1, 14.4, 14.6, 14.7_
  - _Depends: 1_
  - _Area: ui_
  - _Files: src/CareClinicManager/Controllers/ReceptionistController.cs, src/CareClinicManager/ViewModels/Receptionist/DashboardViewModel.cs, src/CareClinicManager/Views/Receptionist/Dashboard.cshtml, src/CareClinicManager/wwwroot/css/receptionist-dashboard.css, tests/CareClinicManager.Tests/ReceptionistDashboardTests.cs_
  - _Verify: dotnet test --filter ReceptionistDashboardTests_
  - _Issue: #17_

- [ ] 6. Patient dashboard
  - _Requirements: 12.1, 14.1, 14.5, 14.6, 14.7_
  - _Depends: 1_
  - _Area: ui_
  - _Files: src/CareClinicManager/Controllers/PatientController.cs, src/CareClinicManager/ViewModels/Patient/DashboardViewModel.cs, src/CareClinicManager/Views/Patient/Dashboard.cshtml, src/CareClinicManager/wwwroot/css/patient-dashboard.css, tests/CareClinicManager.Tests/PatientDashboardTests.cs_
  - _Verify: dotnet test --filter PatientDashboardTests_
  - _Issue: #18_

- [ ] 7. Patient registration and management
  - _Requirements: 2.1, 2.2, 2.3, 2.4, 2.5, 2.6, 2.7_
  - _Depends: 1_
  - _Area: ui_
  - _Files: src/CareClinicManager/Controllers/ReceptionistPatientsController.cs, src/CareClinicManager/ViewModels/Receptionist/PatientViewModels.cs, src/CareClinicManager/Views/Receptionist/Patients/**, src/CareClinicManager/Services/PatientService.cs, src/CareClinicManager/Repositories/PatientRepository.cs, src/CareClinicManager/wwwroot/css/patients.css, tests/CareClinicManager.Tests/PatientManagementTests.cs_
  - _Verify: dotnet test --filter PatientManagementTests_
  - _Issue: #19_

- [ ] 8. Doctor and staff management
  - _Requirements: 3.1, 3.2, 3.3, 3.4, 3.5, 3.6_
  - _Depends: 1_
  - _Area: ui_
  - _Files: src/CareClinicManager/Controllers/AdminStaffController.cs, src/CareClinicManager/ViewModels/Admin/StaffViewModels.cs, src/CareClinicManager/Views/Admin/Staff/**, src/CareClinicManager/Services/StaffService.cs, src/CareClinicManager/wwwroot/css/staff.css, tests/CareClinicManager.Tests/StaffManagementTests.cs_
  - _Verify: dotnet test --filter StaffManagementTests_
  - _Issue: #20_

- [ ] 9. Department and specialization management
  - _Requirements: 4.1, 4.2, 4.3, 4.4, 4.5, 4.6_
  - _Depends: 1_
  - _Area: ui_
  - _Files: src/CareClinicManager/Controllers/AdminDepartmentsController.cs, src/CareClinicManager/ViewModels/Admin/DepartmentViewModels.cs, src/CareClinicManager/Views/Admin/Departments/**, src/CareClinicManager/Views/Admin/Specializations/**, src/CareClinicManager/Services/DepartmentService.cs, src/CareClinicManager/wwwroot/css/departments.css, tests/CareClinicManager.Tests/DepartmentManagementTests.cs_
  - _Verify: dotnet test --filter DepartmentManagementTests_
  - _Issue: #21_

- [ ] 10. Appointment booking, calendar and status tracking
  - _Requirements: 5.1, 5.2, 5.3, 5.4, 5.5, 5.6, 5.7, 5.8, 6.1, 6.2, 6.3, 6.4, 6.5_
  - _Depends: 1_
  - _Area: ui_
  - _Files: src/CareClinicManager/Controllers/ReceptionistAppointmentsController.cs, src/CareClinicManager/Controllers/PatientAppointmentsController.cs, src/CareClinicManager/ViewModels/Receptionist/AppointmentViewModels.cs, src/CareClinicManager/Views/Receptionist/Appointments/**, src/CareClinicManager/Views/Patient/Appointments/**, src/CareClinicManager/Services/AppointmentService.cs, src/CareClinicManager/Repositories/AppointmentRepository.cs, src/CareClinicManager/wwwroot/css/appointments.css, tests/CareClinicManager.Tests/AppointmentTests.cs_
  - _Verify: dotnet test --filter AppointmentTests_
  - _Issue: #22_

- [ ] 11. Consultations and medical records
  - _Requirements: 7.1, 7.2, 7.3, 7.4, 7.5, 7.6, 6.4_
  - _Depends: 1_
  - _Area: ui_
  - _Files: src/CareClinicManager/Controllers/DoctorConsultationsController.cs, src/CareClinicManager/ViewModels/Doctor/ConsultationViewModels.cs, src/CareClinicManager/Views/Doctor/Consultations/**, src/CareClinicManager/Views/Doctor/Patients/**, src/CareClinicManager/Services/ConsultationService.cs, src/CareClinicManager/wwwroot/css/consultations.css, tests/CareClinicManager.Tests/ConsultationTests.cs_
  - _Verify: dotnet test --filter ConsultationTests_
  - _Issue: #23_

- [ ] 12. Prescription management and pharmacy inventory
  - _Requirements: 8.1, 8.2, 8.3, 8.4, 8.5, 8.6, 10.5_
  - _Depends: 1_
  - _Area: ui_
  - _Files: src/CareClinicManager/Controllers/DoctorPrescriptionsController.cs, src/CareClinicManager/ViewModels/Doctor/PrescriptionViewModels.cs, src/CareClinicManager/Views/Doctor/Prescriptions/**, src/CareClinicManager/Services/PrescriptionService.cs, src/CareClinicManager/wwwroot/css/prescriptions.css, tests/CareClinicManager.Tests/PrescriptionTests.cs_
  - _Verify: dotnet test --filter PrescriptionTests_
  - _Issue: #24_

- [ ] 13. Laboratory tests and reports
  - _Requirements: 9.1, 9.2, 9.3, 9.4, 9.5, 9.6_
  - _Depends: 1_
  - _Area: ui_
  - _Files: src/CareClinicManager/Controllers/DoctorLabTestsController.cs, src/CareClinicManager/ViewModels/Doctor/LabTestViewModels.cs, src/CareClinicManager/Views/Doctor/LabTests/**, src/CareClinicManager/Services/LabTestService.cs, src/CareClinicManager/wwwroot/css/labtests.css, tests/CareClinicManager.Tests/LabTestTests.cs_
  - _Verify: dotnet test --filter LabTestTests_
  - _Issue: #25_

- [ ] 14. Medicine and pharmacy inventory management
  - _Requirements: 10.1, 10.2, 10.3, 10.4, 10.6_
  - _Depends: 1_
  - _Area: ui_
  - _Files: src/CareClinicManager/Controllers/AdminMedicinesController.cs, src/CareClinicManager/ViewModels/Admin/MedicineViewModels.cs, src/CareClinicManager/Views/Admin/Medicines/**, src/CareClinicManager/Services/MedicineService.cs, src/CareClinicManager/Repositories/MedicineRepository.cs, src/CareClinicManager/wwwroot/css/medicines.css, tests/CareClinicManager.Tests/MedicineTests.cs_
  - _Verify: dotnet test --filter MedicineTests_
  - _Issue: #26_

- [ ] 15. Billing, invoices and payments
  - _Requirements: 11.1, 11.2, 11.3, 11.4, 11.5, 11.6, 12.3_
  - _Depends: 1_
  - _Area: ui_
  - _Files: src/CareClinicManager/Controllers/ReceptionistInvoicesController.cs, src/CareClinicManager/Controllers/PatientInvoicesController.cs, src/CareClinicManager/ViewModels/Receptionist/InvoiceViewModels.cs, src/CareClinicManager/Views/Receptionist/Invoices/**, src/CareClinicManager/Views/Patient/Invoices/**, src/CareClinicManager/Services/InvoiceService.cs, src/CareClinicManager/wwwroot/css/invoices.css, tests/CareClinicManager.Tests/InvoiceTests.cs_
  - _Verify: dotnet test --filter InvoiceTests_
  - _Issue: #27_

- [ ] 16. Patient profile and medical history
  - _Requirements: 12.1, 12.2, 12.3, 12.4_
  - _Depends: 1_
  - _Area: ui_
  - _Files: src/CareClinicManager/Controllers/PatientMedicalHistoryController.cs, src/CareClinicManager/ViewModels/Patient/MedicalHistoryViewModel.cs, src/CareClinicManager/Views/Patient/MedicalHistory.cshtml, src/CareClinicManager/wwwroot/css/medical-history.css, tests/CareClinicManager.Tests/MedicalHistoryTests.cs_
  - _Verify: dotnet test --filter MedicalHistoryTests_
  - _Issue: #28_

- [ ] 17. Notifications and system settings
  - _Requirements: 13.1, 13.2, 13.3, 13.4, 13.5_
  - _Depends: 1_
  - _Area: ui_
  - _Files: src/CareClinicManager/Controllers/AdminSettingsController.cs, src/CareClinicManager/Controllers/AdminNotificationsController.cs, src/CareClinicManager/ViewModels/Admin/SettingsViewModels.cs, src/CareClinicManager/Views/Admin/Settings.cshtml, src/CareClinicManager/Views/Admin/Notifications.cshtml, src/CareClinicManager/Services/NotificationService.cs, src/CareClinicManager/wwwroot/css/settings.css, tests/CareClinicManager.Tests/SettingsTests.cs_
  - _Verify: dotnet test --filter SettingsTests_
  - _Issue: #29_
