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
        }

        private void loggain_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void instantlogc(object sender, RoutedEventArgs e)
        {
            Main.NavigationService.Navigate(new CreateUser());
        }

        private void emailTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if(emailTextBox.Foreground == Brushes.LightGray)
            {
                emailTextBox.Text = "";
                emailTextBox.Foreground = Brushes.Black;
            }
        }

        private void passWordTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (passwordTextBox.Foreground == Brushes.LightGray)
            {
                passwordTextBox.Text = "";
                passwordTextBox.Foreground = Brushes.Black;
            }
        }
        private void emailTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (emailTextBox.Text == "")
            {
                emailTextBox.Text = "E-post";
                emailTextBox.Foreground = Brushes.LightGray;
            }
        }

        private void passWordTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (passwordTextBox.Text == "")
            {
                passwordTextBox.Text = "Lösenord";
                passwordTextBox.Foreground = Brushes.LightGray;
            }
        }
    }
}
