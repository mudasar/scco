using System.Windows;
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
        }

        private void UpdateButtonOnClick(object sender, RoutedEventArgs e)
        {
            Settings.Default.DatabaseServer = DatabaseServerBox.Text;

            var branchName = BranchComboBox.SelectedItem;
            Settings.Default.BranchName = branchName.ToString().ToLower();

            var databaseEnvironment = DatabaseEnvironmentComboBox.SelectedItem;
            Settings.Default.DatabaseEnvironment = databaseEnvironment.ToString().ToLower();
            Settings.Default.Save();
            
            Views.MessageWindow.ShowNotifyMessage("Database configuration updated!");
            Close();
        }
    }
}
