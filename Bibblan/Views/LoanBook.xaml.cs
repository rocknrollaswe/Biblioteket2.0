using Bibblan.Models;
using Bibblan.Services;
using System;
using System.Collections.Generic;
using System.Linq;
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

namespace Bibblan.Views
{
    /// <summary>
    /// Interaction logic for LoanBook.xaml
    /// </summary>
    public partial class LoanBook : Page
    {
        BookStockLoan b = new BookStockLoan();  
        List<BookStockLoan> virtualBooksToLoan = new List<BookStockLoan>();
        public LoanBook()
        {            
            InitializeComponent();
            ClearAndRetrieveVirtualDb();
            Validation();

            LVLoanBook.ItemsSource = virtualBooksToLoan;     
        }
        public void Validation()
        {
            if (GlobalClass.loanPermission == 0 && GlobalClass.userPermission == 0) //Gömmer för ordinarie användare utan lånekort
            {
                loanButton.Visibility = Visibility.Collapsed; //Låna knappen göms
            }
        }
        private void searchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            Searchfunction();
        }
        private void Searchfunction()
        {
            LVLoanBook.ClearValue(ItemsControl.ItemsSourceProperty);

            List<BookStockLoan> bookList = virtualBooksToLoan.Where(x => x.Title.ToLower().Contains(searchBar.Text.ToLower())
                                                    || x.Author.ToLower().Contains(searchBar.Text.ToLower())
                                                    || x.Category.ToString().Contains(searchBar.Text.ToLower()))

                                                   .ToList(); //tar fram böckerna som innehåller userinput för TITEL 

            if (bookList != null) // VÄLDIGT simpel sökfunktion, ska byggas på
            {
                LVLoanBook.ItemsSource = bookList;
                return;
            }
            else if (Int32.TryParse(searchBar.Text, out var _)) //kollar om userInput är en int eller ej
            {
                List<BookStockLoan> query = virtualBooksToLoan.Where(x => x.Title.ToLower().Contains(searchBar.Text.ToLower())).DefaultIfEmpty().ToList();
                LVLoanBook.ItemsSource = query;
                return;
            }
        }
        private void LVLoanBook_SelectionChanged(object sender, SelectionChangedEventArgs e)
        { 
            if (LVLoanBook.SelectedItem != null)
            {
                b = LVLoanBook.SelectedItem as BookStockLoan;
                
                MessageBox.Show($"Beskrivning:\n{b.Description}");
            }
        }
        private void loanButton_Click(object sender, RoutedEventArgs e)
        {
            if (GlobalClass.userPermission < 0) { MessageBox.Show("Du har inte behörighet att göra detta"); return; }

            User query = DbInitialiser.Db.Users.Where(x => x.UserId == GlobalClass.currentUserID).FirstOrDefault();
            if (query.HasLoanCard == 1)
            {
                Loanlog loanLog = new Loanlog();
                BookStockLoan b = LVLoanBook.SelectedItem as BookStockLoan;

                var bookToLoan = DbInitialiser.Db.Stocks.Where(x => x.Isbn == b.Isbn && x.Available != 0).FirstOrDefault();

                if (bookToLoan== null)
                {
                    MessageBox.Show("Boken du vill låna är inte tillgänglig för tillfället");
                    return;
                }
                else
                {
                    MessageBoxResult result = MessageBox.Show("Är det säkert att du vill låna den här boken?", "Meddelande", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);

                    switch (result)
                    {
                        case MessageBoxResult.Yes:
                            {
                                bookToLoan.Available = 0; // sätter tillgänglighet på aktuell bok till 'ej tillänglig'

                                loanLog.StockId = bookToLoan.StockId;              //Skapar upp ny loanlog och populerar den
                                loanLog.UserId = (int)GlobalClass.currentUserID;
                                loanLog.Loandate = DateTime.Now.Date;
                                loanLog.Returndate = DateTime.Now.AddMonths(1);

                                DbInitialiser.Db.Loanlogs.Add(loanLog);
                                DbInitialiser.Db.Stocks.Update(bookToLoan);

                                DbInitialiser.Db.SaveChanges(); // sparar databasen

                                ClearAndRetrieveVirtualDb();
                                LVLoanBook.ClearValue(ItemsControl.ItemsSourceProperty);
                                LVLoanBook.ItemsSource = virtualBooksToLoan;
                                MessageBox.Show($"Du har nu lånat {b.Title}.\nDatum för återlämning är sen {loanLog.Returndate}");
                            }
                            break;
                        case MessageBoxResult.No:
                        return;
                    }
                }
                return;
            }
            else
            {
                MessageBox.Show("Du har inga låneprivilegier på ditt lånekort för tillfället. Kontakta bibliotekarie.");
                return;
            }
        }
        public void ClearAndRetrieveVirtualDb()
        {
            virtualBooksToLoan.Clear();

            string titleTemp = "";
            int? availableTemp = -1;
            int counter = 0;

            foreach (var item in DbInitialiser.Db.BookStockLoans)
            {
                
                if (item.Title == titleTemp && item.Available == availableTemp)
                {
                    continue;
                }
                if (item.Title == titleTemp && item.Available != 0)
                {
                    virtualBooksToLoan.Remove(virtualBooksToLoan[counter - 1]);
                    virtualBooksToLoan.Add(item);
                    titleTemp = item.Title;
                    availableTemp = item.Available;
                   
                    continue;
                }
                if (item.Title == titleTemp && availableTemp == 1)
                {
                    continue;
                }
                if(item.Available == null)
                {
                    continue; 
                }

                virtualBooksToLoan.Add(item);
                titleTemp = item.Title;
                availableTemp = item.Available;
                counter++;
            }
        }
    }
}
