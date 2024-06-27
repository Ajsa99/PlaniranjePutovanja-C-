using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace Putovanja.Pages
{
    public partial class TravelPage : Page
    {
        private readonly int idPutovanja;
        public ObservableCollection<Destinacija> Destinacije { get; set; }

        public TravelPage(int id)
        {
            InitializeComponent();
            idPutovanja = id;
            LoadTravelDetails(id);
            LoadDestinations(id);

            // Provera tipa korisnika i podešavanje UI-a prema tome
            if (MainWindow.LoggedInUserType == "Korisnik")
            {
                // Prikaži dugme "Rezerviši" za korisnike i obradi status rezervacije
                btnRezervisi.Visibility = Visibility.Visible;

                try
                {
                    string errorMessage;
                    var putovanje = DatabaseContext.GetTravelDetails(idPutovanja, out errorMessage);

                    if (putovanje != null)
                    {
                        bool isReserved = DatabaseContext.CheckReservation(MainWindow.LoggedInUserId, idPutovanja);

                        DataContext = new { Travel = putovanje };

                        if (isReserved)
                        {
                            btnRezervisi.Visibility = Visibility.Collapsed;
                            tbRezervisano.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            btnRezervisi.Visibility = Visibility.Visible;
                            tbRezervisano.Visibility = Visibility.Collapsed;
                        }
                    }
                    else
                    {
                        DataContext = null;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Greška prilikom učitavanja detalja putovanja: {ex.Message}");
                }
            }
            else if (MainWindow.LoggedInUserType == "Agencija")
            {
                // Sakrij sve elemente za agencije
                btnRezervisi.Visibility = Visibility.Collapsed;
                tbRezervisano.Visibility = Visibility.Collapsed;
            }
        }

        private void LoadTravelDetails(int idPutovanja)
        {
            try
            {
                string errorMessage;
                var putovanje = DatabaseContext.GetTravelDetails(idPutovanja, out errorMessage);

                if (putovanje != null)
                {
                    DataContext = new { Travel = putovanje };
                }
                else
                {
                    DataContext = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Greška prilikom učitavanja detalja putovanja: {ex.Message}");
            }
        }

        private void LoadDestinations(int idPutovanja)
        {
            try
            {
                string errorMessage;
                var destinacije = DatabaseContext.GetDestinationsForTravel(idPutovanja, out errorMessage);

                if (destinacije != null)
                {
                    Destinacije = new ObservableCollection<Destinacija>(destinacije);
                    dgTermini.ItemsSource = Destinacije;
                }
                else
                {
                    MessageBox.Show("Greška prilikom učitavanja destinacija.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Greška prilikom učitavanja destinacija: {ex.Message}");
            }
        }

        private void Rezervisi_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string errorMessage;
                MessageBoxResult result = MessageBox.Show("Da li ste sigurni da želite da rezervišete ovo putovanje?", "Potvrda rezervacije", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    bool success = DatabaseContext.InsertReservation(MainWindow.LoggedInUserId, idPutovanja, out errorMessage);
                    if (success)
                    {
                        LoadTravelDetails(idPutovanja);
                        MessageBox.Show("Uspešno rezervisano putovanje!");
                        NavigationService.Navigate(new Reservations());
                    }
                    else
                    {
                        MessageBox.Show("Greška prilikom rezervacije: " + errorMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Greška prilikom rezervacije: " + ex.Message);
            }
        }

    }
}
