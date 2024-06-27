using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Collections.Specialized.BitVector32;

namespace Putovanja.Pages
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Page
    {
        public Login()
        {
            InitializeComponent();
        }

        private void textEmail_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtEmail.Focus();

        }

        private void txtemail_textchanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtEmail.Text) && txtEmail.Text.Length > 0)
            {
                textEmail.Visibility = Visibility.Collapsed;
                errorEmail.Text = string.Empty;
            }
            else
            {
                textEmail.Visibility = Visibility.Visible;
            }
        }
        private void textPassword_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtPassword.Focus();
        }

        private void txtPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtPassword.Password) && txtPassword.Password.Length > 0)
            {
                textPassword.Visibility = Visibility.Collapsed;
                errorPassword.Text = string.Empty;
            }
            else
            {
                textPassword.Visibility = Visibility.Visible;
            }
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            bool isValid = true;
            // Provera da li su polja za unos popunjena
            if (string.IsNullOrEmpty(txtEmail.Text) || !txtEmail.Text.Contains("@") ||
                !(txtEmail.Text.EndsWith("@gmail.com") || txtEmail.Text.EndsWith("@hotmail.com")))
            {
                errorEmail.Text = "Unesite validan email.";
                isValid = false;
            }

            if (string.IsNullOrEmpty(txtPassword.Password))
            {
                errorPassword.Text = "Lozinka je obavezna.";
                isValid = false;
            }

            string email = txtEmail.Text;
            string password = txtPassword.Password;

            if (isValid)
            {
                // Provera logovanja korisnika
                if (DatabaseContext.AuthenticateUser(email, password, out string userType, out int userId))
                {
                    // Postavljanje tipa ulogovanog korisnika
                    MainWindow.LoggedInUserType = userType;
                    MainWindow.LoggedInUserId = userId;

                    if (userType == "Korisnik")
                    {
                        NavigationService.Navigate(new Destinations());
                    }
                    else if (userType == "Agencija")
                    {
                        NavigationService.Navigate(new ProfilAgency());
                    }

                }
                else
                {
                    MessageBox.Show("Pogresan Email ili lozinka.", "Prijava neuspesna", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK, MessageBoxOptions.RightAlign);
                }
            }
        }
        private void Image_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Register());
        }
    }
}
