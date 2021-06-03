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
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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
         
        }
        public event EventHandler<string> WrongEntry;
        protected virtual void OnWrongEntry(string e)
        {
            WrongEntry?.Invoke(this, e);
        }
        static void Alert(object sender, string message)
        {
            MessageBox.Show($"{message}", "Meddelande", MessageBoxButton.OK);
            return;
        }

        private void CreateUserButton(object sender, RoutedEventArgs e)
        {
            if (firstName.Text == "" || firstName.Foreground == Brushes.LightGray || lastName.Text == "" || lastName.Foreground == Brushes.LightGray || eMail.Text == "" || eMail.Foreground == Brushes.LightGray || SSN.Text == ""|| SSN.Foreground == Brushes.LightGray) //Kollar om user input är tomt
            {
                MessageBox.Show("Du har inte angett data i samtliga fält!", "Meddelande", MessageBoxButton.OK);
                return;
            }
            if (!Regex.IsMatch(eMail.Text, @"^[a-zåäöA-ZÅÄÖ][\w\.-]*[a-zåäöA-ZÅÄÖ0-9]@[a-zåäöA-ZÅÄÖ0-9][\w\.-]*[a-zåäöA-ZÅÄÖ0-9]\.[a-zåäöA-ZÅÄÖ][a-zåäöA-ZÅÄÖ\.]*[a-zåäöA-ZÅÄÖ]$"))
            {
                MessageBox.Show("Ange korrekt e-mail.", "Meddelande", MessageBoxButton.OK);
                eMail.Select(0, eMail.Text.Length);
                eMail.Focus();
                return;
            }
            if (passWord.Password.Length == 0)
            {
                MessageBox.Show("Ange Lösenord.", "Meddelande", MessageBoxButton.OK);
                passWord.Focus();
                return;
            }
            if (!Regex.IsMatch(firstName.Text, @"^[a-zåäöA-ZÅÄÖ]+$"))
            {
                MessageBox.Show("Ange bara bokstäver.", "Meddelande", MessageBoxButton.OK);
                firstName.Focus();
                return;
            }
            if(!Regex.IsMatch(lastName.Text, @"^[a-zåäöA-ZÅÄÖ]+$"))
            {
                MessageBox.Show("Ange bara bokstäver.", "Meddelande", MessageBoxButton.OK);
                lastName.Focus();
                return;
            }
            if (!Regex.IsMatch(SSN.Text, @"^([0-9]{12})$"))
            {
                MessageBox.Show("Ange bara siffror.", "Meddelande", MessageBoxButton.OK);
                firstName.Focus();
                return;
            }
            if(!Regex.IsMatch(passWord.Password, @"^(?=.*?[A-ZÅÄÖ])(?=.*?[a-zåäö])(?=.*?[0-9]).{8,}$"))
            {
                MessageBox.Show("Ange minst en stor och liten bokstav samt en siffra.", "Meddelande", MessageBoxButton.OK);
                passWord.Focus();
                return;
            }

            var user = new User();

            if (DbInitialiser.Db.Users.Where(y => y.Socialsecuritynumber == Encryption.Encrypt(SSN.Text)).FirstOrDefault() != null) //kollar om angivna SSN redan existerar i DB:n
            {
                MessageBox.Show("Personnummret är redan kopplat till ett konto!", "Meddelande", MessageBoxButton.OK); //detta ska ändras till ett event
                return;
            }
            else if (DbInitialiser.Db.Users.Where(y => y.Email == eMail.Text).FirstOrDefault() != null)
            {
                MessageBox.Show("Emailen är redan kopplat till ett konto!", "Meddelande", MessageBoxButton.OK); //detta ska ändras till ett event
                return;
            }

            UserService.RegisterUser(firstName.Text, lastName.Text, eMail.Text.ToLower(), SSN.Text, passWord.Password);

            MessageBox.Show("Du har nu skapat upp en användare.", "Meddelande", MessageBoxButton.OK);

            MainWindow mainWindow = GlobalClass.currentinstance;
            mainWindow.cheatButton.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));  //triggar cheatButton i createuser för att ändra visibility av alla element
            NavigationService.Navigate(null);
        }
        //Nedan är 'focus' för Grå hints inom Textboxen
        private void FirstFocus(object sender, RoutedEventArgs e)
        {
            Thematics.Watermark.ForFocus(firstName);
        }
        private void FirstLost(object sender, RoutedEventArgs e) 
        {
            Thematics.Watermark.ForLostFocus(firstName, "Förnamn");
        }
        private void LastFocus(object sender, RoutedEventArgs e)
        {
            Thematics.Watermark.ForFocus(lastName);
        }
        private void LastLost(object sender, RoutedEventArgs e) 
        {
            Thematics.Watermark.ForLostFocus(lastName, "Efternamn");
        }
        private void EmailFocus(object sender, RoutedEventArgs e)
        {
            Thematics.Watermark.ForFocus(eMail);
        }
        private void EmailLost(object sender, RoutedEventArgs e)
        {
            Thematics.Watermark.ForLostFocus(eMail, "E-post");
        }
        private void SSNFocus(object sender, RoutedEventArgs e)
        {
            Thematics.Watermark.ForFocus(SSN);
        }
        private void SSNLost(object sender, RoutedEventArgs e) 
        {
            Thematics.Watermark.ForLostFocus(SSN, "Personnummer");
        }
        private void PassFocus(object sender, RoutedEventArgs e)
        {
            passwordInfo.Visibility = Visibility.Visible;
        }
        private void PassLost(object sender, RoutedEventArgs e)
        {
            passwordInfo.Visibility = Visibility.Collapsed;
            if (passWord.Password == null || passWord.Password == "")
            {
                passWordFalse.Visibility = Visibility.Visible;
            }
        }
        private void PassFalseFocus(object sender, RoutedEventArgs e)
        {
            passWordFalse.Visibility = Visibility.Collapsed;
            passWord.Focus();
        }
        //Textchanged för validering 
        private void firstName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Regex.IsMatch(firstName.Text, @"^[a-zA-Z]+$") && firstName.Foreground == Brushes.Black)
            {
                firstNameImgValid.Visibility = Visibility.Visible;
                firstNameImg.Visibility = Visibility.Collapsed;
            }
            else if (!Regex.IsMatch(firstName.Text, @"^[a-zA-Z]+$") && firstName.Foreground == Brushes.Black)
            {
                firstNameImgValid.Visibility = Visibility.Collapsed;
                firstNameImg.Visibility = Visibility.Visible;
            }
        }
        private void lastName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Regex.IsMatch(lastName.Text, @"^[a-zA-Z]+$") && lastName.Foreground == Brushes.Black)
            {
                lastNameImgValid.Visibility = Visibility.Visible;
                lastNameImg.Visibility = Visibility.Collapsed;
            }
            else if (!Regex.IsMatch(lastName.Text, @"^[a-zA-Z]+$") && lastName.Foreground == Brushes.Black)
            {
                lastNameImgValid.Visibility = Visibility.Collapsed;
                lastNameImg.Visibility = Visibility.Visible;
            }
        }
        private void eMail_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Regex.IsMatch(eMail.Text, @"^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$") && eMail.Foreground == Brushes.Black)
            {
                eMailImgValid.Visibility = Visibility.Visible;
                eMailImg.Visibility = Visibility.Collapsed;
            }
            else if (!Regex.IsMatch(eMail.Text, @"^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$") && eMail.Foreground == Brushes.Black)
            {
                eMailImgValid.Visibility = Visibility.Collapsed;
                eMailImg.Visibility = Visibility.Visible;
            }
        }
        private void SSN_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Regex.IsMatch(SSN.Text, @"^([0-9]{12})$") && SSN.Foreground == Brushes.Black)
            {
                SSNImgValid.Visibility = Visibility.Visible;
                SSNImg.Visibility = Visibility.Collapsed;
            }
            else if (!Regex.IsMatch(SSN.Text, @"^([0-9]{12})$") && SSN.Foreground == Brushes.Black)
            {
                SSNImgValid.Visibility = Visibility.Collapsed;
                SSNImg.Visibility = Visibility.Visible;
            }
        }
        private void passWord_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (Regex.IsMatch(passWord.Password, @"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9]).{8,}$"))
            {
                passWordImgValid.Visibility = Visibility.Visible;
                passWordImg.Visibility = Visibility.Collapsed;
            }
            else if (!Regex.IsMatch(passWord.Password, @"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9]).{8,}$"))
            {
                passWordImgValid.Visibility = Visibility.Collapsed;
                passWordImg.Visibility = Visibility.Visible;
            }
        }

        private void Back_button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = GlobalClass.currentinstance;
            mainWindow.cheatButton.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));  //triggar cheatButton i createuser för att ändra visibility av alla element
            NavigationService.Navigate(null);
        }
    
    
    
    
    
    
    }
}
