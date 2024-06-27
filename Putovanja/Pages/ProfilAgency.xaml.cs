using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Putovanja.Pages
{
    public partial class ProfilAgency : Page
    {
        public ObservableCollection<Putovanje> Putovanja { get; set; }
        public int LoggedInUserId { get; set; }

        public ProfilAgency()
        {
            InitializeComponent();
            Putovanja = new ObservableCollection<Putovanje>();
            DataContext = this;
            LoadDataFromDatabase();
        }

        private void LoadDataFromDatabase()
        {
            try
            {
                var putovanja = DatabaseContext.GetPutovanjaId(MainWindow.LoggedInUserId).ToList();

                putovanja.Reverse();

                foreach (var putovanje in putovanja)
                {
                    Putovanja.Add(putovanje);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Greška prilikom učitavanja podataka: " + ex.Message);
            }
        }

        private void DeletePutovanje(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button != null)
            {
                Putovanje putovanje = button.DataContext as Putovanje;
                if (putovanje != null)
                {
                    try
                    {
                        MessageBoxResult result = MessageBox.Show("Da li ste sigurni da želite da obrišete ovo putovanje?", "Potvrda brisanja", MessageBoxButton.YesNo, MessageBoxImage.Question);
                        if (result == MessageBoxResult.Yes)
                        {
                            DatabaseContext.DeletePutovanje(putovanje.idPutovanja);
                            Putovanja.Remove(putovanje);
                            MessageBox.Show("Putovanje je uspešno obrisano.");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Greška prilikom brisanja putovanja: " + ex.Message);
                    }
                }
            }
        }

        private void ViewDestination(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button != null)
            {
                var putovanje = button.DataContext as Putovanje;
                if (putovanje != null)
                {
                    TravelPage1 travelPage1 = new TravelPage1(putovanje.idPutovanja);
                    NavigationService.Navigate(travelPage1);
                }
            }
        }
    }
}
