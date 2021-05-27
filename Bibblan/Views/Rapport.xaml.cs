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
        IEnumerable<UserReport> userReport;
        public Rapport()
        {
            InitializeComponent();

            foreach (var item in DbInitialiser.Db.UserReports)
            {
                dbVirtual.Add(item);
            }
        }
        private void seeUserButton_Click(object sender, RoutedEventArgs e)
        {
            if(epostTextBox.Text != null || epostTextBox.Text != "" || epostTextBox.Text != "E-post")
            {
                userReport = dbVirtual.Where(x => x.Email.Contains(epostTextBox.Text));
                LVReport.ItemsSource = userReport;
            }
            else
            {
                MessageBox.Show("Var vänlig fyll i en e-post adress");
            }
        }
        private void seeDeletedObjects_Click(object sender, RoutedEventArgs e)
        {

        }
        private void bookStockButton_Click(object sender, RoutedEventArgs e)
        {
            if(LVReport.SelectedItem != null)
            {
                UserReport chosenBookReport = LVReport.SelectedItem as UserReport;
                GlobalClass.chosenBookReport = chosenBookReport;
                Stock? stockToBook = DbInitialiser.Db.Stocks.Where(x => x.StockId == chosenBookReport.StockId).FirstOrDefault();
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

        private async void downloadReportButton_Click(object sender, RoutedEventArgs e)
        {
            if(LVReport.ItemsSource != null && userReport != null)
            {
                using (var streamWriter = new System.IO.StreamWriter("output.csv", false))
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
    }
}
