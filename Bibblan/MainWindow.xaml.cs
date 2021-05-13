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

            var home = new Home();
            this.Hide();
            home.Show();
        }

        private void loggain_Click(object sender, RoutedEventArgs e)
        {
            var connectedUser = DbInitialiser.Db.Users.Where(x => x.Email.ToLower() == emailTextBox.Text.ToLower()).SingleOrDefault();
            if (connectedUser.Password.SequenceEqual(Encryption.Encrypt(passwordTextBox.Text)) == true) //SequenceEqual går igenom ByteArrayerna och checkar värdena mot varandra. Detta är en långsam funktion, dock så funkar den då vi inte har 10000 användare
            {
                GlobalClass.userPermission = connectedUser.Permissions;  //sätter våra globala variabler för den specifika användaren
                GlobalClass.userFirstName = connectedUser.Firstname;       //FYLL PÅ HÄR OM VI BEHÖVER FLER GLOBALA VARIABLER

                if(GlobalClass.userPermission == 2)     //exempel på hur vi gör navigering och funktioner beroende på permissions
                {
                    Main.NavigationService.Navigate(new AdminPage());
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
                emailTextBox.Text = "";
                emailTextBox.Foreground = Brushes.Black;
            }
        }

        private void passWordTextBox_GotFocus(object sender, RoutedEventArgs e)     //funktioner så den grå texten försvinner och kommer tbx vid rätt tillfällen
        {
            if (passwordTextBox.Foreground == Brushes.LightGray)
            {
                passwordTextBox.Text = "";
                passwordTextBox.Foreground = Brushes.Black;
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

        private void passWordTextBox_LostFocus(object sender, RoutedEventArgs e)        //funktioner så den grå texten försvinner och kommer tbx vid rätt tillfällen
        {
            if (passwordTextBox.Text == "")
            {
                passwordTextBox.Text = "Lösenord";
                passwordTextBox.Foreground = Brushes.LightGray;
            }
        }
    }
}
