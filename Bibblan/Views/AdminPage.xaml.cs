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

namespace Bibblan.Views
{
    /// <summary>
    /// Interaction logic for AdminPage.xaml
    /// </summary>
    public partial class AdminPage : Window
    {
        public AdminPage()
        {
            InitializeComponent();
        }
        private void createUserClick(object sender, RoutedEventArgs e)
        {
            CreateUser createUserPage = new CreateUser();
            createUserPage.Show();
            this.Close();
        }
        private void addBooksClick(object sender, RoutedEventArgs e)
        {
            Addbooks addBooksPage = new Addbooks();
            addBooksPage.Show();
            this.Close();
        }
        private void logoutClick(object sender, RoutedEventArgs e)
        {
            Login logoutpage = new Login();
            logoutpage.Show();
            this.Close();
        }

        private void searchClick(object sender, RoutedEventArgs e)
        {
            SearchbookA searchPage = new SearchbookA();
            searchPage.Show();
            this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DeleteUser deleteUser = new DeleteUser();
            deleteUser.Show();
            this.Close(); 

        }
    }
}
