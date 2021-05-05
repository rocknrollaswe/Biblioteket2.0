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
           
            if (DbInitialiser.Db.Books.Where(b => b.Title == title).FirstOrDefault() != null && DbInitialiser.Db.Books.Where(b => b.Edition == DateTime.Parse(edition)).FirstOrDefault() != null)
            {
                OnWrongEntry("Boken du försöker lägga till finns redan i systemet");
                return;
            }


            book.Title = title;
            book.Author = author;
            book.Description = description;
            book.Edition = DateTime.Parse(edition);
            book.Price = decimal.Parse(price);
            book.Ddk = int.Parse(ddk);
            book.Sab = sab;
            book.Publisher = publisher;
            book.CategoryId = isEbook1Else0;


            DbInitialiser.Db.Add(book); // lägger till boken i systemet, nu finns det ett uppräknat isbn, men vi behöver isbn för att skapa upp en ny stock
                                        // hämtar isbn för den nyss tillagda boken
            DbInitialiser.Db.SaveChanges();

            AddStockBook(title, edition, howMany);

        }
        public void AddStockBook(string title, string edition, int amount)
        {
            IEnumerable<Book> isbnBook = DbInitialiser.Db.Books.Where
                (b => b.Title == title && b.Edition == DateTime.Parse(edition));

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

            AddBook(titlebox.Text, authorBox.Text, descriptionBox.Text, editionBox.Text, priceBox.Text, DDKbox.Text, SABbox.Text, publisherBox.Text, ebokCheck, Convert.ToInt32(howManyBox.Text)); 

           
        }

        
    }
}
