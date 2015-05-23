using System;
using System.Windows;
using SCCO.WPF.MVC.CS.Models.TimeDeposit;

namespace SCCO.WPF.MVC.CS.Views.TimeDepositModule
{
    /// <summary>
    /// Interaction logic for OpenTimeDepositView.xaml
    /// </summary>
    public partial class OpenTimeDepositView
    {

        private TimeDepositViewModel _viewModel;
        private Models.Nfmb _member;
        public OpenTimeDepositView()
        {
            InitializeComponent();
            btnOrPosting.Click += OrPostingOnClick;
        }

        private void OrPostingOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            var postTimeDepositView = new PostTimeDepositView(_member, _viewModel);
            if(postTimeDepositView.ShowDialog()==true)
            {
                DialogResult = true;
                Close();
            }
        }

        public OpenTimeDepositView(Models.Nfmb member):this()
        {
            _viewModel = new TimeDepositViewModel();
            _member = member;          
            _viewModel.DateIn = Controllers.MainController.UserTransactionDate;

            var products = new TimeDepositProducts();
            var collection = TimeDepositProduct.GetAll();
            foreach (var timeDepositProduct in collection)
            {
                products.Add(timeDepositProduct);
            }

            _viewModel.Products = products;
            DataContext = _viewModel;

        }
    }
}
