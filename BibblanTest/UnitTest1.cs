using System;
using Bibblan.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Bibblan.Models;


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
            string description = "Efter en flygkrasch uppt�cker en grupp engelska skolpojkar(6 - 12 �r) att de befinner sig p� en �de � i Stilla havet." +
                "Pojkarna har evakuerats fr�n England p� grund av ett k�rnvapenkrig.Ingen vuxen finns p� �n och pojkarna organiserar sig s� gott de kan, glada �ver sin nyvunna frihet.";
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

       







    }

    






}
