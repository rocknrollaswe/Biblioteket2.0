using Bibblan.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Bibblan.Services
{
    public class UserService
    {
        public static User RegisterUser(string firstname, string lastname, string email, string ssn, string password)
        {
            User registeredUser = new User();

            registeredUser.Firstname = firstname;
            registeredUser.Lastname = lastname;
            registeredUser.Email = email.ToLower();
            registeredUser.Socialsecuritynumber = Encryption.Encrypt(ssn);
            registeredUser.Password = Encryption.Encrypt(password);
            registeredUser.HasLoanCard = 0;
            registeredUser.Permissions = 0;

            DbInitialiser.Db.Add(registeredUser);
            DbInitialiser.Db.SaveChanges();

            return registeredUser;
        }
        public static bool Login(string emailBox, string passwordTextBox)
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
                    return true; 
                }
            }
            
            return false; 


        }
    }
}
