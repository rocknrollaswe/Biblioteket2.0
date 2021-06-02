using System;
using System.Collections.Generic;
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
    }
}
