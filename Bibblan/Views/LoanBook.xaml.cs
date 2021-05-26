using Bibblan.Models;
using Bibblan.Services;
using System;
using System.Collections.Generic;
using System.Linq;
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

namespace Bibblan.Views
{
    /// <summary>
    /// Interaction logic for LoanBook.xaml
    /// </summary>
    public partial class LoanBook : Page
    {
        List<Book> virtualBooks = new List<Book>();
        List<Loanlog> virtualLoan = new List<Loanlog>();

        public LoanBook()
        {
            InitializeComponent();
            foreach (var item in DbInitialiser.Db.Books)
            {
                virtualBooks.Add(item);
            }

            foreach (var item in DbInitialiser.Db.Loanlogs)
            {
                virtualLoan.Add(item);
            }


            DataContext = virtualBooks;
            DataContext = virtualLoan;
            LVLoanBook.ItemsSource = virtualBooks;
            LVAvailable.ItemsSource = virtualLoan;
        }

        private void searchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            Searchfunction();
        }

        private void Searchfunction()
        {
            LVLoanBook.ClearValue(ItemsControl.ItemsSourceProperty);


            List<Book> bookList = virtualBooks.Where(x => x.Title.ToLower().Contains(searchBar.Text.ToLower())
                                                    || x.Author.ToLower().Contains(searchBar.Text.ToLower())
                                                    || x.Category.ToString().Contains(searchBar.Text.ToLower()))

                                                   .ToList(); //tar fram böckerna som innehåller userinput för TITEL 

            if (bookList != null) // VÄLDIGT simpel sökfunktion, ska byggas på
            {
                LVLoanBook.ItemsSource = bookList;
                return;
            }
            else if (Int32.TryParse(searchBar.Text, out var _)) //kollar om userInput är en int eller ej
            {
                List<Book> query = virtualBooks.Where(x => x.Title.ToLower().Contains(searchBar.Text.ToLower())).DefaultIfEmpty().ToList();

                LVLoanBook.ItemsSource = query;

                return;
            }
        }

   

        private void LVLoanBook_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LVLoanBook.SelectedItem != null)
            {
                Book b = LVLoanBook.SelectedItem as Book;
                
                MessageBox.Show($"Beskrivning:\n{b.Description}");
              
            }
            
        }

        private void loanButton_Click(object sender, RoutedEventArgs e)
        {

            Book b = LVLoanBook.SelectedItem as Book;

            
           
        }
    }
}
