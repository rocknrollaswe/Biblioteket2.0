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
    /// Interaction logic for Rapport.xaml
    /// </summary>
    public partial class Rapport : Page
    {
        List<Book> dbVirtual = new List<Book>();
        List<Stock> dbVirtual2 = new List<Stock>();
        public Rapport()
        {
            InitializeComponent();
            DatabaseInitialiser();

        }

        private void DatabaseInitialiser()
        {
            foreach (var item in DbInitialiser.Db.Books)
            {
                dbVirtual.Add(item);
            }
            foreach (var item in DbInitialiser.Db.Stocks)
            {
                dbVirtual2.Add(item);
            }
        }


        private void seeUserButton_Click(object sender, RoutedEventArgs e)
        {

        }
        private void seeDeletedObjects_Click(object sender, RoutedEventArgs e)
        {
            var fan = dbVirtual2.Join(
                            dbVirtual,
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
                            }) ;


            LVReportUser.Visibility = Visibility.Hidden;
            LVReportObject.Visibility = Visibility.Visible;

            DataContext = fan;
            LVReportObjectView.ItemsSource = fan.Where(x => x.Discarded == 1); //StockId, Isbn, Comment, Condition, Discarded
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
