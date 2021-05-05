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
using System.Windows.Shapes;
using Bibblan.Models;
using Bibblan.Services;
using System.Linq;

namespace Bibblan.Views
{
    /// <summary>
    /// Interaction logic for SearchBook.xaml
    /// </summary>
    public partial class SearchBook : Window
    {
        public SearchBook()
        {
            InitializeComponent();
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            if (DbInitialiser.Db.Books.Where(x => x.Title == SearchBox.Text || x.Sab == SearchBox.Text || x.Author == SearchBox.Text || x.Publisher == SearchBox.Text).FirstOrDefault() != null) // VÄLDIGT simpel sökfunktion, ska byggas på
            {
                MessageBox.Show("GRATTIS DU ÄR BÄST");
                return;
            }
            else if (DbInitialiser.Db.Books.Where(x => x.Isbn == Convert.ToInt32(SearchBox.Text)).FirstOrDefault() != null) // ev finna ett sätt att få in denna i övre if-satsen
            {
                MessageBox.Show("DET FUNKAR IAF.."); 
                return;
            }
            //var test = DbInitialiser.Db.Books.Find(DbInitialiser.Db.Books.Where(x => x.Title == SearchBox.Text));
        }
    }
}
