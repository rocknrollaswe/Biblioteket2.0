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
using System.Linq;
using Bibblan.Services; 


namespace Bibblan.Views
{
    /// <summary>
    /// Interaction logic for WelcomePage.xaml
    /// </summary>
    public partial class WelcomePage : Page
    {
        List<StockLoanLogBooksJoin> booksToReturn = new List<StockLoanLogBooksJoin>();  
        public WelcomePage()
        {
            InitializeComponent();
            ClearAndRetrieveVirtualDb();

            LVBooksLoanedByUser.ItemsSource = booksToReturn;
        }

        private void ClearAndRetrieveVirtualDb()
        {
            booksToReturn.Clear(); 

            foreach (var item in DbInitialiser.Db.StockLoanLogBooksJoins)
            {
                if (item.Userid == GlobalClass.currentUserID && item.Pending != 1)
                {
                    booksToReturn.Add(item);
                }
                continue;
            }

        }

        private void ReturnBookButton_Click(object sender, RoutedEventArgs e)
        {
            
            var bookId = LVBooksLoanedByUser.SelectedItem as StockLoanLogBooksJoin;
            

            var bookToReturn = DbInitialiser.Db.Loanlogs.Where(x => x.StockId == bookId.Stockid).FirstOrDefault();
            bookToReturn.Pending = 1;
            DbInitialiser.Db.SaveChanges(); 

            ClearAndRetrieveVirtualDb();
            LVBooksLoanedByUser.ClearValue(ItemsControl.ItemsSourceProperty);
            LVBooksLoanedByUser.ItemsSource = booksToReturn;

            return; 
        }
        private void ReturnAllBooksButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in booksToReturn)
            {
                var book = DbInitialiser.Db.Loanlogs.Where(x => x.StockId == item.Stockid).FirstOrDefault();
                book.Pending = 1; 
                
            }
            DbInitialiser.Db.SaveChanges(); 

            ClearAndRetrieveVirtualDb();
            LVBooksLoanedByUser.ClearValue(ItemsControl.ItemsSourceProperty);
            LVBooksLoanedByUser.ItemsSource = booksToReturn;

            return;
        }

    }
}
