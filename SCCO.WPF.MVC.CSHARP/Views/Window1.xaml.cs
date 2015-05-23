using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SCCO.WPF.MVC.CS.Views
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();

            Warning.Click += warning_click;
            Alert.Click += alert_click;
            Notify.Click += notify_click;
            Confirm.Click += confirm_click;
        }

        private void warning_click(object sender, EventArgs e) {
            MessageWindow.ShowWarningMessage("This is a warning message");
        }
        private void alert_click(object sender, EventArgs e)
        {
            MessageWindow.ShowAlertMessage("This is an alert message");
        }

        private void notify_click(object sender, EventArgs e)
        {
            MessageWindow.ShowNotifyMessage("This is a notify message");
        }

        private void confirm_click(object sender, EventArgs e)
        {
            MessageWindow.ShowConfirmMessage("This is a confirm message");
        }

    }
}
