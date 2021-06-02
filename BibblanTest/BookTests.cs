using System;
using Bibblan.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Bibblan.Models;
using System.Linq; 


namespace BibblanTest
{ 
    [TestClass]
      
    public class BookTests
    { 
            
        [TestMethod]
        public void AddBookTest()
        {  
            //Arrange
            BiblioteketContext.testConnectionString = "Server = tcp:bladerunnerdb.database.windows.net,1433; Initial Catalog = Biblioteket_Kopiera; Persist Security Info = False; User ID = harrison; " +
            "Password = Blade1234; MultipleActiveResultSets = False; Encrypt = True; TrustServerCertificate = False; Connection Timeout = 30";

            DbInitialiser.InitialiseDB();

          
            bool BookEquals(Book actual, Book other)   //Metod som kollar om b�ckerna �r 'likadana', v�r tidigare override i Equals i Booksmodellen skapade nya problem med grundl�ggande funktioner 
            {
            if (actual.Title == other.Title && actual.Author == other.Author && actual.Description == other.Description && actual.Category == other.Category)
            return true;

            else return false;
            }

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

            
            //Act
            Book actual = new Book() { Title = title, Author = author, Description = description, Edition = int.Parse(edition), Price = int.Parse(price), Ddk = int.Parse(ddk), Sab = sab, Publisher = publisher, Category = isEbook };
           
            Book bookToCheck = BookService.AddBook(title, author, description, edition, price, ddk, sab, publisher, isEbook); //Checkar om vi f�tt in samma bok, b�de lokalt och i databasen
            Book bookToCheckFromDb = DbInitialiser.Db.Books.OrderBy(x=> x.Isbn).Last();  

            Book bookToFail = BookService.AddBook("Inte alls namnet p� Flugornas Herre", author, description, edition, price, ddk, sab, publisher, isEbook); //Checkar om vi f�tt in samma bok, b�de lokalt och i databasen
            Book bookToFailFromDb = DbInitialiser.Db.Books.OrderBy(x => x.Isbn).Last();

            //Assert
            Assert.IsTrue(BookEquals(actual, bookToCheck)); 
            Assert.IsTrue(BookEquals(actual, bookToCheckFromDb)); 
            Assert.IsFalse(BookEquals(actual, bookToFail));
            Assert.IsFalse(BookEquals(actual, bookToFailFromDb)); 

            DbInitialiser.Db.Books.Remove(bookToCheck); // Tar bort b�ckerna fr�n testdatabasen n�r testet �r klart.
            DbInitialiser.Db.Books.Remove(bookToFail);
            DbInitialiser.Db.SaveChanges();

        }









    }

    






}
