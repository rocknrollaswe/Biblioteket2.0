using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Bibblan.Services
{
    public class Encryption
    {
        public static byte[] Encrypt(string toEncrypt)
        {

            byte[] data = Encoding.UTF8.GetBytes(toEncrypt); //Gör om string till bytearray

            byte[] encryptedArray = SHA256.Create().ComputeHash(data); //Krypterar bytearray

            return encryptedArray;
        }
    }
}
