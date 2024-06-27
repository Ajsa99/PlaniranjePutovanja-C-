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
                // Dobavljanje podataka iz baze preko DatabaseContext-a
                var putovanja = DatabaseContext.GetPutovanjaId(MainWindow.LoggedInUserId).ToList();  // Pretvoriti u listu da biste mogli koristiti Reverse()

                // Obrnuti redosled elemenata
                putovanja.Reverse();

                foreach (var putovanje in putovanja)
                {
                    Putovanja.Add(putovanje);
                }
            }
            catch (Exception ex)
            {
                // Prikazivanje poruke o grešci ako učitavanje podataka ne uspe
                MessageBox.Show("Greška prilikom učitavanja podataka: " + ex.Message);
            }
        }

        //private void DeletePutovanje(object sender, RoutedEventArgs e)
        //{
        //    Button button = sender as Button;
        //    if (button != null)
        //    {
        //        Putovanje putovanje = button.DataContext as Putovanje;
        //        if (putovanje != null)
        //        {
        //            try
        //            {
        //                DatabaseContext.DeletePutovanje(putovanje.idPutovanja); // Brisanje putovanja iz baze
        //                Putovanja.Remove(putovanje); // Uklanjanje putovanja iz liste na UI
        //                MessageBox.Show("Putovanje je uspešno obrisano.");
        //            }
        //            catch (Exception ex)
        //            {
        //                MessageBox.Show("Greška prilikom brisanja putovanja: " + ex.Message);
        //            }
        //        }
        //    }
        //}

        private void ViewDestination(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button != null)
            {
                // Pretpostavka da je DataContext element tipa Putovanje koji ima ID
                var putovanje = button.DataContext as Putovanje;
                if (putovanje != null)
                {
                    // Prebacivanje na novu stranicu sa prosleđenim ID-em
                    TravelPage1 travelPage1 = new TravelPage1(putovanje.idPutovanja);
                    NavigationService.Navigate(travelPage1);
                }
            }
        }
    }
}
