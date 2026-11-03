namespace CareClinicManager.ViewModels.Doctor;

/// <summary>
/// Data shown on the Doctor dashboard: KPI statistics plus the lists a doctor
/// works with day to day — today's appointments, patient records, consultation
/// history, prescriptions and laboratory requests.
/// </summary>
public class DashboardViewModel
{
    // KPI cards
    public int TodayAppointmentsCount { get; set; }
    public int PatientsCount { get; set; }
    public int ConsultationsCount { get; set; }
    public int PrescriptionsCount { get; set; }
    public int LabRequestsCount { get; set; }

    // Lists
    public List<AppointmentRow> TodayAppointments { get; set; } = new();
    public List<PatientRow> Patients { get; set; } = new();
    public List<ConsultationRow> Consultations { get; set; } = new();
    public List<PrescriptionRow> Prescriptions { get; set; } = new();
    public List<LabRequestRow> LabRequests { get; set; } = new();

    public class AppointmentRow
    {
        public int Id { get; set; }
        public string PatientName { get; set; } = string.Empty;
        public string DepartmentName { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }
        public string Status { get; set; } = string.Empty;
    }

    public class PatientRow
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }

    public class ConsultationRow
    {
        public int Id { get; set; }
        public string PatientName { get; set; } = string.Empty;
        public string Diagnosis { get; set; } = string.Empty;
        public DateTime Date { get; set; }
    }

    public class PrescriptionRow
    {
        public int Id { get; set; }
        public string PatientName { get; set; } = string.Empty;
        public string MedicineName { get; set; } = string.Empty;
        public string Dosage { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public bool Issued { get; set; }
    }

    public class LabRequestRow
    {
        public int Id { get; set; }
        public string PatientName { get; set; } = string.Empty;
        public string TestType { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }
}
