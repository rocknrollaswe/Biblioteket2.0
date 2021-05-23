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
using System.Threading.Tasks;

namespace Bibblan.Views
{
    /// <summary>
    /// Interaction logic for SearchbookA.xaml
    /// </summary>
    public partial class SearchbookA : Page
    {
        List<Book> dbVirtual = new List<Book>(); //skapar en virtuell version av vår Books tabell i databasen för att göra queries mot 
        public SearchbookA()
        {
            InitializeComponent();
            Task dbDownload = Task.Run( () =>
            {
                foreach (var item in DbInitialiser.Db.Books)
                {
                    dbVirtual.Add(item);
                }
            });
        }
        private void searchButton_Click(object sender, RoutedEventArgs e)      
        {
            Task.WaitAll();

            List<Book> bookQuery = dbVirtual.Where(x => x.Title.ToLower().Contains(searchBox.Text.ToLower()) || x.Sab.ToLower().Contains(searchBox.Text.ToLower()) || x.Author.ToLower().Contains(searchBox.Text.ToLower()) || x.Publisher.ToLower().Contains(searchBox.Text.ToLower())).DefaultIfEmpty().ToList();
            if (bookQuery != null) // VÄLDIGT simpel sökfunktion, ska byggas på
            {
                List<string> list = new List<string>();
                list = showResults(bookQuery);
                resultBox.ItemsSource = list;
                return;
            }
            else if (Int32.TryParse(searchBox.Text, out var _)) //kollar om userInput är en int eller ej
            {
                if (dbVirtual.Where(x => x.Isbn == Convert.ToInt32(searchBox.Text)).DefaultIfEmpty() != null)
                {
                    MessageBox.Show("DET FUNKAR IAF..");
                    return;
                }
                return;
            }
        }
        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e) //Denna funktion gör så att dropdown autocomplete menyn visar värden
        {
            autoList.ClearValue(ItemsControl.ItemsSourceProperty);
            if (searchBox.Text.Length > 2)
            {
                 OpenSuggestionBox();

                List<Book> showBooks = dbVirtual.Where(x => x.Title.ToLower().Contains(searchBox.Text.ToLower())).ToList(); //tar fram böckerna som innehåller userinput för TITEL just nu, ska läggas till mer än bara titel
                List<string> titleList = new List<string>();
                foreach (var item in showBooks)
                {
                    titleList.Add(item.Title);
                }

                autoList.ItemsSource = titleList;
            }
            autoList.ClearValue(ItemsControl.ItemsSourceProperty);
            if (searchBox.Text == null || searchBox.Text == "")
            {
                CloseSuggestionBox();
                return;
            }
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
        private List<string> showResults(List<Book> a)
        {
            List<string> temp = new List<string>();

            foreach (var item in a)
            {
                temp.Add(item.ToString());
            }

            return temp;
        }
    }
}
