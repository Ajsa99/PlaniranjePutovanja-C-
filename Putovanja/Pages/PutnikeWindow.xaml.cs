using System;
using System.Collections.ObjectModel;
using System.Windows;

namespace Putovanja.Pages
{
    public partial class PutnikeWindow : Window
    {
        private int idPutovanja;

        public PutnikeWindow(int idPutovanja)
        {
            InitializeComponent();
            this.idPutovanja = idPutovanja;
            LoadPutnici(idPutovanja);
        }

        private void LoadPutnici(int idPutovanja)
        {
            try
            {
                string errorMessage;
                var putnici = DatabaseManager.GetPutniciByPutovanjeId(idPutovanja, out errorMessage);

                if (putnici != null)
                {
                    // Postavi listu putnika kao ItemsSource za DataGrid
                    dgPutnici.ItemsSource = new ObservableCollection<dynamic>(putnici);
                }
                else
                {
                    MessageBox.Show(errorMessage);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Greška prilikom učitavanja putnika: {ex.Message}");
            }
        }

        private void Zatvori_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
