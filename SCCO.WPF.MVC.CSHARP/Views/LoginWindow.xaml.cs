using System;
using System.Diagnostics;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using SCCO.WPF.MVC.CS.Controllers;
using SCCO.WPF.MVC.CS.Models;

namespace SCCO.WPF.MVC.CS.Views
{
    public partial class LoginWindow
    {
        private int _attempts;

        public LoginWindow()
        {
            InitializeComponent();

            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);

            lblVersion.Content = string.Format("Version {0}", fileVersionInfo.FileVersion);

            Loaded += (sender, args) => txtUsername.Focus();

            btnOK.Click += (sender, args) => Loggin();
            btnCancel.Click += (sender, args) => CancelLoggin();

            btnOK.GotFocus += OkButtonOnGotFocus;
            btnOK.LostFocus += OkButtonOnLostFocus;

            btnCancel.GotFocus += CancelButtonOnGotFocus;
            btnCancel.LostFocus += CancelButtonOnLostFocus;
        }

        private void CancelButtonOnGotFocus(object sender, RoutedEventArgs e)
        {
            btnCancel.Foreground = System.Windows.Media.Brushes.GreenYellow;
        }

        private void CancelButtonOnLostFocus(object sender, RoutedEventArgs e)
        {
            btnCancel.Foreground = System.Windows.Media.Brushes.WhiteSmoke;
        }

        private void CancelLoggin()
        {
            Environment.Exit(0);
        }

        private void Loggin()
        {
            if (!Database.DatabaseController.Validate())
            {
                MessageWindow.ShowAlertMessage("Unable to connect to database server.");
                return;
            }

            var id = User.FindMatch(txtUsername.Text, txtPassword.Password);
            if(id > 0)
            {
                var loggedUser = new User();
                loggedUser.Find(id);
                MainController.LoggedUser = loggedUser;
                Hide();
                MainController.ShowMainWindow();
            }
            else
            {
                _attempts++;
                MessageWindow.ShowAlertMessage("Access Denied! Username and password combination is not valid!");
                if (_attempts >= 3)
                {
                    MessageWindow.ShowAlertMessage("Access Denied! Unauthorized user!");
                    Environment.Exit(0);
                }
            }
        }

        private void OkButtonOnGotFocus(object sender, RoutedEventArgs e)
        {
            btnOK.Foreground = System.Windows.Media.Brushes.GreenYellow;
        }

        private void OkButtonOnLostFocus(object sender, RoutedEventArgs e)
        {
            btnOK.Foreground = System.Windows.Media.Brushes.WhiteSmoke;
        }

        private void WindowOnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F5 && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                MainController.ShowDatabaseConfigurationWindow();
            }
        }
    }
}
