using System;
using Bibblan.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BibblanTest
{ 
    [TestClass]
    public class UnitTest1
    {
       [TestMethod]
       [DataRow("Hej", "Hej")]
       [DataRow("Tja", "Tjena")]
        public void EncryptionTestShouldReturn(string actual, string expected)
        {
            //Act
            byte[] actualEncrypted = Bibblan.Services.Encryption.Encrypt(actual);
            byte[] expectedEncrypted = Bibblan.Services.Encryption.Encrypt(expected);

            //Assert
            Assert.AreEqual(actual, expected); 

            



        }
  
    
    
    
    
    
    
    
    
    
    }
}
