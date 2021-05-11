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
using System.Windows.Navigation;
using Bibblan.Models;
using Bibblan.Services;
using Bibblan.Views;
using System.Linq;
using System.Threading; 

namespace Bibblan.Views
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class CreateUser : Page
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
            MessageBox.Show($"{message}", "Meddelande", MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }

        private void CreateUserButton(object sender, RoutedEventArgs e)
        {
            CreateUserButton1.Opacity = 0.5;
            Thread.Sleep(500);
            CreateUserButton1.Opacity = 1; 

            if (firstName.Text == "" || lastName.Text == "" || eMail.Text == "" || SSN.Text == "") //Kollar om user input är tomt
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

            user.Firstname = firstName.Text;
            user.Lastname = lastName.Text;
            user.Email = eMail.Text;
            user.Permissions = 0; //Detta ska admin kunna ändra senare

            user.Socialsecuritynumber = Encryption.Encrypt(SSN.Text); //Flyttade encryption metoden till Services.Encryption.cs, så vi kan använda den överallt i programmet. 
            user.Username = Encryption.Encrypt(userName.Text);
            user.Password = Encryption.Encrypt(passWord.Password);

            DbInitialiser.Db.Add(user);
            DbInitialiser.Db.SaveChanges();
            MessageBox.Show("Du har nu skapat upp en användare."); 

        }


        private void menuButtonClick(object sender, RoutedEventArgs e) //Navigering till Adminsida
        {
            NavigationService nav = NavigationService.GetNavigationService(this);
            nav.Navigate(new AdminPage());
        }    


        //Nedan är 'focus' för Grå hints inom Textboxen
        private void FirstFocus(object sender, RoutedEventArgs e)
        {
            firstNameInputFocus.Visibility = System.Windows.Visibility.Collapsed;
            firstName.Visibility = System.Windows.Visibility.Visible;
            firstName.Focus();
        }
        private void FirstLost(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(firstName.Text))
            {
                firstNameInputFocus.Visibility = System.Windows.Visibility.Collapsed;
                firstName.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void LastFocus(object sender, RoutedEventArgs e)
        {
            lastNameInputFocus.Visibility = System.Windows.Visibility.Collapsed;
            lastName.Visibility = System.Windows.Visibility.Visible;
            lastName.Focus();
        }
        private void LastLost(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(lastNameInputFocus.Text))
            {
                lastNameInputFocus.Visibility = System.Windows.Visibility.Collapsed;
                lastName.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void EmailFocus(object sender, RoutedEventArgs e)
        {
            emailInputFocus.Visibility = System.Windows.Visibility.Collapsed;
            eMail.Visibility = System.Windows.Visibility.Visible;
            eMail.Focus();
        }
        private void EmailLost(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(emailInputFocus.Text))
            {
                emailInputFocus.Visibility = System.Windows.Visibility.Collapsed;
                eMail.Visibility = System.Windows.Visibility.Visible;
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
            if (string.IsNullOrEmpty(SSNInputFocus.Text))
            {
                SSNInputFocus.Visibility = System.Windows.Visibility.Collapsed;
                SSN.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void UserFocus(object sender, RoutedEventArgs e)
        {
            userNameInputFocus.Visibility = System.Windows.Visibility.Collapsed;
            userName.Visibility = System.Windows.Visibility.Visible;
            userName.Focus();
        }
        private void UserLost(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(userName.Text))
            {
                userNameInputFocus.Visibility = System.Windows.Visibility.Collapsed;
                userName.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void PassFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(passWord.Password))
            {
                passwordInputFocus.Visibility = System.Windows.Visibility.Collapsed;
                passWord.Visibility = System.Windows.Visibility.Visible;
                passWord.Focus();
            }
        }

        private void PassLost(object sender, RoutedEventArgs e)
        {
            passwordInputFocus.Visibility = System.Windows.Visibility.Collapsed;
            passWord.Visibility = System.Windows.Visibility.Visible;
        }
    }
}
