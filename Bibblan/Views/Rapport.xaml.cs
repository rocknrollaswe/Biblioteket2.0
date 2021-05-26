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
        List<Loanlog> dbVirtual = new List<Loanlog>();
        public Rapport()
        {
            InitializeComponent();

            //foreach (var item in DbInitialiser.Db.Loanlogs)
            //{
            //    dbVirtual.Add(item);
            //}
        }


        private void seeUserButton_Click(object sender, RoutedEventArgs e)
        {
            List<UserReport> bigJoin = new List<UserReport>();

            LVReport.ItemsSource = bigJoin;

            //var join2Test = DbInitialiser.Db.Loanlogs.Join(DbInitialiser.Db.Stocks,x => x.StockId,c => c.StockId,(x, c) => new{isbn = c.Isbn, stockId = x.StockId}).ToList();
            //var join3Test = join2Test.Join(DbInitialiser.Db.Books,x => x.isbn,c => c.Isbn, (x, c) => new{title = c.Title}).ToList();
        }
        private void seeDeletedObjects_Click(object sender, RoutedEventArgs e)
        {

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
