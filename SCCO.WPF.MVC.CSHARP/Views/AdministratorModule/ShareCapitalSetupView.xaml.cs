using System.Windows;
using SCCO.WPF.MVC.CS.Controllers;

namespace SCCO.WPF.MVC.CS.Views.AdministratorModule
{
    public partial class ShareCapitalSetupView
    {
        private readonly ShareCapitalSetupViewModel _viewModel;
        private bool _isReadOnly;

        public ShareCapitalSetupView()
        {
            InitializeComponent();
            _viewModel = new ShareCapitalSetupViewModel();
            DataContext = _viewModel;

            ShareCapitalCodeSearchControl.Click += OnSearch;
            UpdateButton.Click += OnUpdate;
        }

        public bool IsReadOnly
        {
            get { return _isReadOnly; }
            set
            {
                _isReadOnly = value;
                UpdateButton.IsEnabled = !IsReadOnly;
            }
        }

        private void OnSearch(object sender, RoutedEventArgs e)
        {
            var account = MainController.SearchAccount();
            if (account == null) return;
            _viewModel.AccountCode = account.AccountCode;
        }

        private void OnUpdate(object sender, RoutedEventArgs routedEventArgs)
        {
            _viewModel.Update();
            DialogResult = true;
            Close();
        }
    }
}