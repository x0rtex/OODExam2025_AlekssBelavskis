using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PatientApp
{
    /// <summary>
    /// Interaction logic for AppointmentWindow.xaml
    /// </summary>
    public partial class AppointmentWindow : Window
    {
        private readonly PatientData _db;
        private readonly Patient _patient;
        private readonly Appointment _appointment;
        
        public AppointmentWindow(PatientData db, Patient patient, Appointment appointment = null)
        {
            InitializeComponent();
            _db = db;
            _patient = patient;
            _appointment = appointment;
            UpdateAppointmentDetails();
        }

        // Update fields with appointment details if appointment is being editted
        private void UpdateAppointmentDetails()
        {
            if (_appointment == null)
                return;

            DtpAppointmentDate.SelectedDate = _appointment.AppointmentTime.Date;
            TmpAppointmentTime.SelectedTime = _appointment.AppointmentTime;
            TbxNotes.Text = _appointment.AppointmentNotes;
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e) => AddAppointment();

        private void AddAppointment()
        {
            DateTime appointmentDate = DtpAppointmentDate.SelectedDate.Value.Date;
            TimeSpan appointmentTime = TmpAppointmentTime.SelectedTime.Value.TimeOfDay;
            appointmentDate = new DateTime(appointmentDate.Year, appointmentDate.Month, appointmentDate.Day).Add(appointmentTime);

            Appointment appointment = new Appointment()
            {
                AppointmentTime = appointmentDate,
                AppointmentNotes = TbxNotes.Text,
                Patient = _patient
            };

            try
            {
                _db.Appointments.Add(appointment);
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Database error while adding appointment: {ex.Message}");
            }
            finally
            {
                Close();
            }
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (_appointment == null)
            {
                MessageBox.Show("No appointment selected to edit.");
                return;
            }

            EditAppointment(_appointment);
        }

        private void EditAppointment(Appointment appointment)
        {
            DateTime appointmentDate = DtpAppointmentDate.SelectedDate.Value.Date;
            TimeSpan appointmentTime = TmpAppointmentTime.SelectedTime.Value.TimeOfDay;
            appointmentDate = new DateTime(appointmentDate.Year, appointmentDate.Month, appointmentDate.Day).Add(appointmentTime);

            Appointment edittedAppointment = new Appointment()
            {
                AppointmentTime = appointmentDate,
                AppointmentNotes = TbxNotes.Text,
                Patient = _patient
            };

            try
            {
                _db.Appointments.Remove(appointment); 
                _db.Appointments.Add(edittedAppointment);
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Database error while adding appointment: {ex.Message}");
            }
            finally
            {
                Close();
            }
        }
    }
}
