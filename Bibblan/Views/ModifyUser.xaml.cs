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

namespace Bibblan.Views
{
    /// <summary>
    /// Interaction logic for ModifyUser.xaml
    /// </summary>
    public partial class ModifyUser : Page
    {
        List<User> dbVirtualUser = new List<User>();
        public ModifyUser()
        {
            InitializeComponent();
            foreach (var item in DbInitialiser.Db.Users)
            {
                dbVirtualUser.Add(item);
            }
            DataContext = dbVirtualUser;
            LVModifyUser.ItemsSource = dbVirtualUser;
        }
    }
}
