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
using Bibblan.Models;
using Bibblan.Services;
using System.Linq;

namespace Bibblan.Views
{
    /// <summary>
    /// Interaction logic for SearchbookA.xaml
    /// </summary>
    public partial class SearchbookA : Window
    {
        List<Book> dbVirtual = new List<Book>(); //skapar en virtuell version av vår Books tabell i databasen för att göra queries mot 
        public SearchbookA()
        {
            InitializeComponent();
            foreach (var item in DbInitialiser.Db.Books)
            {
                dbVirtual.Add(item);
            }
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            if (dbVirtual.Where(x => x.Title.Contains(searchBox.Text) || x.Sab.Contains(searchBox.Text) || x.Author.Contains(searchBox.Text) || x.Publisher.Contains(searchBox.Text)).DefaultIfEmpty() != null) // VÄLDIGT simpel sökfunktion, ska byggas på
            {
                MessageBox.Show("GRATTIS DU ÄR BÄST");
                return;
            }
            else if (Int32.TryParse(searchBox.Text, out var _)) //kollar om userInput är en int eller ej
            {
                if (dbVirtual.Where(x => x.Isbn == Convert.ToInt32(searchBox.Text)).DefaultIfEmpty() != null)
                {
                    MessageBox.Show("DET FUNKAR IAF..");
                    return;
                }
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

            List<Book> yes = dbVirtual.Where(x => x.Title.ToLower().Contains(searchBox.Text.ToLower())).ToList(); //tar fram böckerna som innehåller userinput för TITEL just nu, ska läggas till mer än bara titel
            List<string> titleList = new List<string>();
            foreach (var item in yes)
            {
                titleList.Add(item.Title);
            }

            autoList.ItemsSource = titleList;
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
            AdminPage adminPage = new AdminPage();
            adminPage.Show();
            this.Close();
        }
    }
}
