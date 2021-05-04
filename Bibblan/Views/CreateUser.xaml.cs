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
        }
        private void CreateUserButton(object sender, RoutedEventArgs e)
        {

            if (Firstname.Text == "" || Lastname.Text == "" || Email.Text == "" || SSN.Text == "") { return; }

            int check = 0;
            var user = new User();
            var encryptCheck = Encrypt(SSN.ToString());

            foreach (var item in DbInitialiser.Db.Users)
            {
                for (int i = 0; i < item.Socialsecuritynumber.Length; i++)
                {
                    if (item.Socialsecuritynumber[i] == encryptCheck[i])
                    {
                        check++;
                    }
                }
                if (check == item.Socialsecuritynumber.Length)
                {
                    Console.WriteLine("The user already exists!");
                    return;
                }
            }

            user.Firstname = Firstname.Text;
            user.Lastname = Lastname.Text;
            user.Email = Email.Text;
            user.Permissions = 0;

            user.Socialsecuritynumber = Encrypt(SSN.Text);
            user.Username = Encrypt(Username.Text);
            user.Password = Encrypt(Password.Text);

            DbInitialiser.Db.Add(user);
            DbInitialiser.Db.SaveChanges();
        }


        public byte[] Encrypt(string toEncrypt)
        {

            byte[] data = Encoding.UTF8.GetBytes(toEncrypt); //Gör om string till bytearray

            byte[] encryptedArray = SHA256.Create().ComputeHash(data); //Krypterar bytearray

            return encryptedArray;
        }

            // Lägg till knappar och kod här!





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
