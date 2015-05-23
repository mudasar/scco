using System.Collections.Generic;
using System.Linq;
using System.Windows;
using SCCO.WPF.MVC.CS.Models;

namespace SCCO.WPF.MVC.CS.Views {
    /// <summary>
    /// Interaction logic for BranchMaintenanceWindow.xaml
    /// </summary>
    public partial class BranchMaintenanceWindow {
        public BranchMaintenanceWindow() {
            InitializeComponent();
            _currentBranch = new Branch();
            DataContext = _currentBranch;
        }

        private Branch _currentBranch = new Branch();


        private void Create(object sender, RoutedEventArgs e) {
            DataContext = _currentBranch = new Branch();
        }

        private void Read(object sender, RoutedEventArgs e) {
            List<Branch> branches = Branch.GetList();
            List<SearchItem> searchItems =
                branches.Select(branch => new SearchItem(branch.BranchId, branch.BranchName)).ToList();

            var searchWindow = new SearchWindow(searchItems);
            searchWindow.ShowDialog();
            if (searchWindow.DialogResult == true) {
                DataContext = _currentBranch = new Branch(searchWindow.SelectedItem.ItemId);
            }
        }

        private void Update(object sender, RoutedEventArgs e) {
            if (string.IsNullOrEmpty(_currentBranch.BranchName)) {
                MessageWindow.ShowAlertMessage("Branch Name must not be empty!");
                return;
            }
            if (_currentBranch.BranchId == 0) {
                _currentBranch.Create();
                return;
            }
            _currentBranch.Update();
        }

        private void Delete(object sender, RoutedEventArgs e) {
            if (
                MessageWindow.ShowConfirmMessage(
                    "You are about to delete current Branch information. Do you want to proceed?") ==
                MessageBoxResult.Yes) {
                _currentBranch.Destroy();
                MessageWindow.ShowNotifyMessage("Branch information deleted!");
                DataContext = _currentBranch = new Branch(0);
            }
        }
    }
}
