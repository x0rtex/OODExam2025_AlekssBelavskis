using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Linq;
using System.Windows;

namespace PatientApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly PatientData _db = new PatientData();
        public MainWindow()
        {
            InitializeComponent();
            UpdatePatientList();
        }

        // Update patient listbox
        private void UpdatePatientList() => LbxPatients.ItemsSource = GetPatients();

        private List<Patient> GetPatients()
        {
            return _db.Patients
                .OrderBy(p => p.Surname)
                .ToList();
        }

        // Update appointments listbox when a patient is selected
        private void LbxPatients_SelectionChanged(object sender, SelectionChangedEventArgs e) => UpdateAppointmentsList();

        private void UpdateAppointmentsList() 
        {
            Patient selectedPatient = (Patient)LbxPatients.SelectedItem;

            if (selectedPatient is null)
                return;

            LbxAppointments.ItemsSource = GetAppointments(selectedPatient);
        }

        private List<Appointment> GetAppointments(Patient patient)
        {
            return _db.Appointments
                .Where(a => a.Patient.PatientID == patient.PatientID)
                .OrderBy(a => a.AppointmentTime)
                .ToList();
        }

        // Add a new patient to database
        private void BtnAddPatient_Click(object sender, RoutedEventArgs e) => AddPatient();

        private void AddPatient()
        {
            string firstName = TbxPatientFirstName.Text;
            string surname = TbxPatientSurname.Text;
            if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(surname))
            {
                MessageBox.Show("Please enter a first name and surname.");
                return;
            }

            string contactNumber = TbxPatientContactNumber.Text;
            if (contactNumber.Length < 10)
            {
                MessageBox.Show("Please enter a valid phone number.");
                return;
            }

            DateTime? dob = DtpPatientDOB.SelectedDate;
            if (dob == null)
            {
                MessageBox.Show("Please select a date of birth.");
                return;
            }

            Patient newPatient = new Patient()
            {
                FirstName = firstName,
                Surname = surname,
                DOB = (DateTime)dob,
                ContactNumber = contactNumber,
            };

            try
            {
                _db.Patients.Add(newPatient);
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Database error while adding patient: {ex.Message}");
            }
            finally
            {
                UpdatePatientList();
                TbxPatientFirstName.Clear();
                TbxPatientSurname.Clear();
                TbxPatientContactNumber.Clear();
                DtpPatientDOB.SelectedDate = null;
            }
        }

        // Add a new appointment to the selected patient
        private void BtnAddAppointment_Click(object sender, RoutedEventArgs e)
        {
            if (LbxPatients.SelectedItem is null)
            {
                MessageBox.Show("Please select a patient first.");
                return;
            }

            try
            {
                new AppointmentWindow(_db, (Patient)LbxPatients.SelectedItem).ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Something went while adding the appointment: {ex.Message}");
            }
            finally
            {
                UpdateAppointmentsList();
            }
        }

        // Edit appointment of selected patient
        private void BtnEditAppointment_Click(object sender, RoutedEventArgs e)
        {
            if (LbxPatients.SelectedItem is null)
            {
                MessageBox.Show("Please select a patient first.");
                return;
            }

            if (LbxAppointments.SelectedItem is null)
            {
                MessageBox.Show("Please select an appointment to edit.");
                return;
            }
           
            try
            {
                new AppointmentWindow(_db, (Patient)LbxPatients.SelectedItem, (Appointment)LbxAppointments.SelectedItem).ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Something went while editting the appointment: {ex.Message}");
            }
            finally
            {
                UpdateAppointmentsList();
            }
        }

        // Clear unfilled textboxes when they gain focus
        private void TbxPatientFirstName_GotFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TbxPatientFirstName.Text))
                TbxPatientFirstName.Clear();
        }

        private void TbxPatientSurname_GotFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TbxPatientSurname.Text))
                TbxPatientSurname.Clear();
        }

        private void TbxPatientContactNumber_GotFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TbxPatientContactNumber.Text))
                TbxPatientContactNumber.Clear();
        }

        // Readd placeholder text to unfilled textboxes when they lose focus
        private void TbxPatientFirstName_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TbxPatientFirstName.Text))
                TbxPatientFirstName.Text = "Enter first name";
        }

        private void TbxPatientSurname_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TbxPatientSurname.Text))
                TbxPatientSurname.Text = "Enter surname";
        }

        private void TbxPatientContactNumber_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TbxPatientContactNumber.Text))
                TbxPatientContactNumber.Text = "Enter phone number";
        }
    }
}
