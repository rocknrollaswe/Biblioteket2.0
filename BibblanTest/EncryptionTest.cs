using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Bibblan.Services;
using Bibblan.Models;

namespace BibblanTest
{
    [TestClass]
    public class EncryptionTest
    {
        public void SetupTestDb()
        {
            BiblioteketContext.testConnectionString = "Server = tcp:bladerunnerdb.database.windows.net,1433; Initial Catalog = Biblioteket_Kopiera; Persist Security Info = False; User ID = harrison; " +
            "Password = Blade1234; MultipleActiveResultSets = False; Encrypt = True; TrustServerCertificate = False; Connection Timeout = 30";

            DbInitialiser.InitialiseDB();
        }

        [TestMethod]

        public void EncryptTest()
        {
            SetupTestDb();

            string actual = "Hej hallå";
            string expectedSuccess = "Hej hallå";
            string expectedFail = "Tjena hejdå";

            byte[] actualEncrypted = Encryption.Encrypt(actual);
            byte[] expectedSuccessEncrypted = Encryption.Encrypt(expectedSuccess);
            byte[] expectedFailEncrypted = Encryption.Encrypt(expectedFail);

            Assert.IsTrue(actualEncrypted.SequenceEqual(expectedSuccessEncrypted));
            Assert.IsFalse(actualEncrypted.SequenceEqual(expectedFailEncrypted));
        }
    }
}
