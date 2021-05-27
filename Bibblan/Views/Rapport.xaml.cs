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
    /// Interaction logic for Rapport.xaml
    /// </summary>
    public partial class Rapport : Page
    {
        List<UserReport> dbVirtual = new List<UserReport>();
        List<Book> dbVirtualBooks = new List<Book>();
        List<Stock> dbVirtualStocks = new List<Stock>();
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
            if(epostTextBox.Text != null || epostTextBox.Text != "")
            {
                var userReport = dbVirtual.Where(x => x.Email.Contains(epostTextBox.Text));
                LVReport.ItemsSource = userReport;
            }
            else
            {
                MessageBox.Show("Var vänlig fyll i en e-post adress");
            }
        }
        private void seeDeletedObjects_Click(object sender, RoutedEventArgs e)
        {
            IEnumerable<dynamic> fan = dbVirtualStocks.Join(
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

            LVReportUser.Visibility = Visibility.Hidden;
            LVReportObject.Visibility = Visibility.Visible;

            DataContext = fan;
            LVReportObjectView.ItemsSource = fan.Where(x => x.Discarded == 1); //StockId, Isbn, Comment, Condition, Discarded
            blah(fan);

        }

        private void blah(IEnumerable<dynamic> Fan)
        {
            string filepath = "shit.txt";
            try
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(filepath, false))
                {
                    foreach (var item in Fan.Where(x => x.Discarded == 1))
                    {
                        file.WriteLine(item.StockId + "," + item.Isbn + "," + item.BookTitle + "," + item.Edition + "," + item.Comment + "," + item.Condition);
                    }               
                }
            }
            catch(Exception ex)
            {
               MessageBox.Show(ex.Message);
            }
        }
        private void bookStockButton_Click(object sender, RoutedEventArgs e)
        {
            if(LVReport.SelectedItem != null)
            {
                GlobalClass.chosenBookReport = LVReport.SelectedItem as UserReport;

                this.NavigationService.Navigate(new BookStock());
            }
            else
            {
                MessageBox.Show("Var vänlig tryck i ett värde i listan");
            }
        }

        private void epostTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (epostTextBox.Foreground == Brushes.LightGray)
            {
                epostTextBox.Text = "";
                epostTextBox.Foreground = Brushes.Black;
            }
        }
        private void epostTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (epostTextBox.Text == "" || epostTextBox.Text == null)
            {
                epostTextBox.Foreground = Brushes.LightGray;
                epostTextBox.Text = "E-post";
            }
        }
    }
}
