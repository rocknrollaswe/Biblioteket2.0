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
            if (LVModifyUser.SelectedItem != null)
            {
                User u = LVModifyUser.SelectedItem as User;

                firstName.Text = u.Firstname;
                lastName.Text = u.Lastname;
                eMail.Text = u.Email;
                permissionComboBox.SelectedIndex = u.Permissions;
                loanRightsComboBox.SelectedIndex = (byte)u.HasLoanCard;
                CommentBox.Text = u.UserComment; 

                firstName.Foreground = Brushes.Black;
                lastName.Foreground = Brushes.Black;
                eMail.Foreground = Brushes.Black;
                CommentBox.Foreground = Brushes.Black; 
            }
        }

        private void rButtonChangeUser_Click(object sender, RoutedEventArgs e) // ändrar content på den orangea knappen beroende på iklickat val
        {
            if(rButtonChangeUser.IsChecked == true)
                ButtonChangeUser.Content = rButtonChangeUser.Content; 
        }

        private void rButtonRemoveUser_Click(object sender, RoutedEventArgs e) // ändrar content på den orangea knappen beroende på iklickat val
        {
            if (rButtonRemoveUser.IsChecked == true)
                ButtonChangeUser.Content = rButtonRemoveUser.Content;
        }

        private void ButtonChangeUser_Click(object sender, RoutedEventArgs e) // knapp för att ändra data på användare
        {
            User userToChange = LVModifyUser.SelectedItem as User; // sätter de nya ändringarna på vald användare

            if(userToChange != null) 
            {
                if (rButtonChangeUser.IsChecked == true)
                {

                    MessageBoxResult result = MessageBox.Show("Är det säkert att du vill ändra den här användaren?", "Meddelande", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);

                    switch (result)
                    {
                        case MessageBoxResult.Yes:

                            if (!Regex.IsMatch(firstName.Text, @"^[a-zA-Z\dåäöÅÄÖ-]*$") && firstName.Foreground == Brushes.Black || firstName.Text == "" || firstName.Text == "Förnamn")
                            {
                                MessageBox.Show("Du har inte angett ett korrekt förnamn!");
                                return;
                                
                            }
                            else
                            {
                                if (userToChange.Firstname != firstName.Text)
                                    userToChange.Firstname = firstName.Text;

                            };


                            if (!Regex.IsMatch(lastName.Text, @"^[a-zA-Z]+$") && lastName.Foreground == Brushes.Black || lastName.Text == "" || lastName.Text == "Efternamn")
                            {
                                MessageBox.Show("Du har inte angett ett korrekt efternamn!");
                                return;
                            }
                            else
                            {
                                if (userToChange.Lastname != lastName.Text)
                                    userToChange.Lastname = lastName.Text;
                            }

                            if (!Regex.IsMatch(eMail.Text, @"^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$") && eMail.Foreground == Brushes.Black || eMail.Text == "")
                            {
                                MessageBox.Show("Du har inte angett korrekt e-post!");
                                return;
                            }
                            else
                            {
                                if (userToChange.Email != eMail.Text)
                                    userToChange.Email = eMail.Text;
                            }

                            if (passWordText.Visibility == Visibility.Collapsed && !Regex.IsMatch(passWord.Password, @"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9]).{8,}$"))
                            {
                                MessageBox.Show("Lösenordet ska innehålla minst en stor bokstav, en siffra och måste vara 8 tecken långt!");
                                return;

                            }
                            else
                            {
                                userToChange.Password = Encryption.Encrypt(passWord.Password);
                            }

                            if (permissionComboBox.SelectedItem.ToString() != "--Användare--" && permissionComboBox.SelectedIndex != userToChange.Permissions)
                                userToChange.Permissions = permissionComboBox.SelectedIndex;

                            if (loanRightsComboBox.SelectedItem.ToString() != "--Lånerättighet--" && loanRightsComboBox.SelectedIndex != userToChange.HasLoanCard)
                                userToChange.HasLoanCard = (byte)loanRightsComboBox.SelectedIndex;
                            if(userToChange.UserComment != CommentBox.Text)
                            {
                                if (CommentBox.Text == "Anmärkningar" || CommentBox.Foreground == Brushes.LightGray)
                                    return; 
                                else
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
                else
                    MessageBox.Show("Du måste välja en användare att ändra först!", "Meddelande", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return; 

            }


            if (rButtonRemoveUser.IsChecked == true)
            {
                User u = (LVModifyUser.SelectedItem as User);

                MessageBoxResult result = MessageBox.Show("Är det säkert att du vill ta bort den här användaren?", "Meddelande", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);

                switch (result) 
                {
                    case MessageBoxResult.Yes:
                        DbInitialiser.Db.Users.Remove(u);
                        DbInitialiser.Db.SaveChanges();
                        MessageBox.Show("Du har tagit bort användaren");

                        
                        LVModifyUser.ClearValue(ItemsControl.ItemsSourceProperty);

                        ClearAndRetrieveVirtualDb();

                        break; 

                    case MessageBoxResult.No:
                        break; 
                }
                
            }
        }
            
       
        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e) //Denna funktion gör så att dropdown autocomplete menyn visar värden
        {
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

        private void firstNameBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (firstName.Foreground == Brushes.LightGray)
            {
                firstName.Text = "";
                firstName.Foreground = Brushes.Black;
            }
        }
        private void firstNameBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (firstName.Text == "" || firstName.Text == null)
            {
                firstName.Foreground = Brushes.LightGray;
                firstName.Text = "Förnamn";
            }
        }

        //--------------------

        private void lastNameBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (lastName.Foreground == Brushes.LightGray)
            {
                lastName.Text = "";
                lastName.Foreground = Brushes.Black;
            }
        }
        private void lastNameBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (lastName.Text == "" || firstName.Text == null)
            {
                lastName.Foreground = Brushes.LightGray;
                lastName.Text = "Efternamn";
            }
        }

        //--------------------

        private void emailBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (eMail.Foreground == Brushes.LightGray)
            {
                eMail.Text = "";
                eMail.Foreground = Brushes.Black;
            }
        }

       
        private void emailBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (eMail.Text == "" || firstName.Text == null)
            {
                eMail.Foreground = Brushes.LightGray;
                eMail.Text = "E-post";
            }
        }
        
        //------------
        private void passwordBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if(passWordText.Foreground == Brushes.LightGray)
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


        private void CommentBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (CommentBox.Foreground == Brushes.LightGray)
            {
                CommentBox.Text = "";
                CommentBox.Foreground = Brushes.Black;
            }

        

        private void CommentBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (CommentBox.Text == "" || CommentBox.Text == null)
            {
                CommentBox.Foreground = Brushes.LightGray;
                CommentBox.Text = "Anmärkningar";
            }
       
        }

        private void passWordText_GotFocus(object sender, RoutedEventArgs e)
        {
            passWordText.Visibility = Visibility.Collapsed;
            passWord.Focus(); 
        }

        void ClearAndRetrieveVirtualDb() 
        {
            dbVirtual.Clear(); 

            foreach (var item in DbInitialiser.Db.Users)
            {
                dbVirtual.Add(item);
            }

        }

       
    }
}
