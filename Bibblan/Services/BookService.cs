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
using System.Windows.Navigation;
using Bibblan.Models;
using Bibblan.Services;
using Bibblan.Views;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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
      
              public static void Login(string emailBox, string passwordTextBox)
        {
            string emailLow = emailBox.ToLower();
            var userList = DbInitialiser.Db.Users.ToList();
            var connectedUser = userList.Find(x => x.Email == emailLow);
            if (connectedUser != null)
            {
                if (connectedUser.Password.SequenceEqual(Encryption.Encrypt(passwordTextBox)) == true) //SequenceEqual går igenom ByteArrayerna och checkar värdena mot varandra. Detta är en långsam funktion, dock så funkar den då vi inte har 10000 användare
                {
                    GlobalClass.userPermission = connectedUser.Permissions;  //sätter våra globala variabler för den specifika användaren
                    GlobalClass.userFirstName = connectedUser.Firstname;       //FYLL PÅ HÄR OM VI BEHÖVER FLER GLOBALA VARIABLER
                    GlobalClass.currentUserID = connectedUser.UserId;
                    GlobalClass.loanPermission = connectedUser.HasLoanCard;
                }
                else
                {
                    MessageBox.Show("Fel uppgifter angivna");
                }
            }
            else
            {
                MessageBox.Show("Fel uppgifter angivna");
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
