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




            //user.Firstname = firstName.Text;
            //user.Lastname = lastName.Text;
            //user.Email = eMail.Text.ToLower();
            //user.HasLoanCard = 0;
            //user.Permissions = 0; //Detta ska admin kunna ändra senare
            //user.Socialsecuritynumber = Encryption.Encrypt(SSN.Text); //Flyttade encryption metoden till Services.Encryption.cs, så vi kan använda den överallt i programmet. 
            //user.Password = Encryption.Encrypt(passWord.Password);
        }
    }
}
