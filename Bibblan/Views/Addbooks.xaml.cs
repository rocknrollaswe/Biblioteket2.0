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
           
            foreach (var item in DbInitialiser.Db.Books)
            {
                virtualBooks.Add(item);
            }
            DataContext = virtualBooks;
            LVBooks.ItemsSource = virtualBooks;
        }
      
       
        private void addBooksButton_Click(object sender, RoutedEventArgs e)
        {
            if (titleBox.Text.Length == 0 || titleBox.Foreground == Brushes.LightGray)
            {
                MessageBox.Show("Ange Titel!");
                titleBox.Focus();
                return;
            }
            if (!Regex.IsMatch(authorBox.Text, @"^[a-zåäöA-ZÅÄÖ\s]+$") ||authorBox.Foreground == Brushes.LightGray) 
            {
                MessageBox.Show("Ange Författare!", "Meddelande", MessageBoxButton.OK);
                authorBox.Focus();
                return;
            }
            if (descriptionBox.Text.Length == 0 || descriptionBox.Text.Length >= 500 || descriptionBox.Foreground == Brushes.LightGray  )
            {
                MessageBox.Show("Ange Beskrivning! Får ej vara mer än 500 tecken.", "Meddelande", MessageBoxButton.OK);
                descriptionBox.Focus();
                return;
            }
            if (!Regex.IsMatch(editionBox.Text, @"^([0-9]{4})$"))
            {
                MessageBox.Show("Ange årtal ÅÅÅÅ i Upplaga!", "Meddelande", MessageBoxButton.OK);
                editionBox.Focus();
                return;
            }
            if (Int32.TryParse(editionBox.Text, out var _) == true)
            {
                if (Convert.ToInt32(editionBox.Text) > Convert.ToInt32(DateTime.Now.Year))
                {
                    MessageBox.Show("Boken har inte kommit ut ännu!", "Meddelande", MessageBoxButton.OK);
                    editionBox.Focus();
                    return;
                }
            }
            if (publisherBox.Text.Length == 0 || publisherBox.Foreground == Brushes.LightGray)
            {
                MessageBox.Show("Ange Förlag!", "Meddelande", MessageBoxButton.OK);
                publisherBox.Focus();
                return;
            }
            if (!Regex.IsMatch(priceBox.Text, @"^[0-9]{1,4}$"))
            {

                MessageBox.Show("Ange Pris!", "Meddelande", MessageBoxButton.OK);


                priceBox.Focus();
                return;
            }
            if (!Regex.IsMatch(ddkBox.Text, @"^([0-9]{3})$"))
            {

                MessageBox.Show("Ange bara siffror i DDK!", "Meddelande", MessageBoxButton.OK);

                ddkBox.Focus();
                return;
            }
            if (!Regex.IsMatch(sabBox.Text, @"^([a-zåäöA-ZÅÄÖ]{1,3})$") || sabBox.Foreground == Brushes.LightGray)
            {


                MessageBox.Show("Ange Sab! Max 3 bokstäver.", "Meddelande", MessageBoxButton.OK);

                sabBox.Focus();
                return;
            }
            if (!Regex.IsMatch(amountBox.Text, @"^([0-9]{1,3})$") || Convert.ToInt32(amountBox.Text)>=100)
            {
                MessageBox.Show("Ange Antal! Får ej vara mer än 99 st", "Meddelande", MessageBoxButton.OK);
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
                    MessageBox.Show("Boken du försöker lägga till finns redan i systemet", "Meddelande", MessageBoxButton.OK);
                    return;
                }
            }

            if (ebok.IsChecked == false)
            {
                if (DbInitialiser.Db.Books.Where(b => b.Title == titleBox.Text && b.Category == 0).FirstOrDefault() != null && DbInitialiser.Db.Books.Where(b => b.Edition == int.Parse(editionBox.Text) && b.Category == 0).FirstOrDefault() != null)
                {
                    MessageBox.Show("Boken du försöker lägga till finns redan i systemet", "Meddelande", MessageBoxButton.OK);
                    return;
                }
            }

            MessageBoxResult result = MessageBox.Show($"Är det säkert att du vill lägga till den här boken? " +
                $"\nTitel: {titleBox.Text}\nFörfattare: {authorBox.Text}\nBeskrivning: {descriptionBox.Text}\nUpplaga: {editionBox.Text}\nFörlag: {publisherBox.Text}\nPris: {priceBox.Text}\nDDK: {ddkBox.Text}\nSab: {sabBox.Text}\nAntal: {amountBox.Text} styck",
                "Meddelande", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
           
            if (result == MessageBoxResult.Yes)
            {
                if (GlobalClass.userPermission < 1) { MessageBox.Show("Du har inte behörighet att göra detta", "Meddelande", MessageBoxButton.OK, MessageBoxImage.Exclamation); return; }

                Book bookToAdd = BookService.AddBook(titleBox.Text, authorBox.Text, descriptionBox.Text, editionBox.Text, priceBox.Text, ddkBox.Text, sabBox.Text, publisherBox.Text, ebokCheck);

                BookService.AddStockBook(bookToAdd, Convert.ToInt32(amountBox.Text));

                MessageBox.Show("Du har nu lagt till en bok!", "Meddelande", MessageBoxButton.OK);
                virtualBooks.Clear();

                foreach (var item in DbInitialiser.Db.Books)
                {
                    virtualBooks.Add(item);
                }
                LVBooks.Items.Refresh();

                Thematics.Clearer(titleBox, authorBox, descriptionBox, editionBox, publisherBox, priceBox, ddkBox, sabBox, amountBox);
            }
            return;
        }
        private void TitleFocus(object sender, RoutedEventArgs e)
        {
            Thematics.Watermark.ForFocus(titleBox);
        }
        private void TitleLost(object sender, RoutedEventArgs e)
        {
            Thematics.Watermark.ForLostFocus(titleBox, "Titel");
        }
        private void AuthorFocus(object sender, RoutedEventArgs e)
        {
            Thematics.Watermark.ForFocus(authorBox);
        }
        private void AuthorLost(object sender, RoutedEventArgs e)
        {
            Thematics.Watermark.ForLostFocus(authorBox, "Författare");
        }
        private void DescriptionFocus(object sender, RoutedEventArgs e)
        {
            Thematics.Watermark.ForFocus(descriptionBox);
        }
        private void DescriptionLost(object sender, RoutedEventArgs e)
        {
            Thematics.Watermark.ForLostFocus(descriptionBox, "Beskrivning");
        }
        private void EditionFocus(object sender, RoutedEventArgs e)
        {
            Thematics.Watermark.ForFocus(editionBox);
        }
        private void EditionLost(object sender, RoutedEventArgs e)
        {
            Thematics.Watermark.ForLostFocus(editionBox, "Upplaga");
        }
        private void PublisherFocus(object sender, RoutedEventArgs e)
        {
            Thematics.Watermark.ForFocus(publisherBox);
        }
        private void PublisherLost(object sender, RoutedEventArgs e)
        {
            Thematics.Watermark.ForLostFocus(publisherBox, "Förlag");
        }
        private void PriceFocus(object sender, RoutedEventArgs e)
        {
            Thematics.Watermark.ForFocus(priceBox);
        }
        private void PriceLost(object sender, RoutedEventArgs e)
        {
            Thematics.Watermark.ForLostFocus(priceBox, "Pris");
        }
        private void DdkFocus(object sender, RoutedEventArgs e)
        {
            Thematics.Watermark.ForFocus(ddkBox);
        }
        private void DdkLost(object sender, RoutedEventArgs e)
        {
            Thematics.Watermark.ForLostFocus(ddkBox, "DDK");
        }
        private void SabFocus(object sender, RoutedEventArgs e)
        {
            Thematics.Watermark.ForFocus(sabBox);
        }
        private void SabLost(object sender, RoutedEventArgs e)
        {
            Thematics.Watermark.ForLostFocus(sabBox, "Sab");
        }
        private void AmountFocus(object sender, RoutedEventArgs e)
        {
            Thematics.Watermark.ForFocus(amountBox);
        }
        private void AmountLost(object sender, RoutedEventArgs e)
        {
            Thematics.Watermark.ForLostFocus(amountBox, "Antal");
        }
       
        private void viewBookStock_Click(object sender, RoutedEventArgs e)
        {
            if (LVBooks.SelectedItem == null)
            {
                MessageBox.Show("Välj en bok!");
                return;
            }
            else
            {
                GlobalClass.chosenBook = LVBooks.SelectedItem as Book;
                this.NavigationService.Navigate(new BookStock());
            }
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