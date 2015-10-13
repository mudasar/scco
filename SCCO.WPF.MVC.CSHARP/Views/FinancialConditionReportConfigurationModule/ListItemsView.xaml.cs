using System.Linq;
using System.Windows;
using SCCO.WPF.MVC.CS.Controllers;
using SCCO.WPF.MVC.CS.Models;

namespace SCCO.WPF.MVC.CS.Views.FinancialConditionReportConfigurationModule
{
    public partial class ListItemsView : IListDetailView
    {
        private ViewModel _viewModel;
        private FinancialConditionReportConfigurationCollection _lookup;

        public ListItemsView()
        {
            InitializeComponent();
            InitializeEvents();
            RefreshDisplay();
        }

        private void InitializeEvents()
        {
            SearchButton.Click += (sender, args) => Search();
            AddButton.Click += (sender, args) => Add();
            EditButton.Click += (sender, args) => Edit();
            DeleteButton.Click += (sender, args) => Delete();
            PreviewButton.Click += (sender, args) => PreviewReport();
        }

        #region Implementation of IListDetailView

        public void Add()
        {
            var addView = new AddItemView();
            if (addView.ShowDialog() == true)
            {
                _lookup.Add(addView.NewItem);
                _viewModel.Collection.Add(addView.NewItem);
            }
        }

        public void Edit()
        {
            if (_viewModel.SelectedItem == null) return;
            var editView = new EditItemView(_viewModel.SelectedItem.ID);
            if (editView.ShowDialog() == true)
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
                _lookup.Remove(_lookup.FirstOrDefault(item => item.ID == _viewModel.SelectedItem.ID));
                _viewModel.Collection.Remove(_viewModel.SelectedItem);
            }
        }

        public void Search()
        {
            if (_lookup == null) return;
            if (!_lookup.Any()) return;

            var searchItem = SearchTextBox.Text;
            if (searchItem.Trim().Length == 0)
            {
                RefreshDisplay();
            }
            else
            {
                var filteredItem = from item in _lookup
                                   where
                                       item.AccountCode.ToLower().Contains(searchItem.ToLower()) ||
                                       item.AccountTitle.ToLower().Contains(
                                           searchItem.ToLower())
                                   select item;

                var collection = new FinancialConditionReportConfigurationCollection();
                foreach (var item in filteredItem)
                {
                    collection.Add(item);
                }
                _viewModel.Collection = collection;

                DataContext = _viewModel;
            }
        }

        public void RefreshDisplay()
        {
            _lookup = FinancialConditionReportConfiguration.CollectAll();
            _viewModel = new ViewModel
            {
                Collection = FinancialConditionReportConfiguration.CollectAll()
            };
            DataContext = _viewModel;
        }

        #endregion


        private void PreviewReport()
        {
            var asOf = MainController.LoggedUser.TransactionDate;

            var result = ReportController.GenerateStatementFinancialCondition(asOf);

            if (!result.Success)
            {
                MessageWindow.ShowAlertMessage(result.Message);
            }
        }
    }
}
