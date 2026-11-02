using CareClinicManager.Models;
using Microsoft.EntityFrameworkCore;

namespace CareClinicManager.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Patient> Patients => Set<Patient>();
    public DbSet<Doctor> Doctors => Set<Doctor>();
    public DbSet<Staff> Staff => Set<Staff>();
    public DbSet<Department> Departments => Set<Department>();
    public DbSet<Specialization> Specializations => Set<Specialization>();
    public DbSet<Appointment> Appointments => Set<Appointment>();
    public DbSet<MedicalRecord> MedicalRecords => Set<MedicalRecord>();
    public DbSet<Consultation> Consultations => Set<Consultation>();
    public DbSet<Prescription> Prescriptions => Set<Prescription>();
    public DbSet<LaboratoryTest> LaboratoryTests => Set<LaboratoryTest>();
    public DbSet<LaboratoryReport> LaboratoryReports => Set<LaboratoryReport>();
    public DbSet<Medicine> Medicines => Set<Medicine>();
    public DbSet<PharmacyInventory> PharmacyInventory => Set<PharmacyInventory>();
    public DbSet<Invoice> Invoices => Set<Invoice>();
    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<Notification> Notifications => Set<Notification>();
    public DbSet<SystemSetting> SystemSettings => Set<SystemSetting>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(e =>
        {
            e.HasIndex(u => u.UserName).IsUnique();
            e.HasOne(u => u.Patient)
                .WithMany()
                .HasForeignKey(u => u.PatientId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<Patient>(e =>
        {
            e.HasOne(p => p.MedicalRecord)
                .WithOne(m => m.Patient)
                .HasForeignKey<MedicalRecord>(m => m.PatientId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Doctor>(e =>
        {
            e.HasOne(d => d.Department)
                .WithMany(dep => dep.Doctors)
                .HasForeignKey(d => d.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);
            e.HasOne(d => d.Specialization)
                .WithMany(s => s.Doctors)
                .HasForeignKey(d => d.SpecializationId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Staff>(e =>
        {
            e.HasOne(s => s.Department)
                .WithMany(dep => dep.Staff)
                .HasForeignKey(s => s.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Appointment>(e =>
        {
            e.HasOne(a => a.Patient)
                .WithMany(p => p.Appointments)
                .HasForeignKey(a => a.PatientId)
                .OnDelete(DeleteBehavior.Restrict);
            e.HasOne(a => a.Doctor)
                .WithMany(d => d.Appointments)
                .HasForeignKey(a => a.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);
            e.HasOne(a => a.Department)
                .WithMany(dep => dep.Appointments)
                .HasForeignKey(a => a.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Consultation>(e =>
        {
            e.HasOne(c => c.MedicalRecord)
                .WithMany(m => m.Consultations)
                .HasForeignKey(c => c.MedicalRecordId)
                .OnDelete(DeleteBehavior.Cascade);
            e.HasOne(c => c.Appointment)
                .WithMany(a => a.Consultations)
                .HasForeignKey(c => c.AppointmentId)
                .OnDelete(DeleteBehavior.Restrict);
            e.HasOne(c => c.Doctor)
                .WithMany()
                .HasForeignKey(c => c.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Prescription>(e =>
        {
            e.HasOne(p => p.MedicalRecord)
                .WithMany(m => m.Prescriptions)
                .HasForeignKey(p => p.MedicalRecordId)
                .OnDelete(DeleteBehavior.Cascade);
            e.HasOne(p => p.Consultation)
                .WithMany(c => c.Prescriptions)
                .HasForeignKey(p => p.ConsultationId)
                .OnDelete(DeleteBehavior.Restrict);
            e.HasOne(p => p.Medicine)
                .WithMany(m => m.Prescriptions)
                .HasForeignKey(p => p.MedicineId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<LaboratoryTest>(e =>
        {
            e.HasOne(t => t.MedicalRecord)
                .WithMany(m => m.LaboratoryTests)
                .HasForeignKey(t => t.MedicalRecordId)
                .OnDelete(DeleteBehavior.Cascade);
            e.HasOne(t => t.Patient)
                .WithMany()
                .HasForeignKey(t => t.PatientId)
                .OnDelete(DeleteBehavior.Restrict);
            e.HasOne(t => t.Doctor)
                .WithMany()
                .HasForeignKey(t => t.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<LaboratoryReport>(e =>
        {
            e.HasOne(r => r.LaboratoryTest)
                .WithOne(t => t.Report)
                .HasForeignKey<LaboratoryReport>(r => r.LaboratoryTestId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<PharmacyInventory>(e =>
        {
            e.HasOne(i => i.Medicine)
                .WithMany()
                .HasForeignKey(i => i.MedicineId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Invoice>(e =>
        {
            e.HasOne(i => i.Patient)
                .WithMany(p => p.Invoices)
                .HasForeignKey(i => i.PatientId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Payment>(e =>
        {
            e.HasOne(p => p.Invoice)
                .WithMany(i => i.Payments)
                .HasForeignKey(p => p.InvoiceId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Notification>(e =>
        {
            e.HasOne(n => n.Patient)
                .WithMany(p => p.Notifications)
                .HasForeignKey(n => n.PatientId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
