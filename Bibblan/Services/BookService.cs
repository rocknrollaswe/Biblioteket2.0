using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bibblan.Models; 

namespace Bibblan.Services
{
    
    public static class BookService
    {
        public static Book AddBook(string title, string author, string description, string edition, string price, string ddk, string sab, string publisher, int isEbook1Else0)
        {
            var book = new Book();

            book.Title = title;
            book.Author = author;
            book.Description = description;
            book.Edition = int.Parse(edition);
            book.Price = int.Parse(price);
            book.Ddk = int.Parse(ddk);
            book.Sab = sab;
            book.Publisher = publisher;
            book.Category = isEbook1Else0;

            DbInitialiser.Db.Add(book); 
            DbInitialiser.Db.SaveChanges();
         
            return book; 
        }
        public static Loanlog AddLoanlog(int stockId, int userId, DateTime loanDate, DateTime returnDate)
        {
            Loanlog loanLog = new Loanlog();

            loanLog.StockId = stockId;
            loanLog.UserId = userId;
            loanLog.Loandate = loanDate;
            loanLog.Returndate = returnDate;

            DbInitialiser.Db.Loanlogs.Add(loanLog);
            DbInitialiser.Db.SaveChanges();

            return loanLog;
        }
        public static void AddStockBook(Book book, int amount)
        {
            IEnumerable<Book> isbnBook = DbInitialiser.Db.Books.Where
                (b => b.Title == book.Title && b.Edition == book.Edition && b.Category == book.Category); // hämtar isbn för den nyss tillagda boken

            Book b = isbnBook.FirstOrDefault();

            for (int i = 0; i < amount; i++)
            {
                var stock = new Stock();
                stock.Isbn = Convert.ToInt32(b.Isbn);
                stock.Condition = "Nyskick";
                stock.Discarded = 0;
                stock.Available = 1;
                DbInitialiser.Db.Stocks.Add(stock);
            }
            DbInitialiser.Db.SaveChanges();
        }
    }
}
