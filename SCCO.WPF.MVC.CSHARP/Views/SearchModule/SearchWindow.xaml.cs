using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using SCCO.WPF.MVC.CS.Models;

namespace SCCO.WPF.MVC.CS.Views.SearchModule
{
    public partial class SearchWindow
    {
        public SearchWindow()
        {
            InitializeComponent();
            txtKeyword.TextChanged += (sender, args) => FilterByKeyword();
        }

        private readonly SearchItemViewModel _viewModel;

        public SearchItem SelectedItem { get; set; }

        public SearchWindow(IEnumerable<SearchItem> searchItems)
            : this()
        {
            _viewModel = new SearchItemViewModel {SearchItems = new SearchItems()};
            foreach (var searchItem in searchItems)
            {
                _viewModel.SearchItems.Add(searchItem);
            }
            DataContext = _viewModel;
        }

        private void FilterByKeyword()
        {
            string keyword = txtKeyword.Text.ToUpper();
            IOrderedEnumerable<SearchItem> query = from item in _viewModel.SearchItems
                                                   where item.ItemName.ToUpper().Contains(keyword.ToUpper())
                                                   orderby item.ItemName
                                                   select item;
            grdList.ItemsSource = query;
        }

        private void SelectButtonClick(object sender, RoutedEventArgs e)
        {
            if (_viewModel.SelectedItem != null)
            {
                SelectedItem = _viewModel.SelectedItem;
                DialogResult = true;
                Close();
            }
            else
            {
                const string message = "Please select an existing record from the list.";
                MessageWindow.ShowAlertMessage(message);
            }
        }

        private void BaseWindow_Loaded(object sender, RoutedEventArgs e)
        {
            txtKeyword.Focus();
        }

        private void grdList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (_viewModel.SelectedItem == null) return;
            SelectedItem = _viewModel.SelectedItem;
            DialogResult = true;
            Close();
        }
    }
}
