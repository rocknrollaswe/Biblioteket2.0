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
    /// Interaction logic for SearchBook.xaml
    /// </summary>
    public partial class SearchBook : Window
    {
        List<Book> test = new List<Book>();
        public SearchBook()
        {
            InitializeComponent();
            foreach (var item in DbInitialiser.Db.Books)
            {
                test.Add(item);
            }
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            if (DbInitialiser.Db.Books.Where(x => x.Title.Contains(SearchBox.Text) || x.Sab.Contains(SearchBox.Text) || x.Author.Contains(SearchBox.Text) || x.Publisher.Contains(SearchBox.Text)).FirstOrDefault() != null) // VÄLDIGT simpel sökfunktion, ska byggas på
            {
                MessageBox.Show("GRATTIS DU ÄR BÄST");
                return;
            }
            else if(Int32.TryParse(SearchBox.Text, out var _)) //kollar om userInput är en int eller ej
            {
                if (DbInitialiser.Db.Books.Where(x => x.Isbn == Convert.ToInt32(SearchBox.Text)).FirstOrDefault() != null) 
                {
                    MessageBox.Show("DET FUNKAR IAF..");
                    return;
                }
            }
        }
        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e) //Denna funktion gör så att dropdown autocomplete menyn visar värden
        {
            autoList.ClearValue(ItemsControl.ItemsSourceProperty);
            if (SearchBox.Text == null || SearchBox.Text == "")
            {
                CloseSuggestionBox();
                return;
            }

            OpenSuggestionBox();

            List<Book> yes = test.Where(x => x.Title.ToLower().Contains(SearchBox.Text.ToLower())).ToList(); //tar fram böckerna som innehåller userinput för TITEL just nu, ska läggas till mer än bara titel
            List<string> titleList = new List<string>();
            foreach(var item in yes)
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

            SearchBox.Text = autoList.SelectedItem.ToString();
            autoList.SelectedIndex = -1;
            CloseSuggestionBox();
        }

        private void OpenSuggestionBox()
        {
            autoListPopup.Visibility = Visibility.Visible;
            autoListPopup.IsOpen = true;
            autoList.Visibility = Visibility.Visible;
        }

        private void CloseSuggestionBox()
        {
            autoListPopup.Visibility = Visibility.Collapsed;
            autoListPopup.IsOpen = false;
            autoList.Visibility = Visibility.Collapsed;
        }
    }
}
