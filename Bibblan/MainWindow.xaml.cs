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
using Bibblan.Models;

namespace Bibblan
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<User> dbVirtual = new List<User>();
    
        public MainWindow()
        {
            InitializeComponent();
            DbInitialiser.InitialiseDB();
            Task dbDownload = Task.Run(() =>
            {
                foreach (var item in DbInitialiser.Db.Users)
                {
                    dbVirtual.Add(item);
                }
            });
            dbDownload.Wait(); 
        }
        private void loggain_Click(object sender, RoutedEventArgs e)
        {
            if (UserService.Login(emailTextBox.Text, passwordTextBox.Password))
            {
                if (GlobalClass.userPermission == 2 || GlobalClass.userPermission == 1 || GlobalClass.userPermission == 0)      //Navigera till Home så länge man har en permission
                {
                    GlobalClass.currentMainWindowInstance = this;
                    var home = new Home();
                    this.Hide();
                    home.Show();
                }
                
            }
            else MessageBox.Show("Fel uppgifter angivna!", "Meddelande", MessageBoxButton.OK, MessageBoxImage.Exclamation); 

        }
        private void instantlogc(object sender, RoutedEventArgs e)      //gästloggin funktionen.
        {
            
            GlobalClass.userPermission = -1;        //Sätter permission till -1 för att visa att det inte är en inloggad användare
            GlobalClass.userFirstName = "Gäst";
            Main.NavigationService.Navigate(new GuestPage()); //skickas just nu till Gästsidan för sök
        }
        private void emailTextBox_GotFocus(object sender, RoutedEventArgs e)        //funktioner så den grå texten försvinner och kommer tbx vid rätt tillfällen
        {
            if(emailTextBox.Foreground == Brushes.LightGray)
            {
                emailTextBox.Text = "email@gmail.com"; //TA BORT NÄR VI SLÄPPER PROGRAMMET
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
            Thematics.Watermark.ForLostFocus(emailTextBox, "E-post");
        }
        private void registerButton_Click(object sender, RoutedEventArgs e)
        {
            passwordTextBox.Visibility = Visibility.Collapsed;
            passWordFalse.Visibility = Visibility.Collapsed;
            emailTextBox.Visibility = Visibility.Collapsed;
            guestButton.Visibility = Visibility.Collapsed;
            registerButton.Visibility = Visibility.Collapsed;
            loginButton.Visibility = Visibility.Collapsed;
            Main.NavigationService.Navigate(new CreateUser());
            GlobalClass.currentMainWindowInstance = this; 
        }
        private void cheatButton_Click(object sender, RoutedEventArgs e)
        {
            passwordTextBox.Visibility = Visibility.Visible;
            passWordFalse.Visibility = Visibility.Visible;
            emailTextBox.Visibility = Visibility.Visible;
            guestButton.Visibility = Visibility.Visible;
            registerButton.Visibility = Visibility.Visible;
            loginButton.Visibility = Visibility.Visible;
        }
    }
}
