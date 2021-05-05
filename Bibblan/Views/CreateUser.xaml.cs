using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Security.Cryptography;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Bibblan.Models;
using Bibblan.Services;
using System.Linq;

namespace Bibblan.Views
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class CreateUser : Window
    {
        public CreateUser()
        {
            InitializeComponent();
            WrongEntry += Alert;
        }

        public event EventHandler<string> WrongEntry;
        protected virtual void OnWrongEntry(string e)
        {
            WrongEntry?.Invoke(this, e);
        }
        static void Alert(object sender, string message)
        {
            MessageBox.Show($"Meddelande!\n{message}");
            return;
        }

        private void CreateUserButton(object sender, RoutedEventArgs e)
        {

            if (firstName.Text == "" || lastName.Text == "" || eMail.Text == "" || SSN.Text == "") //Kollar om user input är tomt
            {
                OnWrongEntry("Du har inte angett data i samtliga fält!"); //test
                return;
            }

            var user = new User();

            if (DbInitialiser.Db.Users.Where(y => y.Socialsecuritynumber == Encryption.Encrypt(SSN.Text)).FirstOrDefault() != null) //kollar om angivna SSN redan existerar i DB:n
            {
                MessageBox.Show("Användaren existerar redan"); //detta ska ändras till ett event
                return;
            }

            user.Firstname = firstName.Text;
            user.Lastname = lastName.Text;
            user.Email = eMail.Text;
            user.Permissions = 0; //Detta ska admin kunna ändra senare

            user.Socialsecuritynumber = Encryption.Encrypt(SSN.Text); //Flyttade encryption metoden till Services.Encryption.cs, så vi kan använda den överallt i programmet. 
            user.Username = Encryption.Encrypt(userName.Text);
            user.Password = Encryption.Encrypt(passWord.Text);

            DbInitialiser.Db.Add(user);
            DbInitialiser.Db.SaveChanges();
        }
    }
}
