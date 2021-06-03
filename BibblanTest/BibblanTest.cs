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
using System.Windows.Navigation;
using Bibblan.Models;
using Bibblan.Services;
using Bibblan.Views;
using Bibblan;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace BibblanTest
{ 
    [TestClass]
      
    public class UnitTest1
    {

        [TestMethod]
        public void AddBookTest()
        {
            //arrange
            string title = "Flugornas Herre";
            string author = "William Golding";
            string description = "Efter en flygkrasch upptäcker en grupp engelska skolpojkar(6 - 12 år) att de befinner sig på en öde ö i Stilla havet." +
                "Pojkarna har evakuerats från England på grund av ett kärnvapenkrig.Ingen vuxen finns på ön och pojkarna organiserar sig så gott de kan, glada över sin nyvunna frihet.";
            string edition = "1959";
            string price = "10";
            string ddk = "800";
            string sab = "H";
            string publisher = "Faber and Faber";
            int isEbook = 0;

            Book actual = new Book() { Title = title, Author = author, Description = description, Edition = int.Parse(edition), Price = int.Parse(price), Ddk = int.Parse(ddk), Sab = sab, Publisher = publisher, Category = isEbook };
            

            Book bookToCheck = BookService.AddBook(title, author, description, edition, price, ddk, sab, publisher, isEbook);

            //assert
            Assert.AreEqual(actual, bookToCheck);

            BiblioteketContext.testConnectionString = "Server = tcp:bladerunnerdb.database.windows.net,1433; Initial Catalog = Biblioteket_Kopiera; Persist Security Info = False; User ID = harrison; " +
                "Password = Blade1234; MultipleActiveResultSets = False; Encrypt = True; TrustServerCertificate = False; Connection Timeout = 30"; 

            DbInitialiser.InitialiseDB(); 

            DbInitialiser.Db.Books.Add(bookToCheck);
            DbInitialiser.Db.SaveChanges(); 

        }
        [TestClass]
        public class Login
        {
            [TestMethod]
            public void AuthorizedAccessTest() // Test for Admin
            {
                BookService.Login("email@gmail.com", "Password1");
                Assert.AreEqual(2, GlobalClass.userPermission);
            }
            [TestMethod]
            public void UnauthorizedAccessTest() // Test for Wrong User
            {
                BiblioteketContext.testConnectionString = "Server = tcp:bladerunnerdb.database.windows.net,1433; Initial Catalog = Biblioteket_Kopiera; Persist Security Info = False; User ID = harrison; " +
"Password = Blade1234; MultipleActiveResultSets = False; Encrypt = True; TrustServerCertificate = False; Connection Timeout = 30";

                DbInitialiser.InitialiseDB();
                GlobalClass.userPermission = -99;
                BookService.Login("evilhacker@email.com", "Password1");
                Assert.IsFalse(BookEquals(null, GlobalClass.userPermission));
            }
            bool BookEquals(int? Incorrectvalue, int? GlobalClass)   //Metod som kollar om böckerna är 'likadana', vår tidigare override i Equals i Booksmodellen skapade nya problem med grundläggande funktioner 
            {
                if (GlobalClass == Incorrectvalue)
                    return true;

                else return false;
            }
        }
    }

    






}
