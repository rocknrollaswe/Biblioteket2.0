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
using System.Windows.Navigation;
using Bibblan.Views; 
using Bibblan.Services;

namespace Bibblan.Views
{
    /// <summary>
    /// Interaction logic for AdminPage.xaml
    /// </summary>
    public partial class AdminPage : Page
    {
        public AdminPage()
        {
            InitializeComponent();
            MessageBox.Show(GlobalClass.userPermission.ToString()); //ENDAST FÖR ATT SE SÅ LOGIN FUNGERAR. SKA TAS BORT
        }
        private void createUserClick(object sender, RoutedEventArgs e)
        {
            NavigationService nav = NavigationService.GetNavigationService(this);
            nav.Navigate(new CreateUser());
        }
        private void addBooksClick(object sender, RoutedEventArgs e)
        {
            NavigationService nav = NavigationService.GetNavigationService(this);
            nav.Navigate(new Addbooks());
        }
        private void logoutClick(object sender, RoutedEventArgs e)
        {
            NavigationService nav = NavigationService.GetNavigationService(this);
            //nav.Navigate(new Login());
            GlobalClass.userFirstName = null;
            GlobalClass.userPermission = null;
        }

        private void searchClick(object sender, RoutedEventArgs e)
        {
            NavigationService nav = NavigationService.GetNavigationService(this);
            nav.Navigate(new SearchbookA());
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService nav = NavigationService.GetNavigationService(this);
            nav.Navigate(new DeleteUser());

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            NavigationService nav = NavigationService.GetNavigationService(this);
            //nav.Navigate(new UserPage2());
        }
    }
}
