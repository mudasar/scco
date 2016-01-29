using System.Linq;
using System.Windows;
using System.Windows.Input;
using SCCO.WPF.MVC.CS.Models.Loan;

namespace SCCO.WPF.MVC.CS.Views.LoanModule
{
    public partial class LoanProductsListWindow : IListDetailView
    {
        private LoanProductViewModel _viewModel;
        private LoanProductCollection _lookup;

        public LoanProductsListWindow()
        {
            InitializeComponent();
            Requery();
            RefreshDisplay();

            txtSearch.TextChanged += (sender, args) => Search();

            btnAdd.Click += (sender, args) => Add();
            btnEdit.Click += (sender, args) => Edit();
            btnDelete.Click += (sender, args) => Delete();

            KeyDown += (sender, e) =>
                {
                    if (e.Key == Key.F5)
                    {
                        Requery();
                    }
                };
        }


        #region Implementation of IListDetailView

        public void Add()
        {
            const string title = "Add new Loan Product";
            const string message = "Please enter a description";
            var inputWindow = new InputWindow(message, title);
            if (inputWindow.ShowDialog() == true)
            {
                var input = inputWindow.InputText;
                if(string.IsNullOrEmpty(input))
                {
                    MessageWindow.ShowAlertMessage("Please specify a Loan Product Name");
                    return;
                }
                var newLoanProduct = LoanProduct.FindByName(input);
                if (newLoanProduct != null)
                {
                    MessageWindow.ShowAlertMessage(input + "already exists");
                    return;
                }

                var editView = new EditLoanProductView(input);
                if (editView.ShowDialog() == true)
                {
                    newLoanProduct = new LoanProduct();
                    newLoanProduct.Find(editView.CurrentItem.ID);
                    _lookup.Add(newLoanProduct);
                    _viewModel.Collection.Add(newLoanProduct);
                }                
            }
        }

        public void Edit()
        {
            if (_viewModel.SelectedItem == null) return;
            var editView = new EditLoanProductView(_viewModel.SelectedItem.ID);
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
                Requery();
                RefreshDisplay();
            }
        }

        public void Search()
        {
            if (_lookup == null) return;
            if (!_lookup.Any()) return;

            var searchItem = txtSearch.Text;
            if (searchItem.Trim().Length == 0)
            {
                RefreshDisplay();
            }
            else
            {
                var filteredItem = from item in _lookup
                                   where item.Name.ToLower().Contains(searchItem.ToLower())
                                   select item;

                var viewModel = new LoanProductViewModel { Collection = new LoanProductCollection() };
                foreach (var item in filteredItem)
                {
                    viewModel.Collection.Add(item);
                }
                _viewModel = viewModel;
                DataContext = _viewModel;
            }
        }

        public void RefreshDisplay()
        {
            _viewModel = new LoanProductViewModel();
            {
                _viewModel.Collection = _lookup;
            }
            DataContext = _viewModel;
        }

        #endregion

        private void Requery()
        {
            _lookup = LoanProduct.CollectAll();
        }
    }
}
