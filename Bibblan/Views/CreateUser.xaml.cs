using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Security.Cryptography;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Bibblan.Models;
using Bibblan.Services;
using System.Linq;

namespace Bibblan.Views
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class CreateUser : Window
    {
        public CreateUser()
        {
            InitializeComponent();
            WrongEntry += Alert;
        }

        public event EventHandler<string> WrongEntry;
        protected virtual void OnWrongEntry(string e)
        {
            WrongEntry?.Invoke(this, e);
        }
        static void Alert(object sender, string message)
        {
            MessageBox.Show($"Meddelande!\n{message}");
            return;
        }

        private void CreateUserButton(object sender, RoutedEventArgs e)
        {

            if (Firstname.Text == "" || Lastname.Text == "" || Email.Text == "" || SSN.Text == "") //Kollar om user input är tomt
            {
                OnWrongEntry("Du har inte angett data i samtliga fält!"); //test
                return;
            }

            var user = new User();

            if (DbInitialiser.Db.Users.Where(y => y.Socialsecuritynumber == Encryption.Encrypt(SSN.Text)).FirstOrDefault() != null) //kollar om angivna SSN redan existerar i DB:n
            {
                MessageBox.Show("Användaren existerar redan"); //detta ska ändras till ett event
                return;
            }

            user.Firstname = Firstname.Text;
            user.Lastname = Lastname.Text;
            user.Email = Email.Text;
            user.Permissions = 0; //Detta ska admin kunna ändra senare

            user.Socialsecuritynumber = Encryption.Encrypt(SSN.Text); //Flyttade encryption metoden till Services.Encryption.cs, så vi kan använda den överallt i programmet. 
            user.Username = Encryption.Encrypt(Username.Text);
            user.Password = Encryption.Encrypt(Password.Text);

            DbInitialiser.Db.Add(user);
            DbInitialiser.Db.SaveChanges();
        }
        private void FirstFocus(object sender, RoutedEventArgs e)
        {
            FirstnameInputFocus.Visibility = System.Windows.Visibility.Collapsed;
            Firstname.Visibility = System.Windows.Visibility.Visible;
            Firstname.Focus();
        }

        private void FirstLost(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(Firstname.Text))
            {
                FirstnameInputFocus.Visibility = System.Windows.Visibility.Collapsed;
                Firstname.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void LastFocus(object sender, RoutedEventArgs e)
        {
            LastnameInputFocus.Visibility = System.Windows.Visibility.Collapsed;
            Lastname.Visibility = System.Windows.Visibility.Visible;
            Lastname.Focus();
        }

        private void LastLost(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(LastnameInputFocus.Text))
            {
                LastnameInputFocus.Visibility = System.Windows.Visibility.Collapsed;
                Lastname.Visibility = System.Windows.Visibility.Visible;
            }
        }
        private void EmailFocus(object sender, RoutedEventArgs e)
        {
            EmailInputFocus.Visibility = System.Windows.Visibility.Collapsed;
            Email.Visibility = System.Windows.Visibility.Visible;
            Email.Focus();
        }

        private void EmailLost(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(EmailInputFocus.Text))
            {
                EmailInputFocus.Visibility = System.Windows.Visibility.Collapsed;
                Email.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void SSNFocus(object sender, RoutedEventArgs e)
        {
            SSNInputFocus.Visibility = System.Windows.Visibility.Collapsed;
            SSN.Visibility = System.Windows.Visibility.Visible;
            SSN.Focus();
        }

        private void SSNLost(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(EmailInputFocus.Text))
            {
                SSNInputFocus.Visibility = System.Windows.Visibility.Collapsed;
                SSN.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void UserFocus(object sender, RoutedEventArgs e)
        {
            UsernameInputFocus.Visibility = System.Windows.Visibility.Collapsed;
            Username.Visibility = System.Windows.Visibility.Visible;
            Username.Focus();
        }

        private void UserLost(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(Username.Text))
            {
                UsernameInputFocus.Visibility = System.Windows.Visibility.Collapsed;
                Username.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void PassFocus(object sender, RoutedEventArgs e)
        {
            PasswordInputFocus.Visibility = System.Windows.Visibility.Collapsed;
            Password.Visibility = System.Windows.Visibility.Visible;
        }

        private void PassLost(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(Password.Text))
            {
                PasswordInputFocus.Visibility = System.Windows.Visibility.Collapsed;
                Password.Visibility = System.Windows.Visibility.Visible;
                Password.Focus();
            }
        }
    }
}
