using System;
using System.Collections.ObjectModel;
using System.Data.Entity;
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
                var putovanja = DatabaseManager.GetPutovanjaId(MainWindow.LoggedInUserId).ToList();

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
                            using (var context = new PlaniranjePutovanjaEntities7())
                            {
                                // Učitaj putovanje sa povezanim entitetima koristeći query syntax
                                var putovanjeToDelete = (from p in context.Putovanje
                                                         where p.idPutovanja == putovanje.idPutovanja
                                                         select p)
                                                        .Include(p => p.Destinacija1)
                                                        .Include(p => p.Rezervacija)
                                                        .SingleOrDefault();

                                if (putovanjeToDelete != null)
                                {
                                    // Prvo obriši povezane destinacije
                                    context.Destinacija.RemoveRange(putovanjeToDelete.Destinacija1);

                                    // Zatim obriši povezane rezervacije
                                    context.Rezervacija.RemoveRange(putovanjeToDelete.Rezervacija);

                                    // Na kraju obriši putovanje
                                    context.Putovanje.Remove(putovanjeToDelete);

                                    context.SaveChanges();
                                }
                            }

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
