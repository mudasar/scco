using System.Collections.Generic;
using System.Linq;
using System.Windows;
using SCCO.WPF.MVC.CS.Models;

namespace SCCO.WPF.MVC.CS.Views {
    /// <summary>
    /// Interaction logic for MembershipTypeMaintenanceWindow.xaml
    /// </summary>
    public partial class MembershipTypeMaintenanceWindow {
        public MembershipTypeMaintenanceWindow() {
            InitializeComponent();
            _currentMembershipType = new MembershipType();
            DataContext = _currentMembershipType;
        }

        private MembershipType _currentMembershipType = new MembershipType();


        private void Create(object sender, RoutedEventArgs e) {
            DataContext = _currentMembershipType = new MembershipType();
        }

        private void Read(object sender, RoutedEventArgs e) {
            List<MembershipType> membershipTypes = MembershipType.GetList();
            List<SearchItem> searchItems =
                membershipTypes.Select(membershipType => new SearchItem(membershipType.MembershipTypeId, membershipType.Description)).ToList();

            var searchWindow = new SearchWindow(searchItems);
            searchWindow.ShowDialog();
            if (searchWindow.DialogResult == true) {
                DataContext = _currentMembershipType = new MembershipType(searchWindow.SelectedItem.ItemId);
            }
        }

        private void Update(object sender, RoutedEventArgs e) {
            if (string.IsNullOrEmpty(_currentMembershipType.Description)) {
                MessageWindow.ShowAlertMessage("MembershipType Name must not be empty!");
                return;
            }
            if (_currentMembershipType.MembershipTypeId == 0) {
                _currentMembershipType.Create();
                return;
            }
            _currentMembershipType.Update();
        }

        private void Delete(object sender, RoutedEventArgs e) {
            if (
                MessageWindow.ShowConfirmMessage(
                    "You are about to delete current MembershipType information. Do you want to proceed?") ==
                MessageBoxResult.Yes) {
                _currentMembershipType.Destroy();
                MessageWindow.ShowNotifyMessage("MembershipType information deleted!");
                DataContext = _currentMembershipType = new MembershipType(0);
            }
        }
    }
}
