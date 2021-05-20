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
           
            if (firstName.Text == "" || lastName.Text == "" || eMail.Text == "" || SSN.Text == "") //Kollar om user input är tomt
            {
                OnWrongEntry("Du har inte angett data i samtliga fält!");
                return;
            }

            if (!Regex.IsMatch(eMail.Text, @"^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$"))
            {
                MessageBox.Show("Ange korrekt e-mail.");
                eMail.Select(0, eMail.Text.Length);
                eMail.Focus();
                return;
            }

            if (passWord.Password.Length == 0)
            {
                MessageBox.Show("Ange Lösenord.");
                passWord.Focus();
                return;
            }

            if (!Regex.IsMatch(firstName.Text, @"^[a-zA-Z]+$"))
            {
                MessageBox.Show("Ange bara bokstäver.");
                firstName.Focus();
                return;
            }
            
            if(!Regex.IsMatch(lastName.Text, @"^[a-zA-Z]+$"))
            {
                MessageBox.Show("Ange bara bokstäver.");
                lastName.Focus();
                return;
            }

            if (!Regex.IsMatch(SSN.Text, @"^([0-9]{12})$"))
            {
                MessageBox.Show("Ange bara siffror.");
                firstName.Focus();
                return;
            }

            if(!Regex.IsMatch(passWord.Password, @"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9]).{8,}$"))
            {
                MessageBox.Show("Ange minst en stor och liten bokstav samt en siffra.");
                passWord.Focus();
                return;
            }

            var user = new User();

            if (DbInitialiser.Db.Users.Where(y => y.Socialsecuritynumber == Encryption.Encrypt(SSN.Text)).FirstOrDefault() != null) //kollar om angivna SSN redan existerar i DB:n
            {
                MessageBox.Show("Personnummret är redan kopplat till ett konto!"); //detta ska ändras till ett event
                return;
            }
            else if (DbInitialiser.Db.Users.Where(y => y.Email == eMail.Text).FirstOrDefault() != null)
            {
                MessageBox.Show("Emailen är redan kopplat till ett konto!"); //detta ska ändras till ett event
                return;
            }

            user.Firstname = firstName.Text;
            user.Lastname = lastName.Text;
            user.Email = eMail.Text.ToLower();
            user.Permissions = 0; //Detta ska admin kunna ändra senare

            user.Socialsecuritynumber = Encryption.Encrypt(SSN.Text); //Flyttade encryption metoden till Services.Encryption.cs, så vi kan använda den överallt i programmet. 
           
            user.Password = Encryption.Encrypt(passWord.Password);

            DbInitialiser.Db.Add(user);
            DbInitialiser.Db.SaveChanges();
            MessageBox.Show("Du har nu skapat upp en användare.");

            MainWindow mainWindow = Application.Current.MainWindow as MainWindow;       //navigerar tbx till MainWindow 
            mainWindow.cheatButton.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));  //triggar cheatButton i createuser för att ändra visibility av alla element
            NavigationService.Navigate(null);
        }


        private void menuButtonClick(object sender, RoutedEventArgs e) //Navigering till Adminsida
        {
            NavigationService nav = NavigationService.GetNavigationService(this);
            nav.Navigate(new AdminPage());
        }    


        //Nedan är 'focus' för Grå hints inom Textboxen
        private void FirstFocus(object sender, RoutedEventArgs e)
        {
            if(firstName.Foreground == Brushes.LightGray)
            {
                firstName.Text = "";
                firstName.Foreground = Brushes.Black;
            }
        }
        private void FirstLost(object sender, RoutedEventArgs e)
        {
            if(firstName.Text == "" || firstName.Text == null)
            {
                firstName.Foreground = Brushes.LightGray;
                firstName.Text = "Förnamn";
            }
        }

        private void LastFocus(object sender, RoutedEventArgs e)
        {
            if (lastName.Foreground == Brushes.LightGray)
            {
                lastName.Text = "";
                lastName.Foreground = Brushes.Black;
            }
        }
        private void LastLost(object sender, RoutedEventArgs e)
        {
            if(lastName.Text == "" || firstName.Text == null)
            {
                lastName.Foreground = Brushes.LightGray;
                lastName.Text = "Efternamn";
            }
        }

        private void EmailFocus(object sender, RoutedEventArgs e)
        {
            if (eMail.Foreground == Brushes.LightGray)
            {
                eMail.Text = "";
                eMail.Foreground = Brushes.Black;
            }
        }
        private void EmailLost(object sender, RoutedEventArgs e)
        {
            if (eMail.Text == "" || firstName.Text == null)
            {
                eMail.Foreground = Brushes.LightGray;
                eMail.Text = "E-post";
            }
        }

        private void SSNFocus(object sender, RoutedEventArgs e)
        {
            if (SSN.Foreground == Brushes.LightGray)
            {
                SSN.Text = "";
                SSN.Foreground = Brushes.Black;
            }
        }
        private void SSNLost(object sender, RoutedEventArgs e)
        {
            if (SSN.Text == "" || firstName.Text == null)
            {
                SSN.Foreground = Brushes.LightGray;
                SSN.Text = "Personnummer";
            }
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
    }
}
