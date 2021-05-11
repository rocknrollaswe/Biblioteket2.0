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
using Bibblan.Models;
using Bibblan.Services;
using System.Linq; 

namespace Bibblan.Views
{
    /// <summary>
    /// Interaction logic for DeleteUser.xaml
    /// </summary>
    public partial class DeleteUser : Page
    {
        List<User> dbVirtual = new List<User>();

        public DeleteUser()
        {
            InitializeComponent();
            foreach (var item in DbInitialiser.Db.Users)
            {
                dbVirtual.Add(item);
            }

        }

        //DbInitialiser.Db.Users.Where(x => x.Socialsecuritynumber == Encryption.Encrypt(SSN)).SingleOrDefault();


        private void searchButton_Click(object sender, RoutedEventArgs e)
        {
            List<User> userQuery = dbVirtual.Where(x => x.Firstname.ToLower().Contains(searchBox.Text.ToLower()) || x.Lastname.ToLower().Contains(searchBox.Text.ToLower()) || x.Email.ToLower().Contains(searchBox.Text.ToLower())).DefaultIfEmpty().ToList();

            if (userQuery != null) // VÄLDIGT simpel sökfunktion, ska byggas på
            {
                //List<User> list = new List<string>();
                //list = showResults(userQuery);
                resultBox.ItemsSource = userQuery;
                return;
            }
            else if (dbVirtual.Where(x => x.Socialsecuritynumber == Encryption.Encrypt(searchBox.Text)).SingleOrDefault() != null) //kollar om userInput är en int eller ej
            {
                //List<string> list = new List<string>();
                //list = showResults(userQuery);
                resultBox.ItemsSource = userQuery;
                return;
            }
            

        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e) //Denna funktion gör så att dropdown autocomplete menyn visar värden
        {
            autoList.ClearValue(ItemsControl.ItemsSourceProperty);
            if (searchBox.Text == null || searchBox.Text == "")
            {
                CloseSuggestionBox();
                return;
            }

            OpenSuggestionBox();

            List<User> yes = dbVirtual.Where(x => x.Lastname.ToLower().Contains(searchBox.Text.ToLower())).ToList(); //tar fram böckerna som innehåller userinput för TITEL just nu, ska läggas till mer än bara titel
            List<string> lastNameList = new List<string>();
            foreach (var item in yes)
            {
                lastNameList.Add(item.Lastname);
            }

            autoList.ItemsSource = lastNameList;
        }

        private void autoList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (autoList.SelectedIndex <= -1)
            {
                CloseSuggestionBox();
                return;
            }

            searchBox.Text = autoList.SelectedItem.ToString();
            autoList.SelectedIndex = -1;
            CloseSuggestionBox();
        }

        private void OpenSuggestionBox()
        {
            autoList.Visibility = Visibility.Visible;
        }
        private void CloseSuggestionBox()
        {
            autoList.Visibility = Visibility.Collapsed;
        }


        private void menuClick(object sender, RoutedEventArgs e)
        {
            NavigationService nav = NavigationService.GetNavigationService(this);
            nav.Navigate(new AdminPage());

        }

        private List<string> showResults(List<User> a)
        {
            List<string> temp = new List<string>();

            foreach (var item in a)
            {
                temp.Add(item.ToString());
            }

            return temp;
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {

            User u = (resultBox.SelectedItem as User);
          
            DbInitialiser.Db.Users.Remove(u);
            DbInitialiser.Db.SaveChanges(); 
            MessageBox.Show("Du har tagit bort användaren"); 
        }
    }
}
