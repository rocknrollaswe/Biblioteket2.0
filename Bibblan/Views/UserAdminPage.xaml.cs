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

namespace Bibblan.Views
{
    /// <summary>
    /// Interaction logic for UserAdminPage.xaml
    /// </summary>
    public partial class UserAdminPage : Page
    {
       
        public UserAdminPage()
        {
            InitializeComponent();
          
        }

        private void rButtonChangeUser_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {

        }
    }
}
