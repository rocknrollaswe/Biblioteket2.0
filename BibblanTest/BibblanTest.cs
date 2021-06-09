using System;
using System.Collections.Generic;
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
using Bibblan;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace BibblanTest
{
    [TestClass]

    public class UnitTest1
    {
        [TestClass]
        public class Login
        {
            [TestMethod]
            public void AuthorizedAccessTest() // Test for Admin
            {
                UserService.Login("email@gmail.com", "Password1");
                Assert.AreEqual(2, GlobalClass.userPermission);
                LogOut.LogOutUser();
            }
            [TestMethod]
            public void UnauthorizedAccessTest() // Test for Wrong User
            {
                BiblioteketContext.testConnectionString = "Server = tcp:bladerunnerdb.database.windows.net,1433; Initial Catalog = Biblioteket_Kopiera; Persist Security Info = False; User ID = harrison; " +
                                                          "Password = Blade1234; MultipleActiveResultSets = False; Encrypt = True; TrustServerCertificate = False; Connection Timeout = 30";
                DbInitialiser.InitialiseDB();

                UserService.Login("evilhacker@email.com", "Password1");
                Assert.IsFalse(UserEquals(0, GlobalClass.userPermission));
                Assert.IsFalse(UserEquals(1, GlobalClass.userPermission));
                Assert.IsFalse(UserEquals(2, GlobalClass.userPermission));
                LogOut.LogOutUser();
            }
            bool UserEquals(int? Incorrectvalue, int? GlobalClass)   //Metod som kollar om böckerna är 'likadana', vår tidigare override i Equals i Booksmodellen skapade nya problem med grundläggande funktioner 
            {
                if (GlobalClass == Incorrectvalue)
                    return true;

                else return false;
            }
        }
    }
}
