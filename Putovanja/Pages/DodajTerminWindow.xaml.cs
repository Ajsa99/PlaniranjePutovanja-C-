using System;
using System.Windows;
using System.Windows.Controls;

namespace Putovanja.Pages
{
    public partial class DodajTerminWindow : Window
    {
        private readonly int idPutovanja;

        public DodajTerminWindow(int idPutovanja)
        {
            InitializeComponent();
            this.idPutovanja = idPutovanja;
        }

        private void Dodaj_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Validacija unosa
                string naziv = txtNaziv.Text.Trim();
                string opis = txtOpis.Text.Trim();
                DateTime datumPolaska = dpDatumPolaska.SelectedDate ?? DateTime.MinValue;
                string vremePolaska = txtVremePolaska.Text.Trim();
                DateTime datumPovratka = dpDatumPovratka.SelectedDate ?? DateTime.MinValue;
                string vremePovratka = txtVremePovratka.Text.Trim();

                // Provera da li su sva polja popunjena
                if (string.IsNullOrWhiteSpace(naziv) || string.IsNullOrWhiteSpace(opis) ||
                    datumPolaska == DateTime.MinValue || string.IsNullOrWhiteSpace(vremePolaska) ||
                    datumPovratka == DateTime.MinValue || string.IsNullOrWhiteSpace(vremePovratka))
                {
                    MessageBox.Show("Molimo Vas da popunite sva polja.");
                    return;
                }

                // Kreiranje novog termina
                Destinacija noviTermin = new Destinacija
                {
                    idPutovanja = idPutovanja,
                    Naziv = naziv,
                    Opis = opis,
                    DatumPolaska = datumPolaska,
                    DatumPovratka = datumPovratka,
                    VremeOd = vremePolaska,
                    VremeDo = vremePovratka
                };

                // Upisivanje novog termina u bazu
                string errorMessage;
                bool success = DatabaseContext.InsertDestination(noviTermin, out errorMessage);

                if (success)
                {
                    MessageBox.Show("Novi termin je uspešno dodat.");
                    DialogResult = true; // Postavi DialogResult na true da označi uspešno zatvaranje prozora
                }
                else
                {
                    MessageBox.Show("Greška prilikom dodavanja termina: " + errorMessage);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Greška prilikom dodavanja termina: " + ex.Message);
            }
        }

        private void Odustani_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false; // Postavi DialogResult na false da označi odustajanje
        }

        private void txtVremePolaska_TextChanged(object sender, RoutedEventArgs e)
        {
            // Ovde možete implementirati logiku vezanu za promenu teksta u txtVremePolaska TextBox-u
        }

        private void dpDatumPolaska_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            // Postavi minimalni datum za dpDatumPovratka na izabrani datum u dpDatumPolaska
            dpDatumPovratka.DisplayDateStart = dpDatumPolaska.SelectedDate;
        }

        private void dpDatumPovratka_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            // Opciono: Možete implementirati dodatnu logiku za promenu izabranog datuma u dpDatumPovratka
        }
    }
}
