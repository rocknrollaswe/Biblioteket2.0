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

namespace Bibblan.Views
{
    /// <summary>
    /// Interaction logic for Rapport.xaml
    /// </summary>
    public partial class Rapport : Page
    {
        public Rapport()
        {
            InitializeComponent();
        }
        private void seeUserButton_Click(object sender, RoutedEventArgs e)
        {

        }
        private void seeDeletedObjects_Click(object sender, RoutedEventArgs e)
        {

        }


        private void epostTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (epostTextBox.Foreground == Brushes.LightGray)
            {
                epostTextBox.Text = "";
                epostTextBox.Foreground = Brushes.Black;
            }
        }
        private void epostTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (epostTextBox.Text == "" || epostTextBox.Text == null)
            {
                epostTextBox.Foreground = Brushes.LightGray;
                epostTextBox.Text = "E-post";
            }
        }
    }
}
