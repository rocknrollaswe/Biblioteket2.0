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
