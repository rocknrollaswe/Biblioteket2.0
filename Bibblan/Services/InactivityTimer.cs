using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Timers;
using System.Windows;
using Bibblan.Services;
using Bibblan.Models;
using Bibblan.Views;
using System.Windows.Media;

namespace Bibblan.Services
{
    public static class InactivityTimer
    {
        public static System.Timers.Timer logoutTimer = null;

        public static void SetTimer()
        {
            if (logoutTimer == null)
            {
                logoutTimer = new System.Timers.Timer(600000);
                logoutTimer.Elapsed += InactivityCheck;
                logoutTimer.AutoReset = false;
                logoutTimer.Enabled = true;
            }
        }
        
        public static void InactivityCheck(object source, ElapsedEventArgs e)
        {
            SendInactivityStatus();
        }

        public static void ResetTimer()
        {
            if(logoutTimer != null)
            {
                logoutTimer.Stop();
                logoutTimer = null;
                SetTimer();
            }
        }
        public static void SendInactivityStatus()
        { 
            MainWindow mainwindow = GlobalClass.currentMainWindowInstance;
            logoutTimer = null;
            mainwindow.Dispatcher.Invoke(() =>
            {
                GlobalClass.currentHomeInstance.inactivityFrame.Visibility = Visibility.Visible;
                GlobalClass.currentMainWindowInstance.passwordTextBox.Password = "";
                GlobalClass.currentMainWindowInstance.passWordFalse.Text = "Lösenord";
                GlobalClass.currentMainWindowInstance.passWordFalse.Foreground = Brushes.LightGray;
                GlobalClass.currentMainWindowInstance.passWordFalse.Visibility = Visibility.Visible;
                GlobalClass.currentMainWindowInstance.emailTextBox.Text = "E-post";
                GlobalClass.currentMainWindowInstance.emailTextBox.Foreground = Brushes.LightGray;

                MessageBox.Show("Du loggas nu ut på grund av inaktivitet");
                GlobalClass.currentHomeInstance.Close();
                mainwindow.Show();
            });
        }
        public static void StopTimer()
        {
            logoutTimer.Stop();
            logoutTimer = null;
        }
    }
}
