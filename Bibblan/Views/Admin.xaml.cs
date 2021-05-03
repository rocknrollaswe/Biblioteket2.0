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
using System.Windows.Shapes;

namespace Bibblan.Views
{
    /// <summary>
    /// Interaction logic for Admin.xaml
    /// </summary>
    public partial class Admin : Window
    {
        public Admin()
        {
            InitializeComponent();

            string SSN = "123456789011";
            var test = DbInitialiser.Db.Users.Where(x => x.Socialsecuritynumber == Encryption.Encrypt(SSN)).SingleOrDefault(); //plockar ut användare i databasen med angiven SSN || Ev problem nu när vi har flera av samma SSN i databasen. SingleOrDefault() funkar bara om det finns ett specifikt SSN i hela databasen
            DbInitialiser.Db.Users.Remove(test);                                                                               //Tar bort den utplockade användaren
                                                                                                                               //Detta ska göras om till admin/bibliotikarie ta bort användare funktion!!
        }
        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void UserButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BooksButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Seminarie_Click(object sender, RoutedEventArgs e)
        {

        }

        private void RapportButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
