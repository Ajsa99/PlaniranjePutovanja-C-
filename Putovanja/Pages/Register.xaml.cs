using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Putovanja.Pages
{
    /// <summary>
    /// Interaction logic for Register.xaml
    /// </summary>
    public partial class Register : Page
    {
        public Register()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            cmbType.SelectedIndex = 0;
            cmbType_SelectionChanged(null, null);
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
        
        private void textAgencyName_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtAgencyName.Focus();
        }

        private void txtAgencyName_TextChanged(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(textAgencyName.Text) && textAgencyName.Text.Length > 0)
            {
                textAgencyName.Visibility = Visibility.Collapsed;
                errorAgencyName.Text = string.Empty;
            }
            else
            {
                textAgencyName.Visibility = Visibility.Visible;
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

        private void txtName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtName.Text) && txtName.Text.Length > 0)
            {
                textName.Visibility = Visibility.Collapsed;
                errorName.Text = string.Empty;
            }
            else
            {
                textName.Visibility = Visibility.Visible;
            }
        }

        private void textName_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtName.Focus();
        }

        private void txtSurname_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtSurname.Text) && txtSurname.Text.Length > 0)
            {
                textSurname.Visibility = Visibility.Collapsed;
                errorSurname.Text = string.Empty;
            }
            else
            {
                textSurname.Visibility = Visibility.Visible;
            }
        }

        private void textSurname_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtSurname.Focus();
        }

        private void textPassword1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtPassword1.Focus();
        }

        private void txtPassword1_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtPassword1.Password) && txtPassword1.Password.Length > 0)
            {
                textPassword1.Visibility = Visibility.Collapsed;
                errorPassword1.Text = string.Empty;
            }
            else
            {
                textPassword1.Visibility = Visibility.Visible;
            }
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            bool isValid = true;

            // Validacija polja
            if (cmbType.SelectedItem == null)
            {
                errorType.Text = "Molimo odaberite tip korisnika.";
                isValid = false;
            }
            else
            {
                string selectedType = ((ComboBoxItem)cmbType.SelectedItem).Content.ToString();
                if (selectedType == "Korisnik")
                {
                    if (string.IsNullOrEmpty(txtName.Text))
                    {
                        errorName.Text = "Ime je obavezno.";
                        isValid = false;
                    }

                    if (string.IsNullOrEmpty(txtSurname.Text))
                    {
                        errorSurname.Text = "Prezime je obavezno.";
                        isValid = false;
                    }
                }
                else if (selectedType == "Agencija")
                {
                    if (string.IsNullOrEmpty(txtAgencyName.Text))
                    {
                        errorAgencyName.Text = "Naziv agencije je obavezan.";
                        isValid = false;
                    }
                }
            }


            // Ako je sve validno
            if (isValid)
            {
                string selectedType = ((ComboBoxItem)cmbType.SelectedItem).Content.ToString();
                if (selectedType == "Korisnik")
                {
                    if (DatabaseManager.RegisterMember(txtName.Text, txtSurname.Text, txtEmail.Text, txtPassword.Password, out string errorMessage))
                    {
                        MessageBox.Show("Uspešno ste se registrovali kao korisnik.", "Registracija uspešna", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK, MessageBoxOptions.RightAlign);
                        this.NavigationService.Navigate(new Login());
                    }
                    else
                    {
                        MessageBox.Show(errorMessage);
                    }
                }
                else if (selectedType == "Agencija")
                {
                    if (DatabaseManager.RegisterAgency(txtAgencyName.Text, txtEmail.Text, txtPassword.Password, out string errorMessage))
                    {
                        MessageBox.Show("Uspešno ste registrovali agenciju.", "Registracija uspešna", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK, MessageBoxOptions.RightAlign);
                        this.NavigationService.Navigate(new Login());
                    }
                    else
                    {
                        MessageBox.Show(errorMessage);
                    }
                }
            }
        }


        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Login());
        }

        private void Image_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void cmbType_SelectionChanged(object sender, SelectionChangedEventArgs e)
{
    ComboBoxItem selectedItem = (ComboBoxItem)cmbType.SelectedItem;
    if (selectedItem != null)
    {
        string selectedType = selectedItem.Content.ToString();
        switch (selectedType)
        {
            case "Korisnik":
                // Prikazi elemente za korisnika
                borderAgency.Visibility = Visibility.Collapsed;
                errorAgencyName.Visibility = Visibility.Collapsed;
                borderName.Visibility = Visibility.Visible;
                errorName.Visibility = Visibility.Visible;
                borderSurname.Visibility = Visibility.Visible;
                errorSurname.Visibility = Visibility.Visible;
                borderEmail.Visibility = Visibility.Visible;
                borderPassword.Visibility = Visibility.Visible;
                borderRepeatPassword.Visibility = Visibility.Visible;
                break;
            case "Agencija":
                // Prikazi elemente za agenciju
                borderAgency.Visibility = Visibility.Visible;
                errorAgencyName.Visibility = Visibility.Visible;
                borderName.Visibility = Visibility.Collapsed;
                errorName.Visibility = Visibility.Collapsed;
                borderSurname.Visibility = Visibility.Collapsed;
                errorSurname.Visibility = Visibility.Collapsed;
                borderEmail.Visibility = Visibility.Visible;
                borderPassword.Visibility = Visibility.Visible;
                borderRepeatPassword.Visibility = Visibility.Visible;
                break;
        }
    }
}


    }
}
