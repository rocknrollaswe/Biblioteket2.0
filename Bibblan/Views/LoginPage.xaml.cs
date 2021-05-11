using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Navigation;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Bibblan.Views
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Page
    {
        public Login()
        {
            InitializeComponent();
        }

        //Lägg till knappar och kod här!




        private void UserFocus(object sender, RoutedEventArgs e)
        {
            UsernameInputFocus.Visibility = System.Windows.Visibility.Collapsed;
            UsernameInput.Visibility = System.Windows.Visibility.Visible;
            UsernameInput.Focus();
        }

        private void UserLost(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(UsernameInput.Text))
            {
                UsernameInputFocus.Visibility = System.Windows.Visibility.Collapsed;
                UsernameInput.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void PassFocus(object sender, RoutedEventArgs e)
        {
            PasswordInputFocus.Visibility = System.Windows.Visibility.Collapsed;
            PasswordInput.Visibility = System.Windows.Visibility.Visible;
        }

        private void PassLost(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(PasswordInput.Text))
            {
                PasswordInputFocus.Visibility = System.Windows.Visibility.Collapsed;
                PasswordInput.Visibility = System.Windows.Visibility.Visible;
                PasswordInput.Focus();
            }
        }
    }
}
