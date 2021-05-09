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
    /// Interaction logic for Addbooks.xaml
    /// </summary>
    public partial class Addbooks : Window
    {
        public Addbooks()
        {
            InitializeComponent();
            WrongEntry += Alert;

        }


        public static void Alert(object sender, string message)
        {
            MessageBox.Show($"{message}", "Meddelande", MessageBoxButton.OK, MessageBoxImage.Information);
            return;

            //Console.WriteLine($"Meddelande: {message}"); //detta ska tas bort sen

        }

        public event EventHandler<string> WrongEntry;

        protected virtual void OnWrongEntry(string e)
        {
            WrongEntry?.Invoke(this, e);
        }


        public void AddBook(string title, string author, string description, string edition, string price, string ddk, string sab, string publisher, int isEbook1Else0, int howMany)
        {
            var book = new Book();

            if (DbInitialiser.Db.Books.Where(b => b.Title == title).FirstOrDefault() != null && DbInitialiser.Db.Books.Where(b => b.Edition == int.Parse(edition)).FirstOrDefault() != null)
            {
                OnWrongEntry("Boken du försöker lägga till finns redan i systemet");
                return;
            }


            book.Title = title;
            book.Author = author;
            book.Description = description;
            book.Edition = int.Parse(edition);
            book.Price = decimal.Parse(price);
            book.Ddk = int.Parse(ddk);
            book.Sab = sab;
            book.Publisher = publisher;
            book.Category = isEbook1Else0;


            DbInitialiser.Db.Add(book); // lägger till boken i systemet, nu finns det ett uppräknat isbn, men vi behöver isbn för att skapa upp en ny stock
                                        // hämtar isbn för den nyss tillagda boken
            DbInitialiser.Db.SaveChanges();

            AddStockBook(title, edition, howMany);

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
                DbInitialiser.Db.Add(stock);
            }

            DbInitialiser.Db.SaveChanges();

        }

        private void AddBookButton_Click(object sender, RoutedEventArgs e)
        {
            int ebokCheck = 0;


            if (ebokBox.IsChecked == true)
                ebokCheck = 1;

            AddBook(trueTitle.Text, trueAuth.Text, trueDesc.Text, trueEdition.Text, truePrice.Text, trueDDK.Text, trueSAB.Text, truePublish.Text, ebokCheck, Convert.ToInt32(trueAmount.Text));


        }



        private void menuButtonClick(object sender, RoutedEventArgs e)
        {
            AdminPage menupage = new AdminPage();
            menupage.Show();
            this.Close();
        }

        private void TitleFocus(object sender, RoutedEventArgs e)
        {
            falseTitle.Visibility = System.Windows.Visibility.Collapsed;
            trueTitle.Visibility = System.Windows.Visibility.Visible;
            trueTitle.Focus();
        }

        private void TitleLost(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(trueTitle.Text))
            {
                falseTitle.Visibility = System.Windows.Visibility.Collapsed;
                trueTitle.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void AuthFocus(object sender, RoutedEventArgs e)
        {
            falseAuth.Visibility = System.Windows.Visibility.Collapsed;
            trueAuth.Visibility = System.Windows.Visibility.Visible;
            trueAuth.Focus();
        }

        private void AuthLost(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(trueAuth.Text))
            {
                falseAuth.Visibility = System.Windows.Visibility.Collapsed;
                trueAuth.Visibility = System.Windows.Visibility.Visible;
            }
        }
        private void EditionFocus(object sender, RoutedEventArgs e)
        {
            falseEdition.Visibility = System.Windows.Visibility.Collapsed;
            trueEdition.Visibility = System.Windows.Visibility.Visible;
            trueEdition.Focus();
        }

        private void EditionLost(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(trueEdition.Text))
            {
                falseEdition.Visibility = System.Windows.Visibility.Collapsed;
                trueEdition.Visibility = System.Windows.Visibility.Visible;
            }
        }
        private void DDKFocus(object sender, RoutedEventArgs e)
        {
            falseDDK.Visibility = System.Windows.Visibility.Collapsed;
            trueDDK.Visibility = System.Windows.Visibility.Visible;
            trueDDK.Focus();
        }

        private void DDKLost(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(trueDDK.Text))
            {
                falseDDK.Visibility = System.Windows.Visibility.Collapsed;
                trueDDK.Visibility = System.Windows.Visibility.Visible;
            }
        }
        private void SABFocus(object sender, RoutedEventArgs e)
        {
            falseSAB.Visibility = System.Windows.Visibility.Collapsed;
            trueSAB.Visibility = System.Windows.Visibility.Visible;
            trueSAB.Focus();
        }

        private void SABLost(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(trueSAB.Text))
            {
                falseSAB.Visibility = System.Windows.Visibility.Collapsed;
                trueSAB.Visibility = System.Windows.Visibility.Visible;
            }
        }
        private void PublishFocus(object sender, RoutedEventArgs e)
        {
            falsePublish.Visibility = System.Windows.Visibility.Collapsed;
            truePublish.Visibility = System.Windows.Visibility.Visible;
            truePublish.Focus();
        }

        private void PublishLost(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(truePublish.Text))
            {
                falsePublish.Visibility = System.Windows.Visibility.Collapsed;
                truePublish.Visibility = System.Windows.Visibility.Visible;
            }
        }
        private void PriceFocus(object sender, RoutedEventArgs e)
        {
            falsePrice.Visibility = System.Windows.Visibility.Collapsed;
            truePrice.Visibility = System.Windows.Visibility.Visible;
            truePrice.Focus();
        }

        private void PriceLost(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(truePrice.Text))
            {
                falsePrice.Visibility = System.Windows.Visibility.Collapsed;
                truePrice.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void DescFocus(object sender, RoutedEventArgs e)
        {
            falseDesc.Visibility = System.Windows.Visibility.Collapsed;
            trueDesc.Visibility = System.Windows.Visibility.Visible;
            trueDesc.Focus();
        }
        private void DescLost(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(trueDesc.Text))
            {
                falseDesc.Visibility = System.Windows.Visibility.Collapsed;
                trueDesc.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void AmountFocus(object sender, RoutedEventArgs e)
        {
            falseAmount.Visibility = System.Windows.Visibility.Collapsed;
            trueAmount.Visibility = System.Windows.Visibility.Visible;
            trueAmount.Focus();
        }

        private void AmountLost(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(trueAmount.Text))
            {
                falseAmount.Visibility = System.Windows.Visibility.Collapsed;
                trueAmount.Visibility = System.Windows.Visibility.Visible;
            }
        }
    }
}
