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
using System.Threading;
using Microsoft.VisualBasic;
using System.Text.RegularExpressions;
using System.ComponentModel;

namespace Bibblan.Views
{
    /// <summary>
    /// Interaction logic for BookStock.xaml
    /// </summary>
    public partial class BookStock : Page
    {
        List<Stock> dbVirtual = new List<Stock>();
        IEnumerable<Stock> defaultStocks;
        Stock selectedStock = new Stock();
        DateTime timerCheat;
        public BookStock()
        {
            InitializeComponent();

            foreach (var item in DbInitialiser.Db.Stocks)
            {
                dbVirtual.Add(item);
            }

            searchBar.Text = "Sök";
            defaultStocks = dbVirtual.Where(x => x.Isbn.ToString() == GlobalClass.chosenBook.Isbn.ToString()).ToList();

            if (GlobalClass.chosenBookReport != null) //går in här om användaren kommer ifrån rapporteringssidan
            {
                var userReportStock = dbVirtual.Where(x => x.StockId == GlobalClass.chosenBookReport.StockId).ToList();
                LVBookStock.ItemsSource = userReportStock;
            }
            else //går in här om användaren kommer ifrån rapporteringssidan
            {
                LVBookStock.ItemsSource = defaultStocks;
            }

            bookTitle.Content = GlobalClass.chosenBook.Title;

            if(GlobalClass.chosenBook.Category == 1)
            {
                bookTitle.Content += " (E-bok)";
            }
        }
        private void removeBookButton_Click(object sender, RoutedEventArgs e)
        {
            if (GlobalClass.userPermission < 1) { MessageBox.Show("Du har inte behörighet att göra detta"); return; }

            if (LVBookStock.SelectedItem != null)
            {
                if(commentComboBox.SelectedItem != null)
                {
                    ComboBoxItem commentSelected = (ComboBoxItem)commentComboBox.SelectedItem;

                    var dbStockItem = DbInitialiser.Db.Stocks.Where(x => x.StockId == selectedStock.StockId).SingleOrDefault();
                    dbStockItem.Comment = commentSelected.Content.ToString();
                    dbStockItem.Discarded = 1;
                    dbStockItem.Available = 0; 
                    DbInitialiser.Db.SaveChanges();

                    LVBookStock.ClearValue(ItemsControl.ItemsSourceProperty);
                    ClearAndRetrieveVirtualDb();
                    LVBookStock.ItemsSource = dbVirtual.Where(x => x.Isbn == GlobalClass.chosenBook.Isbn);

                    MessageBox.Show("Bok utrangerad!");
                }
                else
                {
                    MessageBox.Show("Lägg till kommentar angående varför exemplaret utrangeras!");
                }
            }
            else
            {
                MessageBox.Show("Välj ett exemplar!");
            }
        }
        private void addBooksButton_Click(object sender, RoutedEventArgs e)
        {
            if (GlobalClass.userPermission < 1) { MessageBox.Show("Du har inte behörighet att göra detta"); return; }

            if (Regex.IsMatch(amountBox.Text, @"^([0-9])$"))
            {
                for(int i = 0; i < Convert.ToInt32(amountBox.Text); i++)
                {
                    Stock dbInput = new Stock() { Isbn = GlobalClass.chosenBook.Isbn, Condition = "Nyskick", Discarded = 0, Available = 1};
                    DbInitialiser.Db.Stocks.Add(dbInput);
                    DbInitialiser.Db.SaveChanges();

                    LVBookStock.ClearValue(ItemsControl.ItemsSourceProperty);
                    ClearAndRetrieveVirtualDb();
                    LVBookStock.ItemsSource = dbVirtual.Where(x => x.Isbn == GlobalClass.chosenBook.Isbn);
                }
                MessageBox.Show($"{amountBox.Text} böcker tillagda!");
            }
            else
            {
                MessageBox.Show("Vänligen sätt en mängd i siffror");
            }
        }
        private void searchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (searchBar.Text.Length >= 2 && searchBar.Text != "Sök")
            {
                LVBookStock.ClearValue(ItemsControl.ItemsSourceProperty);
#nullable enable
                List<Stock>? showStocks = new List<Stock>();
#nullable disable
                showStocks = defaultStocks.Where(x => x.Condition.ToLower().Contains(searchBar.Text.ToLower())).DefaultIfEmpty().ToList();

                LVBookStock.ItemsSource = showStocks;
            }
            else if (searchBar.Text == "0")
            {
                LVBookStock.ClearValue(ItemsControl.ItemsSourceProperty);
#nullable enable
                List<Stock>? showStocks = new List<Stock>();
#nullable disable

                if (Int32.TryParse(searchBar.Text, out _) == true)
                {
                    showStocks = defaultStocks.Where(x => x.Discarded == Convert.ToInt32(searchBar.Text)).DefaultIfEmpty().ToList();
                    LVBookStock.ItemsSource = showStocks;
                }
            }
            else if(searchBar.Text == "1")
            {
                LVBookStock.ClearValue(ItemsControl.ItemsSourceProperty);
#nullable enable
                List<Stock>? showStocks = new List<Stock>();
#nullable disable
                if (Int32.TryParse(searchBar.Text, out _) == true)
                {
                    showStocks = defaultStocks.Where(x => x.Discarded == Convert.ToInt32(searchBar.Text)).DefaultIfEmpty().ToList();
                    LVBookStock.ItemsSource = showStocks;
                }
            }
            else
            {
                LVBookStock.ClearValue(ItemsControl.ItemsSourceProperty);
                LVBookStock.ItemsSource = defaultStocks;
            }

        }
        private void LVBookStock_Selected(object sender, RoutedEventArgs e)
        {
            timerCheat = DateTime.Now;
            if (LVBookStock.SelectedItem != null)
            {
                selectedStock = LVBookStock.SelectedItem as Stock;
                isbnBox.Foreground = Brushes.Black;
                isbnBox.Text = selectedStock.Isbn.ToString();
                stockIdBox.Foreground = Brushes.Black;
                stockIdBox.Text = selectedStock.StockId.ToString();
                if(selectedStock.Comment != null)
                {
                    commentComboBox.Text = selectedStock.Comment.ToString();
                }
                if(selectedStock.Condition != null)
                {
                    conditionComboBox.Text = selectedStock.Condition.ToString();
                }
            }
            else
            {
                isbnBox.Foreground = Brushes.LightGray;
                isbnBox.Text = "";
                stockIdBox.Foreground = Brushes.LightGray;
                stockIdBox.Text = "";
            }
        }
        private void conditionComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (GlobalClass.userPermission < 1) { MessageBox.Show("Du har inte behörighet att göra detta"); return; }

