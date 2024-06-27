using System;
using System.Windows;
using System.Windows.Controls;

namespace Putovanja.Pages
{
    public partial class Reservations : Page
    {
        private readonly int loggedInUserId;

        public Reservations()
        {
            InitializeComponent();
            loggedInUserId = MainWindow.LoggedInUserId;
            LoadReservations();
        }

        private void LoadReservations()
        {
            try
            {
                string errorMessage;
                var reservations = DatabaseContext.GetReservationsForUser(loggedInUserId, out errorMessage);

                if (reservations != null)
                {
                    ReservationsListView.ItemsSource = reservations;
                }
                else
                {
                    MessageBox.Show(errorMessage);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Greška prilikom učitavanja rezervacija: {ex.Message}");
            }
        }

        private void ViewDestination(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button != null)
            {
                var idPutovanja = (int)button.DataContext.GetType().GetProperty("IdPutovanja").GetValue(button.DataContext, null);

                TravelPage travelPage = new TravelPage(idPutovanja);
                NavigationService.Navigate(travelPage);
            }
        }
    }
}
