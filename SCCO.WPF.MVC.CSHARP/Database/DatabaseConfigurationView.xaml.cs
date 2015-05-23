using System.Windows;
using System.Windows.Input;
using SCCO.WPF.MVC.CS.Controllers;
using SCCO.WPF.MVC.CS.Properties;

namespace SCCO.WPF.MVC.CS.Database
{
    /// <summary>
    /// Interaction logic for DatabaseConfigurationView.xaml
    /// </summary>
    public partial class DatabaseConfigurationView
    {
        public DatabaseConfigurationView()
        {
            InitializeComponent();
            DatabaseServerBox.Text = Settings.Default.DatabaseServer;
            BranchComboBox.Items.Add("BULACAN");
            BranchComboBox.Items.Add("LAWA");
            BranchComboBox.Items.Add("POLO");
            BranchComboBox.SelectedItem = Settings.Default.BranchName.ToUpper();

            DatabaseEnvironmentComboBox.Items.Add("DEMO");
            DatabaseEnvironmentComboBox.Items.Add("PRODUCTION");
            DatabaseEnvironmentComboBox.SelectedItem = Settings.Default.DatabaseEnvironment.ToUpper();

            DatabasePassword.Password = Utilities.Password.Decrypt(Settings.Default.DatabasePassword);
            DatabasePort.Text = string.Format("{0}",Settings.Default.DatabasePort);

            KeyDown += (s, e) =>
                {
                    if (e.Key == Key.F5 && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
                    {
                        AdvancePanel.Visibility = AdvancePanel.Visibility == Visibility.Collapsed
                                                      ? Visibility.Visible
                                                      : Visibility.Collapsed;
                    }
                };
        }

        private void UpdateButtonOnClick(object sender, RoutedEventArgs e)
        {
            Settings.Default.DatabaseServer = DatabaseServerBox.Text;

            var branchName = BranchComboBox.SelectedItem;
            Settings.Default.BranchName = branchName.ToString().ToLower();

            var databaseEnvironment = DatabaseEnvironmentComboBox.SelectedItem;
            Settings.Default.DatabaseEnvironment = databaseEnvironment.ToString().ToLower();

            if (AdvancePanel.Visibility == Visibility.Visible)
            {
                Settings.Default.DatabasePassword = Utilities.Password.Encrypt(DatabasePassword.Password);
                uint port;
                System.UInt32.TryParse(DatabasePort.Text, out port);
                Settings.Default.DatabasePort = port;
            }
            Settings.Default.Save();
            
            Views.MessageWindow.ShowNotifyMessage("Database configuration updated!");
            Close();
        }
    }
}
