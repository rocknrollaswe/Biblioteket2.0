using Bibblan.Models;
using System;
using System.Collections.Generic;
using System.Text;

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
        public static void Login(int userPermission, string userFirstName, int userId, byte? loanPermission)
        {
            GlobalClass.userPermission = userPermission;
            GlobalClass.userFirstName = userFirstName;
            GlobalClass.currentUserID = userId;
            GlobalClass.loanPermission = loanPermission;
        }
    }
}
