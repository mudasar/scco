namespace SCCO.WPF.MVC.CS.Views.InitialSetupModule
{
    public partial class BranchNameSetupWindow
    {
        public BranchNameSetupWindow()
        {
            InitializeComponent();
            BranchesComboBox.Items.Add("BULACAN");
            BranchesComboBox.Items.Add("LAWA");
            BranchesComboBox.Items.Add("POLO");
            BranchesComboBox.SelectedItem = Properties.Settings.Default.BranchName.ToUpper();
            SaveButton.Click += (sender, args) =>
                {
                    var branchName = BranchesComboBox.SelectedItem;
                    Properties.Settings.Default.BranchName = branchName.ToString().ToLower();
                    Properties.Settings.Default.Save();
                    Close();
                };
        }
    }
}
