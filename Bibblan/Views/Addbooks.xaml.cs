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
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Bibblan.Views
{
    /// <summary>
    /// Interaction logic for Addbooks.xaml
    /// </summary>
    public partial class Addbooks : Page
    {

        List<Book> virtualBooks = new List<Book>();
        public Addbooks()
        {
            InitializeComponent();
            WrongEntry += Alert;
            foreach (var item in DbInitialiser.Db.Books)
            {
                virtualBooks.Add(item);
            }

            DataContext = virtualBooks;
            LVBooks.ItemsSource = virtualBooks;
        }

        public static void Alert(object sender, string message)
        { 
            MessageBox.Show($"{message}", "Meddelande", MessageBoxButton.OK, MessageBoxImage.Information);
            return;

        }

        public event EventHandler<string> WrongEntry;

        protected virtual void OnWrongEntry(string e)
        {
            WrongEntry?.Invoke(this, e);
        }

        private void addBooksButton_Click(object sender, RoutedEventArgs e)
        {
            if (titleBox.Text == "" || authorBox.Text == "" || descriptionBox.Text == "" || editionBox.Text == "" || priceBox.Text == "" || ddkBox.Text == "" || sabBox.Text == "" || publisherBox.Text == "") //Kollar om user input är tomt
            {
                OnWrongEntry("Du har inte angett data i samtliga fält!");
                return;
            }

            if (titleBox.Text.Length == 0)
            {
                MessageBox.Show("Ange Titel!");
                titleBox.Focus();
                return;
            }

            if (authorBox.Text.Length == 0)
            {
                MessageBox.Show("Ange Författare!");
                authorBox.Focus();
                return;
            }

            if (descriptionBox.Text.Length == 0)
            {
                MessageBox.Show("Ange Beskrivning!");
                descriptionBox.Focus();
                return;
            }

            if (!Regex.IsMatch(editionBox.Text, @"^([0-9]{4})$"))
            {
                MessageBox.Show("Ange årtal ÅÅÅÅ i Upplaga!");
                editionBox.Focus();
                return;
            }
            if (publisherBox.Text.Length == 0)
            {
                MessageBox.Show("Ange Förlag!");
                publisherBox.Focus();
                return;
            }

            if (!Regex.IsMatch(priceBox.Text, @"^[0-9]{1,10}$"))

            {
                MessageBox.Show("Ange Pris!");
                priceBox.Focus();
                return;
            }

            if (!Regex.IsMatch(ddkBox.Text, @"^([0-9]{3})$"))
            {
                MessageBox.Show("Ange bara siffror i DDK!");
                ddkBox.Focus();
                return;
            }

            if (sabBox.Text.Length == 0)
            {
                MessageBox.Show("Ange Sab!");
                sabBox.Focus();
                return;
            }

            if (!Regex.IsMatch(amountBox.Text, @"^([0-9]{1,3})$") || Convert.ToInt32(amountBox.Text)>=100)
            {
                MessageBox.Show("Ange Antal! Får ej vara mer än 99 st");
                amountBox.Focus();
                return;
            }

            int ebokCheck = 0;

            if (ebok.IsChecked == true)
                ebokCheck = 1;

            var book = new Book();
           

            if (ebok.IsChecked == true)
            {
                if (DbInitialiser.Db.Books.Where(b => b.Title == titleBox.Text && b.Category == 1).FirstOrDefault() != null && DbInitialiser.Db.Books.Where(b => b.Edition == int.Parse(editionBox.Text) && b.Category == 1).FirstOrDefault() != null)
                {
                    OnWrongEntry("Boken du försöker lägga till finns redan i systemet");
                    return;


                }
            }

            if (ebok.IsChecked == false)
            {
                if (DbInitialiser.Db.Books.Where(b => b.Title == titleBox.Text && b.Category == 0).FirstOrDefault() != null && DbInitialiser.Db.Books.Where(b => b.Edition == int.Parse(editionBox.Text) && b.Category == 0).FirstOrDefault() != null)
                {
                    OnWrongEntry("Boken du försöker lägga till finns redan i systemet");
                    return;


                }
            }



            MessageBoxResult result = MessageBox.Show($"Är det säkert att du vill lägga till den här boken? " +
                $"\nTitel: {titleBox.Text}\nFörfattare: {authorBox.Text}\nBeskrivning: {descriptionBox.Text}\nUpplaga: {editionBox.Text}\nFörlag: {publisherBox.Text}\nPris: {priceBox.Text}\nDDK: {ddkBox.Text}\nSab: {sabBox.Text}\nAntal: {amountBox.Text} styck",
                "Meddelande", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
           
            if (result == MessageBoxResult.Yes)
            {
                AddBook(titleBox.Text, authorBox.Text, descriptionBox.Text, editionBox.Text, priceBox.Text, ddkBox.Text, sabBox.Text, publisherBox.Text, ebokCheck, Convert.ToInt32(amountBox.Text));

                LVBooks.Items.Refresh();
                Clearer();

            }
            return;
        }

        public void AddBook(string title, string author, string description, string edition, string price, string ddk, string sab, string publisher, int isEbook1Else0, int howMany)
        {
            var book = new Book();
   
            book.Title = titleBox.Text;
            book.Author = authorBox.Text;
            book.Description = descriptionBox.Text;
            book.Edition = int.Parse(editionBox.Text);
            book.Price = decimal.Parse(priceBox.Text);
            book.Ddk = int.Parse(ddkBox.Text);
            book.Sab = sabBox.Text;
            book.Publisher = publisherBox.Text;
            book.Category = isEbook1Else0;


            DbInitialiser.Db.Add(book); // lägger till boken i systemet, nu finns det ett uppräknat isbn, men vi behöver isbn för att skapa upp en ny stock
                                        // hämtar isbn för den nyss tillagda boken
            DbInitialiser.Db.SaveChanges();

            MessageBox.Show("Du har nu lagt till en bok!");

            virtualBooks.Clear();

            foreach (var item in DbInitialiser.Db.Books)
            {
                virtualBooks.Add(item);
            }



            AddStockBook(title, edition, howMany);

        }

        public void Clearer()
        {
            LVBooks.Items.Refresh();
            titleBox.Foreground = Brushes.LightGray;
            titleBox.Text = "Titel";
            authorBox.Foreground = Brushes.LightGray;
            authorBox.Text = "Författare";
            descriptionBox.Foreground = Brushes.LightGray;
            descriptionBox.Text = "Beskrivning";
            editionBox.Foreground = Brushes.LightGray;
            editionBox.Text = "Upplaga";
            publisherBox.Foreground = Brushes.LightGray;
            publisherBox.Text = "Förlag";
            priceBox.Foreground = Brushes.LightGray;
            priceBox.Text = "Pris";
            ddkBox.Foreground = Brushes.LightGray;
            ddkBox.Text = "DDK";
            sabBox.Foreground = Brushes.LightGray;
            sabBox.Text = "Sab";
            amountBox.Foreground = Brushes.LightGray;
            amountBox.Text = "Antal";
        }

        private void TitleFocus(object sender, RoutedEventArgs e)
        {
            if (titleBox.Foreground == Brushes.LightGray)
            {
                titleBox.Text = "";
                titleBox.Foreground = Brushes.Black;
            }
        }
        private void TitleLost(object sender, RoutedEventArgs e)
        {
            if (titleBox.Text == "" || titleBox.Text == null)
            {
                titleBox.Foreground = Brushes.LightGray;
                titleBox.Text = "Titel";
            }
        }

        private void AuthorFocus(object sender, RoutedEventArgs e)
        {
            if (authorBox.Foreground == Brushes.LightGray)
            {
                authorBox.Text = "";
                authorBox.Foreground = Brushes.Black;
            }
        }
        private void AuthorLost(object sender, RoutedEventArgs e)
        {
            if (authorBox.Text == "" || titleBox.Text == null)
            {
                authorBox.Foreground = Brushes.LightGray;
                authorBox.Text = "Författare";
            }
        }

        private void DescriptionFocus(object sender, RoutedEventArgs e)
        {
            if (descriptionBox.Foreground == Brushes.LightGray)
            {
                descriptionBox.Text = "";
                descriptionBox.Foreground = Brushes.Black;
            }
        }
        private void DescriptionLost(object sender, RoutedEventArgs e)
        {
            if (descriptionBox.Text == "" || titleBox.Text == null)
            {
                descriptionBox.Foreground = Brushes.LightGray;
                descriptionBox.Text = "Beskrivning";
            }
        }

        private void EditionFocus(object sender, RoutedEventArgs e)
        {
            if (editionBox.Foreground == Brushes.LightGray)
            {
                editionBox.Text = "";
                editionBox.Foreground = Brushes.Black;
            }
        }
        private void EditionLost(object sender, RoutedEventArgs e)
        {
            if (editionBox.Text == "" || titleBox.Text == null)
            {
                editionBox.Foreground = Brushes.LightGray;
                editionBox.Text = "Upplaga";
            }
        }

        private void PublisherFocus(object sender, RoutedEventArgs e)
        {
            if (publisherBox.Foreground == Brushes.LightGray)
            {
                publisherBox.Text = "";
                publisherBox.Foreground = Brushes.Black;
            }
        }
        private void PublisherLost(object sender, RoutedEventArgs e)
        {
            if (publisherBox.Text == "" || titleBox.Text == null)
            {
                publisherBox.Foreground = Brushes.LightGray;
                publisherBox.Text = "Förlag";
            }
        }


        private void PriceFocus(object sender, RoutedEventArgs e)
        {
            if (priceBox.Foreground == Brushes.LightGray)
            {
                priceBox.Text = "";
                priceBox.Foreground = Brushes.Black;
            }
        }
        private void PriceLost(object sender, RoutedEventArgs e)
        {
            if (priceBox.Text == "" || titleBox.Text == null)
            {
                priceBox.Foreground = Brushes.LightGray;
                priceBox.Text = "Pris";
            }
        }

        private void DdkFocus(object sender, RoutedEventArgs e)
        {
            if (ddkBox.Foreground == Brushes.LightGray)
            {
                ddkBox.Text = "";
                ddkBox.Foreground = Brushes.Black;
            }
        }
        private void DdkLost(object sender, RoutedEventArgs e)
        {
            if (ddkBox.Text == "" || titleBox.Text == null)
            {
                ddkBox.Foreground = Brushes.LightGray;
                ddkBox.Text = "DDK";
            }
        }

        private void SabFocus(object sender, RoutedEventArgs e)
        {
            if (sabBox.Foreground == Brushes.LightGray)
            {
                sabBox.Text = "";
                sabBox.Foreground = Brushes.Black;
            }
        }
        private void SabLost(object sender, RoutedEventArgs e)
        {
            if (sabBox.Text == "" || titleBox.Text == null)
            {
                sabBox.Foreground = Brushes.LightGray;
                sabBox.Text = "Sab";
            }
        }


        private void AmountFocus(object sender, RoutedEventArgs e)
        {
            if (amountBox.Foreground == Brushes.LightGray)
            {
                amountBox.Text = "";
                amountBox.Foreground = Brushes.Black;
            }
        }
        private void AmountLost(object sender, RoutedEventArgs e)
        {
            if (amountBox.Text == "" || titleBox.Text == null)
            {
                amountBox.Foreground = Brushes.LightGray;
                amountBox.Text = "Antal";
            }
        }


        public void AddStockBook(string title, string edition, int amount)
        {
            IEnumerable<Book> isbnBook = DbInitialiser.Db.Books.Where
                (b => b.Title == title && b.Edition == int.Parse(edition));

            Book b = isbnBook.FirstOrDefault();

            for (int i = 0; i < amount; i++)
            {
                var stock = new Stock();
                stock.Isbn = Convert.ToInt32(b.Isbn);
                stock.Condition = "Nyskick";
                stock.Discarded = 0;
                DbInitialiser.Db.Add(stock);
            }


            DbInitialiser.Db.SaveChanges();

        }

        private void viewBookStock_Click(object sender, RoutedEventArgs e)
        {

            GlobalClass.chosenBook = LVBooks.SelectedItem as Book;

            this.NavigationService.Navigate(new BookStock()); 
        }
        private void searchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            Searchfunction();
        }

        private void Searchfunction()
        {
            LVBooks.ClearValue(ItemsControl.ItemsSourceProperty);


            List<Book> bookList = virtualBooks.Where(x => x.Title.ToLower().Contains(searchBar.Text.ToLower())
                                                    || x.Author.ToLower().Contains(searchBar.Text.ToLower()))
                                                   .ToList(); //tar fram böckerna som innehåller userinput för TITEL just nu, ska läggas till mer än bara titel

            if (bookList != null) // VÄLDIGT simpel sökfunktion, ska byggas på
            {
                LVBooks.ItemsSource = bookList;
                return;
            }
            else if (Int32.TryParse(searchBar.Text, out var _)) //kollar om userInput är en int eller ej
            {
                List<Book> query = virtualBooks.Where(x => x.Title.ToLower().Contains(searchBar.Text.ToLower())).DefaultIfEmpty().ToList();

                LVBooks.ItemsSource = query;

                return;
            }
        }
    }
}