using System.Collections.Generic;
using System.Linq;
using System.Windows;
using SCCO.WPF.MVC.CS.Models;
using SCCO.WPF.MVC.CS.Models.Loan;
using SCCO.WPF.MVC.CS.Models.TimeDeposit;
using SCCO.WPF.MVC.CS.Views.LoanModule;
using SCCO.WPF.MVC.CS.Views.TimeDepositModule;

namespace SCCO.WPF.MVC.CS.Views.ForwardedBalanceModule
{
    public partial class ForwardedBalanceListDetailView : IListDetailView
    {
        private ForwardedBalanceCollection _lookup;
        private ForwardedBalanceViewModel _viewModel;
        private List<string> _listTimeDepositCode;
        private List<string> _listLoanReceivableCode;

        public ForwardedBalanceListDetailView()
        {
            InitializeComponent();

            _listTimeDepositCode = Account.GetListOfTimeDepositCode();
            _listLoanReceivableCode = Account.GetListOfLoanReceivableCode();

            RefreshDisplay();

            btnSearch.Click += (sender, args) => Search();
            btnAdd.Click += (sender, args) => Add();
            btnEdit.Click += (sender, args) => Edit();
            btnDelete.Click += (sender, args) => Delete();
            
            btnTimeDepositDetails.Click += (sender, args) => ShowTimeDepositDetails();
            btnLoanDetails.Click += (sender, args) => ShowLoanDetails();
        }

        private void ShowTimeDepositDetails()
        {
            if (_viewModel.SelectedItem == null) return;
            if (!_listTimeDepositCode.Contains(_viewModel.SelectedItem.AccountCode)) return;

            if (_viewModel.SelectedItem.TimeDepositDetails == null)
            {
                _viewModel.SelectedItem.TimeDepositDetails = new TimeDepositDetails();
            }
            var view = new TimeDepositDetailsView(_viewModel.SelectedItem.TimeDepositDetails);
            view.ShowDialog();
        }

        private void ShowLoanDetails()
        {
            if (_viewModel.SelectedItem == null) return;
            if (!_listLoanReceivableCode.Contains(_viewModel.SelectedItem.AccountCode)) return;

            if(_viewModel.SelectedItem.LoanDetails == null)
            {
                _viewModel.SelectedItem.LoanDetails = new LoanDetails();
            }
            var view = new LoanDetailsWindow(_viewModel.SelectedItem.LoanDetails);
            view.ShowDialog();
        }

        #region Implementation of IListDetailView

        public void Add()
        {
            var addForwardedBalanceView = new AddForwardedBalanceView();
            if (addForwardedBalanceView.ShowDialog() == true)
            {
                _viewModel.Collection = _lookup = ForwardedBalance.CollectAll();
            }
        }

        public void Edit()
        {
            if (_viewModel.SelectedItem == null) return;
            var editForwardedBalanceView = new EditForwardedBalanceView(_viewModel.SelectedItem.ID);
            if (editForwardedBalanceView.ShowDialog() == true)
            {
                _viewModel.SelectedItem.Find(_viewModel.SelectedItem.ID);
            }
        }

        public void Delete()
        {
            if (_viewModel.SelectedItem == null) return;
            if (MessageWindow.ConfirmDeleteRecord() == MessageBoxResult.Yes)
            {
                _viewModel.SelectedItem.Destroy();
                _viewModel.Collection = _lookup = ForwardedBalance.CollectAll();
            }
        }

        public void Search()
        {
            if (_lookup == null) return;
            if (!_lookup.Any()) return;

            string searchItem = txtSearch.Text;
            if (searchItem.Trim().Length == 0)
            {
                _viewModel.Collection = _lookup;
            }
            else
            {
                IEnumerable<ForwardedBalance> filteredItem = from item in _lookup
                                                             where
                                                                 item.MemberName.ToLower().Contains(searchItem.ToLower()) ||
                                                                 item.AccountTitle.ToLower().Contains(
                                                                     searchItem.ToLower())
                                                             select item;

                var collection = new ForwardedBalanceCollection();
                foreach (ForwardedBalance item in filteredItem)
                {
                    collection.Add(item);
                }
                _viewModel.Collection = collection;
            }
        }

        public void RefreshDisplay()
        {
            _lookup = ForwardedBalance.CollectAll();
            _viewModel = new ForwardedBalanceViewModel();
            _viewModel.Collection = ForwardedBalance.CollectAll();
            DataContext = _viewModel;
        }

        #endregion
    }
}