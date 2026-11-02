using CareClinicManager.Models;
using Microsoft.AspNetCore.Identity;

namespace CareClinicManager.Data;

/// <summary>
/// Seeds an empty database with sample data and one login per role.
/// Leaves a database that already has data untouched.
/// </summary>
public static class DbSeeder
{
    public static void Seed(AppDbContext db)
    {
        if (db.Users.Any())
        {
            return; // already seeded
        }

        var hasher = new PasswordHasher<User>();

        // Departments
        var cardiology = new Department { Name = "Cardiology" };
        var dermatology = new Department { Name = "Dermatology" };
        var general = new Department { Name = "General Medicine" };
        var pediatrics = new Department { Name = "Pediatrics" };
        db.Departments.AddRange(cardiology, dermatology, general, pediatrics);

        // Specializations
        var cardiologist = new Specialization { Name = "Cardiologist" };
        var dermatologist = new Specialization { Name = "Dermatologist" };
        var generalPractitioner = new Specialization { Name = "General Practitioner" };
        var pediatrician = new Specialization { Name = "Pediatrician" };
        db.Specializations.AddRange(cardiologist, dermatologist, generalPractitioner, pediatrician);

        // Doctors
        var doctor1 = new Doctor { Name = "Dr. Alice Carter", Role = "Doctor", Department = cardiology, Specialization = cardiologist, Phone = "555-0101", Email = "alice.carter@clinic.local" };
        var doctor2 = new Doctor { Name = "Dr. Bob Nguyen", Role = "Doctor", Department = general, Specialization = generalPractitioner, Phone = "555-0102", Email = "bob.nguyen@clinic.local" };
        db.Doctors.AddRange(doctor1, doctor2);

        // Staff
        db.Staff.AddRange(
            new Staff { Name = "Carol Smith", Role = "Receptionist", Department = general, Phone = "555-0103", Email = "carol.smith@clinic.local" },
            new Staff { Name = "Dan Lee", Role = "Nurse", Department = pediatrics, Phone = "555-0104", Email = "dan.lee@clinic.local" }
        );

        // Patients
        var patient1 = new Patient { Name = "Eve Johnson", DateOfBirth = new DateTime(1985, 4, 12), Gender = "Female", Phone = "555-0201", Email = "eve.johnson@example.com", Address = "12 Oak Street" };
        var patient2 = new Patient { Name = "Frank Miller", DateOfBirth = new DateTime(1990, 8, 3), Gender = "Male", Phone = "555-0202", Email = "frank.miller@example.com", Address = "34 Pine Avenue" };
        db.Patients.AddRange(patient1, patient2);

        // Medical records
        var record1 = new MedicalRecord { Patient = patient1 };
        var record2 = new MedicalRecord { Patient = patient2 };
        db.MedicalRecords.AddRange(record1, record2);

        // Medicines
        var med1 = new Medicine { Name = "Amoxicillin", StockQuantity = 200 };
        var med2 = new Medicine { Name = "Paracetamol", StockQuantity = 500 };
        var med3 = new Medicine { Name = "Ibuprofen", StockQuantity = 300 };
        db.Medicines.AddRange(med1, med2, med3);

        // Pharmacy inventory (single stock quantity per medicine)
        db.PharmacyInventory.AddRange(
            new PharmacyInventory { Medicine = med1, StockQuantity = 200 },
            new PharmacyInventory { Medicine = med2, StockQuantity = 500 },
            new PharmacyInventory { Medicine = med3, StockQuantity = 300 }
        );

        // Appointments
        db.Appointments.AddRange(
            new Appointment { Patient = patient1, Doctor = doctor1, Department = cardiology, Date = DateTime.Today.AddDays(1), Time = new TimeSpan(9, 0, 0), Status = "Scheduled" },
            new Appointment { Patient = patient2, Doctor = doctor2, Department = general, Date = DateTime.Today.AddDays(2), Time = new TimeSpan(10, 30, 0), Status = "Scheduled" }
        );

        // System settings
        db.SystemSettings.AddRange(
            new SystemSetting { Key = "ReminderLeadHours", Value = "24" },
            new SystemSetting { Key = "ClinicName", Value = "Care Clinic Manager" }
        );

        db.SaveChanges();

        // Users (one login per role), hashed passwords
        var admin = new User { UserName = "admin", Role = "Admin" };
        admin.PasswordHash = hasher.HashPassword(admin, "Admin@123");

        var doctorUser = new User { UserName = "doctor", Role = "Doctor" };
        doctorUser.PasswordHash = hasher.HashPassword(doctorUser, "Doctor@123");

        var receptionist = new User { UserName = "receptionist", Role = "Receptionist" };
        receptionist.PasswordHash = hasher.HashPassword(receptionist, "Receptionist@123");

        var patientUser = new User { UserName = "patient", Role = "Patient", PatientId = patient1.Id };
        patientUser.PasswordHash = hasher.HashPassword(patientUser, "Patient@123");

        db.Users.AddRange(admin, doctorUser, receptionist, patientUser);
        db.SaveChanges();
    }
}
