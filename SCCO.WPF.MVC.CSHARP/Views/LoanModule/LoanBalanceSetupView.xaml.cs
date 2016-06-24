namespace SCCO.WPF.MVC.CS.Views.LoanModule
{
    internal partial class LoanBalanceSetupView
    {
        public LoanBalanceSetupView(LoanReconstructionViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel;

            SaveButton.Click += (sender, args) =>
            {
                DialogResult = true;
                Close();
            };
        }
    }
}
