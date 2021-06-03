using System;
using System.Collections.Generic;
using System.Text;
using Bibblan.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Bibblan.Models;
using System.Linq;

namespace BibblanTest
{
    public class UserTests
    {
        public void SetupTestDb()
        {
            BiblioteketContext.testConnectionString = "Server = tcp:bladerunnerdb.database.windows.net,1433; Initial Catalog = Biblioteket_Kopiera; Persist Security Info = False; User ID = harrison; " +
            "Password = Blade1234; MultipleActiveResultSets = False; Encrypt = True; TrustServerCertificate = False; Connection Timeout = 30";

            DbInitialiser.InitialiseDB();
        }
    }

    [TestClass]
    public class UserTest
    {
        TestData td = new TestData();

        [TestMethod]
        public void RegUserTest()
        {
            //Arrange
            td.SetupTestDb();

            bool UserEquals(User actual, User other)
            {
                if (actual.Firstname == other.Firstname && actual.Lastname == other.Lastname && actual.Email == other.Email && actual.Socialsecuritynumber.SequenceEqual(other.Socialsecuritynumber) && actual.Password.SequenceEqual(other.Password))
                    return true;

                else return false;
            }

            string firstname = "Patricia";
            string lastname = "Warg";
            string email = "patrcia.warg@email.com";
            string ssn = "199509217894";
            string password = "Mango123";

            //Act
            User expected = new User() { Firstname = firstname, Lastname = lastname, Email = email, Socialsecuritynumber = Encryption.Encrypt(ssn), Password = Encryption.Encrypt(password)};
            User actual = UserService.RegisterUser(firstname, lastname, email, ssn, password);

            User expectedFalse = new User() { Firstname = "Amelia", Lastname = lastname, Email = email, Socialsecuritynumber = Encryption.Encrypt(ssn), Password = Encryption.Encrypt(password) };

            //Assert
            Assert.IsTrue(UserEquals(expected, actual));
            Assert.IsFalse(UserEquals(expectedFalse, actual));

            //CleanUp
            DbInitialiser.Db.Users.Remove(actual); // Tar bort användaren från testdatabasen när testet är klart.
            DbInitialiser.Db.SaveChanges();


        }







        }


    }

