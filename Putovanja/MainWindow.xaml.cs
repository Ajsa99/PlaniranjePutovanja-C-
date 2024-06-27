using Putovanja.Pages;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Putovanja
{
    public partial class MainWindow : Window
    {
        public static int LoggedInUserId { get; set; }
        public static string LoggedInUserType { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Navigate(new Login());
            MainFrame.Navigated += MainFrame_Navigated;

            ConfigureNavigationBar();
        }

        private void ProfilAgency_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MainFrame.Navigate(new ProfilAgency());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Greška prilikom navigacije na ProfilAgency stranicu: " + ex.Message);
            }
        }

        private void Reservations_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Reservations());
        }

        private void TravelForm_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new TravelForm());
        }
        private void Destinations_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Destinations());
        }

        private void ConfigureNavigationBar()
        {
            if (LoggedInUserType == "Korisnik")
            {
                ProfilAgencyButton.Visibility = Visibility.Collapsed;
                DestinationsButton.Visibility = Visibility.Visible;
                ReservationsButton.Visibility = Visibility.Visible;
                TravelFormButton.Visibility = Visibility.Collapsed;
            }
            else if (LoggedInUserType == "Agencija")
            {
                ProfilAgencyButton.Visibility = Visibility.Visible;
                DestinationsButton.Visibility = Visibility.Visible;
                ReservationsButton.Visibility = Visibility.Collapsed;
                TravelFormButton.Visibility = Visibility.Visible;
            }
        }

        private void MainFrame_Navigated(object sender, NavigationEventArgs e)
        {
            if (MainFrame.Content is Register || MainFrame.Content is Login)
            {
                NavBar.Visibility = Visibility.Collapsed;
            }
            else
            {
                NavBar.Visibility = Visibility.Visible;
                ConfigureNavigationBar(); // Podesi navigacioni bar svaki put kad se naviguje na novu stranicu
            }
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            // Resetuj tip ulogovanog korisnika

            LoggedInUserType = null;

            MainFrame.Navigate(new Login());
        }

    }
}
