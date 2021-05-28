using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Bibblan.Services;
using Bibblan.Views;
using Bibblan.Models;

namespace Bibblan.Views
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : Window
    {
        public Home()
        {
            InitializeComponent();
            Validation();
            Main2.Navigate(new WelcomePage()); 
        }

        public void Validation()
        {
            if (GlobalClass.userPermission == 0) //Gömmer för ordinarie användare
            {
                user_button.Visibility = Visibility.Collapsed; //Administrera Användare göms
                AddBooks.Visibility = Visibility.Collapsed; //Lägg till Böcker göms
                ReturnedBooksButton.Visibility = Visibility.Collapsed;  
               
            }
        }

        private void Menu_Click(object sender, RoutedEventArgs e)
        {
            Main2.Navigate(new UserAdminPage());
        }

        private void Stock_Click(object sender, RoutedEventArgs e)
        {
            Main2.Navigate(new BookStock());
        }
        private void AddBooks_Click(object sender, RoutedEventArgs e)
        {
            Main2.Navigate(new Addbooks());
        }

        private void LogOut_Click(object sender, RoutedEventArgs e)
        {
            var mainwindow = new MainWindow();
            this.Close();
            mainwindow.Show();
        }
        
        private void Rapportering_Click(object sender, RoutedEventArgs e)
        {
            Main2.Navigate(new Rapport());
        }
   
        private void LoanBooks_Click(object sender, RoutedEventArgs e) 
        {
            Main2.Navigate(new LoanBook()); 
        
        }

        private void ReturnedBooks_button_Click(object sender, RoutedEventArgs e)
        {
            Main2.Navigate(new ReturnedBooksPage()); 
        }

        private void Home_button_Click(object sender, RoutedEventArgs e)
        {
            Main2.Navigate(new WelcomePage()); 
        }
    }
}
