using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Putovanja.Pages
{
    public partial class TravelPage1 : Page
    {
        public ObservableCollection<Putovanje> Putovanja { get; set; }
        public ObservableCollection<Destinacija> Destinacije { get; set; }

        private readonly int idPutovanja;

        public TravelPage1(int Id)
        {
            InitializeComponent();
            Putovanja = new ObservableCollection<Putovanje>();
            Destinacije = new ObservableCollection<Destinacija>(); // Inicijalizovali smo Destinacije
            idPutovanja = Id;
            LoadTravelDetails(Id);
            LoadTermine(Id);
        }

        private void LoadTravelDetails(int idPutovanja)
        {
            try
            {
                // Dohvatanje putovanja iz baze na osnovu IdPutovanja
                using (var context = new PlaniranjePutovanjaEntities7())
                {
                    var putovanje = context.Putovanje.FirstOrDefault(p => p.idPutovanja == idPutovanja);
                    if (putovanje != null)
                    {
                        // Postavljanje DataContext-a da bi se podaci vezali za XAML kontrolama
                        DataContext = new { Travel = putovanje };
                    }
                    else
                    {
                        // Ako putovanje nije pronađeno, možete prikazati odgovarajuću poruku ili obaveštenje
                        // Možete postaviti DataContext na null ili na objekat sa podrazumevanim vrednostima
                        DataContext = null;
                    }
                }
            }
            catch (Exception ex)
            {
                // Možete obraditi grešku, na primer, prikazom MessageBox-a ili zapisivanjem u log
                MessageBox.Show("Greška prilikom učitavanja detalja putovanja: " + ex.Message);
            }
        }

        private void LoadTermine(int idPutovanja)
        {
            try
            {
                using (var context = new PlaniranjePutovanjaEntities7())
                {
                    // Dohvati termine za određeno putovanje iz baze
                    Destinacije = new ObservableCollection<Destinacija>(
                        context.Destinacija.Where(d => d.idPutovanja == idPutovanja).ToList()
                    );

                    // Ponovno postavljanje ItemsSource na DataGrid
                    dgTermini.ItemsSource = Destinacije;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Greška prilikom učitavanja termina: " + ex.Message);
            }
        }


        private void EditTravel_Click(object sender, RoutedEventArgs e)
        {
            // Pretpostavka da postoji stranica za izmenu putovanja, nazvana EditTravelPage
            // Možete proslediti idPutovanja ili trenutni objekat putovanja toj stranici

            EditTravel editTravel = new EditTravel(idPutovanja); // Prosleđujemo id putovanja
            NavigationService.Navigate(editTravel);
        }

        private void DodajTermin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Kreiraj novi prozor za unos termina
                DodajTerminWindow dodajTerminWindow = new DodajTerminWindow(idPutovanja);

                // Prikaz novog prozora kao dijaloga (modalno)
                bool? result = dodajTerminWindow.ShowDialog();

                // Provera rezultata nakon zatvaranja prozora (ako je potrebno)
                if (result == true)
                {
                    // Opciono: osveži ili reaguj na promene nakon unosa termina
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Greška prilikom otvaranja prozora za dodavanje termina: {ex.Message}");
            }
        }

        private void DeleteTermin_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button != null)
            {
                Destinacija destinacija = button.DataContext as Destinacija;
                if (destinacija != null)
                {
                    try
                    {
                        // Brisanje destinacije iz baze
                        using (var context = new PlaniranjePutovanjaEntities7())
                        {
                            var terminToDelete = context.Destinacija.Find(destinacija.idDestinacija);
                            if (terminToDelete != null)
                            {
                                context.Destinacija.Remove(terminToDelete);
                                context.SaveChanges();
                            }
                        }

                        // Uklanjanje destinacije iz ObservableCollection
                        Destinacije.Remove(destinacija);
                        MessageBox.Show("Termin je uspešno obrisan.");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Greška prilikom brisanja termina: " + ex.Message);
                    }
                }
            }
        }
        private void PogledajPutnike_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Otvaranje novog prozora ili dijaloga za prikaz putnika koji su rezervisali
                PutnikeWindow prikaziPutnikeWindow = new PutnikeWindow(idPutovanja);
                prikaziPutnikeWindow.ShowDialog(); // Prikaz kao modalni dijalog

                // Opciono: reagovanje na promene nakon zatvaranja prozora (ako je potrebno)
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Greška prilikom otvaranja prozora za prikaz putnika: {ex.Message}");
            }
        }


    }

}