            var dateTimeBug = DateTime.Now;
            if(dateTimeBug > timerCheat.AddSeconds(1)) //hejdå käre bugg, det var kul så länge det varade. Men nu måste vi lägga ned dig i din grav, puss :*
            {
                if (selectedStock != null && conditionComboBox.SelectedItem != null)
                {
                    var dbStockItem = DbInitialiser.Db.Stocks.Where(x => x.StockId == selectedStock.StockId).Last();//   SingleOrDefault();
                    ComboBoxItem comboBoxSelection = (ComboBoxItem)conditionComboBox.SelectedItem;
                    dbStockItem.Condition = comboBoxSelection.Content.ToString();
                    DbInitialiser.Db.SaveChanges();

                    ICollectionView view = CollectionViewSource.GetDefaultView(LVBookStock.ItemsSource);
                    view.Refresh();
                }
            }
        }
        private void searchBar_GotFocus(object sender, RoutedEventArgs e)
        {
            Thematics.Watermark.ForFocus(searchBar);
        }
        private void searchBar_LostFocus(object sender, RoutedEventArgs e)
        {
            Thematics.Watermark.ForLostFocus(searchBar, "Sök");
        }
        private void amountBox_GotFocus(object sender, RoutedEventArgs e)
        {
            Thematics.Watermark.ForFocus(amountBox);
        }
        private void amountBox_LostFocus(object sender, RoutedEventArgs e)
        {
            Thematics.Watermark.ForLostFocus(amountBox, "antal");
        }
        public void ClearAndRetrieveVirtualDb()
        {
            dbVirtual.Clear();

            foreach (var item in DbInitialiser.Db.Stocks)
            {
                dbVirtual.Add(item);
            }
        }
    }
}
