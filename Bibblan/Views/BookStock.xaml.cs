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
using System.Threading.Tasks;

namespace Bibblan.Views
{
    /// <summary>
    /// Interaction logic for BookStock.xaml
    /// </summary>
    public partial class BookStock : Page
    {
        List<Stock> dbVirtual = new List<Stock>();
        public BookStock()
        {
            InitializeComponent();
            Task dbDownload = Task.Run(() =>
            {
                foreach (var item in DbInitialiser.Db.Stocks)
                {
                    dbVirtual.Add(item);
                }
            });
        }

        private void removeBookButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void addBooksButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void searchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            Task.WaitAll();

            LVBookStock.ClearValue(ItemsControl.ItemsSourceProperty);

            if (searchBar.Text.Length > 0)
            {
                List<Stock> showBooks = dbVirtual.Where(x => x.Isbn.ToString().Contains(searchBar.Text.ToLower())).ToList(); //tar fram böckerna som innehåller userinput för TITEL just nu, ska läggas till mer än bara titel

                LVBookStock.ItemsSource = showBooks;
            }
            
            if (searchBar.Text == null || searchBar.Text == "")
            {
                LVBookStock.ClearValue(ItemsControl.ItemsSourceProperty);
                return;
            }
            
        }
    }
}
