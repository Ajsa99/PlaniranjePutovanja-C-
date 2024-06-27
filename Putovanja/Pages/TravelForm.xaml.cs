using Microsoft.Win32;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Putovanja.Pages
{
    public partial class TravelForm : Page
    {
        public TravelForm()
        {
            InitializeComponent();
            PopulateTimeComboBoxes();
        }

        private void SelectImage_Click(object sender, RoutedEventArgs e)
        {
            // Open file dialog to select an image file
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files (*.jpg, *.png, *.jpeg)|*.jpg;*.png;*.jpeg|All files (*.*)|*.*";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);

            if (openFileDialog.ShowDialog() == true)
            {
                // Get the selected image file path
                string selectedImagePath = openFileDialog.FileName;
                txtImagePath.Text = selectedImagePath;

                // Display the selected image in the Image control
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(selectedImagePath);
                bitmap.EndInit();
                imgTravel.Source = bitmap;
            }
        }


        // Metoda za pretvaranje slike u byte[]
        private byte[] ConvertImageToByteArray(BitmapImage bitmapImage)
        {
            byte[] data;
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bitmapImage));

            using (MemoryStream ms = new MemoryStream())
            {
                encoder.Save(ms);
                data = ms.ToArray();
            }

            return data;
        }


        private void PopulateTimeComboBoxes()
        {
            // Primer popunjavanja ComboBox-a sa vremenima (možete prilagoditi svojim potrebama)
            for (int hour = 0; hour <= 23; hour++)
            {
                for (int minute = 0; minute <= 59; minute += 15)
                {
                    string timeString = $"{hour:D2}:{minute:D2}";
                    TimeTo.Items.Add(timeString);
                    TimeFrom.Items.Add(timeString);
                }
            }
        }

        private void TimeToComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Obrada izbora vremena
            string selectedTime = TimeTo.SelectedItem as string;
            if (!string.IsNullOrEmpty(selectedTime))
            {
                // Možete parsirati izabrano vreme i koristiti ga po potrebi
                TimeSpan selectedTimeSpan;
                if (TimeSpan.TryParse(selectedTime, out selectedTimeSpan))
                {
                    // Ovde možete upotrebiti selectedTimeSpan
                }
            }
        }

        private void TimeFromComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Obrada izbora vremena
            string selectedTime = TimeFrom.SelectedItem as string;
            if (!string.IsNullOrEmpty(selectedTime))
            {
                // Možete parsirati izabrano vreme i koristiti ga po potrebi
                TimeSpan selectedTimeSpan;
                if (TimeSpan.TryParse(selectedTime, out selectedTimeSpan))
                {
                    // Ovde možete upotrebiti selectedTimeSpan
                }
            }
        }

        private void textNameTravel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtNameTravel.Focus();
        }

        private void txtNameTravel_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtNameTravel.Text) && txtNameTravel.Text.Length > 0)
            {
                textNameTravel.Visibility = Visibility.Collapsed;
                errorNameTravel.Text = string.Empty; // Ukloni grešku
            }
            else
            {
                textNameTravel.Visibility = Visibility.Visible;
            }
        }

        private void textDestinationsTravel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtDestinationsTravel.Focus();
        }

        private void txtDestinationsTravel_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtDestinationsTravel.Text) && txtDestinationsTravel.Text.Length > 0)
            {
                textDestinationsTravel.Visibility = Visibility.Collapsed;
                errorDestinationsTravel.Text = string.Empty; // Ukloni grešku
            }
            else
            {
                textDestinationsTravel.Visibility = Visibility.Visible;
            }
        }

        private void textHotelNameTravel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtHotelNameTravel.Focus();
        }

        private void txtHotelNameTravel_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtHotelNameTravel.Text) && txtHotelNameTravel.Text.Length > 0)
            {
                textHotelNameTravel.Visibility = Visibility.Collapsed;
                errorHotelNameTravel.Text = string.Empty; // Ukloni grešku
            }
            else
            {
                textHotelNameTravel.Visibility = Visibility.Visible;
            }
        }

        private void textDescriptionTravel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtDescriptionTravel.Focus();
        }

        private void txtDescriptionTravel_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtDescriptionTravel.Text) && txtDescriptionTravel.Text.Length > 0)
            {
                textDescriptionTravel.Visibility = Visibility.Collapsed;
                errorDescriptionTravel.Text = string.Empty; // Ukloni grešku
            }
            else
            {
                textDescriptionTravel.Visibility = Visibility.Visible;
            }
        }

        private void textNumberTravel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtNumberTravel.Focus();
        }

        private void txtNumberTravel_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtNumberTravel.Text) && txtNumberTravel.Text.Length > 0)
            {
                textNumberTravel.Visibility = Visibility.Collapsed;
                errorNumberTravel.Text = string.Empty; // Clear error message
            }
            else
            {
                textNumberTravel.Visibility = Visibility.Visible;
            }
        }

        private void textPriceTravel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtPriceTravel.Focus();
        }

        private void txtPriceTravel_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtPriceTravel.Text) && txtPriceTravel.Text.Length > 0)
            {
                textPriceTravel.Visibility = Visibility.Collapsed;
                errorPriceTravel.Text = string.Empty; // Ukloni grešku
            }
            else
            {
                textPriceTravel.Visibility = Visibility.Visible;
            }
        }

        private void TimeFrom_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Provera izabrane stavke u ComboBox-u
            if (TimeFrom.SelectedItem != null)
            {
                // Parsiranje izabrane stavke u TimeSpan
                TimeSpan selectedTime;
                if (TimeSpan.TryParse((string)TimeFrom.SelectedItem, out selectedTime))
                {
                    // Postavljanje izabranog vremena
                    // Možete ga dodatno upotrebiti prema potrebi
                }
            }
        }

        private void TimeTo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Provera izabrane stavke u ComboBox-u
            if (TimeTo.SelectedItem != null)
            {
                // Parsiranje izabrane stavke u TimeSpan
                TimeSpan selectedTime;
                if (TimeSpan.TryParse((string)TimeTo.SelectedItem, out selectedTime))
                {
                    // Postavljanje izabranog vremena
                    // Možete ga dodatno upotrebiti prema potrebi
                }
            }
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {

            // Validacija svih polja pre upisa u bazu
            SetErrorMessages();

            // Provera da li postoje error poruke
            if (string.IsNullOrEmpty(errorNameTravel.Text) &&
                string.IsNullOrEmpty(errorDestinationsTravel.Text) &&
                string.IsNullOrEmpty(errorHotelNameTravel.Text) &&
                string.IsNullOrEmpty(errorDescriptionTravel.Text) &&
                string.IsNullOrEmpty(errorDateFromTravel.Text) &&
                string.IsNullOrEmpty(errorTimeFromTravel.Text) &&
                string.IsNullOrEmpty(errorDateToTravel.Text) &&
                string.IsNullOrEmpty(errorTimeToTravel.Text) &&
                string.IsNullOrEmpty(errorNumberTravel.Text) &&
                string.IsNullOrEmpty(errorPriceTravel.Text) &&
                string.IsNullOrEmpty(errorType.Text))
            {
                // Kreiranje instance Putovanje
                Putovanje putovanje = new Putovanje
                {
                    IdAgencija = MainWindow.LoggedInUserId,
                    Naziv = txtNameTravel.Text,
                    Destinacija = txtDestinationsTravel.Text,
                    Hotel = txtHotelNameTravel.Text,
                    Opis = txtDescriptionTravel.Text,
                    BrojPutnika = int.Parse(txtNumberTravel.Text),
                    Cena = decimal.Parse(txtPriceTravel.Text),
                    Status = ((ComboBoxItem)cmbType.SelectedItem).Content.ToString(),
                    Slika = ConvertImageToByteArray(imgTravel.Source as BitmapImage)
                };

                // Postavljanje datuma i vremena
                if (DateFrom.SelectedDate.HasValue)
                {
                    putovanje.DatumPolaska = DateFrom.SelectedDate.Value.Date;
                }

                if (TimeFrom.SelectedItem != null)
                {
                    putovanje.VremeOd = TimeFrom.SelectedItem.ToString();
                }

                if (DateTo.SelectedDate.HasValue)
                {
                    putovanje.DatumPovratka = DateTo.SelectedDate.Value.Date;
                }

                if (TimeTo.SelectedItem != null)
                {
                    putovanje.VremeDo = TimeTo.SelectedItem.ToString();
                }

                // Poziv metode za upis putovanja u bazu
                string errorMessage;
                bool success = DatabaseContext.InsertTravel(putovanje, out errorMessage);

                if (success)
                {
                    MessageBox.Show("Podaci uspešno upisani u bazu.", "Uspeh", MessageBoxButton.OK, MessageBoxImage.Information);
                    NavigationService.Navigate(new ProfilAgency());
                }
                else
                {
                    MessageBox.Show("Greška prilikom upisa u bazu: " + errorMessage, "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            // Clear all fields and error messages
            txtNameTravel.Text = string.Empty;
            txtDestinationsTravel.Text = string.Empty;
            txtHotelNameTravel.Text = string.Empty;
            txtDescriptionTravel.Text = string.Empty;
            txtNumberTravel.Text = string.Empty;
            txtPriceTravel.Text = string.Empty;
            DateFrom.SelectedDate = null;
            TimeFrom.SelectedItem = null;
            DateTo.SelectedDate = null;
            TimeTo.SelectedItem = null;
            cmbType.SelectedItem = null;

            // Clear all error messages
            errorNameTravel.Text = string.Empty;
            errorDestinationsTravel.Text = string.Empty;
            errorHotelNameTravel.Text = string.Empty;
            errorDescriptionTravel.Text = string.Empty;
            errorNumberTravel.Text = string.Empty;
            errorPriceTravel.Text = string.Empty;
            errorDateFromTravel.Text = string.Empty;
            errorTimeFromTravel.Text = string.Empty;
            errorDateToTravel.Text = string.Empty;
            errorTimeToTravel.Text = string.Empty;
            errorType.Text = string.Empty;
        }

        private void SetErrorMessages()
        {
            // Validate image selection
            if (imgTravel.Source == null || !(imgTravel.Source is BitmapImage))
            {
                MessageBox.Show("Morate izabrati sliku putovanja!", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            errorNameTravel.Text = string.IsNullOrEmpty(txtNameTravel.Text) ? "Ime je obavezno polje!" : string.Empty;
            errorDestinationsTravel.Text = string.IsNullOrEmpty(txtDestinationsTravel.Text) ? "Destinacija je obavezno polje!" : string.Empty;
            errorHotelNameTravel.Text = string.IsNullOrEmpty(txtHotelNameTravel.Text) ? "Naziv hotela je obavezno polje!" : string.Empty;
            errorDescriptionTravel.Text = string.IsNullOrEmpty(txtDescriptionTravel.Text) ? "Opis je obavezno polje!" : string.Empty;
            errorPriceTravel.Text = string.IsNullOrEmpty(txtPriceTravel.Text) ? "Cena je obavezno polje!." : string.Empty;
            errorDateFromTravel.Text = !DateFrom.SelectedDate.HasValue ? "Datum polaska je obavezno polje!" : string.Empty;
            errorTimeFromTravel.Text = TimeFrom.SelectedItem == null ? "Vreme polaska je obavezno polje!" : string.Empty;
            errorDateToTravel.Text = !DateTo.SelectedDate.HasValue ? "Datum povratka je obavezno polje!" : string.Empty;
            errorTimeToTravel.Text = TimeTo.SelectedItem == null ? "Vreme povratka je obavezno polje!" : string.Empty;
            errorType.Text = cmbType.SelectedItem == null ? "Status je obavezno polje!" : string.Empty;


            // Validate date and time consistency
            if (DateFrom.SelectedDate.HasValue && DateTo.SelectedDate.HasValue && DateFrom.SelectedDate > DateTo.SelectedDate)
            {
                errorDateFromTravel.Text = "Datum polaska mora biti pre datuma povratka!";
            }

            // Validate price format
            if (string.IsNullOrEmpty(txtPriceTravel.Text))
            {
                errorPriceTravel.Text = "Cena je obavezno polje!";
            }
            else if (!decimal.TryParse(txtPriceTravel.Text, out _))
            {
                errorPriceTravel.Text = "Cena mora da bude validan broj!";
            }
            else
            {
                errorPriceTravel.Text = string.Empty;
            }

            // Validate number format
            if (string.IsNullOrEmpty(txtNumberTravel.Text))
            {
                errorNumberTravel.Text = "Broj putnika je obavezno polje!";
            }
            else if (!decimal.TryParse(txtNumberTravel.Text, out _))
            {
                errorNumberTravel.Text = "Broj putnika mora da bude validan broj!";
            }
            else
            {
                errorNumberTravel.Text = string.Empty;
            }
        }

        private void cmbType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

    }
}
