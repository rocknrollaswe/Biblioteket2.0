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
using Bibblan.Services;
using Bibblan.Views;

namespace Bibblan
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DbInitialiser.InitialiseDB();

           
          

            //var home = new Home();
            //this.Hide();
            //home.Show();
        }

        private void loggain_Click(object sender, RoutedEventArgs e)
        {

            string emailLow = emailTextBox.Text.ToLower();

            var userList = DbInitialiser.Db.Users.ToList();

            var connectedUser = userList.Find(x => x.Email == emailLow);    
                
                //Where(x => x.Email == emailLow).SingleOrDefault();
            if (connectedUser.Password.SequenceEqual(Encryption.Encrypt(passwordTextBox.Password)) == true) //SequenceEqual går igenom ByteArrayerna och checkar värdena mot varandra. Detta är en långsam funktion, dock så funkar den då vi inte har 10000 användare
            {
                GlobalClass.userPermission = connectedUser.Permissions;  //sätter våra globala variabler för den specifika användaren
                GlobalClass.userFirstName = connectedUser.Firstname;       //FYLL PÅ HÄR OM VI BEHÖVER FLER GLOBALA VARIABLER

                if (GlobalClass.userPermission == 2)     //exempel på hur vi gör navigering och funktioner beroende på permissions
                {
                    var home = new Home();
                    this.Close();
                    home.Show();
                }
            }
            else
            {
                MessageBox.Show("Fel uppgifter angivna");
            }
        }

        private void instantlogc(object sender, RoutedEventArgs e)      //gästloggin funktionen.
        {
            GlobalClass.userPermission = -1;        //Sätter permission till -1 för att visa att det inte är en inloggad användare
            GlobalClass.userFirstName = "Gäst";
            Main.NavigationService.Navigate(new SearchbookA()); //skickas just nu till searchbookA, vi gör nog en gästsöksida dock
        }

        private void emailTextBox_GotFocus(object sender, RoutedEventArgs e)        //funktioner så den grå texten försvinner och kommer tbx vid rätt tillfällen
        {
            if(emailTextBox.Foreground == Brushes.LightGray)
            {
                emailTextBox.Text = "email@gmail.com";
                emailTextBox.Foreground = Brushes.Black;
            }
        }

        private void passWordTextBox_GotFocus(object sender, RoutedEventArgs e)     //funktioner så den grå texten försvinner och kommer tbx vid rätt tillfällen
        {
            if (passwordTextBox.Foreground == Brushes.LightGray)
            {
                
                passWordFalse.Visibility = Visibility.Collapsed;
            }
        }

        private void PassFalseFocus(object sender, RoutedEventArgs e)
        {
            passWordFalse.Visibility = Visibility.Collapsed;
            passwordTextBox.Focus();
        }
        private void passWordTextBox_LostFocus(object sender, RoutedEventArgs e)        //funktioner så den grå texten försvinner och kommer tbx vid rätt tillfällen
        {
            if (passwordTextBox.Password == null || passwordTextBox.Password == "")
            {
                passWordFalse.Visibility = Visibility.Visible;
            }
        }
        private void emailTextBox_LostFocus(object sender, RoutedEventArgs e)       //funktioner så den grå texten försvinner och kommer tbx vid rätt tillfällen
        {
            if (emailTextBox.Text == "")
            {
                emailTextBox.Text = "E-post";
                emailTextBox.Foreground = Brushes.LightGray;
            }
        }
        private void registerButton_Click(object sender, RoutedEventArgs e)
        {
            passwordTextBox.Visibility = Visibility.Collapsed;
            emailTextBox.Visibility = Visibility.Collapsed;
            guestButton.Visibility = Visibility.Collapsed;
            registerButton.Visibility = Visibility.Collapsed;
            loginButton.Visibility = Visibility.Collapsed;
            Main.NavigationService.Navigate(new CreateUser());
        }
        private void cheatButton_Click(object sender, RoutedEventArgs e)
        {
            passwordTextBox.Visibility = Visibility.Visible;
            emailTextBox.Visibility = Visibility.Visible;
            guestButton.Visibility = Visibility.Visible;
            registerButton.Visibility = Visibility.Visible;
            loginButton.Visibility = Visibility.Visible;
        }
    }
}
