using System;

namespace PatientApp
{
    public class Appointment
    {
        public int AppointmentID { get; set; }
        public DateTime AppointmentTime { get; set; }
        public string AppointmentNotes { get; set; }

        public virtual Patient Patient { get; set; }

        public override string ToString()
        {
            return $"Date: {AppointmentTime.ToShortDateString()} - Time: {AppointmentTime.ToShortTimeString()}";
        }
    }
}
