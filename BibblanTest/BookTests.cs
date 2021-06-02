using System;
using Bibblan.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Bibblan.Models;


namespace BibblanTest
{ 
    [TestClass]
      
    public class BookTests
    {

        [TestMethod]
        public void AddBookTest()
        {
            //Arrange

            bool BookEquals(Book actual, Book other)
            {
            if (actual.Title == other.Title && actual.Author == other.Author && actual.Description == other.Description && actual.Category == other.Category)
            return true;

            else return false;
            }

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

            
            //Act
            Book actual = new Book() { Title = title, Author = author, Description = description, Edition = int.Parse(edition), Price = int.Parse(price), Ddk = int.Parse(ddk), Sab = sab, Publisher = publisher, Category = isEbook };
           
            Book bookToCheck = BookService.AddBook(title, author, description, edition, price, ddk, sab, publisher, isEbook);

            Book bookToFail = BookService.AddBook("Inte alls namnet på Flugornas Herre", author, description, edition, price, ddk, sab, publisher, isEbook); 

            //Assert
            Assert.IsTrue(BookEquals(actual, bookToCheck));
            Assert.IsFalse(BookEquals(actual, bookToFail)); 

            //BiblioteketContext.testConnectionString = "Server = tcp:bladerunnerdb.database.windows.net,1433; Initial Catalog = Biblioteket_Kopiera; Persist Security Info = False; User ID = harrison; " +
            //    "Password = Blade1234; MultipleActiveResultSets = False; Encrypt = True; TrustServerCertificate = False; Connection Timeout = 30"; 

            //DbInitialiser.InitialiseDB(); 

            //DbInitialiser.Db.Books.Add(bookToCheck);
            //DbInitialiser.Db.SaveChanges(); 

        }

       







    }

    






}
