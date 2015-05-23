using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using SCCO.WPF.MVC.CS.Models;

namespace SCCO.WPF.MVC.CS.Views.SearchModule
{
    public partial class SearchByCodeWindow
    {
        private readonly SearchItemViewModel _viewModel;

        public SearchByCodeWindow()
        {
            InitializeComponent();
            _viewModel = new SearchItemViewModel();
            txtKeyword.TextChanged += (sender, args) => FilterByKeyword();
        }

        public SearchItem SelectedItem { get; set; }

        public SearchByCodeWindow(IEnumerable<SearchItem> searchItems):this()
        {
            _viewModel.SearchItems = new SearchItems();

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
                                                   where item.ItemName.ToUpper().Contains(keyword.ToUpper()) || item.ItemCode.Contains((keyword))
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

        //private void txtKeyword_KeyDown(object sender, KeyEventArgs e)
        //{
        //    Controllers.MainController.MoveFocusToNextControlOnEnter(sender, e);
        //}

        private void BaseWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.F12) return;
            var selectedtem = (SearchItem)grdList.SelectedItem;
            if (selectedtem == null)
                return;
            SelectedItem = selectedtem;
            DialogResult = true;
            Close();
        }
    }
}
