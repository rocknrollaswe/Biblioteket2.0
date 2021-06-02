﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Bibblan.Models;
using Bibblan.Services;
using System.Linq;
using System.Threading.Tasks;
using System.Text.RegularExpressions;


namespace Bibblan.Views
{
    /// <summary>
    /// Interaction logic for UserAdminPage.xaml
    /// </summary>
    public partial class UserAdminPage : Page
    {
        readonly List<User> dbVirtual = new List<User>();
        User u = new User(); 
        public UserAdminPage()
        {
            InitializeComponent();
            
            foreach (var item in DbInitialiser.Db.Users)
            {
                dbVirtual.Add(item);
            }

            LVModifyUser.ItemsSource = dbVirtual;
            CommentBox.DataContext = dbVirtual; 
        }
        private void LVModifyUser_SelectionChanged(object sender, SelectionChangedEventArgs e) // fyller i textboxarna på höger sida för ev ändring 
        {
            if (GlobalClass.userPermission < 1) { MessageBox.Show("Du har inte behörighet att göra detta"); return; }

            if (LVModifyUser.SelectedItem != null)
            {
                u = LVModifyUser.SelectedItem as User;

                firstName.Text = u.Firstname;
                lastName.Text = u.Lastname;
                eMail.Text = u.Email;
                permissionComboBox.SelectedIndex = u.Permissions;
                loanRightsComboBox.SelectedIndex = (byte)u.HasLoanCard;

                if(u.UserComment != "") 
                {
                    CommentBox.Text = u.UserComment;
                    CommentBox.Foreground = Brushes.Black;
                    if (CommentBox.Text == "Amärkningar") { CommentBox.Foreground = Brushes.LightGray;}    
                }
                firstName.Foreground = Brushes.Black;
                lastName.Foreground = Brushes.Black;
                eMail.Foreground = Brushes.Black;
            }
            return; 
        }
        private void rButtonChangeUser_Click(object sender, RoutedEventArgs e) // ändrar content på den orangea knappen beroende på iklickat val
        {
            if (GlobalClass.userPermission < 1) { MessageBox.Show("Du har inte behörighet att göra detta"); return; }

            if (rButtonChangeUser.IsChecked == true) { ButtonChangeUser.Content = rButtonChangeUser.Content; ButtonChangeUser.FontSize = 16; }
        }
        private void rButtonRemoveUser_Click(object sender, RoutedEventArgs e) // ändrar content på den orangea knappen beroende på iklickat val
        {
            if (GlobalClass.userPermission < 1) { MessageBox.Show("Du har inte behörighet att göra detta"); return; }

            if (rButtonRemoveUser.IsChecked == true) { ButtonChangeUser.Content = rButtonRemoveUser.Content; ButtonChangeUser.FontSize = 16; }
        }
        private void ButtonChangeUser_Click(object sender, RoutedEventArgs e) // knapp för att ändra data på användare
        {
            if (GlobalClass.userPermission < 1) { MessageBox.Show("Du har inte behörighet att göra detta"); return; }

            User userToChange = LVModifyUser.SelectedItem as User; // sätter de nya ändringarna på vald användare

            if(LVModifyUser.SelectedItem == null) 
            {
                MessageBox.Show("Du måste välja en användare i listan först.");
                return; 
            }

            if(rButtonChangeUser.IsChecked == false && rButtonRemoveUser.IsChecked == false) 
            {
                MessageBox.Show("Markera om du vill ta bort eller ändra vald användare.");
                return;
            }

            if (userToChange != null && rButtonChangeUser.IsChecked == true)
            {

                MessageBoxResult result = MessageBox.Show("Är det säkert att du vill ändra den här användaren?", "Meddelande", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
                switch (result)
                {
                    case MessageBoxResult.Yes:

                        if (!Regex.IsMatch(firstName.Text, @"^[a-zA-Z\dåäöÅÄÖ-]*$") || firstName.Text == "" || firstName.Text == "Förnamn")
                        {
                            MessageBox.Show("Du har inte angett ett korrekt förnamn!");

                            return;
                        }
                        else if ((userToChange.Permissions == 2 || userToChange.Permissions == 1) && GlobalClass.userPermission != 2)
                        {
                            MessageBox.Show("Du har inte rättigheter för att ändra detta!");
                            return;
                        }
                        else
                        {
                            if (userToChange.Firstname != firstName.Text) { userToChange.Firstname = firstName.Text; }
                        }

                        if (!Regex.IsMatch(lastName.Text, @"^[a-zA-Z]+$") || lastName.Text == "" || lastName.Text == "Efternamn")
                        {
                            MessageBox.Show("Du har inte angett ett korrekt efternamn!");
                            return;
                        }
                        else if ((userToChange.Permissions == 2 || userToChange.Permissions == 1) && GlobalClass.userPermission != 2)
                        {
                            MessageBox.Show("Du har inte rättigheter för att ändra detta!");
                            return;

                        }
                        else
                        {
                            if (userToChange.Lastname != lastName.Text) { userToChange.Lastname = lastName.Text; }
                        }

                        if (!Regex.IsMatch(eMail.Text, @"^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$") || eMail.Text == "E-post" || eMail.Text == "")
                        {
                            MessageBox.Show("Du har inte angett korrekt e-post!");
                            return;
                        }
                        else if ((userToChange.Permissions == 2 || userToChange.Permissions == 1) && GlobalClass.userPermission != 2)
                        {
                            MessageBox.Show("Du har inte rättigheter för att ändra detta!");
                            return;

                        }
                        else
                        {
                            if (userToChange.Email != eMail.Text) { userToChange.Email = eMail.Text; }
                        }

                        if (passWordText.Text != "Ändra Lösenord")
                        {
                            if (!Regex.IsMatch(passWord.Password, @"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9]).{8,}$"))
                            {
                                MessageBox.Show("Lösenordet ska innehålla minst en stor bokstav, en siffra och måste vara 8 tecken långt!");
                                return;

                            }
                            else if ((userToChange.Permissions == 2 || userToChange.Permissions == 1) && GlobalClass.userPermission != 2)
                            {
                                MessageBox.Show("Du har inte rättigheter för att ändra detta!");
                                return;

                            }
                            else
                            {
                                userToChange.Password = Encryption.Encrypt(passWord.Password);
                            }
                        }

                        if (permissionComboBox.SelectedItem.ToString() != "--Användare--" && permissionComboBox.SelectedIndex != userToChange.Permissions)
                        {

                            if ((userToChange.Permissions == 2 || userToChange.Permissions == 1) && GlobalClass.userPermission != 2)
                            {
                                MessageBox.Show("Du har inte rättigheter för att ändra detta!");
                                return;
                            }
                            userToChange.Permissions = permissionComboBox.SelectedIndex;
                        }

                        if (loanRightsComboBox.SelectedItem.ToString() != "--Lånerättighet--" && loanRightsComboBox.SelectedIndex != userToChange.HasLoanCard)
                        {
                            if ((userToChange.Permissions == 2 || userToChange.Permissions == 1) && GlobalClass.userPermission != 2)
                            {
                                MessageBox.Show("Du har inte rättigheter för att ändra detta!");
                                return;
                            }
                            userToChange.HasLoanCard = (byte)loanRightsComboBox.SelectedIndex;
                        }

                        if (userToChange.UserComment != CommentBox.Text)
                        {
                            if (CommentBox.Text == "Anmärkningar" || CommentBox.Foreground == Brushes.LightGray) { return; }
                            else
                            if ((userToChange.Permissions == 2 || userToChange.Permissions == 1) && GlobalClass.userPermission != 2)
                            {
                                MessageBox.Show("Du har inte rättigheter för att ändra detta!");
                                return;

                            }
                            userToChange.UserComment = CommentBox.Text;
                        }

                        DbInitialiser.Db.Update(userToChange);
                        DbInitialiser.Db.SaveChanges();

                        MessageBox.Show("Du har nu ändrat användaren");
                        break;

                    case MessageBoxResult.No:
                        return;
                }
                ClearAndRetrieveVirtualDb();
                return;
            }
            
            if (userToChange != null && rButtonRemoveUser.IsChecked == true)
            {
                User u = (LVModifyUser.SelectedItem as User);
                
                if ((u.Permissions == 2 || u.Permissions == 1) && GlobalClass.userPermission != 2)
                {
                    MessageBox.Show("Du har inte rättigheter för att ändra detta!");
                    return;
                }

                MessageBoxResult result = MessageBox.Show("Är det säkert att du vill ta bort den här användaren?", "Meddelande", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);

                switch (result) 
                {
                    case MessageBoxResult.Yes:
                        DbInitialiser.Db.Users.Remove(u);
                        DbInitialiser.Db.SaveChanges();
                        MessageBox.Show("Du har tagit bort användaren");

                        ClearAndRetrieveVirtualDb();
                        LVModifyUser.ClearValue(ItemsControl.ItemsSourceProperty);
                        LVModifyUser.ItemsSource = dbVirtual; 
                        break; 

                    case MessageBoxResult.No:
                        return;  
                }
            }
            else
            {
                MessageBox.Show("Du måste välja en användare först!", "Meddelande", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
        }
        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e) //Denna funktion gör så att dropdown autocomplete menyn visar värden
        {
            if (GlobalClass.userPermission < 1) { MessageBox.Show("Du har inte behörighet att göra detta"); return; }

            LVModifyUser.ClearValue(ItemsControl.ItemsSourceProperty);
          
            List<User> userList = dbVirtual.Where(x => x.Firstname.ToLower().Substring(0,3).Contains(searchBar.Text.ToLower()) 
                                                    || x.Lastname.ToLower().Substring(0,3).Contains(searchBar.Text.ToLower()) 
                                                    || x.Email.ToLower().Contains(searchBar.Text.ToLower())).ToList(); //tar fram böckerna som innehåller userinput för TITEL just nu, ska läggas till mer än bara titel

            if (userList != null) // VÄLDIGT simpel sökfunktion, ska byggas på
            {
                LVModifyUser.ItemsSource = userList;
                return;
            }
            else if (Int32.TryParse(searchBar.Text, out var _)) //kollar om userInput är en int eller ej
            {
                List<User> query = dbVirtual.Where(x => x.Email.ToLower().Contains(searchBar.Text.ToLower())).DefaultIfEmpty().ToList();

                LVModifyUser.ItemsSource = query;
               
                return;
            }
        }
        private void searchBar_GotFocus(object sender, RoutedEventArgs e)
        {
            searchBar.Text = ""; 
        }
        private void searchBar_LostFocus(object sender, RoutedEventArgs e) 
        {
            return; 
        }
        private void firstNameBox_GotFocus(object sender, RoutedEventArgs e)
        {
            Thematics.Watermark.ForFocus(firstName);
        }
        private void firstNameBox_LostFocus(object sender, RoutedEventArgs e)
        {
            Thematics.Watermark.ForLostFocus(firstName, "Förnamn");
        }
        private void lastNameBox_GotFocus(object sender, RoutedEventArgs e)
        {
            Thematics.Watermark.ForFocus(lastName);
        }
        private void lastNameBox_LostFocus(object sender, RoutedEventArgs e)
        {
            Thematics.Watermark.ForLostFocus(lastName, "Efternamn");
        }
        private void emailBox_GotFocus(object sender, RoutedEventArgs e)
        {
            Thematics.Watermark.ForFocus(eMail);
        }
        private void emailBox_LostFocus(object sender, RoutedEventArgs e)
        {
            Thematics.Watermark.ForLostFocus(eMail, "E-post");
        }
        private void CommentBox_GotFocus(object sender, RoutedEventArgs e)
        {
            Thematics.Watermark.ForFocus(CommentBox);
        }
        private void CommentBox_LostFocus(object sender, RoutedEventArgs e)
        {
            Thematics.Watermark.ForLostFocus(CommentBox, "Anmärkningar");
        }
        private void passWordText_GotFocus(object sender, RoutedEventArgs e)
        {
            passWordText.Visibility = Visibility.Collapsed;
            passWord.Focus(); 
        }
        private void passwordBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (passWordText.Foreground == Brushes.LightGray)
            {
                passWordText.Visibility = Visibility.Collapsed;
                passWord.Foreground = Brushes.Black;
            }
            passWord.Visibility = Visibility.Visible;
        }
        private void passwordBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (passWord.Password == null || passWord.Password == "")
            {
                passWordText.Visibility = Visibility.Visible;
                passWordText.Foreground = Brushes.LightGray;
                passWordText.Text = "Ändra Lösenord";
            }
        }
        public void ClearAndRetrieveVirtualDb() 
        {
            dbVirtual.Clear(); 

            foreach (var item in DbInitialiser.Db.Users)
            {
                dbVirtual.Add(item);
            }
        }
        private void RestrictButton_Click(object sender, RoutedEventArgs e)
        {
            if (GlobalClass.userPermission < 1) { MessageBox.Show("Du har inte behörighet att göra detta"); return; }
            if (LVModifyUser.SelectedItem == null)
            {
                MessageBox.Show("Vänligen välj en användare först!");
                return;
            }

            string whoRestricted = ""; 
            if (GlobalClass.userPermission == 2) { whoRestricted = "admin"; }
            else whoRestricted = "bibliotekarie"; 

            if (u == null)
            {
                MessageBox.Show("Du har inte valt någon användare att spärra"); 
                return;
            }
            else
            {
                if (u.Permissions == 2 && GlobalClass.userPermission != 2) 
                {
                    MessageBox.Show("Du har inte rättigheter att göra detta");
                    return; 
                }

                if ((GlobalClass.userPermission == 1 && u.Permissions < 1)|| GlobalClass.userPermission == 2)
                {
                    MessageBoxResult result = MessageBox.Show("Är det säkert att du vill spärra den här användaren?", "Meddelande", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);

                    switch (result)
                    {
                        case MessageBoxResult.Yes:
                            if (loanRightsComboBox.SelectedItem != null)
                            {
                                u.UserComment += $"\n--- Lånekort spärrat av {whoRestricted} datum: {DateTime.Now}";
                                u.HasLoanCard = 0;
                                loanRightsComboBox.SelectedIndex = (byte)u.HasLoanCard;
                            }
                            DbInitialiser.Db.Update(u);
                            DbInitialiser.Db.SaveChanges();
                            MessageBox.Show("Du har nu spärrat användaren");
                            break;

                        case MessageBoxResult.No:
                            return;
                    }
                }
                else
                {
                    MessageBox.Show("Du har inte rättigheter att göra detta");
                }
                ClearAndRetrieveVirtualDb();
                return; 
            }
        }
    }
}
