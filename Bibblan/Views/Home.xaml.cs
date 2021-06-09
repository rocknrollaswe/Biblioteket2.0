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
using System.Drawing;
using System.ComponentModel;

namespace Bibblan.Views
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : Window
    {
        DateTime resourceSaving = new DateTime();
        DateTime resourceSavingCheck = new DateTime();
        public Home()
        {
            InitializeComponent();
            Validation();
            GlobalClass.currentHomeInstance = this;
            InactivityTimer.SetTimer();  // AUTO LOGOUT GREJ
            Main2.Navigate(new WelcomePage()); 
        }

        public void Validation()
        {
            if (GlobalClass.userPermission == 0) //Gömmer för ordinarie användare
            {
                user_button.Visibility = Visibility.Collapsed; //Administrera Användare göms
                AddBooks.Visibility = Visibility.Collapsed; //Lägg till Böcker göms
                ReturnedBooksButton.Visibility = Visibility.Collapsed;
                Rapportering.Visibility = Visibility.Collapsed; //Rapportering göms
            }
        }

        private void Menu_Click(object sender, RoutedEventArgs e)
        {
            if (GlobalClass.userPermission < 1) { MessageBox.Show("Du har inte behörighet att göra detta", "Meddelande", MessageBoxButton.OK, MessageBoxImage.Exclamation); return; }
            Main2.Navigate(new UserAdminPage());
        }
        private void Stock_Click(object sender, RoutedEventArgs e)
        {
            if (GlobalClass.userPermission < 1) { MessageBox.Show("Du har inte behörighet att göra detta", "Meddelande", MessageBoxButton.OK, MessageBoxImage.Exclamation); return; }
            Main2.Navigate(new BookStock());
        }
        private void AddBooks_Click(object sender, RoutedEventArgs e)
        {
            if (GlobalClass.userPermission < 1) { MessageBox.Show("Du har inte behörighet att göra detta", "Meddelande", MessageBoxButton.OK, MessageBoxImage.Exclamation); return; }
            GlobalClass.chosenBook = null;
            GlobalClass.chosenBookReport = null;
            GlobalClass.deletedObjects = null;
            Main2.Navigate(new Addbooks());

        }
        private void LogOut_Click(object sender, RoutedEventArgs e)
        {
            InactivityTimer.StopTimer();
            var mainwindow = new MainWindow();
            this.Close();
            mainwindow.Show();
        }
        private void Rapportering_Click(object sender, RoutedEventArgs e)
        {
            if (GlobalClass.userPermission < 1) { MessageBox.Show("Du har inte behörighet att göra detta", "Meddelande", MessageBoxButton.OK, MessageBoxImage.Exclamation); return; }
            Main2.Navigate(new Rapport());
        }
        private void LoanBooks_Click(object sender, RoutedEventArgs e) 
        {
            Main2.Navigate(new LoanBook()); 
        }
        private void ReturnedBooks_button_Click(object sender, RoutedEventArgs e)
        {
            if (GlobalClass.userPermission < 1) { MessageBox.Show("Du har inte behörighet att göra detta", "Meddelande", MessageBoxButton.OK, MessageBoxImage.Exclamation); return; }
            Main2.Navigate(new ReturnedBooksPage()); 
        }
        private void Home_button_Click(object sender, RoutedEventArgs e)
        {
            Main2.Navigate(new WelcomePage()); 
        }

        private void Grid_MouseMove(object sender, MouseEventArgs e)
        {
            resourceSaving = DateTime.Now;

            if (resourceSaving == null || resourceSaving > resourceSavingCheck)
            {
                resourceSavingCheck = DateTime.Now.AddSeconds(30);
                InactivityTimer.ResetTimer();
            }
        }
    }
}
