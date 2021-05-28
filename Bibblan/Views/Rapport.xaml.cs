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
using System.Threading;
using System.Timers;

namespace Bibblan.Views
{
    /// <summary>
    /// Interaction logic for Rapport.xaml
    /// </summary>
    public partial class Rapport : Page
    {
        List<UserReport> dbVirtual = new List<UserReport>();
        IEnumerable<UserReport> userReport;
        List<Book> dbVirtualBooks = new List<Book>();
        List<Stock> dbVirtualStocks = new List<Stock>();
        IEnumerable<dynamic> objectJoin;
        public Rapport()
        {
            InitializeComponent();
            DatabaseInitialiser();
        }
        private void DatabaseInitialiser()
        {
            foreach (var item in DbInitialiser.Db.UserReports)
            {
                dbVirtual.Add(item);
            }
            foreach (var item in DbInitialiser.Db.Books)
            {
                dbVirtualBooks.Add(item);
            }
            foreach (var item in DbInitialiser.Db.Stocks)
            {
                dbVirtualStocks.Add(item);
            }
        }
        private void seeUserButton_Click(object sender, RoutedEventArgs e)
        {
            if(epostTextBox.Text != null || epostTextBox.Text != "" || epostTextBox.Text != "E-post")
            {
                userReport = dbVirtual.Where(x => x.Email.Contains(epostTextBox.Text));

                var userReportFinal = userReport.Join(DbInitialiser.Db.Users, x => x.Email, c => c.Email, (x, c) => new { firstName = c.Firstname, lastName = c.Lastname, email = x.Email, returnDate = x.Returndate, stockId = x.StockId, title = x.Title }).ToList();
                
                LVReportUser.ItemsSource = userReportFinal;

                userBorder.Visibility = Visibility.Visible;
                LVReportObject.Visibility = Visibility.Hidden;
            }
            else
            {
                MessageBox.Show("Var vänlig fyll i en e-post adress");
            }
        }
        private void seeDeletedObjects_Click(object sender, RoutedEventArgs e)
        {
            objectJoin = dbVirtualStocks.Join(
                dbVirtualBooks,
                x => x.Isbn,
                c => c.Isbn,
                (x, c) => new
                {
                    StockId = x.StockId,
                    Isbn = x.Isbn,
                    BookTitle = c.Title,
                    Edition = c.Edition,
                    Comment = x.Comment,
                    Condition = x.Condition,
                    Discarded = x.Discarded
                }); //Custom Variable (Join) to grab all the things we want to see in the ListView

            userBorder.Visibility = Visibility.Hidden;
            LVReportObject.Visibility = Visibility.Visible;

            DataContext = objectJoin;
            LVReportObjectView.ItemsSource = objectJoin.Where(x => x.Discarded == 1); //StockId, Isbn, Comment, Condition, Discarded
        }
        private void bookStockButton_Click(object sender, RoutedEventArgs e)
        {
            if(LVReportUser.SelectedItem != null)
            {
                var chosenBookReport = LVReportUser.SelectedItem as dynamic;
                UserReport userReportFinal = new UserReport() {Email = chosenBookReport.email, Returndate = chosenBookReport.returnDate, StockId = chosenBookReport.stockId, Title = chosenBookReport.title };

                
                GlobalClass.chosenBookReport = userReportFinal;
                Stock? stockToBook = DbInitialiser.Db.Stocks.Where(x => x.StockId == userReportFinal.StockId).FirstOrDefault();
                Book? bookToBookStock = DbInitialiser.Db.Books.Where(x => x.Isbn == stockToBook.Isbn).FirstOrDefault();

                GlobalClass.chosenBook = bookToBookStock;

                this.NavigationService.Navigate(new BookStock());
            }
            else
            {
                MessageBox.Show("Var vänlig tryck i ett värde i listan");
            }
        }
        private void epostTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            Thematics.Watermark.ForFocus(epostTextBox);
        }
        private void epostTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            Thematics.Watermark.ForLostFocus(epostTextBox, "E-post");
        }
        private void downloadReportButton_Click(object sender, RoutedEventArgs e)
        {
            if(userBorder.Visibility == Visibility.Visible)
            {
                downloadUserReport();
            }
            else if(LVReportObject.Visibility == Visibility.Visible)
            {
                removedObjectsReport(objectJoin);
            }
        }

        private async void downloadUserReport()
        {
            if (LVReportUser.ItemsSource != null && userReport != null)
            {
                using (var streamWriter = new System.IO.StreamWriter("userReport.csv", false))
                {
                    streamWriter.WriteLine($"               Email: {userReport.First().Email} \n");
                    foreach (var item in userReport)
                    {
                        await streamWriter.WriteLineAsync($"Titel: {item.Title}, StockID: {item.StockId}, Returdatum: {item.Returndate.ToShortDateString()} ");
                    }
                }
            }
            else
            {
                MessageBox.Show("Sök på en användare först!");
                return;
            }
        }
        private void removedObjectsReport(IEnumerable<dynamic> Fan)
        {
            try
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter("discardedReport.csv", false))
                {
                    foreach (var item in Fan.Where(x => x.Discarded == 1))
                    {
                        file.WriteLine(item.StockId + "," + item.Isbn + "," + item.BookTitle + "," + item.Edition + "," + item.Comment + "," + item.Condition);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
