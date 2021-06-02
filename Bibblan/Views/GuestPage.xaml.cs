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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Bibblan.Models;
using Bibblan.Services;
using System.Linq;

namespace Bibblan.Views
{
    /// <summary>
    /// Interaction logic for GuestPage.xaml
    /// </summary>
    public partial class GuestPage : Page
    {
        List<Book> virtualBooks = new List<Book>();
        public GuestPage()
        {
            InitializeComponent();
            foreach (var item in DbInitialiser.Db.Books)
            {
                virtualBooks.Add(item);
            }

            DataContext = virtualBooks;
            LVGuestBook.ItemsSource = virtualBooks;

            descriptionBox.IsReadOnly = true;
        }

        private void LVGuestBook_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LVGuestBook.SelectedItem != null)
            {
                var b = LVGuestBook.SelectedItem as Book;

                descriptionBox.Text = b.Description;

            }
        }

        private void searchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            Searchfunction();
        }

        private void Searchfunction()
        {
            LVGuestBook.ClearValue(ItemsControl.ItemsSourceProperty);


            List<Book> bookList = virtualBooks.Where(x => x.Title.ToLower().Contains(searchBar.Text.ToLower())
                                                    || x.Author.ToLower().Contains(searchBar.Text.ToLower())
                                                    || x.Category.ToString().Contains(searchBar.Text.ToLower()))

                                                   .ToList(); //tar fram böckerna som innehåller userinput för TITEL 

            if (bookList != null) 
            {
                LVGuestBook.ItemsSource = bookList;
                return;
            }
            else if (Int32.TryParse(searchBar.Text, out var _)) //kollar om userInput är en int eller ej
            {
                List<Book> query = virtualBooks.Where(x => x.Title.ToLower().Contains(searchBar.Text.ToLower())).DefaultIfEmpty().ToList();

                LVGuestBook.ItemsSource = query;

                return;
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(null);
        }
    }
}
