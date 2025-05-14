using System;
using System.Collections.Generic;

namespace PatientApp
{
    public class Patient
    {
        public int PatientID { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public DateTime DOB { get; set; }
        public string ContactNumber { get; set; }

        public virtual List<Appointment> Appointments { get; set; }

        public override string ToString()
        {
            return $"{Surname}, {FirstName} - {ContactNumber}";
        }
    }
}
