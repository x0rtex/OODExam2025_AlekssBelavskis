using System.Data.Entity;

namespace PatientApp
{
    public class PatientData : DbContext
    {
        public PatientData() : base("OODExam_AlekssBelavskis") { }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
    }
}
