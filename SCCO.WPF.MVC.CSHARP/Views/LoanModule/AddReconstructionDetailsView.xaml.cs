namespace SCCO.WPF.MVC.CS.Views.LoanModule
{
    internal partial class AddReconstructionDetailsView
    {
        private readonly Particular _viewModel = new Particular();

        public AddReconstructionDetailsView(Particular viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = _viewModel;
            InitializeControls();
        }

        private void InitializeControls()
        {
            stbAccount.Click += delegate
                {
                    var account = Controllers.MainController.SearchAccount();
                    if (account == null) return;
                    stbAccount.Text = account.AccountCode;
                    _viewModel.AccountCode = account.AccountCode;
                    _viewModel.AccountTitle = account.AccountTitle;
                };

            btnOK.Click += (sender, args) =>
                {
                    DialogResult = true;
                };
        }
    }
}
