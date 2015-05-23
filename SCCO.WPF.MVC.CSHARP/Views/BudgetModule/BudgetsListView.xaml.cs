using System.Windows;
using SCCO.WPF.MVC.CS.Models;

namespace SCCO.WPF.MVC.CS.Views.BudgetModule
{
    /// <summary>
    /// Interaction logic for ListBudgetView.xaml
    /// </summary>
    public partial class BudgetsListView
    {
        private readonly BudgetViewModel _viewModel;

        public BudgetsListView()
        {
            InitializeComponent();
            _viewModel = new BudgetViewModel();
            _viewModel.Collection = Controllers.BudgetsController.GetObservableCollection();

            DataContext = _viewModel;

            btnAdd.Click += (sender, args) => Add();
            btnEdit.Click += (sender, args) => Edit();
            btnDelete.Click += (sender, args) => Delete();
        }

        private void Add()
        {
            var searchItem = Controllers.MainController.SearchGeneralLedgerAccount();
            if (searchItem == null) return;

            var model = new Budget();
            model.AccountCode = searchItem.ItemCode;
            model.AccountTitle = searchItem.ItemName;
            model.Year = Controllers.MainController.LoggedUser.TransactionDate.Year;
            var view = new BudgetAddView(model);
            if(view.ShowDialog() == true)
            {
                _viewModel.Collection.Add(model);
                _viewModel.SelectedItem = model;
                grdItems.ScrollIntoView(model);
            }
        }

        private void Edit()
        {
            if (_viewModel.SelectedItem == null) return;
            var model = Controllers.BudgetsController.Find(_viewModel.SelectedItem.ID);
            var editView = new BudgetEditView(model);
            if (editView.ShowDialog() == true)
            {
                _viewModel.SelectedItem.Find(_viewModel.SelectedItem.ID);
            }
        }

        private void Delete()
        {
            if (_viewModel.SelectedItem == null) return;
            if(MessageWindow.ConfirmDeleteRecord() == MessageBoxResult.Yes)
            {
                var item = _viewModel.SelectedItem;
                var result = Controllers.BudgetsController.Delete(item.ID);
                if(!result.Success)
                {
                    MessageWindow.ShowAlertMessage(result.Message);
                    return;
                }

                _viewModel.SelectedItem = null;
                _viewModel.Collection.Remove(item);
            }
        }

    }
}
