using System;
using System.Windows;
using System.Windows.Controls;

namespace Putovanja.Pages
{
    public partial class EditTravel : Page
    {
        private readonly int idPutovanja;
        private Putovanje originalPutovanje;

        public EditTravel(int id)
        {
            InitializeComponent();
            idPutovanja = id;
            LoadOriginalTravel();
        }

        private void LoadOriginalTravel()
        {
            try
            {
                originalPutovanje = DatabaseManager.GetPutovanjeById(idPutovanja);
                if (originalPutovanje != null)
                {
                    DataContext = originalPutovanje;
                }
                else
                {
                    MessageBox.Show("Putovanje nije pronađeno.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Greška prilikom učitavanja podataka o putovanju: " + ex.Message);
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bool success = DatabaseManager.UpdatePutovanje(originalPutovanje, out string errorMessage);
                if (success)
                {
                    MessageBox.Show("Podaci o putovanju su uspešno izmenjeni.");
                    NavigationService.Navigate(new TravelPage1(idPutovanja));
                }
                else
                {
                    MessageBox.Show(errorMessage);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Greška prilikom čuvanja izmena: " + ex.Message);
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack(); // Povratak na prethodnu stranicu
        }

        private void txtStatus_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                TextBox textBox = sender as TextBox;
                if (textBox != null)
                {
                    if (DataContext is Putovanje putovanje)
                    {
                        putovanje.Status = textBox.Text;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Greška prilikom ažuriranja statusa: {ex.Message}");
            }
        }
    }
}
