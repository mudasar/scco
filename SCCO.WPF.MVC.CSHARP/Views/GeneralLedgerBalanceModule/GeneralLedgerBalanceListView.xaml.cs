using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Linq;
using SCCO.WPF.MVC.CS.Models;

namespace SCCO.WPF.MVC.CS.Views.GeneralLedgerBalanceModule
{
    public partial class GeneralLedgerBalanceListView : IListDetailView
    {
        private GeneralLedgerBalanceViewModel _lookup;
        private GeneralLedgerBalanceViewModel _viewModel;

        public GeneralLedgerBalanceListView()
        {
            InitializeComponent();
            _lookup = new GeneralLedgerBalanceViewModel();
            _viewModel = new GeneralLedgerBalanceViewModel();

            RefreshDisplay();

            btnSearch.Click += (sender, args) => Search();
            btnAdd.Click += (sender, args) => Add();
            btnEdit.Click += (sender, args) => Edit();
            btnDelete.Click += (sender, args) => Delete();
            
        }



        #region Implementation of IListDetailView

        public void Add()
        {
            var addGeneralLedgerBalanceView = new GeneralLedgerBalanceView(0);
            if (addGeneralLedgerBalanceView.ShowDialog() == true)
            {
                _lookup.RefreshCollection();
                _viewModel.Collection = _lookup.Collection;
            }
        }

        public void Edit()
        {
            if (_viewModel.SelectedItem == null) return;
            var editGeneralLedgerBalanceView = new GeneralLedgerBalanceView(_viewModel.SelectedItem.ID);
            if (editGeneralLedgerBalanceView.ShowDialog() == true)
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
                _lookup.RefreshCollection();
                _viewModel.Collection = _lookup.Collection;
            }
        }

        public void Search()
        {
            if (_lookup == null) return;
            if (!_lookup.Collection.Any() ) return;

            string searchItem = txtSearch.Text;
            if (searchItem.Trim().Length == 0)
            {
                _viewModel.Collection = _lookup.Collection;
            }
            else
            {
                IEnumerable<GeneralLedgerBalance> filteredItem =
                    _lookup.Collection.Where(item => item.AccountTitle.ToLower().Contains(
                        searchItem.ToLower()));

                var collection = new ObservableCollection<GeneralLedgerBalance>();
                foreach (GeneralLedgerBalance item in filteredItem)
                {
                    collection.Add(item);
                }
                _viewModel.Collection = collection;
            }
        }

        public void RefreshDisplay()
        {
            _lookup.RefreshCollection();
            _viewModel.RefreshCollection();
            DataContext = _viewModel;
        }

        #endregion
    }
}