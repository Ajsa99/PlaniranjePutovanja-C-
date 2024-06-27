using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Putovanja.Pages
{
    public partial class Destinations : Page
    {
        public ObservableCollection<Putovanje> Putovanja { get; set; }
        public int LoggedInUserId { get; set; }

        private List<Putovanje> SvaPutovanja { get; set; }

        public Destinations()
        {
            InitializeComponent();
            Putovanja = new ObservableCollection<Putovanje>();
            SvaPutovanja = new List<Putovanje>();
            DataContext = this;
            LoadDataFromDatabase();
        }

        private void LoadDataFromDatabase()
        {
            try
            {
                // Dobavljanje podataka iz baze preko DatabaseContext-a
                var putovanja = DatabaseContext.GetPutovanja().ToList();  // Pretvoriti u listu da biste mogli koristiti Reverse()

                // Obrnuti redosled elemenata
                putovanja.Reverse();

                foreach (var putovanje in putovanja)
                {
                    Putovanja.Add(putovanje);
                    SvaPutovanja.Add(putovanje);
                }
            }
            catch (Exception ex)
            {
                // Prikazivanje poruke o grešci ako učitavanje podataka ne uspe
                MessageBox.Show("Greška prilikom učitavanja podataka: " + ex.Message);
            }
        }

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
                    TravelPage travelPage = new TravelPage(putovanje.idPutovanja);
                    NavigationService.Navigate(travelPage);
                }
            }
        }

        private void SearchByNaziv_Click(object sender, RoutedEventArgs e)
        {
            string naziv = txtSearchNaziv.Text.ToLower();
            var rezultati = SvaPutovanja.Where(p => p.Naziv.ToLower().Contains(naziv)).ToList();
            UpdateListView(rezultati);
        }

        private void SearchByDestinacija_Click(object sender, RoutedEventArgs e)
        {
            string destinacija = txtSearchDestinacija.Text.ToLower();
            var rezultati = SvaPutovanja.Where(p => p.Destinacija.ToLower().Contains(destinacija)).ToList();
            UpdateListView(rezultati);
        }

        private void SearchByDatumRange_Click(object sender, RoutedEventArgs e)
        {
            if (dpSearchDatumOd.SelectedDate.HasValue && dpSearchDatumDo.SelectedDate.HasValue)
            {
                DateTime datumOd = dpSearchDatumOd.SelectedDate.Value.Date;
                DateTime datumDo = dpSearchDatumDo.SelectedDate.Value.Date;

                var rezultati = SvaPutovanja.Where(p => p.DatumPolaska >= datumOd && p.DatumPovratka <= datumDo).ToList();
                UpdateListView(rezultati);
            }
            else
            {
                MessageBox.Show("Molimo odaberite oba datuma.");
            }
        }


        private void UpdateListView(List<Putovanje> rezultati)
        {
            Putovanja.Clear();
            foreach (var putovanje in rezultati)
            {
                Putovanja.Add(putovanje);
            }
        }

        private void SearchType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbSearchType.SelectedItem != null)
            {
                var selectedItem = (ComboBoxItem)cbSearchType.SelectedItem;
                string searchType = selectedItem.Content.ToString();

                spSearchNaziv.Visibility = Visibility.Collapsed;
                spSearchDestinacija.Visibility = Visibility.Collapsed;
                spSearchDatum.Visibility = Visibility.Collapsed;

                if (searchType == "Sve")
                {
                    spSearchNaziv.Visibility = Visibility.Collapsed;
                    spSearchDestinacija.Visibility = Visibility.Collapsed;
                    spSearchDatum.Visibility = Visibility.Collapsed;
                    UpdateListView(SvaPutovanja); // Prikazi sva putovanja
                }
                if (searchType == "Naziv")
                {
                    spSearchNaziv.Visibility = Visibility.Visible;
                }
                else if (searchType == "Destinacija")
                {
                    spSearchDestinacija.Visibility = Visibility.Visible;
                }
                else if (searchType == "Datum")
                {
                    spSearchDatum.Visibility = Visibility.Visible;
                }
            }
        }
    }
}
