using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Bibblan.Models;
using Bibblan.Services;

namespace Bibblan.Views
{
    /// <summary>
    /// Interaction logic for CreateUser.xaml
    /// </summary>
    public partial class CreateUser : Window
    {
        public CreateUser()
        {
            InitializeComponent();
        }

        private void CreateUserButton(object sender, RoutedEventArgs e)
        {

            if(firstName.Text == "" || lastName.Text == "" || eMail.Text == "" || SSN.Text == "") { return; }

            int check = 0;
            var user = new User();
            var encryptCheck = Encrypt(SSN.ToString());

            foreach (var item in DbInitialiser.Db.Users)
            {
                for (int i = 0; i < item.Socialsecuritynumber.Length; i++)
                {
                    if (item.Socialsecuritynumber[i] == encryptCheck[i])
                    {
                        check++;
                    }
                }
                if (check == item.Socialsecuritynumber.Length)
                {
                    Console.WriteLine("The user already exists!");
                    return;
                }
            }

            user.Firstname = firstName.Text;
            user.Lastname = lastName.Text;
            user.Email = eMail.Text;
            user.Permissions = 0;

            user.Socialsecuritynumber = Encrypt(SSN.Text);
            user.Username = Encrypt(userName.Text);
            user.Password = Encrypt(passWord.Text);

            DbInitialiser.Db.Add(user);
            DbInitialiser.Db.SaveChanges();
        }


        public byte[] Encrypt(string toEncrypt)
        {

            byte[] data = Encoding.UTF8.GetBytes(toEncrypt); //Gör om string till bytearray

            byte[] encryptedArray = SHA256.Create().ComputeHash(data); //Krypterar bytearray

            return encryptedArray;

        }
    }
}
