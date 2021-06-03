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
using System.Windows.Shapes;
using System.Windows.Navigation;
using Bibblan.Models;
using Bibblan.Services;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Bibblan.Services
{
    class Thematics
    {
        public class Watermark
        {
            public static void ForFocus(TextBox WaterText)
            {
                if (WaterText.Foreground == Brushes.LightGray)
                {
                    WaterText.Text = "";
                    WaterText.Foreground = Brushes.Black;
                }
            }
            public static void ForLostFocus(TextBox WaterText, string InputAuto)
            {
                if (WaterText.Text == "" || WaterText.Text == null)
                {
                    WaterText.Foreground = Brushes.LightGray;
                    WaterText.Text = InputAuto;
                }
            }
        }
        public static void Clearer(TextBox titleBox, TextBox authorBox, TextBox descriptionBox, TextBox editionBox, TextBox publisherBox, TextBox priceBox, TextBox ddkBox, TextBox sabBox, TextBox amountBox)
        {
            titleBox.Foreground = Brushes.LightGray;
            titleBox.Text = "Titel";
            authorBox.Foreground = Brushes.LightGray;
            authorBox.Text = "Författare";
            descriptionBox.Foreground = Brushes.LightGray;
            descriptionBox.Text = "Beskrivning";
            editionBox.Foreground = Brushes.LightGray;
            editionBox.Text = "Upplaga";
            publisherBox.Foreground = Brushes.LightGray;
            publisherBox.Text = "Förlag";
            priceBox.Foreground = Brushes.LightGray;
            priceBox.Text = "Pris";
            ddkBox.Foreground = Brushes.LightGray;
            ddkBox.Text = "DDK";
            sabBox.Foreground = Brushes.LightGray;
            sabBox.Text = "Sab";
            amountBox.Foreground = Brushes.LightGray;
            amountBox.Text = "Antal";
        }
        //public void Clearer(ListView Listview, params TextBox[] boxes, params string[] watermark)
        //{
        //    Listview.Items.Refresh();
        //    foreach (TextBox item in boxes)
        //   {
        //        item.Foreground = Brushes.LightGray;
        //        item.Text = "Titel";
        //    }
        //}
    }
}
