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
using Bibblan.Services;
using Bibblan.Models;
using System.Linq; 

namespace Bibblan.Views
{
    /// <summary>
    /// Interaction logic for ReturnedBooksPage.xaml
    /// </summary>
    public partial class ReturnedBooksPage : Page
    {
        List<StockLoanLogBooksJoin> booksToValidate = new List<StockLoanLogBooksJoin>(); 
        public ReturnedBooksPage()
        {
            InitializeComponent();

            ClearAndRetrieveVirtualDb();

            LVBooksReturnedByUser.ItemsSource = booksToValidate; 


        }

        private void ClearAndRetrieveVirtualDb()
        {
            booksToValidate.Clear();

            foreach (var item in DbInitialiser.Db.StockLoanLogBooksJoins)
            {
                if (item.Pending == 1)
                {
                    booksToValidate.Add(item);
                }

                continue;

            }
        }
   
        public void ValidateBookButton_Click(object sender, EventArgs e) 
        {

            if(LVBooksReturnedByUser.SelectedItem != null)
            {

                var selectedBook = LVBooksReturnedByUser.SelectedItem as StockLoanLogBooksJoin;
                var sameBook = DbInitialiser.Db.Stocks.Where(x => x.StockId == selectedBook.Stockid).FirstOrDefault();
   
                sameBook.Available = 1;

                var loanLogToClear = DbInitialiser.Db.Loanlogs.Where(x => x.StockId == sameBook.StockId).FirstOrDefault();
                loanLogToClear.Pending = 0; //sätter loanloggen till 'Färdig'
            
                DbInitialiser.Db.Loanlogs.Remove(loanLogToClear);
                DbInitialiser.Db.SaveChanges();

                ClearAndRetrieveVirtualDb();

                LVBooksReturnedByUser.ClearValue(ItemsControl.ItemsSourceProperty); 

                LVBooksReturnedByUser.ItemsSource = booksToValidate;


                MessageBox.Show($"Du har nu validerat objektet");
                return; 
            }

            MessageBox.Show("Du måste markera ett objekt att validera!");
            return; 
        }

        public void ValidateAllBooksButton_Click(object sender, EventArgs e) 
        {
            if (LVBooksReturnedByUser != null)
            {

                foreach (var item in booksToValidate)
                {
                    var bookToValidate = DbInitialiser.Db.Stocks.Where(x => x.StockId == item.Stockid).FirstOrDefault();

                    bookToValidate.Available = 1;

                    var loanLogToClear = DbInitialiser.Db.Loanlogs.Where(x => x.StockId == item.Stockid).FirstOrDefault();

                    loanLogToClear.Pending = 0;

                    DbInitialiser.Db.Loanlogs.Remove(loanLogToClear);

                }

                DbInitialiser.Db.SaveChanges();

                ClearAndRetrieveVirtualDb();

                LVBooksReturnedByUser.ClearValue(ItemsControl.ItemsSourceProperty);

                LVBooksReturnedByUser.ItemsSource = booksToValidate;

                MessageBox.Show("Du har nu validerat samtliga objekt!");
                return; 
            }

            MessageBox.Show("Det finns inga objekt att validera");
            return; 

        }

    }
}
